CREATE PROCEDURE ESTADO_CLIENTE
  @ClienteId UNIQUEIDENTIFIER
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  UPDATE Clientes
  SET EstadoId = CASE 
                     WHEN EstadoId = 1 THEN 2
                     WHEN EstadoId = 2 THEN 1
                     ELSE EstadoId 
                 END
  WHERE ClienteId = @ClienteId;  

  COMMIT TRANSACTION;

  SELECT @ClienteId AS ClienteId;
END
GO
