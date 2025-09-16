CREATE PROCEDURE DESACTIVAR_CATEGORIA
  @IdCategoria UNIQUEIDENTIFIER,
    @EstadoId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.CATEGORIAS
    SET ESTADO_ID = @EstadoId
    WHERE CATEGORIAS_ID = @IdCategoria;

    IF EXISTS (SELECT 1 FROM dbo.CATEGORIAS WHERE PADRE_ID = @IdCategoria AND ESTADO_ID = 1)
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
        SET ESTADO_ID = @EstadoId
        WHERE CATEGORIAS_ID IN (SELECT CATEGORIAS_ID FROM HijasRecursivas);
    END

    SELECT @IdCategoria AS CategoriaDesactivada;
END