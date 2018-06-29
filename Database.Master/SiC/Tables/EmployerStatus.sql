CREATE TABLE [SiC].[EmployerStatus] (
    [EmployerStatusId]       INT            IDENTITY (1, 1) NOT NULL,
    [EmployerStatusBK]       INT            NOT NULL,
    [EmployerStatusDietCode] NVARCHAR (2)   NULL,
    [EmployerStatus]         NVARCHAR (200) NULL,
    [Hidden]                 BIT            NULL,
    [CreatedBy]              NVARCHAR (150) NULL,
    [CreatedDateTime]        DATETIME       NULL,
    [ModifiedBy]             NVARCHAR (150) NULL,
    [ModifiedDateTime]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([EmployerStatusId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'EmployerStatus', @level2type = N'COLUMN', @level2name = N'EmployerStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción del Estatus Patronal', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'EmployerStatus', @level2type = N'COLUMN', @level2name = N'EmployerStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'EmployerStatus', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'EmployerStatus', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'EmployerStatus', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'EmployerStatus', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'SiC', @level1type = N'TABLE', @level1name = N'EmployerStatus', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

