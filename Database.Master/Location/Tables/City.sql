CREATE TABLE [Location].[City] (
    [CityId]           INT            IDENTITY (1, 1) NOT NULL,
    [City]             NVARCHAR (100) NOT NULL,
    [StateId]          INT            NULL,
    [CountryId]        INT            NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([CityId] ASC),
    CONSTRAINT [FK_City_State] FOREIGN KEY ([StateId]) REFERENCES [Location].[State] ([StateId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'CityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ciudad', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'City';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[State] que contiene el estado', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'StateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Country] que contiene el país', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'CountryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'City', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

