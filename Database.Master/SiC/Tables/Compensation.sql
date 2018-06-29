CREATE TABLE [SiC].[Compensation] (
    [CompensationId]   INT            IDENTITY (1, 1) NOT NULL,
    [CompensationKey]  NVARCHAR (6)   NULL,
    [CompensationKey1] NVARCHAR (2)   NULL,
    [CompensationKey2] NVARCHAR (2)   NULL,
    [CompensationKey3] NVARCHAR (2)   NULL,
    [Description]      NVARCHAR (500) NULL,
    [Concept]          NVARCHAR (100) NULL,
    [Determination]    NVARCHAR (100) NULL,
    [Movement]         NVARCHAR (100) NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    CONSTRAINT [PK_Compensation] PRIMARY KEY CLUSTERED ([CompensationId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'CompensationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de compensación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'CompensationKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de compensación parte 1', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'CompensationKey1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de compensación parte 2', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'CompensationKey2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de compensación parte 3', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'CompensationKey3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción del código de compensación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Concepto', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'Concept';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Determinación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'Determination';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Movimiento', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'Movement';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'Compensation', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

