CREATE PROCEDURE VER_EMPLEADOS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
       E.IdEmpleado,
       E.Cedula,
        E.NombreCompleto,
        E.Telefono,
        E.CorreoElectronico,
        E.Puesto,
        E.Padecimientos,
        E.CuentaBancaria,
        E.TipoContrato,
        E.Jornada,
        E.FechaIngreso,
        E.ESTADO_ID
    FROM EMPLEADOS E
    INNER JOIN ESTADOS ES ON E.ESTADO_ID = ES.ESTADO_ID
END;