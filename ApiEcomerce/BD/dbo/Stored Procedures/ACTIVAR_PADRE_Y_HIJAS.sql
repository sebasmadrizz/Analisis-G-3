CREATE PROCEDURE [dbo].[ACTIVAR_PADRE_Y_HIJAS]
    @IdCategoria UNIQUEIDENTIFIER,
    @ActivarHijas BIT 
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.CATEGORIAS
    SET ESTADO_ID = 1  
    WHERE CATEGORIAS_ID = @IdCategoria;
    IF @ActivarHijas = 1
    BEGIN
        ;WITH HijasRecursivas AS
        (
            SELECT CATEGORIAS_ID
            FROM dbo.CATEGORIAS
            WHERE PADRE_ID = @IdCategoria

            UNION ALL

            SELECT c.CATEGORIAS_ID
            FROM dbo.CATEGORIAS c
            INNER JOIN HijasRecursivas h ON c.PADRE_ID = h.CATEGORIAS_ID
        )
        UPDATE dbo.CATEGORIAS
        SET ESTADO_ID = 1
        WHERE CATEGORIAS_ID IN (SELECT CATEGORIAS_ID FROM HijasRecursivas);
    END

    SELECT @IdCategoria AS CategoriaActivada;
END
