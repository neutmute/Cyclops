CREATE PROCEDURE dbo.Customer_Get
(
	@Id	INT = NULL
)
AS
BEGIN

	SET NOCOUNT ON
 
	SELECT	Id
			,Title
			,FirstName
			,LastName
			,EmailPromotion
			,IsActive
			,IsDeleted
			,DateCreated
			,CreatedBy
			,DateModified
			,ModifiedBy
	FROM	dbo.Customer
	WHERE	Id = @Id 
	OR 		@Id IS NULL
			   
END
GO   
