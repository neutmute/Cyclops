ALTER TABLE [dbo].[Customer]
   ADD CONSTRAINT [DF_Customer_IsDeleted] 
   DEFAULT 0
   FOR IsDeleted


