CREATE PROCEDURE AGREGAR_EMPLEADO
  @IdEmpleado UNIQUEIDENTIFIER,
  @Cedula NVARCHAR(9),
  @Nombre NVARCHAR(150),
  @Telefono NVARCHAR(8),
  @CorreoElectronico NVARCHAR(150),
  @Puesto NVARCHAR(100),
  @Padecimientos NVARCHAR(250),
  @CuentaBancaria NVARCHAR(20),
  @TipoContrato NVARCHAR(50),
  @Jornada NVARCHAR(50),
  @FechaIngreso DATE,
  @EstadoId INT,
  @FechaRegistro DATE
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRANSACTION;

  INSERT INTO [dbo].[EMPLEADOS] (
    IdEmpleado,
    Cedula,
    NombreCompleto,
    Telefono,
    CorreoElectronico,
    Puesto,
    Padecimientos,
    CuentaBancaria,
    TipoContrato,
    Jornada,
    FechaIngreso,
    ESTADO_ID,
    FechaRegistro
  )
  VALUES (
    @IdEmpleado,
    @Cedula,
    @Nombre,
    @Telefono,
    @CorreoElectronico,
    @Puesto,
    @Padecimientos,
    @CuentaBancaria,
    @TipoContrato,
    @Jornada,
    @FechaIngreso,
    @EstadoId,
    @FechaRegistro
  );

  COMMIT TRANSACTION;

 
  SELECT @IdEmpleado;
END;
