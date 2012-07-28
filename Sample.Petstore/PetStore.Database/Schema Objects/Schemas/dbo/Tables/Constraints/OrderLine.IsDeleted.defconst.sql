ALTER TABLE [dbo].[OrderLine]
   ADD CONSTRAINT [DF_Orderline_IsDeleted] 
   DEFAULT 0
   FOR IsDeleted


