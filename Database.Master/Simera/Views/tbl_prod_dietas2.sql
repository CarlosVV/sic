
CREATE VIEW [Simera].[tbl_prod_dietas2]
	AS

	SELECT
		CAST(RIGHT(C.[CaseNumber],9)	AS CHAR(9))			AS [CASE_NUM]
		,CAST(NULL						AS CHAR(1))			AS [CASE_CH_DIG]
		,CAST(E.SSN						AS NUMERIC(9,0))	AS [SOC_SEC_NO]
		,CAST(A.Line1					AS CHAR(30))		AS [STREET]
		,CAST(CS.CivilStatus			AS CHAR(1))			AS [SEX_CIV_STATUS] --Si SEX_CIV_STATUS in (1,6) 'Casado'. Si SEX_CIV_STATUS (0,5) 'Soltero'.  Si SEX_CIV_STATUS in (2,7) 'Divorciado'. Si SEX_CIV_STATUS (3,8) 'Viudo',  SEX_CIV_STATUS (4,9) 'N/A' 
		,CAST(C.[PolicyNo]				AS NUMERIC(10,0))	AS [POL_NUM]
		,CAST(NULL						AS CHAR(1))			AS [POL_CH_DIG]
		,CAST(R.RiskKey					AS CHAR(4))			AS [RISK]
		,CAST(R.RiskGroup				AS CHAR(3))			AS [GRUPO]
		,CAST(NULL						AS CHAR(1))			AS [EMP_STATUS]
		,CAST(A.Line2					AS CHAR(30))		AS [URB]
		,CAST(A.DeliverPl				AS CHAR(2))			AS [PTO_DE_ENTREGA]
		,CAST(A.CkDig					AS CHAR(1))			AS [CK_DIGITO]
		,CAST(A.Letterman				AS CHAR(4))			AS [CARTERO]
		,CAST(NULL						AS CHAR(1))			AS [FILLER1]
		,CAST(Ci.City					AS CHAR(13))		AS [TOWN]
		,CAST(A.ZipCode					AS NUMERIC(5,0))	AS [ZIP]
		,CAST(NULL						AS CHAR(3))			AS [REG_MUNC_ACC] --AccidentCity
		,CAST(St.[State]				AS CHAR(2))			AS [ESTADO]
		,CAST(NULL						AS CHAR(4))			AS [HOUR_WORK]
		,CAST(NULL						AS CHAR(1))			AS [AM_PM_WORK]
		,CAST(C.[AccidentDate]			AS DATE)			AS [ACC_DATE]
		,CAST(NULL						AS CHAR(4))			AS [HOUR]
		,CAST(NULL						AS CHAR(1))			AS [AM_PM]
		,CAST(NULL						AS CHAR(6))			AS [DEJO_TRAB]
		,CaseDate											AS [RADICACION]
		,CAST(NULL						AS CHAR(6))			AS [A1ER_EXAMEN]
		,CAST(NULL						AS NUMERIC(6,0))	AS [FECHA_UPDATE_STATU]
		,CAST(NULL						AS CHAR(1))			AS [PAID_RV]
		,CAST(NULL						AS CHAR(1))			AS [TREATMENT]
		,CAST(NULL						AS CHAR(3))			AS [EST_TIME]
		,CAST(C.DailyWage				AS NUMERIC(5,2))	AS [DAY_JOURNAL]
		,CAST(C.DaysWeek				AS CHAR(3))			AS [DAYS_WORK]
		,CAST(C.DailyComp				AS NUMERIC(6,2))	AS [DAY_COMP]
		,CAST(NULL						AS CHAR(1))			AS [CHECK_RCV]
		,CAST(Com.CompensationKey		AS CHAR(6))			AS [COMP_KEY]
		,CAST(Cl.ClinicCode				AS CHAR(3))			AS [REG_DISP_TRT] --Document says: Left(REG_DISP_TR,1). Buscar la región en Accident.Region Right(REG_DISP_TR,2). Buscar el dispensario en Accident.Dispensario
		,CAST(NULL						AS CHAR(1))			AS [DAYS_WAIT]
		,C.TreatmentWorkDate								AS [CT_A_TRAB]
		,CAST(NULL						AS CHAR(6))			AS [RECIDIVA]
		,CAST(NULL						AS CHAR(6))			AS [ALTA_DEF]
		,C.LastPaymentDate									AS [ULTIMO_PAGO]
		,CAST(NULL						AS CHAR(6))			AS [ULTIMO_DIA]
		,CAST(C.WeeksPaid				AS NUMERIC(3,0))	AS [WEEKS_PAID]
		,CAST(C.DaysPaid				AS NUMERIC(1,0))	AS [DAYS_PAID]
		,CAST(C.LegacyAmountPaid		AS NUMERIC(7,2))	AS [TOTAL_PAID]
		,CAST(C.VocRehabPayments		AS CHAR(2))			AS [PENDING]
		,CAST(NULL						AS CHAR(6))			AS [A1ER_PAGO] --Case.FirstPaymentDate
		,E.BirthDate										AS [DATE_BIRTH]
		,CAST(ES.EmployerStatus			AS CHAR(2))			AS [NO_ASEG]
		,CAST(NULL						AS CHAR(2))			AS [STAT_UBIC]
		,CAST(NULL						AS CHAR(1))			AS [ERROR_IND]
		,CAST(Re.RegionCode				AS CHAR(1))			AS [MAS_ZONE]
		,CAST(C.CompOfficer				AS CHAR(1))			AS [MAS_OFFICIAL]
		,CAST(NULL						AS CHAR(1))			AS [COND_IND]
		,CAST(NULL						AS CHAR(1))			AS [ACT_INACT_CODE]
		,CAST(A.ZipCodeExt				AS NUMERIC(4,0))	AS [ZIP_PLUS]
		,CAST(NULL						AS CHAR(6))			AS [F_ACTUALIZACION] --Segun documento SiC.Case.LastupdateDate
		,CAST(E.LastName				AS CHAR(12))		AS [APELLIDO1]
		,CAST(E.SecondLastName			AS CHAR(12))		AS [APELLIDO2]
		,CAST(E.FirstName				AS CHAR(12))		AS [NOMBRE1]
		,CAST(E.MiddleName 				AS CHAR(12))		AS [NOMBRE2]
		,CAST(NULL						AS CHAR(1))			AS [FLAG_CHG_OPER]
		,CAST(G.GenderCode				AS CHAR(1))			AS [SEXO]
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
		LEFT JOIN [Location].[State] St
			ON A.StateId = St.StateId
		LEFT JOIN [Inca].ActiveOnOff AOO
			ON C.ActiveOnOffId = AOO.ActiveOnOffId
		LEFT JOIN [Inca].ActiveIdent AI
			ON C.ActiveIdentId = AI.ActiveIdentId
		LEFT JOIN [Entity].[CivilStatus] CS
			ON E.CivilStatusId = CS.CivilStatusId
		LEFT JOIN [SiC].[KeyRiskIndicator] R
			ON C.KeyRiskIndicatorId = R.KeyRiskIndicatorId
		LEFT JOIN [Entity].[Gender] G
			ON E.GenderId = G.GenderId
		LEFT JOIN [Location].[Region] Re
			ON C.RegionId = Re.RegionId
		LEFT JOIN [SiC].[EmployerStatus] ES
			ON C.EmployerStatusId = ES.EmployerStatusId
		LEFT JOIN [Location].Clinic Cl
			ON Cl.ClinicId = C.ClinicId_Service
		LEFT JOIN [Location].Region ReS
			ON ReS.RegionId = C.RegionId_Service
		LEFT JOIN [SiC].Compensation Com
			ON C.CompensationId = Com.CompensationId
