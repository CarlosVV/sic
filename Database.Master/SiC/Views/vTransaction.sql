
CREATE VIEW [SiC].[vTransaction]
WITH SCHEMABINDING
AS
		
			SELECT	C.CaseId					AS CaseId
					,C.CaseNumber				AS CaseNumber
					,C.CaseDate					AS CaseDate
					,SUM(ISNULL(T.TransactionAmount,0))	AS TransactionAmount
					,COUNT_BIG(*)				AS  COUNT_BIG
			FROM [SiC].[Case] C
				INNER JOIN [SiC].[Transaction] T
					ON T.CaseId = C.CaseId
			--WHERE C.CaseId = '40084'
			GROUP BY C.CaseId
					,C.CaseNumber
					,C.CaseDate
GO
CREATE UNIQUE CLUSTERED INDEX [IDX_SiC_vTransaction]
    ON [SiC].[vTransaction]([CaseId] ASC, [CaseNumber] ASC);

