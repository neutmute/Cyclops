CREATE PROCEDURE dbo.Customer_Load
AS
BEGIN

	SET NOCOUNT ON

	IF (OBJECT_ID('tempdb..#__CustomerLoad') IS NULL)
	BEGIN
		RAISERROR('Customer_Load expects a temp table context', 15, 1)
		CREATE TABLE #__CustomerLoad (ThisKillsCompilerWarning int)
	END

	SELECT 'Customer'

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
	FROM	dbo.Customer			C
	JOIN	dbo.#__CustomerLoad		L ON C.Id = L.CustomerId
	ORDER BY	L.SortOrder
	
END
GO   
