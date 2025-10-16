CREATE PROCEDURE dbo.AGREGAR_EMPLEADOPLANILLA
    @EmpleadoId UNIQUEIDENTIFIER ,
    @Cedula NVARCHAR(50) ,
    @Nombre NVARCHAR(200),
    @Apellido NVARCHAR(200),
    @CorreoElectronico NVARCHAR(256) ,
    @Telefono NVARCHAR(50) ,
    @FechaIngreso DATETIME ,
    @Padecimientos NVARCHAR(MAX) ,
    @PuestoId UNIQUEIDENTIFIER ,           -- nombre del puesto; si no existe se crea
    @CuentaBancaria NVARCHAR(100) ,
    @HorarioId UNIQUEIDENTIFIER ,
    @Sueldo DECIMAL(18,2) ,
    @EstadoId INT,
    @Banco NVARCHAR(100) ,
    @tipoCuenta NVARCHAR(50) 
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    IF @EmpleadoId IS NULL
        SET @EmpleadoId = NEWID();


        ----------------------------------------------------------------
        -- 1) Insertar en Empleados
        -- Se insertan los campos disponibles; columnas no presentes se dejan NULL.
        ----------------------------------------------------------------
        INSERT INTO Empleado (
            empleado_id,
            nombre,
            apellido,
            correo,   
            fecha_ingreso,          
            telefono,
            cedula,
            Padecimientos,
            estado_id
        )
        VALUES (
            @EmpleadoId,
            @Nombre,
            @Apellido,
            @CorreoElectronico,
            @FechaIngreso,
            @Telefono,
            @Cedula,
            @Padecimientos,
            @EstadoId
        );
        DECLARE @EmpleadoPuestoId UNIQUEIDENTIFIER = NEWID();


         INSERT INTO Empleado_puesto (
            empleado_puesto_id,
            empleado_id,
            puesto_id,
            fecha_inicio,
            fecha_fin,
            estado_id
        )
        VALUES (
            @EmpleadoPuestoId,
            @EmpleadoId,
            @PuestoId,
            @FechaIngreso,
            NULL,
            @EstadoId
        );
        INSERT INTO Sueldos (
            sueldo_id,
            empleado_puesto_id,
            monto,
            fecha_inicio,
            fecha_fin,
            estado_id
        )
        VALUES (
            NEWID(),
            @EmpleadoPuestoId,
            @Sueldo,
            @FechaIngreso,
            
            null,
            @EstadoId
        );

        INSERT INTO Empleado_Horario (
                empleado_horario_id,
                empleado_id,
                horario_id,
                fecha_inicio,
                fecha_fin,
                estado_id
            )
            VALUES (
                NEWID(),
                @EmpleadoId,
                @HorarioId,
                @FechaIngreso,
                NULL,
                @EstadoId
            );

            INSERT INTO Cuenta_empleado (
                cuenta_empleado_id,
                empleado_id,
                banco,
                numero_cuenta,
                tipo_cuenta,
                estado_id
            )
            VALUES (
                NEWID(),
                @EmpleadoId,
                @Banco,
                @CuentaBancaria,
                @tipoCuenta,
                @EstadoId
            );



     

        COMMIT TRANSACTION;

        -- Opcional: devolver IDs relevantes
        SELECT
            @EmpleadoId AS EmpleadoId

END
