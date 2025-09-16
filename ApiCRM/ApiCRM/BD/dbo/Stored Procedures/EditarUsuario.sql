CREATE PROCEDURE EditarUsuario
  @IdUsuario UNIQUEIDENTIFIER,
  @NombreUsuario NVARCHAR(255),
  @CorreoElectronico NVARCHAR(255),
  @Telefono NVARCHAR(255),
  @Direccion NVARCHAR(255),
  @Apellido NVARCHAR(255)  
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  UPDATE [dbo].[Usuarios]
  SET
    NombreUsuario = @NombreUsuario,
    CorreoElectronico = @CorreoElectronico,
    TELEFONO = @Telefono,
    DIRECCION = @Direccion,
    APELLIDO = @Apellido
  WHERE Id = @IdUsuario;

  COMMIT TRANSACTION;

  SELECT @IdUsuario AS Resultado;
END;