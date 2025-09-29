CREATE PROCEDURE AGREGAR_CLIENTE
    @ClienteId UNIQUEIDENTIFIER,
    @TipoCliente NVARCHAR(50),
    @Nombre NVARCHAR(150),
    @Identificacion NVARCHAR(50),
    @Correo NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Direccion NVARCHAR(255),
    @FechaCreacion DATE,
    @FechaActualizacion DATE,
    @EstadoId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Clientes
    (
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
    )
    VALUES
    (
        @ClienteId,
        @TipoCliente,
        @Nombre,
        @Identificacion,
        @Correo,
        @Telefono,
        @Direccion,
        @FechaCreacion,
        @FechaActualizacion,
        @EstadoId
    );

    SELECT @ClienteId;
END;
GO
