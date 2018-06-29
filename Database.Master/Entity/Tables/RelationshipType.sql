CREATE TABLE [Entity].[RelationshipType] (
    [RelationshipTypeId]     INT            IDENTITY (1, 1) NOT NULL,
    [RelationshipType]       NVARCHAR (50)  NOT NULL,
    [RelationshipTypeCode]   NVARCHAR (1)   NULL,
    [SchoolCertification]    BIT            NULL,
    [WidowCertification]     BIT            NULL,
    [VitalData]              BIT            NULL,
    [Handicapped]            BIT            NULL,
    [WithChildren]           BIT            NULL,
    [RelationshipCategoryId] INT            NULL,
    [Hidden]                 BIT            NULL,
    [CreatedBy]              NVARCHAR (150) NULL,
    [CreatedDateTime]        DATETIME       NULL,
    [ModifiedBy]             NVARCHAR (150) NULL,
    [ModifiedDateTime]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([RelationshipTypeId] ASC),
    CONSTRAINT [FK_RelationshipType_RelationshipCategory] FOREIGN KEY ([RelationshipCategoryId]) REFERENCES [Entity].[RelationshipCategory] ([RelationshipCategoryId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'RelationshipTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de relación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'RelationshipType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si tiene Certificación Escolar', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'SchoolCertification';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si tiene Certificación Viuda', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'WidowCertification';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si tiene Datos Vitales', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'VitalData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Discapacitados', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'Handicapped';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Con niños', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'WithChildren';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla Inca.RelationshipCategory que contiene datos de las categorías de relación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'RelationshipCategoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'RelationshipType', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

