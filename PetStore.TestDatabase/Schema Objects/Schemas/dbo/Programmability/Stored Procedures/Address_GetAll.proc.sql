CREATE PROCEDURE [dbo].[Address_GetAll]
AS
SELECT
	[Id],
	[AddressLine1],
	[AddressLine2],
	[City],
	[State],
	[PostalCode],
	[IsActive]
FROM [Address]

RETURN 0