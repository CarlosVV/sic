


-- =============================================
-- Author:		Edwin Torres
-- Description:	Create preexisting cases
-- =============================================
CREATE PROCEDURE [WebApp].[RemovePreexistingCase]

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


DELETE [SiC].[CaseRelationship]
WHERE (CaseId1 = @CaseId1 AND CaseId2 = @CaseId2) OR (CaseId1 = @CaseId2 AND CaseId2 = @CaseId1)


