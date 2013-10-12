ALTER TABLE [dbo].[Order]
	ADD CONSTRAINT [FK_Order_BilltoAddress] 
	FOREIGN KEY (BillToAddressId)
	REFERENCES [Address] (Id)	

