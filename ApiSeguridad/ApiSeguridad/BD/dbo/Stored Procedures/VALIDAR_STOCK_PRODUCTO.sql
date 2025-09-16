CREATE PROCEDURE VALIDAR_STOCK_PRODUCTO

    @ProductoId UNIQUEIDENTIFIER,
    @CantidadSolicitada INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StockDisponible INT;

   
    SELECT @StockDisponible = STOCK
    FROM PRODUCTOS
    WHERE PRODUCTOS_ID = @ProductoId;


    IF @CantidadSolicitada <= @StockDisponible
        SELECT CAST(1 AS BIT) AS Resultado; 
    ELSE
        SELECT CAST(0 AS BIT) AS Resultado; 
END;