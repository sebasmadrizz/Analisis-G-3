CREATE PROCEDURE sp_ObtenerCategoriasPadresConHijas
AS
BEGIN
    SET NOCOUNT ON;

    -- Traer padres con sus hijas en una sola consulta
    SELECT 
        p.Nombre            AS PadreNombre,
        p.Icono             AS PadreIcono,
		p.CATEGORIAS_ID     AS PadreId,

        h.CATEGORIAS_ID      AS HijaId,
        h.PADRE_ID           AS HijaPadreId,
        h.Nombre            AS HijaNombre,
        h.Descripcion       AS HijaDescripcion,
        h.Icono             AS HijaIcono,
        h.ESTADO_ID            AS HijaEstado

    FROM CATEGORIAS p
    LEFT JOIN Categorias h 
        ON h.PADRE_ID = p.CATEGORIAS_ID
    WHERE p.PADRE_ID IS NULL;  -- Solo categorías raíz (padres)
END
GO
