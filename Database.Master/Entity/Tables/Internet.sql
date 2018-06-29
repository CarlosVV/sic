CREATE TABLE [Entity].[Internet] (
    [InternetId]       INT            IDENTITY (1, 1) NOT NULL,
    [EntityId]         INT            NOT NULL,
    [EntityBk]         BIGINT         NOT NULL,
    [SourceId]         INT            NOT NULL,
    [Internet]         NVARCHAR (100) NOT NULL,
    [InternetTypeId]   INT            NULL,
    [Status]           NVARCHAR (30)  NULL,
    [Deleted]          BIT            NULL,
    [DeletedDate]      DATETIME       NULL,
    [ETLFingerprint]   NVARCHAR (50)  NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    CONSTRAINT [FK_Internet_Entity] FOREIGN KEY ([EntityId]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_Internet_InternetType] FOREIGN KEY ([InternetTypeId]) REFERENCES [Entity].[InternetType] ([InternetTypeId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'InternetId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla Entity que contiene datos del Lesionado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id del lesionado en la fuente', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'EntityBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la fuente de donde se obtuvo los datos', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'SourceId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dirección de Internet o Email del Lesionado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'Internet';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[InternetType] donde indica si es un URL o un Email', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'InternetTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estatus del record', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record fue borrado en la fuente', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'Deleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha en que se encontró que fue borrado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'DeletedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Huella del record', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'ETLFingerprint';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Internet', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

