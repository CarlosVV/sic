CREATE TABLE [Payment].[ThirdPartySchedule] (
    [ThirdPartyScheduleId]    INT             IDENTITY (1, 1) NOT NULL,
    [EntityId_Beneficiary]    INT             NULL,
    [EntityId_RemitTo]        INT             NULL,
    [CaseId]                  INT             NULL,
    [ClaimNumber]             NVARCHAR (30)   NULL,
    [OrderIdentifier]         NVARCHAR (30)   NULL,
    [TerminationFlag]         BIT             NULL,
    [EffectiveDate]           DATETIME        NULL,
    [TerminationOrderNumber]  NVARCHAR (30)   NULL,
    [SinglePaymentAmount]     MONEY           NULL,
    [FirstInstallmentAmount]  MONEY           NULL,
    [SecondInstallmentAmount] MONEY           NULL,
    [Comment]                 NVARCHAR (1000) NULL,
    [OrderAmount]             MONEY           NULL,
    [TerminationDate]         DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([ThirdPartyScheduleId] ASC),
    CONSTRAINT [FK_ThirdPartySchedule_Case] FOREIGN KEY ([CaseId]) REFERENCES [SiC].[Case] ([CaseId]),
    CONSTRAINT [FK_ThirdPartySchedule_Entity_Beneficiary] FOREIGN KEY ([EntityId_Beneficiary]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_ThirdPartySchedule_Entity_RemitTo] FOREIGN KEY ([EntityId_RemitTo]) REFERENCES [Entity].[Entity] ([EntityId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de reclamación en agencias externas.', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'ThirdPartySchedule', @level2type = N'COLUMN', @level2name = N'ClaimNumber';

