CREATE PROCEDURE [dbo].[AgregarUsuarioEmpleado]
    @NombreUsuario      VARCHAR(MAX),
    @PasswordHash       VARCHAR(MAX),
    @CorreoElectronico  VARCHAR(MAX),
    @Telefono VARCHAR(MAX),
    @Direccion  VARCHAR(MAX),
    @IdEstado int,
    @Apellido VARCHAR(MAX) 
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id AS UNIQUEIDENTIFIER = NEWID();

    BEGIN TRAN

    -- Insertar nuevo usuario
    INSERT INTO [dbo].[Usuarios] (
        [Id],
        [NombreUsuario],
        [PasswordHash],
        [CorreoElectronico],
        TELEFONO,
        DIRECCION,
        estado_id,
        APELLIDO
    )
    VALUES (
        @Id,
        @NombreUsuario,
        @PasswordHash,
        @CorreoElectronico,
        @Telefono,
        @Direccion,
        @IdEstado,
        @Apellido
    );

    
    INSERT INTO [dbo].[PerfilesxUsuario] (
        [IdUsuario],
        [IdPerfil]
    )
    VALUES (
        @Id,
        1
    );

    COMMIT TRAN
END
GO