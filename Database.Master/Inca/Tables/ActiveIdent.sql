﻿CREATE TABLE [Inca].[ActiveIdent] (
    [ActiveIdentId]    INT            IDENTITY (1, 1) NOT NULL,
    [ActiveIdentCode]  NVARCHAR (1)   NULL,
    [ActiveIdent]      NVARCHAR (50)  NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ActiveIdentId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'ActiveIdentId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de clave de activo o inactivo', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'ActiveIdentCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción clave de activo o inactivo', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'ActiveIdent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Inca', @level1type = N'TABLE', @level1name = N'ActiveIdent', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

