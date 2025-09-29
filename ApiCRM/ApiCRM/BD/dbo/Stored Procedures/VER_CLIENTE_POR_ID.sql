CREATE PROCEDURE VER_CLIENTE_POR_ID
    @ClienteId UNIQUEIDENTIFIER
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
    FROM Clientes  
    WHERE ClienteId = @ClienteId;
END;
GO
