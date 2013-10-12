CREATE PROCEDURE [dbo].[DemoSimple]
	@Param1 int = -1
	,@Param2 VARCHAR(100) = 'DEFAULT VALUE'
AS
	SELECT	@Param1		AS Param1	
			,@Param2	AS Param2
RETURN 0
