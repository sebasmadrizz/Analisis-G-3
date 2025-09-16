CREATE PROCEDURE ProductosXCategoria
    @PageIndex INT,
    @PageSize INT,
    @idCategoria UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT = (@PageIndex - 1) * @PageSize;

    
    SELECT 
    P.PRODUCTOS_ID AS IdProducto,
    P.NOMBRE AS Nombre,
    p.MARCA AS Marca,
    P.PRECIO AS Precio,
    P.DESCRIPCION AS Descripcion,
    P.STOCK AS Stock,
    P.IMAGEN_URL AS ImagenUrl,
    P.FECHA_CREACION AS FechaCreacion,
     pr.Nombre_PROVEEDOR AS NombreProveedor,
    C.NOMBRE AS Categoria,
    E.TIPO AS Estado

  FROM PRODUCTOS P
   INNER JOIN Proveedores PR ON P.PROVEEDOR_ID = PR.PROVEEDOR_ID
  INNER JOIN CATEGORIAS C ON P.CATEGORIAS_ID = C.CATEGORIAS_ID
  INNER JOIN ESTADOS E ON P.ESTADO_ID = E.ESTADO_ID
  WHERE P.CATEGORIAS_ID = @idCategoria
    ORDER BY PRODUCTOS_ID
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    
    SELECT COUNT(*) AS TotalRegistros FROM Productos WHERE CATEGORIAS_ID=@idCategoria;
END
