CREATE TABLE [SiC].[Case] (
    [CaseId]             INT            IDENTITY (1, 1) NOT NULL,
    [CaseNumber]         NVARCHAR (15)  NULL,
    [PolicyNo]           NVARCHAR (11)  NULL,
    [CaseFolderId]       INT            NULL,
    [CaseFolder]         NVARCHAR (10)  NULL,
    [DietBK]             INT            NULL,
    [IncaBk]             INT            NULL,
    [CheckCaseBK]        INT            NULL,
    [Reserve]            MONEY          NULL,
    [DeductedMonthly]    MONEY          NULL,
    [WeeksPaid]          INT            NULL,
    [LegacyAmountPaid]   MONEY          NULL,
    [DailyWage]          MONEY          NULL,
    [DaysWeek]           INT            NULL,
    [DailyComp]          MONEY          NULL,
    [DaysPaid]           MONEY          NULL,
    [ConceptId]          INT            NULL,
    [EntityId]           INT            NULL,
    [EmployerName]       NVARCHAR (75)  NULL,
    [EmployerEIN]        NVARCHAR (9)   NULL,
    [IsMinor]            BIT            NULL,
    [Age]                INT            NULL,
    [CaseDate]           DATETIME       NULL,
    [AccidentDate]       DATETIME       NULL,
    [TreatmentRestDate]  DATETIME       NULL,
    [TreatmentWorkDate]  DATETIME       NULL,
    [DeceaseDate]        DATETIME       NULL,
    [DecisionDate]       DATETIME       NULL,
    [ClinicId]           INT            NULL,
    [RegionId]           INT            NULL,
    [ClinicId_Service]   INT            NULL,
    [RegionId_Service]   INT            NULL,
    [DeathFlag]          BIT            NULL,
    [LastPaymentDate]    DATETIME       NULL,
    [CompOfficer]        NVARCHAR (200) NULL,
    [CompensationId]     INT            NULL,
    [KeyRiskIndicatorId] INT            NULL,
    [EmployerStatusId]   INT            NULL,
    [EntityId_Inca]      INT            NULL,
    [CancellationDate]   DATE           NULL,
    [ProcessDate]        DATE           NULL,
    [ActiveOnOffId]      INT            NULL,
    [ActiveIdentId]      INT            NULL,
    [CancellationId]     INT            NULL,
    [EntityId_Diet]      INT            NULL,
    [VocRehabPayments]   NVARCHAR (2)   NULL,
    [SonoraFingerprint]  NVARCHAR (50)  NULL,
    [CreatedBy]          NVARCHAR (150) NULL,
    [CreatedDateTime]    DATETIME       NULL,
    [ModifiedBy]         NVARCHAR (150) NULL,
    [ModifiedDateTime]   DATETIME       NULL,
    CONSTRAINT [PK_Case] PRIMARY KEY CLUSTERED ([CaseId] ASC),
    CONSTRAINT [FK_SiC_ActiveIdent] FOREIGN KEY ([ActiveIdentId]) REFERENCES [Inca].[ActiveIdent] ([ActiveIdentId]),
    CONSTRAINT [FK_SiC_ActiveOnOff] FOREIGN KEY ([ActiveOnOffId]) REFERENCES [Inca].[ActiveOnOff] ([ActiveOnOffId]),
    CONSTRAINT [FK_SiC_Cancellation] FOREIGN KEY ([CancellationId]) REFERENCES [Inca].[Cancellation] ([CancellationId]),
    CONSTRAINT [FK_SiC_Clinic] FOREIGN KEY ([ClinicId]) REFERENCES [Location].[Clinic] ([ClinicId]),
    CONSTRAINT [FK_SiC_Clinic_Service] FOREIGN KEY ([ClinicId_Service]) REFERENCES [Location].[Clinic] ([ClinicId]),
    CONSTRAINT [FK_SiC_Compensation] FOREIGN KEY ([CompensationId]) REFERENCES [SiC].[Compensation] ([CompensationId]),
    CONSTRAINT [FK_SiC_Concept] FOREIGN KEY ([ConceptId]) REFERENCES [Payment].[Concept] ([ConceptId]),
    CONSTRAINT [FK_SiC_EmployerStatus] FOREIGN KEY ([EmployerStatusId]) REFERENCES [SiC].[EmployerStatus] ([EmployerStatusId]),
    CONSTRAINT [FK_SiC_Entity] FOREIGN KEY ([EntityId]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_SiC_Entity_Diet] FOREIGN KEY ([EntityId_Diet]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_SiC_Entity_Inca] FOREIGN KEY ([EntityId_Inca]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_SiC_KeyRiskIndicator] FOREIGN KEY ([KeyRiskIndicatorId]) REFERENCES [SiC].[KeyRiskIndicator] ([KeyRiskIndicatorId]),
    CONSTRAINT [FK_SiC_Region] FOREIGN KEY ([RegionId]) REFERENCES [Location].[Region] ([RegionId]),
    CONSTRAINT [FK_SiC_Region_Service] FOREIGN KEY ([RegionId_Service]) REFERENCES [Location].[Region] ([RegionId])
);


