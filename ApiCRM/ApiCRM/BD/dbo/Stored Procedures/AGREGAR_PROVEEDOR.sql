-- =============================================
-- Author:      TuNombre
-- Create date: GETDATE()
-- Description: Inserta un nuevo proveedor y retorna su ID
-- =============================================
CREATE PROCEDURE AGREGAR_PROVEEDOR
  @ProveedorId UNIQUEIDENTIFIER,
  @Nombre NVARCHAR(255),
  @CorreoElectronico NVARCHAR(255),
  @Tipo NVARCHAR(100),
  @Direccion NVARCHAR(255),
  @Telefono NVARCHAR(20),
  @EstadoId INT,
  @FechaCreacion DATE,
  @NombreContacto NVARCHAR(255)
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  INSERT INTO [dbo].[PROVEEDORES] (
    PROVEEDOR_ID,
    Nombre_PROVEEDOR,
    Correo_ELECTRONICO,
    TIPO,
    Direccion,
    Telefono,
    ESTADO_ID,
    Fecha_Registro,
    Nombre_Contacto
  )
  VALUES (
    @ProveedorId,
    @Nombre,
    @CorreoElectronico,
    @Tipo,
    @Direccion,
    @Telefono,
    @EstadoId,
    @FechaCreacion,
    @NombreContacto
  );

  COMMIT TRANSACTION;
  SELECT @ProveedorId;
END;