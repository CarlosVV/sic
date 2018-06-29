CREATE VIEW [Simera].[tblCheques]
	AS 

	SELECT 
		P.CheckBk											AS [IDCheque]
		,CAST(Co.ConceptCode		AS VARCHAR(1))			AS [Concepto]
		,CAST(TransactionNum		AS VARCHAR(9))			AS [Cheque]
		,CAST(Cl.ClinicCode			AS VARCHAR(2))			AS [Dispensario]
		,CAST(R.RiskKey				AS VARCHAR(4))			AS [Riesgo]
		,CAST(R.RiskGroup			AS VARCHAR(1))			AS [Grupo]
		,P.Amount											AS [Cantidad]
		,P.IssueDate										AS [FechaEmision]
		,P.FromDate											AS [FechaDesde]
		,P.ToDate											AS [FechaHasta]
		,CAST(P.PaymentDay			AS SMALLINT)			AS [DiasPago]
		,CAST(S.StatusCode			AS VARCHAR(1))			AS [Status]
		,P.StatusChangeDate									AS [FechaCambioStatus]
		,CAST(Cla.Class				AS VARCHAR(1))			AS [ClasePago]
		,CAST(ES.EmployerStatus		AS VARCHAR(1))			AS [Asegurado]
		,CAST(Re.RegionCode			AS VARCHAR(1))			AS [Region]
		,CAST(NULL					AS VARCHAR(1))			AS [Oficina]
		,CAST(NULL					AS VARCHAR(20))			AS [UserEditedStatus]
		,CAST(NULL					AS INT)					AS [IDBatch]
		,C.CheckCaseBK										AS [IDCaso]

  FROM [Payment].[Payment] P
			LEFT JOIN Payment.[Concept] Co
				ON P.ConceptId = Co.ConceptId
			LEFT JOIN [Location].Clinic Cl
				ON P.ClinicId = Cl.ClinicId
			LEFT JOIN [SiC].[KeyRiskIndicator] R
				ON P.KeyRiskIndicatorId = R.KeyRiskIndicatorId
			LEFT JOIN [Payment].[Status] S
				ON P.StatusId = S.StatusId
			LEFT JOIN [Payment].[Class] Cla
				ON P.ClassId = Cla.ClassId
			LEFT JOIN [Location].[Region] Re
				ON P.RegionId = Re.RegionId
			INNER JOIN [SiC].[Case] C
				ON P.[CaseId] = C.[CaseId]
			LEFT JOIN [SiC].EmployerStatus ES
				ON C.EmployerStatusId = ES.EmployerStatusId