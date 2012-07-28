CREATE TABLE [dbo].[Product]
(
	Id					int				NOT NULL IDENTITY(1,1),
	Name				nvarchar(60)	NOT NULL,
	Sku					nvarchar(60)	NOT NULL,
	IsActive			bit				NOT NULL,
	IsDeleted			bit				NOT NULL,
	DateCreated			datetime		NOT NULL,
	CreatedBy			varchar(80)		NOT NULL,
	DateModified		datetime		NOT NULL,
	ModifiedBy			varchar(80)		NOT NULL
)
