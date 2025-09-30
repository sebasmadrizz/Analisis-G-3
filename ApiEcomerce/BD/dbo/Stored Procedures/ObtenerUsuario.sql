CREATE PROCEDURE [dbo].[ObtenerUsuario]
    @NombreUsuario      VARCHAR(MAX),
    @CorreoElectronico  VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        NombreUsuario,
        PasswordHash,
        CorreoElectronico,
        FechaCreacion,
        FechaModificacion,
        UsuarioCrea,
        UsuarioModifica,
        ESTADO_ID as IdEstado
    FROM dbo.Usuarios
    WHERE 
        (NombreUsuario = @NombreUsuario)
        OR
        (CorreoElectronico = @CorreoElectronico);
END
GO