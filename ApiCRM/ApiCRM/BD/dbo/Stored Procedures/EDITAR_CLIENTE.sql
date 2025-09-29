CREATE PROCEDURE EDITAR_CLIENTE
    @ClienteId UNIQUEIDENTIFIER,
    @TipoCliente NVARCHAR(50),
    @Nombre NVARCHAR(150),
    @Identificacion NVARCHAR(50),
    @Correo NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Direccion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Clientes
    SET 
        TipoCliente = @TipoCliente,
        Nombre = @Nombre,
        Identificacion = @Identificacion,
        Correo = @Correo,
        Telefono = @Telefono,
        Direccion = @Direccion,
        FechaActualizacion = GETDATE()
    WHERE ClienteId = @ClienteId;

    SELECT @ClienteId AS ClienteId;
END
GO
