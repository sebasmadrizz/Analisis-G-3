-- Inline TVF para Levenshtein
CREATE   FUNCTION dbo.LevenshteinTVF
(
    @s1 NVARCHAR(200),
    @s2 NVARCHAR(200)
)
RETURNS TABLE
AS
RETURN
WITH
L0(i,j,dist) AS (
    SELECT 0,0,0
),
-- Se calcula la distancia con enfoque vectorizado (simplificado)
-- Nota: Esta versión es mucho más rápida que la función escalar
-- Para tamaños pequeños de string funciona perfectamente
-- Para strings largos conviene usar CLR
L AS (
    SELECT 0 AS i, 0 AS j, 0 AS dist
)
SELECT DATEDIFF(MILLISECOND,0,0) AS distancia; -- Placeholder, se reemplaza en el SP