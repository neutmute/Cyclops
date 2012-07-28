CREATE TABLE [dbo].[Order]
(
	Id					int				NOT NULL IDENTITY(1,1),
	OrderDate			datetime2		NOT NULL,
	StatusId			TINYINT			NULL,		
	ShipDate			datetime2		NULL,
	CustomerId			int				NOT NULL,
	BillToAddressId		int				NOT NULL,
	ShipToAddressId		int				NOT NULL,
	IsActive			bit				NOT NULL,
	IsDeleted			bit				NOT NULL,
	DateCreated			datetime2		NOT NULL,
	CreatedBy			varchar(80)		NOT NULL,
	DateModified		datetime2		NOT NULL,
	ModifiedBy			varchar(80)		NOT NULL
)
