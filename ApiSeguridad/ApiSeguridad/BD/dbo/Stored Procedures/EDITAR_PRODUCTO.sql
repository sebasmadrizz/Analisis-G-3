CREATE PROCEDURE EDITAR_PRODUCTO
  @IdProducto UNIQUEIDENTIFIER,
  @Nombre NVARCHAR(255),
  @Marca NVARCHAR(255),
  @Precio DECIMAL(8,2),
  @Descripcion NVARCHAR(255),
  @Stock INT,
  @ImagenUrl NVARCHAR(255),
  @ProveedorID UNIQUEIDENTIFIER,
  @CategoriasId UNIQUEIDENTIFIER
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  UPDATE [dbo].[PRODUCTOS]
  SET
    NOMBRE = @Nombre,
    MARCA = @Marca,
    PRECIO = @Precio,
    DESCRIPCION = @Descripcion,
    STOCK = @Stock,
    IMAGEN_URL = @ImagenUrl,
    PROVEEDOR_ID=@ProveedorID,
    CATEGORIAS_ID=@CategoriasId
  WHERE PRODUCTOS_ID = @IdProducto;

  COMMIT TRANSACTION;

  SELECT @IdProducto AS Resultado;
END;