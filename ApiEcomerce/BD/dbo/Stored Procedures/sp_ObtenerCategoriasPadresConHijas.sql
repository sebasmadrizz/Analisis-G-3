CREATE PROCEDURE sp_ObtenerCategoriasPadresConHijas
AS
BEGIN
    SET NOCOUNT ON;

    -- Traer padres con sus hijas en una sola consulta
    SELECT 
    p.Nombre      AS PadreNombre,
    p.Icono       AS PadreIcono,
    p.CATEGORIAS_ID AS PadreId,
    
    h.CATEGORIAS_ID AS HijaId,
    h.PADRE_ID      AS HijaPadreId,
    h.Nombre        AS HijaNombre,
    h.Descripcion   AS HijaDescripcion,
    h.Icono         AS HijaIcono,
    h.ESTADO_ID     AS HijaEstado

FROM CATEGORIAS p
LEFT JOIN CATEGORIAS h 
    ON h.PADRE_ID = p.CATEGORIAS_ID
    AND h.ESTADO_ID = 1  -- filtramos las hijas activas aquí
WHERE p.PADRE_ID IS NULL
  AND p.ESTADO_ID = 1;  -- Solo categorías raíz (padres)
END
GO
