CREATE   PROCEDURE dbo.VER_CATEGORIAS_FTS_API_OPT
    @Start INT = 0,
    @Length INT = 10,
    @SearchTerm NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @Start = IIF(@Start < 0, 0, @Start);
    SET @Length = IIF(@Length <= 0, 10, @Length);

    DECLARE @TotalCategorias INT;
    SELECT @TotalCategorias = SUM(row_count)
    FROM sys.dm_db_partition_stats
    WHERE object_id = OBJECT_ID('CATEGORIAS') AND index_id IN (0,1);

    DECLARE @UsaFallback BIT = 0;

    -- Fallback dinámico según tamaño de tabla
    DECLARE @TopFallback INT;
    SET @TopFallback = 
        CASE 
            WHEN @TotalCategorias <= 500 THEN 50
            WHEN @TotalCategorias <= 2000 THEN 100
            WHEN @TotalCategorias <= 10000 THEN 150
            ELSE 200
        END;

    -- No hace falta DROP: cada ejecución tiene su propia temp table
    CREATE TABLE #Candidates (
        CategoriasId UNIQUEIDENTIFIER PRIMARY KEY,
        Nombre NVARCHAR(200),
        PadreId UNIQUEIDENTIFIER,
        NombrePadre NVARCHAR(200),
        Descripcion NVARCHAR(MAX),
        FechaCreacion DATE,
        Estado NVARCHAR(50),
        IsPrefixMatch BIT DEFAULT 0
    );

    IF @SearchTerm IS NOT NULL AND LEN(@SearchTerm) > 0
    BEGIN
        DECLARE @SearchExpression NVARCHAR(200) = 'FORMSOF(INFLECTIONAL, "' + @SearchTerm + '")';

        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, IsPrefixMatch)
        SELECT 
            c.CATEGORIAS_ID,
            c.NOMBRE,
            c.PADRE_ID,
            cp.NOMBRE,
            c.DESCRIPCION,
            c.FECHA_CREACION,
            e.TIPO,
            IIF(c.NOMBRE COLLATE Latin1_General_CI_AI LIKE @SearchTerm + '%' COLLATE Latin1_General_CI_AI, 1, 0)
        FROM CATEGORIAS c
        LEFT JOIN FREETEXTTABLE(CATEGORIAS, NOMBRE, @SearchExpression) ft ON c.CATEGORIAS_ID = ft.[KEY]
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
        WHERE ft.[KEY] IS NOT NULL
           OR c.NOMBRE COLLATE Latin1_General_CI_AI LIKE '%' + @SearchTerm + '%' COLLATE Latin1_General_CI_AI;

        DECLARE @CountCandidates INT = (SELECT COUNT(*) FROM #Candidates);
        IF @CountCandidates = 0 OR @CountCandidates < @TopFallback
        BEGIN
            SET @UsaFallback = 1;

            -- Simplificado: siempre TOP(@TopFallback), evitando cálculos adicionales
            INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, IsPrefixMatch)
            SELECT TOP (@TopFallback)
                c.CATEGORIAS_ID,
                c.NOMBRE,
                c.PADRE_ID,
                cp.NOMBRE,
                c.DESCRIPCION,
                c.FECHA_CREACION,
                e.TIPO,
                0
            FROM CATEGORIAS c
            LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
            JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID
            WHERE NOT EXISTS (
                SELECT 1 FROM #Candidates ca WHERE ca.CategoriasId = c.CATEGORIAS_ID
            )
            ORDER BY c.NOMBRE;
        END
    END
    ELSE
    BEGIN
        INSERT INTO #Candidates (CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, IsPrefixMatch)
        SELECT 
            c.CATEGORIAS_ID,
            c.NOMBRE,
            c.PADRE_ID,
            cp.NOMBRE,
            c.DESCRIPCION,
            c.FECHA_CREACION,
            e.TIPO,
            0
        FROM CATEGORIAS c
        LEFT JOIN CATEGORIAS cp ON c.PADRE_ID = cp.CATEGORIAS_ID
        JOIN ESTADOS e ON c.ESTADO_ID = e.ESTADO_ID;
    END

    -- Conteo de coincidencias ANTES de paginar
    DECLARE @RecordsFiltered INT = (SELECT COUNT(*) FROM #Candidates);

    -- Datos: paginados solo si no es fallback
    IF @UsaFallback = 1
    BEGIN
        SELECT CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, IsPrefixMatch
        FROM #Candidates
        ORDER BY Nombre;
    END
    ELSE
    BEGIN
        SELECT CategoriasId, Nombre, PadreId, NombrePadre, Descripcion, FechaCreacion, Estado, IsPrefixMatch
        FROM #Candidates
        ORDER BY IsPrefixMatch DESC, Nombre ASC
        OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;
    END

    -- Metadatos
    SELECT 
        @TotalCategorias AS recordsTotal,
        @RecordsFiltered AS recordsFiltered,
        @UsaFallback AS usaFallback;

    DROP TABLE #Candidates;
END;