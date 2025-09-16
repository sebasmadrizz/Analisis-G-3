CREATE PROCEDURE VER_PROVEEDORES
AS
BEGIN
  SET NOCOUNT ON;

  SELECT 
  PROVEEDOR_ID,
  Nombre_PROVEEDOR,
  Correo_ELECTRONICO,
  Telefono,
  Direccion,
  Nombre_Contacto,
  Fecha_Registro,
  TIPO,
  ESTADO_ID
  FROM Proveedores
  
   
END;