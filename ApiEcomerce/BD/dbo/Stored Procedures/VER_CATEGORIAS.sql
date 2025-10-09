CREATE PROCEDURE VER_CATEGORIAS_PAGINADO
    @Start INT,
    @Length INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TotalCategorias INT;
    SELECT @TotalCategorias = SUM(row_count)
    FROM sys.dm_db_partition_stats
    WHERE object_id = OBJECT_ID('CATEGORIAS') AND index_id IN (0,1);

    SELECT 
        C.CATEGORIAS_ID AS CategoriasId,
        C.NOMBRE AS Nombre,
        C.PADRE_ID AS PadreId,
        CP.NOMBRE AS NombrePadre,
        C.DESCRIPCION AS Descripcion,
        C.FECHA_CREACION AS FechaCreacion,
        E.TIPO AS Estado
    FROM CATEGORIAS C
    LEFT JOIN CATEGORIAS CP ON C.PADRE_ID = CP.CATEGORIAS_ID
    INNER JOIN ESTADOS E ON C.ESTADO_ID = E.ESTADO_ID
    ORDER BY C.CATEGORIAS_ID
    OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;

    SELECT @TotalCategorias AS TotalCategorias;
END;