CREATE TABLE [SiC].[ICD9] (
    [ICD9Id]             INT            NOT NULL,
    [ICD9Region]         NVARCHAR (30)  NULL,
    [ICD9Code]           DECIMAL (5, 2) NULL,
    [ICD9Description]    NVARCHAR (200) NULL,
    [CompensationRegion] NVARCHAR (30)  NULL,
    [Hidden]             BIT            NULL,
    [CreatedBy]          NVARCHAR (150) NULL,
    [CreatedDateTime]    DATETIME       NULL,
    [ModifiedBy]         NVARCHAR (150) NULL,
    [ModifiedDateTime]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ICD9Id] ASC)
);

