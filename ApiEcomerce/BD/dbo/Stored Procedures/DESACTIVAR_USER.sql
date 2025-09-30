CREATE PROCEDURE [dbo].[DESACTIVAR_USER]
    @idUsuario uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET ESTADO_ID = 2
    WHERE Id = @idUsuario;

END