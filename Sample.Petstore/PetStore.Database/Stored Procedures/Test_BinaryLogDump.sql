CREATE PROCEDURE dbo.Test_BinaryLogDump
	@Input BinaryTableType READONLY
AS
BEGIN
	DECLARE @Numerator		INT
			,@Denominator	INT

	SELECT	@Numerator = 1
			,@Denominator = 0 

	SELECT @Numerator / @Denominator
END
	
