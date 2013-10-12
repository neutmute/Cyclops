
ALTER TABLE [dbo].[Address]
   ADD CONSTRAINT [DF_Address_IsDeleted] 
   DEFAULT 0
   FOR IsDeleted 