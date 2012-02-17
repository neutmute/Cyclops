CREATE PROCEDURE dbo.Order_Save
(
	@Id					INT OUTPUT
	,@OrderDate			DATETIME2
	,@ShipDate			DATETIME2
	,@CustomerId		INT
	,@BillToAddressId	INT
	,@ShipToAddressId	INT
	,@IsActive			BIT
	,@IsDeleted			BIT
	,@DateCreated		DATETIME2
	,@CreatedBy			VARCHAR(80)
	,@DateModified		DATETIME2
	,@ModifiedBy		VARCHAR(80)

	,@Lines				OrderLineTableType READONLY
)
AS
BEGIN
	SET NOCOUNT ON

	-- For testing exception handling
	IF (@CustomerId = -1)
	BEGIN
		RAISERROR('dbo.Order_Save didn''t expect a @CustomerId of -1', 16, 1)		
		RETURN
	END
	
	IF NOT EXISTS(SELECT 1 FROM dbo.[Order] WHERE ID = @ID)
	BEGIN
	
		INSERT dbo.[Order]
		(
			OrderDate
			,ShipDate
			,CustomerId
			,BillToAddressId
			,ShipToAddressId
			,IsActive
			,IsDeleted
			,DateCreated
			,CreatedBy
			,DateModified
			,ModifiedBy
		)
		VALUES
		(
			@OrderDate
			,@ShipDate
			,@CustomerId
			,@BillToAddressId
			,@ShipToAddressId
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
	
		UPDATE 	dbo.[Order]
		SET		OrderDate = @OrderDate
				,ShipDate = @ShipDate
				,CustomerId = @CustomerId
				,BillToAddressId = @BillToAddressId
				,ShipToAddressId = @ShipToAddressId
				,IsActive = @IsActive
				,IsDeleted = @IsDeleted
				,DateCreated = @DateCreated
				,CreatedBy = @CreatedBy
				,DateModified = @DateModified
				,ModifiedBy = @ModifiedBy
		WHERE	Id = @Id
	
	END

	UPDATE	OL
	SET		OrderId				= @Id
			,ProductId			= T.ProductId
			,OrderQty			= T.OrderQty
			,UnitPriceCents		= T.UnitPriceCents
			,IsActive			= T.IsActive
			,IsDeleted			= T.IsDeleted
			,DateCreated		= T.DateCreated
			,CreatedBy			= T.CreatedBy
			,DateModified		= T.DateModified
			,ModifiedBy			= T.ModifiedBy
	FROM	@Lines				T
	JOIN	dbo.OrderLine		OL ON T.Id = OL.Id
	WHERE	T.Id > 0

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
	SELECT	@Id
			,ProductId
			,OrderQty
			,UnitPriceCents
			,IsActive
			,IsDeleted
			,DateCreated
			,CreatedBy
			,DateModified
			,ModifiedBy
	FROM	@Lines
	WHERE	Id = 0
END
GO   
