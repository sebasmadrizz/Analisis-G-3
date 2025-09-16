CREATE PROCEDURE [dbo].[OBTENER_CATEGORIAS_RECURSIVO]
	@IdPadre UNIQUEIDENTIFIER
AS
BEGIN
    WITH CategoriasRecursivas AS (
          SELECT 
            CATEGORIAS_ID,
            PADRE_ID,
            NOMBRE,
            DESCRIPCION,
            FECHA_CREACION,
            ESTADO_ID,
            CAST(CATEGORIAS_ID AS VARCHAR(MAX)) AS Ruta
        FROM CATEGORIAS
        WHERE PADRE_ID = @IdPadre
        UNION ALL
         SELECT 
            c.CATEGORIAS_ID,
            c.PADRE_ID,
            c.NOMBRE,
            c.DESCRIPCION,
            c.FECHA_CREACION,
            c.ESTADO_ID,
            cr.Ruta + '>' + CAST(c.CATEGORIAS_ID AS VARCHAR(MAX))
        FROM CATEGORIAS c
        INNER JOIN CategoriasRecursivas cr ON c.PADRE_ID = cr.CATEGORIAS_ID
        WHERE cr.Ruta NOT LIKE '%' + CAST(c.CATEGORIAS_ID AS VARCHAR(MAX)) + '%'
    )
    SELECT 
        cr.CATEGORIAS_ID AS CategoriasId,
        cr.PADRE_ID AS PadreId,
        cr.NOMBRE AS Nombre,
        cr.DESCRIPCION AS Descripcion,
        cr.FECHA_CREACION AS FechaCreacion,
        e.TIPO AS Estado
    FROM CategoriasRecursivas cr
    INNER JOIN ESTADOS e ON cr.ESTADO_ID = e.ESTADO_ID
    OPTION (MAXRECURSION 32767)
END