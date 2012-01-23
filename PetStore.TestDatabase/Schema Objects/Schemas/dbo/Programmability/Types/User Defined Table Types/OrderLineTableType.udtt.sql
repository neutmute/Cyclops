-- Required for a Table valued parameter sp call
CREATE TYPE dbo.OrderLineTableType AS TABLE 
(
	Id					INT				
	,OrderId			INT				
	,ProductId			INT				
	,OrderQty			INT				
	,UnitPriceCents		INT				
	,IsActive			BIT				
	,IsDeleted			BIT				
	,DateCreated		DATETIME2		
	,CreatedBy			VARCHAR(80)		
	,DateModified		DATETIME2		
	,ModifiedBy			VARCHAR(80)		
)