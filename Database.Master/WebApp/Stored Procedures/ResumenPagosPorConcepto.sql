-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [WebApp].[ResumenPagosPorConcepto] 
	-- Add the parameters for the stored procedure here
	@CaseNumber NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Concepto, ISNULL(Pagado, 0) as Pagado, ISNULL([No Cobrado], 0) as [No Cobrado], ISNULL([Sub Total], 0) as [Sub Total] FROM 
		(
			SELECT P.Amount, CASE WHEN PS.[StatusCode] IN ('C','V') THEN 'No Cobrado' ELSE 'Pagado' END AS Estatus, 'Concepto ' + PC.[ConceptCode] + ' - ' + PC.[Concept] as Concepto
			FROM [SiC DB].[Payment].[Payment] P 
						LEFT JOIN  [SiC DB].[Payment].[Status] PS
							ON P.StatusID = PS.StatusID
						LEFT JOIN [SiC DB].[Payment].[Concept] PC
							ON P.ConceptID = PC.ConceptID
						INNER JOIN [SiC DB].[SiC].[Case] C ON C.CaseFolderId = @CaseNumber AND (C.CaseId = P.CaseId)
		--UNION
		--	SELECT Amount, 'Sub Total' AS Estatus, 'Concepto ' + PC.[ConceptCode] + ' - ' + PC.[Concept] as Concepto
		--	FROM [SiC DB].[Payment].[Payment] P 
		--					LEFT JOIN [SiC DB].[Payment].[Concept] PC
		--					ON P.ConceptID = PC.ConceptID
		--	WHERE CaseNumber = @CaseNumber
		) A
	pivot (sum (Amount) for Estatus IN ([Pagado], [No Cobrado], [Sub Total])) AS Agrupado
END