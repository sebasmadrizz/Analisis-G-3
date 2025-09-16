CREATE PROCEDURE sObtenerDatosCarritoPorUsuario
    @UsuarioId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
       u.Id              AS UsuarioId,
    u.NombreUsuario   AS NombreUsuario,
    u.Apellido        AS Apellido,
    u.CorreoElectronico AS CorreoElectronico,
    u.Telefono        AS Telefono,
    u.Direccion       AS Direccion,

    c.CARRITO_ID      AS CarritoId,
    c.Fecha_Creacion  AS FechaCreacion,
    c.Total           AS TotalCarrito,

    p.PRODUCTOS_ID    AS ProductosId,
    p.Nombre          AS NombreProducto,
    p.Marca           AS Marca,
    p.Precio          AS Precio,
    cp.Cantidad       AS Cantidad,
    cp.Total_Linea    AS TotalLinea
    FROM CARRITO c
    INNER JOIN Usuarios u 
        ON u.Id = c.Usuario_Id
    INNER JOIN CARRITO_PRODUCTO cp 
        ON cp.CARRITO_ID = c.CARRITO_ID
    INNER JOIN PRODUCTOS p 
        ON p.PRODUCTOS_ID = cp.PRODUCTOS_ID
    WHERE c.Usuario_Id = @UsuarioId
    ORDER BY c.Fecha_Creacion DESC;
END