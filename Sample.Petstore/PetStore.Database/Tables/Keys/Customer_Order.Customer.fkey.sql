ALTER TABLE [dbo].[Customer_Order]
	ADD CONSTRAINT [FK_Customer_Order_Customer] 
	FOREIGN KEY (CustomerId)
	REFERENCES Customer (Id)	

