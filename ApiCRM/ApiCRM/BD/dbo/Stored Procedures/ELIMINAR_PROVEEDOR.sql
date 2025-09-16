CREATE PROCEDURE ESTADO_PROVEEDOR
  @IdProveedor UNIQUEIDENTIFIER
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;
  
  UPDATE [dbo].[PROVEEDORES]
  SET ESTADO_ID = CASE 
                    WHEN ESTADO_ID = 1 THEN 2
                    WHEN ESTADO_ID = 2 THEN 1
                    ELSE ESTADO_ID 
                  END
  WHERE PROVEEDOR_ID = @IdProveedor;

  COMMIT TRANSACTION;

  SELECT @IdProveedor AS ProveedorModificado;
END;