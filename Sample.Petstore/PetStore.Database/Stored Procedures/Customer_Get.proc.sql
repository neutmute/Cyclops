CREATE PROCEDURE dbo.Customer_Get
(
	@Id	INT = NULL
)
AS
BEGIN

	SET NOCOUNT ON
 
	CREATE TABLE #__CustomerLoad
	(
		SortOrder				INT IDENTITY(1,1)
		,CustomerId				INT
	)

	INSERT #__CustomerLoad
	(
		CustomerId
	)
	SELECT	Id
	FROM	dbo.Customer
	WHERE	Id = @Id 
	OR 		@Id IS NULL

	EXEC dbo.Customer_Load
			   
END
GO   
