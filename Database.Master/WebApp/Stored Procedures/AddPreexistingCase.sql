
-- =============================================
-- Author:		Edwin Torres
-- Description:	Create preexisting cases
-- =============================================
CREATE PROCEDURE [WebApp].[AddPreexistingCase]

	@CaseNumber1 NVARCHAR(15) 
	,@CaseNumber2 NVARCHAR(15)
	,@CreatedBy NVARCHAR(150)
AS


DECLARE @CaseId1 INT
DECLARE @CaseId2 INT

SELECT @CaseId1=CaseId
FROM [SiC].[Case]
WHERE CaseNumber = @CaseNumber1

SELECT @CaseId2=CaseId
FROM [SiC].[Case]
WHERE CaseNumber = @CaseNumber2


INSERT INTO [SiC].[CaseRelationship]
           ([CaseId1]
           ,[CaseNumber1]
           ,[CaseId2]
           ,[CaseNumber2]
           ,[Hidden]
           ,[CreatedBy]
           ,[CreatedDateTime]
           ,[ModifiedBy]
           ,[ModifiedDateTime])
     VALUES
           (@CaseId1
           ,@CaseNumber1
           ,@CaseId2
           ,@CaseNumber2
           ,0
           ,@CreatedBy
           ,GETDATE()
           ,NULL
           ,NULL)


