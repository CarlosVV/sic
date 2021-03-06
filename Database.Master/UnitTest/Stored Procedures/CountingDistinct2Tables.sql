﻿



CREATE PROC [UnitTest].[CountingDistinct2Tables]
 
@DBName1		NVARCHAR(100) = 'SiC Loading'
,@SchemaName1	NVARCHAR(100) = 'Inca'
,@TableName1	NVARCHAR(100) = 'Prod'
,@ColName1		NVARCHAR(100) = 'IncaID'
,@DBName2		NVARCHAR(100) = 'SiC DB'
,@SchemaName2	NVARCHAR(100) = 'Inca'
,@TableName2	NVARCHAR(100) = 'Detail'
,@ColName2		NVARCHAR(100) = 'MasterID'
AS 

DECLARE @Result NVARCHAR(30) = ''
,@Count1		INT	= 0
,@Count2		INT	= 0
,@SourceID		INT = 1
,@Query1		NVARCHAR(1000) = ''
,@Query2		NVARCHAR(1000) = ''

SET @Query1 =  ('
					SELECT @Count = COUNT(DISTINCT '+@ColName1+')
					FROM ['+@DBName1+'].['+@SchemaName1+'].['+@TableName1+']
')

EXEC sp_executesql
	 @Query1
	,N'@Count INT output'
	,@Count1 output

SET @Query2 =  ('
					SELECT @Count = COUNT(DISTINCT '+@ColName2+')
					FROM ['+@DBName2+'].['+@SchemaName2+'].['+@TableName2+']
')

EXEC sp_executesql
	 @Query2
	,N'@Count INT output'
	,@Count2 output


IF @Count1 = @Count2
	BEGIN 
		SET @Result = 'OK'
	END
ELSE
	BEGIN
		SET @Result = 'FAIL'
	END
	
 

SET @Query1 = '
		SELECT	''Testing Distinct Count of  Records: ['+@DBName1+'].['+@SchemaName1+'].['+@TableName1+'].['+@ColName1+'] vs ['+@DBName2+'].['+@SchemaName2+'].['+@TableName2+'].['+@ColName2+']'' AS Unit_Test_Name
				, '''+ @Result								+'''	AS Result
				, '''+ CAST(@Count1 AS NVARCHAR(10))			+'''	AS '+REPLACE('Count_'+@DBName1+'_'+@SchemaName1+'_'+@TableName1+'_'+@ColName1,' ','_')+'
				, '''+ CAST(@Count2 AS NVARCHAR(10))			+'''	AS '+REPLACE('Count_'+@DBName2+'_'+@SchemaName2+'_'+@TableName2+'_'+@ColName2,' ','_')+'
				,GETDATE() AS ModificationDate

	'

EXEC (@Query1)