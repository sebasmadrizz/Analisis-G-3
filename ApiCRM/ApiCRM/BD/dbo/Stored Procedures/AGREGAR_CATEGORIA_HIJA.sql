CREATE PROCEDURE AGREGAR_CATEGORIA_HIJA
  @IdCategoria UNIQUEIDENTIFIER,
  @IdCategoriaPadre UNIQUEIDENTIFIER,
  @Nombre NVARCHAR(255),
  @Descripcion NVARCHAR(255),
  @FechaCreacion DATE,
  @EstadoId INT
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRANSACTION;

  INSERT INTO [dbo].[CATEGORIAS] (
    CATEGORIAS_ID,
    PADRE_ID,
    NOMBRE,
    FECHA_CREACION,
    DESCRIPCION,
    ESTADO_ID
  )
  VALUES (
    @IdCategoria,
    @IdCategoriaPadre, 
    @Nombre,
    @FechaCreacion,
    @Descripcion,
    @EstadoId
  );

  COMMIT TRANSACTION;

  SELECT @IdCategoria;
END;