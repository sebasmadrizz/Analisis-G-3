CREATE PROCEDURE [dbo].[ExisteCorreo]
    @CorreoElectronico VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CASE 
            WHEN EXISTS (
                SELECT 1 
                FROM dbo.Usuarios 
                WHERE CorreoElectronico = @CorreoElectronico
            ) 
            THEN CAST(1 AS BIT)  
            ELSE CAST(0 AS BIT)  
        END AS Existe;
END