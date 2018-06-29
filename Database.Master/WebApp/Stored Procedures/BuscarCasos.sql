
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [WebApp].[BuscarCasos]
	-- Add the parameters for the stored procedure here
	@Nombre			nvarchar(50) = null,
	@SSN			nvarchar(50) = null,
	@EBT			nvarchar(50) = null,
	@NumeroCaso		nvarchar(50) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT E.FullName as Nombre, E.SSN as [SSN], EBT.[EBTAccount] as EBT, C.[CaseNumber] as NumeroCaso, C.CaseFolderId
	FROM [SiC].[Case] C LEFT JOIN [Entity].[Entity] E 
			ON C.EntityId = E.EntityId
		LEFT JOIN [SiC].[CaseDetail] EBT
			ON C.CaseId = EBT.CaseId
	WHERE	(E.FullName LIKE '%' + @Nombre + '%' OR @Nombre IS NULL)
		AND	(E.SSN = @SSN OR @SSN IS NULL)
		AND (EBT.[EBTAccount] = @EBT OR @EBT IS NULL)
		AND (C.[CaseNumber] = @NumeroCaso OR @NumeroCaso IS NULL)
END
