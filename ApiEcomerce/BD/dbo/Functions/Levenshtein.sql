CREATE FUNCTION [dbo].[Levenshtein]
(@s NVARCHAR (MAX) NULL, @t NVARCHAR (MAX) NULL)
RETURNS INT
AS
 EXTERNAL NAME [SQLCLRFunc].[UserDefinedFunctions].[Levenshtein]

