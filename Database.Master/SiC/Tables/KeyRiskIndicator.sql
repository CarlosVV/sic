CREATE TABLE [SiC].[KeyRiskIndicator] (
    [KeyRiskIndicatorId] INT            IDENTITY (1, 1) NOT NULL,
    [RiskKey]            NVARCHAR (4)   NULL,
    [RiskGroup]          NVARCHAR (3)   NULL,
    [BillingYear]        INT            NULL,
    [Description]        NVARCHAR (100) NULL,
    [Hidden]             BIT            NULL,
    [CreatedBy]          NVARCHAR (150) NULL,
    [CreatedDateTime]    DATETIME       NULL,
    [ModifiedBy]         NVARCHAR (150) NULL,
    [ModifiedDateTime]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([KeyRiskIndicatorId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'KeyRiskIndicatorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Riesgo', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'RiskKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grupo', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'RiskGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Año', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'BillingYear';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'KeyRiskIndicator', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

