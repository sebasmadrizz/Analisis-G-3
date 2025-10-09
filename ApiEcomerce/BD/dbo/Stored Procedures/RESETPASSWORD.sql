CREATE PROCEDURE RESETPASSWORD
    @Email NVARCHAR(255),
    @Password NVARCHAR(255),
    @Token NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DECLARE @UserId UNIQUEIDENTIFIER;

    -- 1. Verificar token
    SELECT @UserId = UserId
    FROM ResetPasswordTokens t
    INNER JOIN USUARIOS u ON t.UserId = u.Id
    WHERE t.TokenHash = @Token
      AND u.CorreoElectronico = @Email
      AND t.Usado = 0
      AND t.ExpiraEn > GETDATE();

    -- Si no encuentra token válido
    IF @UserId IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT 0 AS Exito;
        RETURN; -- Token inválido, expirado o ya usado
    END

    -- 2. Actualizar contraseña del usuario
    UPDATE USUARIOS
    SET PasswordHash = @Password
    WHERE Id = @UserId;

    -- 3. Marcar token como usado
    UPDATE ResetPasswordTokens
    SET Usado = 1
    WHERE TokenHash = @Token;

    COMMIT TRANSACTION;

    SELECT 1 AS Exito; -- Éxito
END;

