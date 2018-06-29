CREATE TABLE [Payment].[Payment] (
    [PaymentId]            INT            IDENTITY (1, 1) NOT NULL,
    [CheckBk]              INT            NOT NULL,
    [CaseKey]              NVARCHAR (2)   NULL,
    [CaseNumber]           NVARCHAR (11)  NULL,
    [TransactionNum]       NVARCHAR (9)   NULL,
    [Amount]               MONEY          NULL,
    [PaymentDay]           SMALLINT       NULL,
    [IssueDate]            DATETIME       NULL,
    [FromDate]             DATETIME       NULL,
    [ToDate]               DATETIME       NULL,
    [StatusChangeDate]     DATETIME       NULL,
    [CaseId]               INT            NULL,
    [ClinicId]             INT            NULL,
    [RegionId]             INT            NULL,
    [EntityId_Beneficiary] INT            NULL,
    [EntityId_RemitTo]     INT            NULL,
    [KeyRiskIndicatorId]   INT            NULL,
    [ConceptId]            INT            NULL,
    [ClassId]              INT            NULL,
    [StatusId]             INT            NULL,
    [TransferTypeId]       INT            NULL,
    [CreatedBy]            NVARCHAR (150) NULL,
    [CreatedDateTime]      DATETIME       NULL,
    [ModifiedBy]           NVARCHAR (150) NULL,
    [ModifiedDateTime]     DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([PaymentId] ASC),
    CONSTRAINT [FK_Payment_Case] FOREIGN KEY ([CaseId]) REFERENCES [SiC].[Case] ([CaseId]),
    CONSTRAINT [FK_Payment_Class] FOREIGN KEY ([ClassId]) REFERENCES [Payment].[Class] ([ClassId]),
    CONSTRAINT [FK_Payment_Clinic] FOREIGN KEY ([ClinicId]) REFERENCES [Location].[Clinic] ([ClinicId]),
    CONSTRAINT [FK_Payment_Concept] FOREIGN KEY ([ConceptId]) REFERENCES [Payment].[Concept] ([ConceptId]),
    CONSTRAINT [FK_Payment_Entity_Beneficiary] FOREIGN KEY ([EntityId_Beneficiary]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_Payment_Entity_RemitTo] FOREIGN KEY ([EntityId_RemitTo]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_Payment_KeyRiskIndicator] FOREIGN KEY ([KeyRiskIndicatorId]) REFERENCES [SiC].[KeyRiskIndicator] ([KeyRiskIndicatorId]),
    CONSTRAINT [FK_Payment_Region] FOREIGN KEY ([RegionId]) REFERENCES [Location].[Region] ([RegionId]),
    CONSTRAINT [FK_Payment_Status] FOREIGN KEY ([StatusId]) REFERENCES [Payment].[Status] ([StatusId]),
    CONSTRAINT [FK_Payment_TransferType] FOREIGN KEY ([TransferTypeId]) REFERENCES [Payment].[TransferType] ([TransferTypeId])
);


GO
CREATE NONCLUSTERED INDEX [NCLIX_Payment_CaseNumber_CaseKey]
    ON [Payment].[Payment]([CaseNumber] ASC)
    INCLUDE([TransactionNum], [Amount], [PaymentDay], [IssueDate], [FromDate], [ToDate], [StatusChangeDate]);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20151201-175337]
    ON [Payment].[Payment]([CaseId] ASC)
    INCLUDE([Amount], [EntityId_Beneficiary]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'PaymentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de cheques en la fuente', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'CheckBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de caso', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'CaseNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de cheque o transacción', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'TransactionNum';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cantidad del cheque', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Días de pago', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'PaymentDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de emisión', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha desde', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'FromDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha hasta', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'ToDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de cambio de estatus', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'StatusChangeDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [SiC].[Case] que tiene información del caso', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'CaseId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Clinic] que contiene información del dispensario', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'ClinicId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Region] que contiene información de la región', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'RegionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[Entity] que tiene información del beneficiario', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'EntityId_Beneficiary';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[Entity] que tiene información de la persona a quien fue remitido el pago', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'EntityId_RemitTo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [SiC].[KeyRiskIndicator] que contiene información de la Clave y el Riesgo', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'KeyRiskIndicatorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Payment].[Concept] que tiene información del concepto del cheque', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'ConceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Payment].[Class] contiene información de la clase de pago', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'ClassId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Payment].[Status] que contiene información del estatus de pago', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'StatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Payment].[TransferType] describe si el pago fue en cheque o transferencia electrónica', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'TransferTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Payment', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

