
CREATE FUNCTION dbo.TestFunc(@x int)
RETURNS int
AS
BEGIN
    RETURN @x + 1;
END