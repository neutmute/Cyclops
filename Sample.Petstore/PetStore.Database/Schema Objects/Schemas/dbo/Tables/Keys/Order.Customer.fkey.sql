ALTER TABLE [dbo].[Order]
	ADD CONSTRAINT [FK_Order_Customer] 
	FOREIGN KEY (CustomerId)
	REFERENCES Customer (Id)	

