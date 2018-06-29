CREATE TABLE [SiC].[CompensationRegion] (
    [CompensationRegionId] INT            NOT NULL,
    [Code]                 INT            NULL,
    [Description]          NVARCHAR (200) NULL,
    [Weeks]                INT            NULL,
    [Region]               NVARCHAR (30)  NULL,
    [SubRegion]            NVARCHAR (30)  NULL,
    [Hidden]               BIT            NULL,
    [CreatedBy]            NVARCHAR (150) NULL,
    [CreatedDateTime]      DATETIME       NULL,
    [ModifiedBy]           NVARCHAR (150) NULL,
    [ModifiedDateTime]     DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([CompensationRegionId] ASC)
);

