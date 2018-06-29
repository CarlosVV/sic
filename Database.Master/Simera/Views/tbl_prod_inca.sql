CREATE VIEW [Simera].[tbl_prod_inca]
	AS 


	
SELECT TOP 1000 
	
	 CAST(C.[CaseNumber]			AS CHAR(12))		AS [INC_CASO]
	,CAST(C.[PolicyNo]				AS CHAR(11))		AS [NUM_POLI]
	,CAST(E.SSN						AS CHAR(9))			AS [SEGSOC]
	,CAST(NULL						AS NUMERIC(2,0))	AS [MUNI]
	,CAST(NULL						AS NUMERIC(2,0))	AS [YEAR]
	,C.[AccidentDate]									AS [ACCDTE]
	,CAST(Co.ConceptCode			AS NUMERIC(1,0))	AS [CNC]
	,CAST(NULL						AS NUMERIC(7,2))	AS [ADJINI]		--Adjudicación Inicial
	,CAST(NULL						AS DATETIME)		AS [ADJ_INIDT]	--Fecha de Adjudicación Inicial
	,CAST(NULL						AS NUMERIC(7,2))	AS [MENS]		--En el documento dice: SiC.Transaction.PaymentAmount
	,CAST(NULL						AS NUMERIC(7,2))	AS [BALANCE]	--AvailableBalance está en el store procedure SiC_Case pero no se subió
	,CAST(NULL						AS NUMERIC(7,2))	AS [ADJ_ADIC]	--Adjudicación Inicial
	,CAST(NULL						AS DATETIME)		AS [ADJ_ADICDT] --Fecha de Adjudicación Inicial
	,CAST(NULL						AS NUMERIC(7,2))	AS [INVERSION]
	,CAST(NULL						AS DATETIME)		AS [INVDT]

	,CAST(NULL						AS NUMERIC(7,2))	AS [ANTICIPO]
	,CAST(NULL						AS DATETIME)		AS [ANTDT]
	,CAST(C.DeductedMonthly			AS NUMERIC(7,2))	AS [MEN_DESC]
	,CAST(NULL						AS NUMERIC(7,2))	AS [BAL_ANT]
	,CAST(E.BirthDate				AS DATETIME)		AS [NACIDT]
	,CAST(RT.RelationshipTypeCode	AS CHAR(1))			AS [CL_REL]
	,CAST(PT.ParticipantType		AS CHAR(1))			AS [CL_ANORMAL] --TutorKey en el store procedure
	,CAST(Ca.CancellationCode		AS CHAR(1))			AS [CL_CANCEL]
	,C.CancellationDate									AS [CANCELDT]
	,CAST(E.FullName				AS CHAR(30))		AS [NOMBRE]
	,CAST(A.Line1					AS CHAR(30))		AS [DIR1]
	,CAST(A.Line2					AS CHAR(30))		AS [DIR2]
	,CAST(Ci.City					AS CHAR(13))		AS [PUEBLO]
	,CAST(A.ZipCode					AS NUMERIC(9,0))	AS [ZIP]
	,CAST(ES.EmployerStatus			AS CHAR(1))			AS [COD_SEG]
	,CAST(AOO.ActiveOnOff			AS CHAR(1))			AS [ONOFF]
	,CAST(NULL						AS NUMERIC(7,2))	AS [PAG_INI]
	,CAST(Com.CompensationKey		AS NUMERIC(4,0))	AS [CL_RIESGO]
	,CAST(C.WeeksPaid				AS NUMERIC(3,0))	AS [SEMANAS]
	,CAST(C.LegacyAmountPaid		AS NUMERIC(7,2))	AS [CANT]
	,C.ProcessDate										AS [PROCESO]
	,CAST(AI.ActiveIdentCode		AS CHAR(1))			AS [IDENT]
	,CAST(NULL						AS CHAR(12))		AS [OLD_NUMBER]
	,CAST(NULL						AS CHAR(1))			AS [CODIGO_CONV]


  FROM [SiC].[Case] C
	LEFT JOIN [Entity].[Entity] E
		ON C.EntityId = E.EntityId
	LEFT JOIN [Payment].[Concept] Co
		ON C.ConceptId = Co.ConceptId
	LEFT JOIN [Inca].[Cancellation] Ca
		ON C.CancellationId = Ca.CancellationId
	LEFT JOIN [Entity].[Address] A
		ON A.EntityId = E.EntityId
	LEFT JOIN [Location].[City] Ci
		ON A.CityId = Ci.CityId
	LEFT JOIN [Inca].ActiveOnOff AOO
		ON C.ActiveOnOffId = AOO.ActiveOnOffId
	LEFT JOIN [Inca].ActiveIdent AI
		ON C.ActiveIdentId = AI.ActiveIdentId
	LEFT JOIN [Entity].[Relationship] R
		ON R.EntityId = C.EntityId_Inca
	LEFT JOIN [Entity].RelationshipType RT
		ON R.RelationshipTypeId = RT.RelationshipTypeId
	LEFT JOIN [Entity].ParticipantType PT
		ON PT.ParticipantTypeId = E.ParticipantTypeId
	LEFT JOIN [SiC].EmployerStatus ES
		ON C.EmployerStatusId = Es.EmployerStatusId
	LEFT JOIN [SiC].Compensation Com
		ON C.CompensationId = Com.CompensationId