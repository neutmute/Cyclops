CREATE PROCEDURE dbo.Customer_Delete
(
	@Id				INT 
	,@IsDeleted		BIT
)
AS
BEGIN
	SET NOCOUNT ON
	
	IF (@IsDeleted = 1)
	BEGIN
		RAISERROR('You cannot delete an already deleted customer', 15, 1)
	END
END
