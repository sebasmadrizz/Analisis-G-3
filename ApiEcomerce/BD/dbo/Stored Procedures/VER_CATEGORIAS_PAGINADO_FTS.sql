
CREATE PROCEDURE VER_CATEGORIAS_PAGINADO_FTS
    @Start INT,
    @Length INT,
    @Draw INT,
    @SearchTerm NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @Start = CASE WHEN @Start < 0 THEN 0 ELSE @Start END;
    SET @Length = CASE WHEN @Length <= 0 THEN 10 ELSE @Length END;

    -- Total de filas usando metadatos
    DECLARE @TotalCategorias INT;
    SELECT @TotalCategorias = SUM(row_count)
    FROM sys.dm_db_partition_stats
    WHERE object_id = OBJECT_ID('CATEGORIAS') AND index_id IN (0,1);

    -- Filtrado y candidatas para Levenshtein (solo un subset para optimizar)
    ;WITH Candidatos AS (
        SELECT TOP (@Length * 5)
            C.CATEGORIAS_ID AS CategoriasId,
            C.NOMBRE AS Nombre,
            C.PADRE_ID AS PadreId,
            CP.NOMBRE AS NombrePadre,
            C.DESCRIPCION AS Descripcion,
            C.FECHA_CREACION AS FechaCreacion,
            E.TIPO AS Estado,
            CASE WHEN @SearchTerm IS NOT NULL THEN dbo.Levenshtein(C.NOMBRE, @SearchTerm) ELSE 0 END AS Distancia
        FROM CATEGORIAS C
        LEFT JOIN CATEGORIAS CP ON C.PADRE_ID = CP.CATEGORIAS_ID
        INNER JOIN ESTADOS E ON C.ESTADO_ID = E.ESTADO_ID
        WHERE (@SearchTerm IS NULL OR FREETEXT((C.NOMBRE, C.DESCRIPCION), @SearchTerm))
    )

    SELECT 
        @TotalCategorias AS recordsTotal,
        (SELECT COUNT(*) FROM Candidatos) AS recordsFiltered,
        C.CategoriasId,
        C.Nombre,
        C.PadreId,
        C.NombrePadre,
        C.Descripcion,
        C.FechaCreacion,
        C.Estado,
        CASE WHEN @SearchTerm IS NOT NULL 
             THEN (SELECT TOP 1 Nombre FROM Candidatos ORDER BY Distancia ASC)
             ELSE NULL
        END AS Sugerencia
    FROM Candidatos C
    ORDER BY C.Distancia ASC, C.CategoriasId ASC
    OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;
END;