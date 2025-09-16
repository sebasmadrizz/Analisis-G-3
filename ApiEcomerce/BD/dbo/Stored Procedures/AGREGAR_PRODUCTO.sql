-- =============================================
-- Author:      TuNombre
-- Create date: GETDATE()
-- Description: Inserta un nuevo producto y retorna su ID
-- =============================================
CREATE PROCEDURE AGREGAR_PRODUCTO
  @IdProducto UNIQUEIDENTIFIER,
  @Nombre NVARCHAR(255),
  @Marca NVARCHAR(255),
  @Precio DECIMAL(8,2),
  @Descripcion NVARCHAR(255),
  @IdProveedor UNIQUEIDENTIFIER,
  @IdCategoria UNIQUEIDENTIFIER,
  @Stock INT,
  @ImagenUrl NVARCHAR(255),
  @FechaCreacion DATE,
  @IdEstado INT
AS
BEGIN
  SET NOCOUNT ON;
    BEGIN TRANSACTION;

    INSERT INTO [dbo].[PRODUCTOS] (
      PRODUCTOS_ID,
      NOMBRE,
      MARCA,
      PRECIO,
      DESCRIPCION,
      PROVEEDOR_ID,
      CATEGORIAS_ID,
      STOCK,
      IMAGEN_URL,
      FECHA_CREACION,
      ESTADO_ID
    )
    VALUES (
      @IdProducto,
      @Nombre,
      @Marca,
      @Precio,
      @Descripcion,
      @IdProveedor,
      @IdCategoria,
      @Stock,
      @ImagenUrl,
      @FechaCreacion,
      @IdEstado
    );

    COMMIT TRANSACTION;
    SELECT @IdProducto;
END;
