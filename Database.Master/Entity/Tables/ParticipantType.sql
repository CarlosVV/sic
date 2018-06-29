CREATE TABLE [Entity].[ParticipantType] (
    [ParticipantTypeId] INT            NOT NULL,
    [ParticipantType]   NVARCHAR (50)  NOT NULL,
    [Status]            BIT            NOT NULL,
    [AllowsPayment]     BIT            NOT NULL,
    [RequiresSSN]       BIT            NOT NULL,
    [Hidden]            BIT            NULL,
    [CreatedBy]         NVARCHAR (150) NULL,
    [CreatedDateTime]   DATETIME       NULL,
    [ModifiedBy]        NVARCHAR (150) NULL,
    [ModifiedDateTime]  DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ParticipantTypeId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'ParticipantTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de participante', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'ParticipantType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estatus', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Permite pagos', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'AllowsPayment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Requiere Seguro Social', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'RequiresSSN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'ParticipantType', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

