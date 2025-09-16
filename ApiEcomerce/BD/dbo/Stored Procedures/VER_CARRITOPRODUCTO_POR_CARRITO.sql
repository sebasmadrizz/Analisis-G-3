CREATE PROCEDURE VER_CARRITOPRODUCTOS_POR_CARRITO
 @CarritoId UNIQUEIDENTIFIER
AS
BEGIN
  SET NOCOUNT ON;

  SELECT 
     cp.CARRITO_PRODUCTO_ID AS CarritoProductoId,
    cp.CARRITO_ID AS CarritoId,
    cp.PRODUCTOS_ID AS ProductosId,
    cp.CANTIDAD AS Cantidad,
    cp.TOTAL_LINEA AS TotalLinea,
    p.NOMBRE AS NombreProducto,
    p.PRECIO AS PrecioUnitario,
    p.IMAGEN_URL AS ImagenUrl,
    p.DESCRIPCION AS Descripcion,
    p.stock as StockDisponible 
  FROM CARRITO_PRODUCTO cp
  INNER JOIN PRODUCTOS p ON cp.PRODUCTOS_ID = p.PRODUCTOS_ID
  WHERE cp.CARRITO_ID = @CarritoId;
END;