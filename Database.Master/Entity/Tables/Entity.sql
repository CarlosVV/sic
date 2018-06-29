CREATE TABLE [Entity].[Entity] (
    [EntityId]            INT              NOT NULL,
    [MergeId]             INT              NULL,
    [EntityBk]            BIGINT           NOT NULL,
    [SourceId]            INT              NOT NULL,
    [FirstName]           NVARCHAR (50)    NULL,
    [MiddleName]          NVARCHAR (50)    NULL,
    [LastName]            NVARCHAR (50)    NULL,
    [SecondLastName]      NVARCHAR (50)    NULL,
    [FullName]            NVARCHAR (200)   NULL,
    [SSN]                 NVARCHAR (11)    NULL,
    [GenderId]            INT              NULL,
    [BirthDate]           DATE             NULL,
    [MarriageDate]        DATE             NULL,
    [CivilStatusId]       INT              NULL,
    [ParticipantTypeId]   INT              NULL,
    [ParticipantStatusId] INT              NULL,
    [Deleted]             BIT              NULL,
    [DeletedDate]         DATETIME         NULL,
    [ETLFingerprint]      VARBINARY (8000) NULL,
    [CreatedBy]           NVARCHAR (150)   NULL,
    [CreatedDateTime]     DATETIME         NULL,
    [ModifiedBy]          NVARCHAR (150)   NULL,
    [ModifiedDateTime]    DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([EntityId] ASC),
    CONSTRAINT [FK_Entity_CivilStatus] FOREIGN KEY ([CivilStatusId]) REFERENCES [Entity].[CivilStatus] ([CivilStatusId]),
    CONSTRAINT [FK_Entity_Gender] FOREIGN KEY ([GenderId]) REFERENCES [Entity].[Gender] ([GenderId]),
    CONSTRAINT [FK_Entity_ParticipantStatus] FOREIGN KEY ([ParticipantStatusId]) REFERENCES [Entity].[ParticipantStatus] ([ParticipantStatusId]),
    CONSTRAINT [FK_Entity_ParticipantType] FOREIGN KEY ([ParticipantTypeId]) REFERENCES [Entity].[ParticipantType] ([ParticipantTypeId])
);


GO
CREATE NONCLUSTERED INDEX [NCLIX_Entity_EntityId]
    ON [Entity].[Entity]([EntityId] ASC)
    INCLUDE([FullName], [SSN]);


GO
CREATE UNIQUE NONCLUSTERED INDEX [NCLIX_EntityId_FullName]
    ON [Entity].[Entity]([EntityId] ASC)
    INCLUDE([FullName]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id único creado en nuestro sistema para cada entidad de cada fuente', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de unificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'MergeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la entidad en la fuente', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'EntityBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la fuente', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'SourceId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Segundo nombre', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'MiddleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Primer apellido', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'LastName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Segundo apellido', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'SecondLastName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre completo', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'FullName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Seguro Social', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'SSN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id externo a tabla [Entity].[Gender] que contiene el género', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'GenderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de cumpleaños', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'BirthDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id externo a tabla [Entity].[CivilStatus] que contiene el estado civil', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'CivilStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id externo a tabla [Entity].[ParticipantType] que contiene el tipo de participante. Ejemplo: Lesionado, Beneficiario, etc.', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'ParticipantTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id externo a tabla [Entity].[ParticipantStatus] que contiene el tipo de participante está activo', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'ParticipantStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record fue borrado en la fuente', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'Deleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha que se detectó borrado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'DeletedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Huella del record', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'ETLFingerprint';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Entity', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

