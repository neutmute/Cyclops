CREATE PROCEDURE [dbo].DemoEnum
	@Param1 int = 0
	,@Param2 VARCHAR(100)
	,@ColourId INT = NULL
AS
	SELECT	@Param1
			,@Param2
RETURN 0
