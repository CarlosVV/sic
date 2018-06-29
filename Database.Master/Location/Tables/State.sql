CREATE TABLE [Location].[State] (
    [StateId]          INT            IDENTITY (1, 1) NOT NULL,
    [State]            NVARCHAR (100) NOT NULL,
    [Abbreviation]     NVARCHAR (2)   NOT NULL,
    [CountryId]        INT            NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([StateId] ASC),
    CONSTRAINT [FK_State_Country] FOREIGN KEY ([CountryId]) REFERENCES [Location].[Country] ([CountryId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'StateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estado', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'State';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Country]', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'CountryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'State', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

