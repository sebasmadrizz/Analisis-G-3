
create PROCEDURE [dbo].[VER_CATEGORIAS_PAGINADO_FTS_OPT_V30]
    @Start INT,
    @Length INT,
    @SearchTerm NVARCHAR(100) = NULL,
    @LevenshteinThreshold INT = NULL,
    @TopForLeven INT = 25
AS
BEGIN
    SET NOCOUNT ON;

    -- Validaciones de paginación
    SET @Start = CASE WHEN @Start < 0 THEN 0 ELSE @Start END;
    SET @Length = CASE WHEN @Length <= 0 THEN 10 ELSE @Length END;

    -- Umbral dinámico
    IF @LevenshteinThreshold IS NULL
    BEGIN
        IF @SearchTerm IS NULL
            SET @LevenshteinThreshold = 0;
        ELSE
        BEGIN
            DECLARE @len INT = LEN(@SearchTerm);
            IF @len <= 4
                SET @LevenshteinThreshold = 2;
            ELSE
                SET @LevenshteinThreshold = CASE WHEN CEILING(@len * 0.25) > 4 THEN 4 ELSE CEILING(@len * 0.25) END;
        END
    END

    -- Total de categorías
    DECLARE @TotalCategorias INT;
    SELECT @TotalCategorias = SUM(row_count)
    FROM sys.dm_db_partition_stats
    WHERE object_id = OBJECT_ID('CATEGORIAS') AND index_id IN (0,1);

    -- Tabla temporal
    IF OBJECT_ID('tempdb..#Candidates') IS NOT NULL DROP TABLE #Candidates;
    CREATE TABLE #Candidates (
        CategoriasId UNIQUEIDENTIFIER PRIMARY KEY,
        Nombre NVARCHAR(200),
        PadreId UNIQUEIDENTIFIER,
        NombrePadre NVARCHAR(200),
        Descripcion NVARCHAR(MAX),
        FechaCreacion DATE,
        Estado NVARCHAR(50),
        FtsRank INT NULL,
        Distancia INT NULL,
        IsPrefixMatch BIT DEFAULT 0
    );

    IF @SearchTerm IS NOT NULL AND LEN(@SearchTerm) > 0
    BEGIN
        -- 1. Insertar todos los candidatos FTS y prefijo (sin TOP), COLLATE AI para ignorar tildes
        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, FtsRank, IsPrefixMatch)
        SELECT 
            c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO,
            ft.RANK,
            CASE WHEN c.NOMBRE COLLATE Latin1_General_CI_AI LIKE @SearchTerm + '%' COLLATE Latin1_General_CI_AI THEN 1 ELSE 0 END
        FROM CATEGORIAS c
        LEFT JOIN FREETEXTTABLE(CATEGORIAS, NOMBRE, @SearchTerm) ft ON c.CATEGORIAS_ID = ft.[KEY]
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
        WHERE ft.[KEY] IS NOT NULL OR c.NOMBRE COLLATE Latin1_General_CI_AI LIKE @SearchTerm + '%' COLLATE Latin1_General_CI_AI;

        -- 2. Insertar Levenshtein solo para ranking/sugerencia
        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, Distancia)
        SELECT TOP (@TopForLeven)
            c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO,
            L.Distancia
        FROM CATEGORIAS c
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
        CROSS APPLY (SELECT dbo.Levenshtein(UPPER(c.NOMBRE), UPPER(@SearchTerm)) AS Distancia) AS L
        WHERE NOT EXISTS (SELECT 1 FROM #Candidates ca WHERE ca.CategoriasId = c.CATEGORIAS_ID)
          AND L.Distancia <= @LevenshteinThreshold
        ORDER BY L.Distancia ASC;
    END
    ELSE
    BEGIN
        -- Sin searchTerm -> devolver todo limitado por paginación
        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado)
        SELECT 
            c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO
        FROM CATEGORIAS c
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID;
    END

    -- Paginación final
    SELECT
        CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado
    FROM #Candidates
    ORDER BY IsPrefixMatch DESC, ISNULL(Distancia, 9999) ASC, ISNULL(FtsRank, 0) DESC, Nombre ASC
    OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;

    -- Sugerencia
    DECLARE @Sugerencia NVARCHAR(200) = N'No existe';
    IF @SearchTerm IS NOT NULL AND EXISTS(SELECT 1 FROM #Candidates)
    BEGIN
        SELECT TOP 1 @Sugerencia = Nombre
        FROM #Candidates
        WHERE IsPrefixMatch = 1 OR (Distancia IS NOT NULL AND Distancia <= @LevenshteinThreshold)
        ORDER BY IsPrefixMatch DESC, Distancia ASC, FtsRank DESC;
    END

    -- Metadatos
    SELECT
        @TotalCategorias AS recordsTotal,
        (SELECT COUNT(*) FROM #Candidates) AS recordsFiltered,
        @Sugerencia AS Sugerencia;

    DROP TABLE #Candidates;
END;