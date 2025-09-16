CREATE PROCEDURE [dbo].[AgregarUsuario]
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

    -- Insertar relación con perfil 2
    INSERT INTO [dbo].[PerfilesxUsuario] (
        [IdUsuario],
        [IdPerfil]
    )
    VALUES (
        @Id,
        2
    );

    COMMIT TRAN
END