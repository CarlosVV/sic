CREATE TABLE [Location].[Region] (
    [RegionId]         INT            IDENTITY (1, 1) NOT NULL,
    [Region]           NVARCHAR (80)  NOT NULL,
    [RegionCode]       NVARCHAR (2)   NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RegionId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'RegionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Región', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'Region';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Region', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

