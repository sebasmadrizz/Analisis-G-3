
CREATE PROCEDURE EDITAR_PROVEEDOR
  @ProveedorId UNIQUEIDENTIFIER,
  @Nombre NVARCHAR(100),
  @CorreoElectronico NVARCHAR(100),
  @Telefono NVARCHAR(20),
  @Direccion NVARCHAR(200),
  @NombreContacto NVARCHAR(100),
  @FechaRegistro DATETIME,
  @Tipo NVARCHAR(MAX),
  @EstadoId INT
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  UPDATE [dbo].Proveedores
  SET
    Nombre_PROVEEDOR = @Nombre,
      Correo_ELECTRONICO = @CorreoElectronico,
      Telefono = @Telefono,
      Direccion = @Direccion,
      Nombre_Contacto = @NombreContacto,
      Fecha_Registro = @FechaRegistro,
      TIPO = @Tipo,
      ESTADO_ID = @EstadoId
    WHERE PROVEEDOR_ID = @ProveedorId;

  COMMIT TRANSACTION;

  SELECT @ProveedorId AS Resultado;
END
