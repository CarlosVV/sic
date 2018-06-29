CREATE TABLE [Location].[Clinic] (
    [ClinicId]         INT            IDENTITY (1, 1) NOT NULL,
    [Clinic]           NVARCHAR (80)  NOT NULL,
    [ClinicCode]       NVARCHAR (2)   NULL,
    [RegionId]         INT            NULL,
    [Hidden]           BIT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ClinicId] ASC),
    CONSTRAINT [FK_Clinic_Region] FOREIGN KEY ([RegionId]) REFERENCES [Location].[Region] ([RegionId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'ClinicId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clínica', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'Clinic';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Region] que contiene las regiones', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'RegionId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el record está oculto', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'Hidden';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Location', @level1type = N'TABLE', @level1name = N'Clinic', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

