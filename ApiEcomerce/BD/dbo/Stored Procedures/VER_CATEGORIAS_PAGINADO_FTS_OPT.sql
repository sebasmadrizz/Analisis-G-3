
CREATE   PROCEDURE VER_CATEGORIAS_PAGINADO_FTS_OPT
    @Start INT,
    @Length INT,
    @SearchTerm NVARCHAR(100) = NULL,
    @LevenshteinThreshold INT = NULL, -- NULL => calcula dinámico (ceil(len*0.25) cap 4)
    @TakeMax INT = 200,               -- fallback limitado
    @TopForLeven INT = 25,            -- cuántos candidatos evaluar con Levenshtein
    @LenDiffMax INT = 1               -- diferencia de longitud permitida en fallback
AS
BEGIN
    SET NOCOUNT ON;

    ------------------------------------------
    -- Validaciones básicas
    ------------------------------------------
    SET @Start = CASE WHEN @Start < 0 THEN 0 ELSE @Start END;
    SET @Length = CASE WHEN @Length <= 0 THEN 10 ELSE @Length END;

    -- Umbral dinámico para Levenshtein
    IF @LevenshteinThreshold IS NULL
    BEGIN
        IF @SearchTerm IS NULL
            SET @LevenshteinThreshold = 0;
        ELSE
        BEGIN
            DECLARE @calc INT = CEILING(LEN(@SearchTerm) * 0.25);
            SET @LevenshteinThreshold = CASE WHEN @calc > 4 THEN 4 ELSE @calc END;
        END
    END

    ------------------------------------------
    -- Total de registros
    ------------------------------------------
    DECLARE @TotalCategorias INT;
    SELECT @TotalCategorias = SUM(row_count)
    FROM sys.dm_db_partition_stats
    WHERE object_id = OBJECT_ID('CATEGORIAS') AND index_id IN (0,1);

    ------------------------------------------
    -- Tabla temporal de candidatos
    ------------------------------------------
    IF OBJECT_ID('tempdb..#Candidates') IS NOT NULL DROP TABLE #Candidates;
    CREATE TABLE #Candidates (
        CategoriasId UNIQUEIDENTIFIER,
        Nombre NVARCHAR(200),
        PadreId UNIQUEIDENTIFIER,
        NombrePadre NVARCHAR(200),
        Descripcion NVARCHAR(MAX),
        FechaCreacion DATE,
        Estado NVARCHAR(50),
        FtsRank INT NULL,
        Distancia INT NULL
    );

    ------------------------------------------
    -- Si hay SearchTerm -> FTS
    ------------------------------------------
    IF @SearchTerm IS NOT NULL
    BEGIN
        DECLARE @ftsForms NVARCHAR(400) = N'FORMSOF(INFLECTIONAL, "' + REPLACE(@SearchTerm, '"', '""') + N'")';
        DECLARE @ftsExact NVARCHAR(400) = N'"' + REPLACE(@SearchTerm, '"', '""') + N'"';
        DECLARE @ftsPrefix NVARCHAR(400) = N'"' + REPLACE(@SearchTerm, '"', '""') + N'*"';

        -- 1) FORMSOF
        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, FtsRank)
        SELECT TOP (@Length * 4)
            c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO, ft.RANK
        FROM CONTAINSTABLE(CATEGORIAS, NOMBRE, @ftsForms) ft
        JOIN CATEGORIAS c ON c.CATEGORIAS_ID = ft.[KEY]
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
        WHERE NOT EXISTS (SELECT 1 FROM #Candidates ch WHERE ch.CategoriasId = c.CATEGORIAS_ID)
        ORDER BY ft.RANK DESC;

        -- 2) FREETEXTTABLE si no obtuvo candidatos
        IF NOT EXISTS (SELECT 1 FROM #Candidates)
        BEGIN
            INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, FtsRank)
            SELECT TOP (@Length * 4)
                c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO, ft.RANK
            FROM FREETEXTTABLE(CATEGORIAS, NOMBRE, @SearchTerm) ft
            JOIN CATEGORIAS c ON c.CATEGORIAS_ID = ft.[KEY]
            LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
            JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
            WHERE NOT EXISTS (SELECT 1 FROM #Candidates ch WHERE ch.CategoriasId = c.CATEGORIAS_ID)
            ORDER BY ft.RANK DESC;
        END

        -- 3) Exact term si sigue vacío
        IF NOT EXISTS (SELECT 1 FROM #Candidates)
        BEGIN
            INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, FtsRank)
            SELECT TOP (@Length * 4)
                c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO, ft.RANK
            FROM CONTAINSTABLE(CATEGORIAS, NOMBRE, @ftsExact) ft
            JOIN CATEGORIAS c ON c.CATEGORIAS_ID = ft.[KEY]
            LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
            JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
            WHERE NOT EXISTS (SELECT 1 FROM #Candidates ch WHERE ch.CategoriasId = c.CATEGORIAS_ID)
            ORDER BY ft.RANK DESC;
        END

        -- 4) Fallback por prefijo si sigue vacío
        IF NOT EXISTS (SELECT 1 FROM #Candidates)
        BEGIN
            INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, FtsRank)
            SELECT TOP (@Length * 4)
                c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO, ft.RANK
            FROM CONTAINSTABLE(CATEGORIAS, NOMBRE, @ftsPrefix) ft
            JOIN CATEGORIAS c ON c.CATEGORIAS_ID = ft.[KEY]
            LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
            JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
            WHERE NOT EXISTS (SELECT 1 FROM #Candidates ch WHERE ch.CategoriasId = c.CATEGORIAS_ID)
            ORDER BY ft.RANK DESC;
        END
    END
    ELSE
    BEGIN
        -- Sin SearchTerm -> devolver set limitado
        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, FtsRank)
        SELECT TOP (@Length * 4)
            c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO, NULL
        FROM CATEGORIAS c
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
        ORDER BY c.NOMBRE;
    END

    ------------------------------------------
    -- Fallback limitado (si sigue vacío)
    ------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM #Candidates) AND @SearchTerm IS NOT NULL
    BEGIN
        IF OBJECT_ID('tempdb..#Fallback') IS NOT NULL DROP TABLE #Fallback;
        CREATE TABLE #Fallback (
            CategoriasId UNIQUEIDENTIFIER,
            Nombre NVARCHAR(200),
            PadreId UNIQUEIDENTIFIER,
            NombrePadre NVARCHAR(200),
            Descripcion NVARCHAR(MAX),
            FechaCreacion DATE,
            Estado NVARCHAR(50)
        );

        INSERT INTO #Fallback (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado)
        SELECT TOP (@TakeMax)
            c.CATEGORIAS_ID, c.NOMBRE, c.PADRE_ID, cp.NOMBRE, c.DESCRIPCION, c.FECHA_CREACION, e.TIPO
        FROM CATEGORIAS c
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
        WHERE 
            (ABS(LEN(c.NOMBRE) - LEN(@SearchTerm)) <= @LenDiffMax
             AND (DIFFERENCE(c.NOMBRE, @SearchTerm) >= 3 OR SOUNDEX(c.NOMBRE) = SOUNDEX(@SearchTerm)))
            OR (LEN(@SearchTerm) >= 4 AND CHARINDEX(UPPER(@SearchTerm), UPPER(c.NOMBRE)) > 0)
        ORDER BY c.FECHA_CREACION DESC;

        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, FtsRank)
        SELECT DISTINCT CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, NULL
        FROM #Fallback;

        DROP TABLE #Fallback;
    END

    ------------------------------------------
    -- Calcular Distancia (Levenshtein)
    ------------------------------------------
    IF @SearchTerm IS NOT NULL
    BEGIN
        UPDATE #Candidates
        SET Distancia = dbo.Levenshtein(UPPER(Nombre), UPPER(@SearchTerm))
        WHERE Distancia IS NULL;
    END

    ------------------------------------------
    -- PAGINADO
    ------------------------------------------
    SELECT CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado
    FROM #Candidates
    ORDER BY ISNULL(Distancia, 9999) ASC, ISNULL(FtsRank,0) DESC, Nombre ASC
    OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;

    ------------------------------------------
    -- Sugerencia
    ------------------------------------------
    DECLARE @Sugerencia NVARCHAR(200) = N'No existe';
    IF @SearchTerm IS NOT NULL
    BEGIN
        ;WITH TopC AS (
            SELECT TOP (@TopForLeven) CategoriasId, Nombre, Distancia, FtsRank
            FROM #Candidates
            ORDER BY ISNULL(Distancia,9999) ASC, ISNULL(FtsRank,0) DESC, Nombre ASC
        )
        SELECT TOP 1 @Sugerencia = Nombre
        FROM TopC
        WHERE Distancia IS NOT NULL AND Distancia <= @LevenshteinThreshold
        ORDER BY Distancia ASC, FtsRank DESC;
    END

    ------------------------------------------
    -- Metadata
    ------------------------------------------
    SELECT
        @TotalCategorias AS recordsTotal,
        (SELECT COUNT(*) FROM #Candidates) AS recordsFiltered,
        @Sugerencia AS Sugerencia;

    DROP TABLE #Candidates;
END;