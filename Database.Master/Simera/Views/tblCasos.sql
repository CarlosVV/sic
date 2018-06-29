CREATE VIEW [Simera].[tblCasos]
	AS 

	SELECT 
		C.[CheckCaseBK]									AS [IDCaso]
		,CAST(C.[CaseNumber]  AS VARCHAR(12))			AS [Caso]
		,CAST(E.[FullName]	  AS VARCHAR(40))			AS [Nombre]
		,CAST(C.PolicyNo	  AS VARCHAR(10))			AS [Poliza]
		,CAST(E.SSN			  AS VARCHAR(9))			AS [SeguroSocial]
	FROM [SiC].[Case] C
		INNER JOIN [Entity].[Entity] E
			ON C.EntityId = E.EntityId