CREATE TABLE [Entity].[Phone] (
    [PhoneId]          INT            IDENTITY (1, 1) NOT NULL,
    [EntityId]         INT            NOT NULL,
    [EntityBk]         BIGINT         NOT NULL,
    [SourceId]         INT            NOT NULL,
    [PhoneNumber]      NVARCHAR (25)  NOT NULL,
    [PhoneExtension]   NVARCHAR (10)  NULL,
    [PhoneTypeId]      INT            NULL,
    [Status]           NVARCHAR (30)  NULL,
    [Deleted]          BIT            NULL,
    [DeletedDate]      DATETIME       NULL,
    [ETLFingerprint]   NVARCHAR (50)  NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    CONSTRAINT [FK_Phone_Entity] FOREIGN KEY ([EntityId]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_Phone_PhoneType] FOREIGN KEY ([PhoneTypeId]) REFERENCES [Entity].[PhoneType] ([PhoneTypeId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'PhoneId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla Entity.Entity que contiene datos del Lesionado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la fuente del Lesionado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'EntityBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fuente de donde se obtuvo el lesionado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'SourceId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número de teléfono', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'PhoneNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Extensión si tiene', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'PhoneExtension';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[PhoneType] que contiene el tipo de teléfono', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'PhoneTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estatus del record', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si se borró el record', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'Deleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha en que se encontró que el record fue borrado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'DeletedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Metadata', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'ETLFingerprint';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Phone', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

