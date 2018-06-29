CREATE TABLE [Entity].[ParticipantStatus] (
    [ParticipantStatusId]       INT            IDENTITY (1, 1) NOT NULL,
    [ParticipantStatusCode]     NVARCHAR (1)   NOT NULL,
    [ParticipantStatus]         NVARCHAR (50)  NOT NULL,
    [ParticipantStatusCategory] NVARCHAR (50)  NOT NULL,
    [Hidden]                    BIT            NULL,
    [CreatedBy]                 NVARCHAR (150) NULL,
    [CreatedDateTime]           DATETIME       NULL,
    [ModifiedBy]                NVARCHAR (150) NULL,
    [ModifiedDateTime]          DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ParticipantStatusId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'ParticipantStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de participante', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'ParticipantStatusCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estatus de participante', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'ParticipantStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Categoría de estatus', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'ParticipantStatusCategory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantStatus', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

