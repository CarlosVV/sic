CREATE TABLE [Entity].[Relationship] (
    [RelationshipId]     INT            IDENTITY (1, 1) NOT NULL,
    [EntityId]           INT            NULL,
    [EntityId_Related]   INT            NULL,
    [RelationshipTypeId] INT            NULL,
    [Deleted]            BIT            NULL,
    [DeletedDate]        DATETIME       NULL,
    [CreatedBy]          NVARCHAR (150) NULL,
    [CreatedDateTime]    DATETIME       NULL,
    [ModifiedBy]         NVARCHAR (150) NULL,
    [ModifiedDateTime]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RelationshipId] ASC),
    CONSTRAINT [FK_Relationship_Entity] FOREIGN KEY ([EntityId]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_Relationship_Entity_Related] FOREIGN KEY ([EntityId_Related]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_Relationship_RelationshipType] FOREIGN KEY ([RelationshipTypeId]) REFERENCES [Entity].[RelationshipType] ([RelationshipTypeId])
);


GO
CREATE NONCLUSTERED INDEX [NCLIX_EntityIdRelated_RelationshipTypeId]
    ON [Entity].[Relationship]([EntityId_Related] ASC)
    INCLUDE([RelationshipTypeId]);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'RelationshipId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[Entity] con persona 1', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[Entity] con persona 2 relacionada a persona 1', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'EntityId_Related';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de relación entre persona 1 y persona 2', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'RelationshipTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si se borró el record', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'Deleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha en que se encontró que el record fue borrado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'DeletedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Relationship', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

