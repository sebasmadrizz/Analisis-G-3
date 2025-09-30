	CREATE TABLE ResetPasswordTokens (
    Id uniqueidentifier  PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    TokenHash NVARCHAR(64) NOT NULL,
    ExpiraEn DATETIME NOT NULL,
    Usado BIT NOT NULL DEFAULT 0
);