CREATE PROCEDURE CambiarContrasena
  @NuevaContraseña VARCHAR(255),
  @Correo VARCHAR(255)
 
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  UPDATE [dbo].[Usuarios]
  SET
    [PasswordHash]=@NuevaContraseña
  WHERE CorreoElectronico = @Correo;

  COMMIT TRANSACTION;

  SELECT NEWID() AS Resultado; 
END;