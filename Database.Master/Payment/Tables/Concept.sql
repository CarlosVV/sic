CREATE TABLE [Payment].[Concept] (
    [ConceptId]        INT            NOT NULL,
    [ConceptCode]      NVARCHAR (2)   NULL,
    [Concept]          NVARCHAR (50)  NULL,
    [ConceptType]      NVARCHAR (50)  NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ConceptId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'ConceptId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código del concepto', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'ConceptCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Concepto de pago', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'Concept';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Concept', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

