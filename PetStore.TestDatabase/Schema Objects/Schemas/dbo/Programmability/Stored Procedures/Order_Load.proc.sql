CREATE PROCEDURE dbo.Order_Load
AS
BEGIN
	
	IF (OBJECT_ID('tempdb..#__OrderLoad') IS NULL)
	BEGIN
		RAISERROR('Order_Load expects a temp table context', 15, 1)
		CREATE TABLE #__OrderLoad (ThisKillsCompilerWarning int)
	END

	CREATE TABLE #__CustomerLoad
	(
		SortOrder				INT IDENTITY(1,1)
		,CustomerId				INT
	)

	INSERT #__CustomerLoad
	(
		CustomerId
	)
	SELECT	CustomerId
	FROM	dbo.[Order]				O
	JOIN	dbo.#__OrderLoad		L ON O.Id = L.OrderId

	SELECT	Id
			,OrderDate
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
	FROM	dbo.[Order]				O
	JOIN	dbo.#__OrderLoad		L ON O.Id = L.OrderId
	ORDER BY	L.SortOrder

	EXEC dbo.Customer_Load
END