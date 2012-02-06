CREATE PROCEDURE dbo.OrderLine_Load
AS
BEGIN

	SET NOCOUNT ON

	IF (OBJECT_ID('tempdb..#__OrderLineLoad') IS NULL)
	BEGIN
		RAISERROR('dbo.OrderLine_Load expects a temp table context', 15, 1)
		CREATE TABLE #__OrderLineLoad (ThisKillsCompilerWarning int)
	END

	SELECT 'OrderLine'

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
	FROM	dbo.OrderLine					S
	JOIN	dbo.#__OrderLineLoad	L ON S.Id = L.OrderLineId
	ORDER BY L.SortOrder
	
END
GO   
