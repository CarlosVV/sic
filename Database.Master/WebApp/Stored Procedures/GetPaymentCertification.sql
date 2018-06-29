-- =============================================
-- Author:		<Roberto Durand>
-- Create date: <03-30-2016>
-- Description:	<Return payment certification related data>
-- =============================================
CREATE PROCEDURE WebApp.GetPaymentCertification
	@CaseNumber NVARCHAR(11)
AS
BEGIN
	SELECT TOP 1000 --Quitar el top 1000
		[CaseNumber]
		,[Amount]
		,P.[FromDate]
		,P.[ToDate]
		,[Concept]
		,[Status]
		,InvoiceNumber
	FROM [SiC DB].[Payment].[Payment] P
	INNER JOIN [SiC DB].[Payment].[Status] S ON P.StatusID = S.StatusID
	INNER JOIN [SiC DB].[Payment].[Concept] C ON P.ConceptID = C.ConceptID
	LEFT JOIN [SiC DB].[SIC].[Transaction] T ON P.CaseId = T.CaseId
	WHERE S.[Status] = 'Pagado' AND P.CaseNumber = @CaseNumber --Cambiar a generado el estatus
END
