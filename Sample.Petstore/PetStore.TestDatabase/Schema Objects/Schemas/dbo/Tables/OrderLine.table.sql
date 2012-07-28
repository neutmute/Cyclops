CREATE TABLE [dbo].[OrderLine]
(
	Id					int				NOT NULL IDENTITY(1,1),
	OrderId				int				NOT NULL,
	ProductId			int				NOT NULL,
	OrderQty			int				NOT NULL,
	UnitPriceCents		int				NOT NULL,
	IsActive			bit				NOT NULL,
	IsDeleted			bit				NOT NULL,
	DateCreated			datetime2		NOT NULL,
	CreatedBy			varchar(80)		NOT NULL,
	DateModified		datetime2		NOT NULL,
	ModifiedBy			varchar(80)		NOT NULL
)
