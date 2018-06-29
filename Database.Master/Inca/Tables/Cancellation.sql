CREATE TABLE [Inca].[Cancellation] (
    [CancellationId]   INT            IDENTITY (1, 1) NOT NULL,
    [CancellationCode] NVARCHAR (1)   NULL,
    [Cancellation]     NVARCHAR (50)  NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([CancellationId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'CancellationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de la cancelación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'CancellationCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripció de la cancelación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'Cancellation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'Cancellation', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

