
CREATE VIEW [SiC].[vBalance]
AS
		
SELECT T.CaseId, T.CaseNumber,P.PaymentAmount, [TransactionAmount]-[PaymentAmount] AS Balance
  FROM [Payment].[vPayment] P
		RIGHT JOIN [SiC].[vTransaction] T
			ON P.CaseId = T.CaseId
--WHERE T.CaseNumber = '20064804726'

