ALTER TABLE [dbo].[Order]
	ADD CONSTRAINT [FK_Order_ShiptoAddress] 
	FOREIGN KEY (ShipToAddressId)
	REFERENCES [Address] (Id)	

