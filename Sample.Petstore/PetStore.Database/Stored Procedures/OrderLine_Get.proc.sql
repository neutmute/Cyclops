CREATE PROCEDURE dbo.OrderLine_Get
(
	@Id			INT	= NULL
	,@OrderId	INT	= NULL
)
AS
BEGIN

	SET NOCOUNT ON
 
	SELECT	Id
			,OrderId
			,ProductId
			,OrderQty
			,UnitPriceCents
			,IsActive
			,IsDeleted
			,DateCreated
			,CreatedBy
			,DateModified
			,ModifiedBy
	FROM	dbo.OrderLine
	WHERE	Id			= @Id 
	OR 		@OrderId	= @OrderId
			   
END
GO   
