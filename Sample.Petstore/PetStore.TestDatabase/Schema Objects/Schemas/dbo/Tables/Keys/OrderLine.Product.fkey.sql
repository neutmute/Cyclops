ALTER TABLE [dbo].[OrderLine]
	ADD CONSTRAINT [FK_OrderLine_Product] 
	FOREIGN KEY (ProductId)
	REFERENCES Product (Id)	

