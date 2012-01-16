CREATE TABLE [dbo].[Order]
(
	Id					int				NOT NULL IDENTITY(1,1),
	OrderDate			datetime		NOT NULL,
	ShipDate			datetime		NULL,
	CustomerId			int				NOT NULL,
	BillToAddressId		int				NOT NULL,
	ShipToAddressId		int				NOT NULL,
	IsActive			bit				NOT NULL,
	IsDeleted			bit				NOT NULL,
	DateCreated			datetime		NOT NULL,
	CreatedBy			varchar(80)		NOT NULL,
	DateModified		datetime		NOT NULL,
	ModifiedBy			varchar(80)		NOT NULL
)
