CREATE TABLE [SiC].[TransactionDetail] (
    [TransactionDetailId]  INT            NOT NULL,
    [TransactionId]        INT            NULL,
    [CompensationRegionId] INT            NULL,
    [Percent]              DECIMAL (3, 2) NULL,
    [Amount]               MONEY          NULL,
    [Hidden]               BIT            NULL,
    [CreatedBy]            NVARCHAR (150) NULL,
    [CreatedDateTime]      DATETIME       NULL,
    [ModifiedBy]           NVARCHAR (150) NULL,
    [ModifiedDateTime]     DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([TransactionDetailId] ASC)
);