GO
CREATE NONCLUSTERED INDEX [NCLIX_Case_CaseNumber]
    ON [SiC].[Case]([CaseNumber] ASC)
    INCLUDE([PolicyNo]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id autogenerado en esta tabla', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de caso ', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'Se obtiene de [Sonora].[dbo].[SIF].[CaseNumber] de no existir se busca en [tcAcess].[dbo].[tbl_prod_dietas].[CASE_NUM] o en [tcAccess].[dbo].[tbl_prod_inca].[INC_CASO]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'Dependiendo del sistema se completa el número de caso añadiendo un 20 o 19 al comienzo', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de póliza ', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'PolicyNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'Se obtiene de [Sonora].[dbo].[SIF].[PolicyNo] de no existir se busca en [tcAcess].[dbo].[tbl_prod_dietas].[POL_NUM] o en [tcAccess].[dbo].[tbl_prod_inca].[NUM_POLI]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'PolicyNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'Tipo de dato', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'PolicyNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla fuente en Case360', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseFolderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'Sonora.dbo.SIF.CaseFolderId', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseFolderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id incremental generado por el sistema de ETLs utilizando la tabla fuente en sistema Inca', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'IncaBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[SiC Loading].[Inca].[Prod].[IncaId]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'IncaBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'IncaBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cantidad separada por el asegurador para cubrir gastos', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'Reserve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'Calculado: Adjudicación Inicial más Adjudicaciones Adicionales', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'Reserve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'Reserve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mensualidad a descontar', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeductedMonthly';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[MEN_DESC]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeductedMonthly';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'Tipo de dato', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeductedMonthly';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Semanas pagadas', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'WeeksPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[SEMANAS]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'WeeksPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'Tipo de dato', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'WeeksPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'LegacyAmountPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'LegacyAmountPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'LegacyAmountPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Jornal Diario', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DailyWage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAcess].[dbo].[tbl_prod_dietas].[DAY_JOURNAL]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DailyWage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'Tipo de dato', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DailyWage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Días que trabaja a la semana', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DaysWeek';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAcess].[dbo].[tbl_prod_dietas].[DAYS_WORK]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DaysWeek';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'Tipo de dato', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DaysWeek';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Compensación diaria', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DailyComp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAcess].[dbo].[tbl_prod_dietas].[DAY_COMP]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DailyComp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'Tipo de dato', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DailyComp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de último pago', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DaysPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAcess].[dbo].[tbl_prod_dietas].[ULTIMO_PAGO]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DaysPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DaysPaid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Payment].[Concept] que contiene el Concepto de Pago', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ConceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[CNC]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ConceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ConceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[Entity] que contiene información del Lesionado, dirección, teléfono etc', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'Múltiples fuentes', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Si es menor de edad al momento de radircar el caso ', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'IsMinor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'Diferencia entre [CaseDate] y [Entity].[Birthday] menor que 18', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'IsMinor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'IsMinor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Edad al momento de radicar el caso', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'Age';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'Diferencia entre [CaseDate] y [Entity].[Birthday]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'Age';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'Age';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de radicación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[CASEDATE]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha del accidente', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'AccidentDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[ACCIDENTDATE]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'AccidentDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'AccidentDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de tratamiento', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'TreatmentRestDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[TRTMTRESTDATE]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'TreatmentRestDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'TreatmentRestDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de tratamiento', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'TreatmentWorkDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[TRTMTWKDATE]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'TreatmentWorkDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'TreatmentWorkDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de muerte', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeceaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[DECEASEDATE]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeceaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeceaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de decisión', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DecisionDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[DECISIONDT]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DecisionDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DecisionDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Clinic] que indica la clínica de radicación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ClinicId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[CLINIC]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ClinicId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ClinicId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Region] que indica la región de radicación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'RegionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[REGION]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'RegionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'RegionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Clinic] que indica la clínica de servicio', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ClinicId_Service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[CLINICSRV]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ClinicId_Service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ClinicId_Service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Region] que indica la región de servicio', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'RegionId_Service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[REGIONSRV]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'RegionId_Service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'RegionId_Service';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si es un caso de muerte', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeathFlag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[DEATHFLAG]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeathFlag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'DeathFlag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de último pago', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'LastPaymentDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAcess].[dbo].[tbl_prod_dietas].[ULTIMO_PAGO]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'LastPaymentDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'LastPaymentDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Oficial de Compensaciones', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CompOfficer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[COMPOFFICER]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CompOfficer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CompOfficer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a tabla [SiC].[Compensation] que contiene los Códigos de Compensación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CompensationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CompensationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CompensationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [SiC].[KeyRiskIndicator] que contiene la clave de grupo y riesgo', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'KeyRiskIndicatorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'KeyRiskIndicatorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'KeyRiskIndicatorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [SiC].[EmployerStatus] que contiene la Clave de Patronos', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EmployerStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[Sonora].[dbo].[SIF].[EMPLOYERSTATUS] || [tcAccess].[dbo].[tbl_prod_inca].[COD_SEG]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EmployerStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EmployerStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla[Entity].[Entity] con Información del Lesionado que se encontró en el sistema Inca', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId_Inca';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId_Inca';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId_Inca';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de cancelación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CancellationDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[CANCELDT]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CancellationDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CancellationDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de proceso', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ProcessDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[PROCESO]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ProcessDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ProcessDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Inca].[ActiveOnOffId] que contiene clave de activo', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ActiveOnOffId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[ONOFF]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ActiveOnOffId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ActiveOnOffId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Inca].[ActiveIdent] que contiene clave de activo o inactivo', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ActiveIdentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[IDENT]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ActiveIdentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ActiveIdentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Inca].[Cancellation] que indica la clave de cancelación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CancellationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAccess].[dbo].[tbl_prod_inca].[CL_CANCEL]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CancellationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CancellationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[Entity] que contiene la informacióm del lesionado que existia en sistema DIET', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId_Diet';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId_Diet';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'EntityId_Diet';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número total de pagos por rehabilitación vocacional', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'VocRehabPayments';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Source', @value = N'[tcAcess].[dbo].[tbl_prod_dietas].[PENDING]', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'VocRehabPayments';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Transformation', @value = N'', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'VocRehabPayments';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Representa los valores del caso que guarda la tabla y provienen de Case', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'SonoraFingerprint';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha creación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Case', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

