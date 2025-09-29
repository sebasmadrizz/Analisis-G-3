CREATE PROCEDURE VER_CLIENTES
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ClienteId,
        TipoCliente,
        Nombre,
        Identificacion,
        Correo,
        Telefono,
        Direccion,
        FechaCreacion,
        FechaActualizacion,
        EstadoId   
    FROM Clientes;
END;
GO
