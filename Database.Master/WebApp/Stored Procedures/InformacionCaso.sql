-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [WebApp].[InformacionCaso]
	-- Add the parameters for the stored procedure here
	@CaseId NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP(1) E.FullName as FullName, C.CaseNumber as CaseNumber, C.PolicyNo as PolicyNumber, E.SSN as SSN
	FROM [SiC DB].[SiC].[Case] C LEFT JOIN [SiC DB].[Entity].Entity E ON C.EntityId = E.EntityId
	WHERE C.CaseFolderId = @CaseId
END