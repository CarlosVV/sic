CREATE TABLE [Inca].[ActiveOnOff] (
    [ActiveOnOffId]    INT            NOT NULL,
    [ActiveOnOff]      NVARCHAR (50)  NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActiveOnOffId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveOnOff', @level2type = N'COLUMN', @level2name = N'ActiveOnOffId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clave de activo', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveOnOff', @level2type = N'COLUMN', @level2name = N'ActiveOnOff';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveOnOff', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveOnOff', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveOnOff', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveOnOff', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveOnOff', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

