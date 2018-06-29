


-- =============================================
-- Author:		Edwin Torres
-- Description:	Get cases related by case number
-- =============================================
CREATE PROCEDURE [WebApp].[GetRelatedCasesByCaseNumber]

	@CaseNumber NVARCHAR(15) = '20144806384'
AS


WITH Data AS (
SELECT	[CaseRelationshipId]
		,[CaseId1]
		,[CaseNumber1]
		,[CaseId2]
		,[CaseNumber2]
		,[Hidden]
FROM [SiC DB].[SiC].[CaseRelationship] R
WHERE CaseNumber1 = @CaseNumber 
UNION 
SELECT	[CaseRelationshipId]
		,[CaseId2]
		,[CaseNumber2]
		,[CaseId1]
		,[CaseNumber1]
		,[Hidden]
FROM [SiC DB].[SiC].[CaseRelationship] R
WHERE [CaseNumber2] = @CaseNumber 
)

SELECT DISTINCT [CaseNumber2]
FROM Data

