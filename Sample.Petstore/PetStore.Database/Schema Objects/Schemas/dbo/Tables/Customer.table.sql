CREATE TABLE [dbo].[Customer]
(
	Id						int				NOT NULL IDENTITY(1,1),
	Title					nvarchar(8)		NULL,
	FirstName				nvarchar(80)	NOT NULL,
	LastName				nvarchar(80)	NOT NULL,
	EmailPromotion			int				NOT NULL,
	IsActive				bit				NOT NULL,
	IsDeleted				bit				NOT NULL,
	DateCreated				datetime		NOT NULL,
	CreatedBy				varchar(80)		NOT NULL,
	DateModified			datetime		NOT NULL,
	ModifiedBy				varchar(80)		NOT NULL
)
