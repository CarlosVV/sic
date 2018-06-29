CREATE TABLE [Entity].[Master] (
    [MergeId]          INT            NOT NULL,
    [FirstName]        NVARCHAR (50)  NULL,
    [MiddleName]       NVARCHAR (50)  NULL,
    [LastName]         NVARCHAR (50)  NULL,
    [SecondLastName]   NVARCHAR (50)  NULL,
    [FullName]         NVARCHAR (200) NULL,
    [GenderId]         INT            NULL,
    [CivilStatusId]    INT            NULL,
    [BirthDate]        DATE           NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([MergeId] ASC),
    CONSTRAINT [FK_Master_CivilStatus] FOREIGN KEY ([CivilStatusId]) REFERENCES [Entity].[CivilStatus] ([CivilStatusId]),
    CONSTRAINT [FK_Master_Gender] FOREIGN KEY ([GenderId]) REFERENCES [Entity].[Gender] ([GenderId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla ', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'MergeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'FirstName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Segundo nombre', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'MiddleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Primer apellido', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'LastName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Segundo apellido', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'SecondLastName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre completo', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'FullName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id externo a tabla Entity.Gender', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'GenderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de cumpleaños', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'BirthDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Master', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

