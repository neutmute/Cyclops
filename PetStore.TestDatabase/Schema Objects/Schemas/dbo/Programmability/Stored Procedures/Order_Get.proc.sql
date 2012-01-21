CREATE PROCEDURE dbo.Order_Get
(
	@Id						INT = NULL
)
AS
BEGIN

	SET NOCOUNT ON

	CREATE TABLE #__OrderLoad
	(
		SortOrder				INT IDENTITY(1,1)
		,OrderId				INT
	)
 
	INSERT #__OrderLoad
	(
		OrderId
	)
	SELECT	Id
	FROM	dbo.[Order]
	WHERE	Id = @Id
	OR 		@Id IS NULL

	EXEC dbo.Order_Load
		   
END
GO   
