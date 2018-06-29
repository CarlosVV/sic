
CREATE VIEW [Payment].[vPayment]
WITH SCHEMABINDING
AS
		

		SELECT	C.CaseId					AS CaseId
				,C.CaseNumber				AS CaseNumber
				,C.CaseDate					AS CaseDate
				,SUM(ISNULL(P.Amount,0))				AS PaymentAmount
				,COUNT_BIG(*)				AS  COUNT_BIG
					
			FROM [SiC].[Case] C
				INNER JOIN [Payment].[Payment] P
					ON P.CaseId = C.CaseId
				INNER JOIN [Payment].[Concept] Co
					ON Co.ConceptId = P.ConceptId
				INNER JOIN [Payment].[Status] S
					ON s.StatusId = P.StatusId
			WHERE ConceptType = 'Incapacidad' AND S.Effect = '+'
			--WHERE  C.CaseId = '40084'
			GROUP BY	C.CaseId
						,C.CaseNumber
						,C.CaseDate

GO
CREATE UNIQUE CLUSTERED INDEX [IDX_SiC_vPayment]
    ON [Payment].[vPayment]([CaseId] ASC, [CaseNumber] ASC);

