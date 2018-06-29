-- =============================================
-- Author:	 Emmanuel Sanchez
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [WebApp].[ResumenPagosPorBeneficiario] 
	-- Add the parameters for the stored procedure here
	@CaseNumber NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	SELECT Beneficiario, ISNULL(Pagado,0) as Pagado, ISNULL([No Cobrado],0) as [No Cobrado], ISNULL([Sub Total],0) as [Sub Total] FROM 
		(	
			SELECT Amount, CASE WHEN PS.[Effect] = '-' THEN 'No Cobrado' ELSE 'Pagado' END AS Estatus, E.FullName as Beneficiario, E.EntityId
			FROM [SiC DB].[SiC].[Case] C
					INNER JOIN [SiC DB].[Payment].[Payment] P
						ON (C.CaseId = P.CaseId) 
					LEFT JOIN [SiC DB].[Payment].[Status] PS
						ON P.StatusID = PS.StatusID
					LEFT JOIN [SiC DB].[Entity].[Entity] E
						ON P.EntityId_Beneficiary = E.EntityId
			WHERE C.CaseFolderId = @CaseNumber
					
			
		--UNION
		--	SELECT DISTINCT Amount, 'Sub Total' AS Estatus, 'Juan del Pueblo' as Beneficiario
		--	FROM [SiC DB].[Check].[Check]
		--	WHERE CaseNumber = @CaseNumber
		) A
	pivot (sum (Amount) for Estatus IN ([Pagado], [No Cobrado], [Sub Total])) AS Agrupado
END