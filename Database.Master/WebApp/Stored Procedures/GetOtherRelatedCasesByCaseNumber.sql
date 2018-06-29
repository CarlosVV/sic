
-- =============================================
-- Author:		Edwin Torres
-- Description:	Get cases related by case number
-- =============================================
CREATE PROCEDURE [WebApp].[GetOtherRelatedCasesByCaseNumber]

	@CaseNumber NVARCHAR(15) 
AS


CREATE TABLE #temp (
	CaseNumber NVARCHAR(15)
)

INSERT INTO #temp
EXEC [WebApp].[GetRelatedCasesByCaseNumber] @CaseNumber


SELECT C2.CaseNumber
FROM [SiC DB].[SiC].[Case] C
	INNER JOIN [SiC DB].[Entity].[Entity] E
		ON C.EntityId = E.EntityId
	INNER JOIN  [SiC DB].[Entity].[Entity] E2
		ON E.MergeId = E2.MergeId
			AND E.EntityId <> E2.EntityId
	INNER JOIN [SiC DB].[SiC].[Case] C2
		ON C2.EntityId = E2.EntityId
	LEFT JOIN #temp t
		ON C2.CaseNumber = t.CaseNumber
WHERE C.CaseNumber = @CaseNumber AND t.CaseNumber IS NULL




