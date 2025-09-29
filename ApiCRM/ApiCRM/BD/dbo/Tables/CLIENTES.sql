CREATE TABLE [dbo].[Clientes]
(
    [ClienteId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [TipoCliente] NVARCHAR(50) NOT NULL,
    [Nombre] NVARCHAR(150) NOT NULL,
    [Identificacion] NVARCHAR(50) UNIQUE NOT NULL,
    [Correo] NVARCHAR(100) UNIQUE NOT NULL,
    [Telefono] NVARCHAR(20) NOT NULL,
    [Direccion] NVARCHAR(255) NOT NULL,
    [FechaCreacion] DATE NOT NULL,
    [FechaActualizacion] DATE NULL,
    [EstadoId] INT NOT NULL,

    CONSTRAINT [FK_Clientes_Estados] FOREIGN KEY ([EstadoId]) 
        REFERENCES [dbo].[Estados](ESTADO_ID)
);
