CREATE TABLE [Payment].[MonthlyConcept] (
    [MonthlyConceptId] INT            NOT NULL,
    [ConceptId]        INT            NOT NULL,
    [MonthyPayment]    NVARCHAR (50)  NULL,
    [Maximum]          NVARCHAR (10)  NULL,
    [Year]             INT            NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([MonthlyConceptId] ASC),
    CONSTRAINT [FK_MonthlyConcept_ConceptId] FOREIGN KEY ([ConceptId]) REFERENCES [Payment].[Concept] ([ConceptId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'MonthlyConceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Payment].[ConceptId] que contiene el concepto de pago', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'ConceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pago mensual', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'MonthyPayment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Máximo', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'Maximum';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Año', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'MonthlyConcept', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

