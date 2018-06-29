CREATE PROCEDURE [Reports].[PaymentDetail]
(
	@CaseNumber NVARCHAR(12),
	@DateFrom DATETIME,
	@DateTo DATETIME
)
AS
	SELECT DISTINCT 
		   P.[TransactionNum] AS Cheque
		   , CO.[ConceptCode] AS ConceptCode
		   , CO.[Concept] AS Concept
		   , CL.Class AS PaymentClassCode
		   , T .[TransferType] AS EBT
		   , CONVERT(VARCHAR(10)
		   , CAST(P.[StatusChangeDate] AS DATETIME), 103) AS DateChangeStatus
		   , CONVERT(VARCHAR(10)
		   , CAST(P.[IssueDate] AS DATETIME), 103) AS DateProgramed
		   , CONVERT(VARCHAR(10)
		   , CAST(P.[IssueDate] AS DATETIME), 112) AS SortDateProgramed
		   , CASE WHEN ConceptType = 'Dieta' THEN CONVERT(VARCHAR(10), FromDate, 103) ELSE 'NA' END AS DateFrom
		   , CASE WHEN ConceptType = 'Dieta' THEN CONVERT(VARCHAR(10), ToDate, 103) ELSE 'NA' END AS DateTo
		   , CASE WHEN S.Effect = '-' THEN P.Amount * -1 ELSE P.Amount END AS Amount
		   , S.StatusCode AS StatusCode, S.[Status] AS PaymentStatus
		   , RE.[Region] AS Region, CL.[Concept] AS PaymentClass
		   , LTRIM(RTRIM(C.CaseNumber)) AS CaseNumber
		   , RT.[RelationshipType] AS Beneficiary
		   , C.[PolicyNo] AS PolicyNo, CASE WHEN LTRIM(RTRIM(CE.SSN)) = '' OR CE.SSN IS NULL THEN '' ELSE LEFT(CE.SSN, 3) + '-' + RIGHT(LEFT(CE.SSN, 5), 2) + '-' + RIGHT(CE.SSN, 4) END AS SSN
		   , CAST(CASE WHEN RT.[RelationshipType] IS NULL THEN 1 ELSE 0 END AS BIT) AS IsInjured
		   , LTRIM(RTRIM(CE.[FullName])) AS InjuredFullName
		   , LTRIM(RTRIM(PE.[FullName])) AS BeneficiaryFullName
		   , P.[StatusID] AS StatusID
		   , P.[ConceptId] AS ConceptID
		   , P.[PaymentDay] AS PaymentDay
		   , CD.[EBTAccount] AS EBTAccount
		   , EntityId_Beneficiary

	FROM SIC.[Case] C INNER JOIN
		[Payment].[Payment] P ON C.CaseId = P.CaseId AND C.CaseNumber = @CaseNumber LEFT JOIN
		[Payment].[Concept] CO ON P.ConceptId = CO.ConceptId LEFT JOIN
		[Payment].[Class] CL ON P.ClassId = CL.ClassId LEFT JOIN
		[Payment].[Status] S ON P.StatusId = S.StatusId LEFT JOIN
		[Payment].[TransferType] T ON P.TransferTypeId = T .TransferTypeId LEFT JOIN
		[Location].[Region] RE ON C.RegionId = RE.RegionId LEFT JOIN
		[Entity].[Entity] PE ON P.EntityId_Beneficiary = PE.EntityId LEFT JOIN
		[Entity].[Relationship] R ON PE.EntityId = R.EntityId_Related LEFT JOIN
		[Entity].[RelationshipType] RT ON R.RelationshipTypeId = RT.RelationshipTypeId LEFT JOIN
		[Entity].[Entity] CE ON C.EntityID = CE.EntityID LEFT JOIN
		SiC.CaseDetail CD ON C.CaseId = CD.CaseId AND P.CaseKey = CD.CaseKey
	WHERE ((P.IssueDate >= @DateFrom OR @DateFrom IS NULL) AND (P.IssueDate <= @DateTo OR @DateTo IS NULL))