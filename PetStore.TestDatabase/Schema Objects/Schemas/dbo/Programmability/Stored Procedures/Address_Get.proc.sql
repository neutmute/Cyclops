CREATE PROCEDURE dbo.Address_Get
(
	@Id				INT = NULL
)
AS
BEGIN

	SET NOCOUNT ON

	SELECT	Id
			,AddressLine1
			,AddressLine2
			,City
			,[State]
			,PostalCode
			,IsActive
			,IsDeleted
			,DateCreatedUtc
			,CreatedBy
			,DateModifiedUtc
			,ModifiedBy
	FROM	dbo.[Address]			A
	WHERE	Id = @Id 
	OR 		@Id IS NULL

END
GO   
