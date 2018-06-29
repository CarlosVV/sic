

CREATE PROC [UnitTest].[RunUnitTests]
AS
BEGIN 
		
		TRUNCATE TABLE [UnitTest].[Results]
		
		
		DECLARE 
		 @DBName1		NVARCHAR(100) = 'SiC Loading'
		,@SchemaName1	NVARCHAR(100) = 'Inca'
		,@TableName1	NVARCHAR(100) = 'Prod'
		,@DBName2		NVARCHAR(100) = 'SiC DB'
		,@SchemaName2	NVARCHAR(100) = 'Inca'
		,@TableName2	NVARCHAR(100) = 'Master'
		INSERT INTO [UnitTest].[Results]
			(
				[TestName],
				[Result],
				[Count1], 
				[Count2], 
				[ModificationDate]
			)
		EXEC [UnitTest].[CountingRecords2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@DBName2
						,@SchemaName2
						,@TableName2
		

		SET @SchemaName1	= 'Check'
		SET @TableName1		= 'Cheques'
		SET @SchemaName2	= 'Check'
		SET @TableName2		= 'Check'				
		INSERT INTO [UnitTest].[Results]
			(
				[TestName],
				[Result],
				[Count1], 
				[Count2], 
				[ModificationDate]
			)
		EXEC [UnitTest].[CountingRecords2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@DBName2
						,@SchemaName2
						,@TableName2

	
		SET @SchemaName1	= 'Entity'
		SET @TableName1		= 'Entity'
		SET @SchemaName2	= 'Entity'
		SET @TableName2		= 'Entity'				
		INSERT INTO [UnitTest].[Results]
			(
				[TestName],
				[Result],
				[Count1], 
				[Count2], 
				[ModificationDate]
			)
		EXEC [UnitTest].[CountingRecords2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@DBName2
						,@SchemaName2
						,@TableName2


		DECLARE	@ColName1		NVARCHAR(100) = 'IncaID'
				,@ColName2		NVARCHAR(100) = 'MasterID'
		
		
		SET @SchemaName1	= 'Inca'
		SET @TableName1		= 'Prod' 
		SET @SchemaName2	= 'Inca'
		SET @TableName2		= 'Detail'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingDistinct2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2
		
		
		SET @SchemaName1	= 'Inca'
		SET @TableName1		= 'Prod' 
		SET @ColName1		= 'NUM_POLI'
		SET @SchemaName2	= 'Inca'
		SET @TableName2		= 'Master' 
		SET @ColName2		= 'PolicyNo'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2
		
		
		SET @ColName1		= 'IDENT'
		SET @ColName2		= 'ActiveIdentID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2
		
		
		SET @ColName1		= 'ONOFF'
		SET @ColName2		= 'ActiveOnOffID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2
		
		
		SET @ColName1		= 'CL_CANCEL'
		SET @ColName2		= 'CancellationID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2
		
		SET @ColName1		= 'CNC'
		SET @ColName2		= 'PaymentConceptID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2

		SET @SchemaName1	= 'Check'
		SET @TableName1		= 'Cheques'
		SET @ColName1		= 'ClasePago'
		SET @SchemaName2	= 'Check'
		SET @TableName2		= 'Check'
		SET @ColName2		= 'PaymentClassID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2
				

		SET @ColName1		= 'Status'
		SET @ColName2		= 'PaymentStatusID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2


		SET @ColName1		= 'Concepto'
		SET @ColName2		= 'PaymentConceptID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingNull2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2

		SET @TableName1		= 'Casos'
		SET @ColName1		= 'CasosID'
		SET @ColName2		= 'EntityID'
		INSERT INTO [UnitTest].[Results]
					(
						[TestName],
						[Result],
						[Count1], 
						[Count2], 
						[ModificationDate]
					)
		EXEC [UnitTest].[CountingDistinct2Tables]
						@DBName1
						,@SchemaName1
						,@TableName1
						,@ColName1
						,@DBName2
						,@SchemaName2
						,@TableName2
						,@ColName2
				

END