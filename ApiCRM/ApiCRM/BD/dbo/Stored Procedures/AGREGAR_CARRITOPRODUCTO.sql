CREATE PROCEDURE AGREGAR_CARRITOPRODUCTO
    @CarritoProductoId UNIQUEIDENTIFIER,
    @CarritoId UNIQUEIDENTIFIER,
    @ProductosId UNIQUEIDENTIFIER,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;

    DECLARE @PrecioUnitario DECIMAL(8,2);
    DECLARE @TotalLinea DECIMAL(10,2);
    DECLARE @CantidadActual INT;
    SELECT 
        @PrecioUnitario = PRECIO
      
    FROM PRODUCTOS
    WHERE PRODUCTOS_ID = @ProductosId;

    SET @TotalLinea = @Cantidad * @PrecioUnitario;

    IF EXISTS (
        SELECT 1 FROM CARRITO_PRODUCTO 
        WHERE CARRITO_ID = @CarritoId AND PRODUCTOS_ID = @ProductosId
    )
    BEGIN
        SELECT @CantidadActual = CANTIDAD
        FROM CARRITO_PRODUCTO
        WHERE CARRITO_ID = @CarritoId AND PRODUCTOS_ID = @ProductosId;
        UPDATE CARRITO_PRODUCTO
        SET 
            CANTIDAD = CANTIDAD + @Cantidad,
            TOTAL_LINEA = (CANTIDAD + @Cantidad) * @PrecioUnitario
        WHERE CARRITO_ID = @CarritoId AND PRODUCTOS_ID = @ProductosId;
    END
    ELSE
    BEGIN
        INSERT INTO CARRITO_PRODUCTO (
            CARRITO_PRODUCTO_ID,
            CARRITO_ID,
            CANTIDAD,
            TOTAL_LINEA,
            PRODUCTOS_ID
        )
        VALUES (
            @CarritoProductoId,
            @CarritoId,
            @Cantidad,
            @TotalLinea,
            @ProductosId
        );
    END

    COMMIT TRANSACTION;

    SELECT @CarritoProductoId;
END;