CREATE PROCEDURE [dbo].CONTAR_HIJAS_TOTALES
    @IdCategoria UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CantidadHijas INT = 0;
    DECLARE @PadreId UNIQUEIDENTIFIER = NULL;
    DECLARE @PadreActivo BIT = 0;
    SELECT @CantidadHijas = COUNT(*)
    FROM dbo.CATEGORIAS
    WHERE PADRE_ID = @IdCategoria;

    SELECT @PadreId = PADRE_ID
    FROM dbo.CATEGORIAS
    WHERE CATEGORIAS_ID = @IdCategoria;
    IF @PadreId IS NOT NULL
    BEGIN
        SELECT @PadreActivo = CASE WHEN ESTADO_ID = 1 THEN 1 ELSE 0 END
        FROM dbo.CATEGORIAS
        WHERE CATEGORIAS_ID = @PadreId;
    END

    SELECT
        @IdCategoria AS IdCategoria,
        CASE WHEN @CantidadHijas > 0 THEN 1 ELSE 0 END AS EsPadre,
        @CantidadHijas AS CantidadHijas,
        @PadreId AS PadreId,
        @PadreActivo AS PadreActivo;
END