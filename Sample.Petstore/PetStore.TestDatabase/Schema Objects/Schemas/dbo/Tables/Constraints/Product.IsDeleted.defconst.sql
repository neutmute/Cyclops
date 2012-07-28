ALTER TABLE [dbo].[Product]
   ADD CONSTRAINT [DF_Product_IsDeleted] 
   DEFAULT 0
   FOR IsDeleted


