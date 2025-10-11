CREATE OR ALTER PROCEDURE [dbo].[VER_PRODUCTOS_FTS_OPTIMIZADO]
    @PageIndex INT = 1,       -- número de página (1, 2, 3, ...)
    @PageSize INT = 10,       -- cantidad de registros por página
    @SearchTerm NVARCHAR(200),
    @MaxCandidates INT = 15
AS
BEGIN
    SET NOCOUNT ON;

    -- Validaciones seguras
    IF @PageIndex < 1 SET @PageIndex = 1;
    IF @PageSize <= 0 SET @PageSize = 10;
    IF @MaxCandidates <= 0 SET @MaxCandidates = 15;
    IF @MaxCandidates > 100 SET @MaxCandidates = 100;

    -- Calcula el offset clásico
    DECLARE @Start INT = (@PageIndex - 1) * @PageSize;

    -- Total de productos
    DECLARE @TotalProductos INT;
    SELECT @TotalProductos = SUM(row_count)
    FROM sys.dm_db_partition_stats
    WHERE object_id = OBJECT_ID(N'dbo.PRODUCTOS') AND index_id IN (0,1);

    -- Normalización del término
    DECLARE @t NVARCHAR(200) = LOWER(LTRIM(RTRIM(ISNULL(@SearchTerm, N''))));
    SET @t = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@t,'á','a'),'é','e'),'í','i'),'ó','o'),'ú','u');
    SET @t = REPLACE(@t,'ñ','n'); SET @t = REPLACE(@t,'ü','u');

    IF LEN(@t) < 2
    BEGIN
        SELECT @TotalProductos AS TotalRegistros, 0 AS RegistrosFiltrados, CAST(0 AS BIT) AS UsaFallback;
        SELECT TOP (0)
            CAST(NULL AS UNIQUEIDENTIFIER) AS PRODUCTOS_ID,
            CAST(NULL AS NVARCHAR(200)) AS NOMBRE,
            CAST(NULL AS NVARCHAR(200)) AS MARCA,
            CAST(NULL AS DECIMAL(10,2)) AS PRECIO,
            CAST(NULL AS NVARCHAR(500)) AS DESCRIPCION,
            CAST(NULL AS NVARCHAR(200)) AS CATEGORIA,
            CAST(NULL AS INT) AS STOCK,
            CAST(NULL AS NVARCHAR(300)) AS IMAGEN_URL,
            CAST(NULL AS TINYINT) AS Score;
        RETURN 0;
    END

    -- Tokens
    DECLARE @token0 NVARCHAR(100) = (SELECT TOP (1) value FROM STRING_SPLIT(@t, ' ') WHERE LEN(value) >= 1);
    DECLARE @expr NVARCHAR(512) =
    (
        SELECT STRING_AGG('"' + REPLACE(value,'"','""') + '*"', ' OR ')
        FROM STRING_SPLIT(@t, ' ') WHERE LEN(value) >= 2
    );
    IF @expr IS NULL SET @expr = '';

    DECLARE @K TABLE (id UNIQUEIDENTIFIER PRIMARY KEY, score TINYINT);
    DECLARE @KCount INT = 0;
    DECLARE @UsaFallback BIT = 0;

    -- 1️⃣ Exact match (nombre o marca normalizados)
    INSERT INTO @K(id, score)
    SELECT p.PRODUCTOS_ID, 100
    FROM dbo.PRODUCTOS p
    JOIN dbo.CATEGORIAS c ON c.CATEGORIAS_ID = p.CATEGORIAS_ID
    WHERE p.NOMBRE_NORMALIZADO = @t 
       OR p.MARCA_NORMALIZADA = @t 
       OR c.NOMBRE_NORMALIZADO = @t;
    SET @KCount = (SELECT COUNT(1) FROM @K);

    -- 2️⃣ Prefijo (LIKE) normalizado
    IF @KCount < (@Start + @PageSize)
    BEGIN
        INSERT INTO @K(id, score)
        SELECT p.PRODUCTOS_ID, 80
        FROM dbo.PRODUCTOS p
        JOIN dbo.CATEGORIAS c ON c.CATEGORIAS_ID = p.CATEGORIAS_ID
        WHERE (p.NOMBRE_NORMALIZADO LIKE @t + '%' 
               OR p.MARCA_NORMALIZADA LIKE @t + '%' 
               OR c.NOMBRE_NORMALIZADO LIKE @t + '%')
          AND NOT EXISTS (SELECT 1 FROM @K k WHERE k.id = p.PRODUCTOS_ID)
        OPTION (RECOMPILE);
        SET @KCount = (SELECT COUNT(1) FROM @K);
    END

    -- 3️⃣ Full-Text Search (FTS)
    IF @KCount < (@Start + @PageSize) AND LEN(@expr) > 0
    BEGIN
        DECLARE @limit INT = CASE WHEN @MaxCandidates < (@Start + @PageSize + 3) THEN @MaxCandidates ELSE (@Start + @PageSize + 3) END;

        ;WITH FTKeys AS
        (
            SELECT TOP (@limit) ft.[KEY] AS id, ft.[RANK] AS rnk
            FROM CONTAINSTABLE(dbo.PRODUCTOS, (NOMBRE_NORMALIZADO, MARCA_NORMALIZADA), @expr) ft
            ORDER BY ft.[RANK] DESC
        )
        INSERT INTO @K(id, score)
        SELECT f.id,
               CASE WHEN f.rnk IS NULL THEN 40
                    ELSE CASE WHEN f.rnk >= 1000 THEN 79 ELSE 40 + (f.rnk * 39 / 1000) END END
        FROM FTKeys f
        WHERE NOT EXISTS (SELECT 1 FROM @K k WHERE k.id = f.id);

        SET @KCount = (SELECT COUNT(1) FROM @K);
    END

    -- 4️⃣ Boost por coincidencias parciales normalizadas
    IF @KCount > 0
    BEGIN
        DECLARE @Tokens TABLE (value NVARCHAR(200) PRIMARY KEY);
        INSERT INTO @Tokens(value)
        SELECT DISTINCT value FROM STRING_SPLIT(@t, ' ') WHERE LEN(value) >= 2;

        UPDATE k SET score = CASE WHEN k.score + 20 > 100 THEN 100 ELSE k.score + 20 END
        FROM @K k
        JOIN dbo.PRODUCTOS p ON p.PRODUCTOS_ID = k.id
        WHERE EXISTS (SELECT 1 FROM @Tokens s WHERE p.NOMBRE_NORMALIZADO LIKE '%' + s.value + '%' 
                                           OR p.MARCA_NORMALIZADA LIKE '%' + s.value + '%');
    END

    -- 5️⃣ Fallback (SOUNDEX)
    IF @KCount = 0
    BEGIN
        SET @UsaFallback = 1;
        DECLARE @snd NVARCHAR(10) = SOUNDEX(@t);

        INSERT INTO @K(id, score)
        SELECT TOP (@PageSize + 3) p.PRODUCTOS_ID, 15
        FROM dbo.PRODUCTOS p
        WHERE SOUNDEX(p.NOMBRE_NORMALIZADO) = @snd OR SOUNDEX(p.MARCA_NORMALIZADA) = @snd
        ORDER BY LEN(p.NOMBRE_NORMALIZADO), p.NOMBRE_NORMALIZADO;
        SET @KCount = (SELECT COUNT(1) FROM @K);
    END

    -- 📊 Metadatos
    DECLARE @RegistrosFiltrados INT = (SELECT COUNT(1) FROM @K);
    SELECT @TotalProductos AS TotalRegistros, @RegistrosFiltrados AS RegistrosFiltrados, @UsaFallback AS UsaFallback;

    -- Resultado paginado
    ;WITH Ranked AS
    (
        SELECT id, score, ROW_NUMBER() OVER (ORDER BY score DESC, id) AS rn
        FROM @K
    ),
    PageIds AS
    (
        SELECT id, score FROM Ranked WHERE rn > @Start AND rn <= (@Start + @PageSize)
    )
    SELECT p.PRODUCTOS_ID AS IdProducto, p.NOMBRE, p.MARCA, p.PRECIO, LEFT(p.DESCRIPCION,500) AS DESCRIPCION,
           c.NOMBRE AS CATEGORIA, p.STOCK, p.IMAGEN_URL AS ImagenUrl, p.FECHA_CREACION AS FechaCreacion, e.TIPO AS Estado, pg.score AS Score
    FROM PageIds pg
    JOIN dbo.PRODUCTOS p ON p.PRODUCTOS_ID = pg.id
    JOIN dbo.CATEGORIAS c ON c.CATEGORIAS_ID = p.CATEGORIAS_ID
    JOIN dbo.ESTADOS e ON p.ESTADO_ID = e.ESTADO_ID
    ORDER BY pg.score DESC, p.NOMBRE ASC;
END;
GO