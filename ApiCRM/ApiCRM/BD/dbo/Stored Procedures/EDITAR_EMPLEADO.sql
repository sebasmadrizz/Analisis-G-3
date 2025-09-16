CREATE PROCEDURE EDITAR_EMPLEADO
  @IdEmpleado UNIQUEIDENTIFIER,
  @Nombre NVARCHAR(150),
  @CorreoElectronico NVARCHAR(150),
  @Puesto NVARCHAR(100),
  @Padecimientos NVARCHAR(250),
  @CuentaBancaria NVARCHAR(20),
  @TipoContrato NVARCHAR(50),
  @Jornada NVARCHAR(50),
  @Telefono NVARCHAR(8)

AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  UPDATE [dbo].[EMPLEADOS]
  SET
      NombreCompleto   = @Nombre,
      CorreoElectronico = @CorreoElectronico,
      Puesto           = @Puesto,
      Padecimientos    = @Padecimientos,
      CuentaBancaria   = @CuentaBancaria,
      TipoContrato     = @TipoContrato,
      Jornada          = @Jornada,
      Telefono         = @Telefono
  WHERE IdEmpleado = @IdEmpleado;

  COMMIT TRANSACTION;

  
  SELECT @IdEmpleado;
END;
