CREATE TABLE [Payment].[Status] (
    [StatusId]         INT            IDENTITY (1, 1) NOT NULL,
    [StatusCode]       NVARCHAR (1)   NULL,
    [Status]           NVARCHAR (50)  NULL,
    [Effect]           NVARCHAR (1)   NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([StatusId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'StatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de estatus', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'StatusCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estatus del pago', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Efecto', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'Effect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Payment', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

