CREATE TABLE dbo.Customer
(
	Id						int				NOT NULL IDENTITY(1,1),
	Title					nvarchar(8)		NULL,
	FirstName				nvarchar(80)	NOT NULL,
	LastName				nvarchar(80)	NOT NULL,
	EmailPromotion			int				NULL,
	IsActive				bit				NOT NULL CONSTRAINT DF_Customer_IsActive	DEFAULT(1),
	IsDeleted				bit				NOT NULL CONSTRAINT DF_Customer_IsDeleted	DEFAULT(0),
	DateCreated				datetimeoffset	NOT NULL CONSTRAINT DF_Customer_DateCreated	DEFAULT(SYSDATETIMEOFFSET()),
	CreatedBy				varchar(80)		NOT NULL,
	DateModified			datetime		NULL,
	ModifiedBy				varchar(80)		NULL,
	DateOfBirth				DATETIMEOFFSET  NULL
)
