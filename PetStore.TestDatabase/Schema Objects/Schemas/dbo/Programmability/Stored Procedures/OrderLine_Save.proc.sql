CREATE PROCEDURE dbo.OrderLine_Save
(
	@Id					INT OUTPUT
	,@OrderId			INT
	,@ProductId			INT
	,@OrderQty			INT
	,@UnitPriceCents	INT
	,@IsActive			BIT
	,@IsDeleted			BIT
	,@DateCreated		DATETIME
	,@CreatedBy			VARCHAR(80)
	,@DateModified		DATETIME
	,@ModifiedBy		VARCHAR(80)
)
AS
BEGIN
	SET NOCOUNT ON
	
	IF NOT EXISTS(SELECT 1 FROM dbo.OrderLine WHERE ID = @ID)
	BEGIN
	
		INSERT dbo.OrderLine
		(
			OrderId
			,ProductId
			,OrderQty
			,UnitPriceCents
			,IsActive
			,IsDeleted
			,DateCreated
			,CreatedBy
			,DateModified
			,ModifiedBy
		)
		VALUES
		(
			@OrderId
			,@ProductId
			,@OrderQty
			,@UnitPriceCents
			,@IsActive
			,@IsDeleted
			,@DateCreated
			,@CreatedBy
			,@DateModified
			,@ModifiedBy
		)	
		
		SET @Id = SCOPE_IDENTITY()
	
	END
	ELSE
	BEGIN
	
		UPDATE 	dbo.OrderLine
		SET		OrderId = @OrderId
				,ProductId = @ProductId
				,OrderQty = @OrderQty
				,UnitPriceCents = @UnitPriceCents
				,IsActive = @IsActive
				,IsDeleted = @IsDeleted
				,DateCreated = @DateCreated
				,CreatedBy = @CreatedBy
				,DateModified = @DateModified
				,ModifiedBy = @ModifiedBy
		WHERE	Id = @Id
	
	END
END
GO   
