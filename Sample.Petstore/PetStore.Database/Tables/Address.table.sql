CREATE TABLE [dbo].[Address]
(
	Id					int				NOT NULL IDENTITY(1,1),
	AddressLine1		nvarchar(60)	NOT NULL,
	AddressLine2		nvarchar(60)	NULL,
	County				nvarchar(60)	NULL,
	City				nvarchar(30)	NOT NULL,
	[State]				int				NOT NULL,
	PostalCode			nvarchar(15)	NOT NULL,
	IsActive			bit				NOT NULL,
	IsDeleted			bit				NOT NULL,
	DateCreatedUtc		datetime		NOT NULL,
	CreatedBy			varchar(80)		NOT NULL,
	DateModifiedUtc		datetime		NOT NULL,
	ModifiedBy			varchar(80)		NOT NULL
)
