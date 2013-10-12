ALTER TABLE [dbo].[Customer_Order]
	ADD CONSTRAINT [FK_Customer_Order_Order] 
	FOREIGN KEY (OrderId)
	REFERENCES [Order] (Id)	
