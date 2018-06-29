CREATE TABLE [Entity].[Gender] (
    [GenderId]         INT            NOT NULL,
    [GenderCode]       NVARCHAR (1)   NULL,
    [Gender]           NVARCHAR (10)  NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([GenderId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'GenderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de género, solo se aceptan: F o M', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'GenderCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción de género', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'Gender';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Gender', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

