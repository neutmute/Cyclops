ALTER TABLE [dbo].[Order]
   ADD CONSTRAINT [DF_Order_IsDeleted] 
   DEFAULT 0
   FOR IsDeleted


