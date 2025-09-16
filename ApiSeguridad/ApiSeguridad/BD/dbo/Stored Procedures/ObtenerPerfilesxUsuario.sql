CREATE PROCEDURE [dbo].[ObtenerPerfilesxUsuario]
    @NombreUsuario      VARCHAR(MAX),
    @CorreoElectronico  VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.Id ,
        p.Nombre,
        p.FechaCreacion,
        p.FechaModificacion
    FROM dbo.Usuarios u
    INNER JOIN dbo.PerfilesxUsuario pxu ON u.Id = pxu.IdUsuario
    INNER JOIN dbo.Perfiles p ON pxu.IdPerfil = p.Id
    WHERE 
        (u.NombreUsuario = @NombreUsuario)
        OR
        (u.CorreoElectronico = @CorreoElectronico);
END