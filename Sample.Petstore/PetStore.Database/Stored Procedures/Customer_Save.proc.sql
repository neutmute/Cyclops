﻿CREATE PROCEDURE dbo.Customer_Save
(
	@Id					INT OUTPUT
	,@Title				NVARCHAR(8)
	,@FirstName			NVARCHAR(80)
	,@LastName			NVARCHAR(80)
	,@EmailPromotion	INT
	,@IsActive			BIT
	,@IsDeleted			BIT			 = 0
	,@DateCreated		DATETIME
	,@CreatedBy			VARCHAR(80)
	,@DateModified		DATETIME
	,@ModifiedBy		VARCHAR(80)
	,@DateOfBirth		DATETIMEOFFSET = NULL		-- Testing mapping of DateTimeOffset
)
AS
BEGIN
	SET NOCOUNT ON
	
	IF NOT EXISTS(SELECT 1 FROM dbo.Customer WHERE ID = @ID)
	BEGIN
	
		INSERT dbo.Customer
		(
			Title
			,FirstName
			,LastName
			,EmailPromotion
			,IsActive
			,IsDeleted
			,DateCreated
			,CreatedBy
			,DateModified
			,ModifiedBy
			,DateOfBirth
		)
		VALUES
		(
			@Title
			,@FirstName
			,@LastName
			,@EmailPromotion
			,@IsActive
			,@IsDeleted
			,@DateCreated
			,@CreatedBy
			,@DateModified
			,@ModifiedBy
			,@DateOfBirth
		)	
		
		SET @Id = SCOPE_IDENTITY()
	
	END
	ELSE
	BEGIN
	
		UPDATE 	dbo.Customer
		SET		Title = @Title
				,FirstName = @FirstName
				,LastName = @LastName
				,EmailPromotion = @EmailPromotion
				,IsActive = @IsActive
				,IsDeleted = @IsDeleted
				,DateCreated = @DateCreated
				,CreatedBy = @CreatedBy
				,DateModified = @DateModified
				,ModifiedBy = @ModifiedBy
		WHERE	Id = @Id
	
	END
END
GO   
