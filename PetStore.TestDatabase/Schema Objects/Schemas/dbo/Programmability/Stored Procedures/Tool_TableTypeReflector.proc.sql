-- Lazy way of constructing a data table in the image of the table type
-- eg: Tool_TableTypeReflector 'OrderlineTableType'
CREATE PROCEDURE Tool_TableTypeReflector
	@TableName VARCHAR(100)
AS
BEGIN
	DECLARE @Sql VARCHAR(500) = 'DECLARE @target ' + @TableName + '; SELECT * FROM @target'
	EXECUTE(@Sql)
END