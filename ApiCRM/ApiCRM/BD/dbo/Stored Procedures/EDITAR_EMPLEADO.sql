CREATE OR ALTER PROCEDURE dbo.EDITAR_EMPLEADOPLANILLA
    @EmpleadoId UNIQUEIDENTIFIER,
    @Cedula NVARCHAR(50),
    @Nombre NVARCHAR(200),
    @Apellido NVARCHAR(200),
    @CorreoElectronico NVARCHAR(256),
    @Telefono NVARCHAR(50),
    @FechaIngreso DATETIME,
    @Padecimientos NVARCHAR(MAX),
    @PuestoId UNIQUEIDENTIFIER,
    @CuentaBancaria NVARCHAR(100),
    @HorarioId UNIQUEIDENTIFIER,
    @Sueldo DECIMAL(18,2),
    @Banco NVARCHAR(100),
    @tipoCuenta NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    ----------------------------------------------------------------
    -- 1) Actualizar Empleado
    ----------------------------------------------------------------
    UPDATE Empleado
    SET
        nombre = @Nombre,
        apellido = @Apellido,
        correo = @CorreoElectronico,
        fecha_ingreso = @FechaIngreso,
        telefono = @Telefono,
        cedula = @Cedula,
        Padecimientos = @Padecimientos
    WHERE empleado_id = @EmpleadoId;

    ----------------------------------------------------------------
    -- 2) Actualizar Empleado_puesto
    ----------------------------------------------------------------
    UPDATE Empleado_puesto
    SET
        puesto_id = @PuestoId,
        fecha_inicio = @FechaIngreso
        
    WHERE empleado_id = @EmpleadoId;

    ----------------------------------------------------------------
    -- 3) Actualizar Sueldos (tomando el registro activo más reciente)
    ----------------------------------------------------------------
    UPDATE Sueldos
    SET
        monto = @Sueldo,
        fecha_inicio = @FechaIngreso
    WHERE empleado_puesto_id = (
        SELECT TOP 1 empleado_puesto_id
        FROM Empleado_puesto
        WHERE empleado_id = @EmpleadoId
        ORDER BY fecha_inicio DESC
    );

    ----------------------------------------------------------------
    -- 4) Actualizar Empleado_Horario
    ----------------------------------------------------------------
    UPDATE Empleado_Horario
    SET
        horario_id = @HorarioId,
        fecha_inicio = @FechaIngreso
    WHERE empleado_id = @EmpleadoId;

    ----------------------------------------------------------------
    -- 5) Actualizar Cuenta_empleado
    ----------------------------------------------------------------
    UPDATE Cuenta_empleado
    SET
        banco = @Banco,
        numero_cuenta = @CuentaBancaria,
        tipo_cuenta = @tipoCuenta
    WHERE empleado_id = @EmpleadoId;

    COMMIT TRANSACTION;

    -- Opcional: devolver el ID del empleado
    SELECT @EmpleadoId AS EmpleadoId;
END

