CREATE PROCEDURE VER_EMPLEADOS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
    -- Información principal del Empleado (E)
    E.empleado_id,
    E.nombre,
    E.apellido,
    E.correo,
    E.telefono,
    E.fecha_ingreso,
    E.cedula,
    E.Padecimientos,

    -- Información del Puesto Asignado (P y EP)
    P.nombre AS Nombre_Puesto,
    EP.fecha_inicio AS Puesto_Fecha_Inicio,
    EP.fecha_fin AS Puesto_Fecha_Fin,

    -- Información de Sueldos (S) - Unida a través de Empleado_Puesto
    S.monto AS Monto_Sueldo,

    -- Información de Vacaciones (V)
    V.fecha_inicio AS Vacaciones_Fecha_Inicio,
    V.fecha_final AS Vacaciones_Fecha_Final,
    V.dias AS Dias_Vacaciones_Tomados,

    -- Información de Horario (H) - Unida a través de Empleado_Horario (EH)
    H.entrada AS Horario_Entrada,
    H.salida AS Horario_Salida,

    -- Información de Cuenta Bancaria (CE)
    CE.banco AS Banco,
    CE.numero_cuenta AS Numero_Cuenta,
    CE.tipo_cuenta AS Tipo_Cuenta

FROM
    [Proyecto].[dbo].[Empleado] AS E

-- 1. Asignación de Puesto (Relación con Puesto y Sueldos)
INNER JOIN
    Empleado_Puesto AS EP ON E.empleado_id = EP.empleado_id
INNER JOIN
    Puesto AS P ON EP.puesto_id = P.puesto_id

-- 2. Sueldos (Histórico de sueldos para esa asignación de puesto)
INNER JOIN
    Sueldos AS S ON EP.empleado_puesto_id = S.empleado_puesto_id

-- 3. Cuentas Bancarias (Debe tener una cuenta activa)
INNER JOIN
    Cuenta_empleado AS CE ON E.empleado_id = CE.empleado_id

-- 4. Vacaciones (Historial de vacaciones tomadas)
INNER JOIN
    Vacaciones AS V ON E.empleado_id = V.empleado_id

-- 5. Horario (Asignación de horario)
INNER JOIN
    Empleado_Horario AS EH ON E.empleado_id = EH.empleado_id
INNER JOIN
    Horario AS H ON EH.horario_id = H.horario_id

ORDER BY
    E.apellido, P.nombre, S.fecha_inicio DESC;
END;