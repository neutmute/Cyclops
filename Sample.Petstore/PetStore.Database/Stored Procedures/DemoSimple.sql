CREATE PROCEDURE [dbo].[DemoSimple]
	@Param1 int = 0
	,@Param2 VARCHAR(100)
AS
	SELECT	@Param1
			,@Param2
RETURN 0
