CREATE TABLE [SiC].[Transaction] (
    [TransactionId]        INT             IDENTITY (1, 1) NOT NULL,
    [CaseId]               INT             NOT NULL,
    [TransactionTypeId]    INT             NULL,
    [TransactionAmount]    MONEY           NULL,
    [TransactionDate]      DATETIME        NULL,
    [Comment]              NVARCHAR (1000) NULL,
    [MonthlyInstalment]    MONEY           NULL,
    [NumberOfWeeks]        INT             NULL,
    [RejectedReason]       NVARCHAR (250)  NULL,
    [CreatedBy]            NVARCHAR (150)  NULL,
    [CreatedDateTime]      DATETIME        NULL,
    [ModifiedBy]           NVARCHAR (150)  NULL,
    [ModifiedDateTime]     DATETIME        NULL,
    [InvoiceNumber]        NVARCHAR (25)   NULL,
    [CaseId_Reference]     INT             NULL,
    [CaseNumber_Reference] NVARCHAR (15)   NULL,
    [FromDate]             DATE            NULL,
    [ToDate]               DATE            NULL,
    PRIMARY KEY CLUSTERED ([TransactionId] ASC),
    CONSTRAINT [FK_Transaction_Case] FOREIGN KEY ([CaseId]) REFERENCES [SiC].[Case] ([CaseId]),
    CONSTRAINT [FK_Transaction_TransactionType] FOREIGN KEY ([TransactionTypeId]) REFERENCES [SiC].[TransactionType] ([TransactionTypeId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'TransactionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [SiC].[Case] que contiene la información del caso de esta transacción', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'CaseId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Payment].[TransactionType] que contiene el tipo de Transaction Por ejemplo: Inversión, Adjudicación Inicial, etc', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'TransactionTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cantidad de la transacción', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'TransactionAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de la transacción', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'TransactionDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Comentarios', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'Comment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mensualidad', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'MonthlyInstalment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Razón de rechazo de la transacción por parte Sup. Compensaciones o Jefe Reclamaciones desde pantalla de Aprobaciones.', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'RejectedReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha creación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Para indicar si tuviera una factura al cobro', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'InvoiceNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Para indicar el id del caso del cual se está descontando', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'CaseId_Reference';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número del caso del cual se está descontando', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'CaseNumber_Reference';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Periodo de la dieta desde cuando se está descontando', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'FromDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Periodo de la dieta hasta cuando se está descontando', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Transaction', @level2type = N'COLUMN', @level2name = N'ToDate';

