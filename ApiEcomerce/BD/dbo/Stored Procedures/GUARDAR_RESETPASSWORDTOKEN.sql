CREATE PROCEDURE GUARDAR_RESETPASSWORDTOKEN
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @TokenHash NVARCHAR(255),
    @ExpiraEn DATETIME,
    @Usado BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    INSERT INTO [dbo].ResetPasswordTokens (
        Id,
        UserId,
        TokenHash,
        ExpiraEn,
        Usado
    )
    VALUES (
        @Id,
        @UserId,
        @TokenHash,
        @ExpiraEn,
        @Usado
    );

    COMMIT TRANSACTION;

    -- Retorna el Id del token insertado
    SELECT 1 AS Exito;
END;

