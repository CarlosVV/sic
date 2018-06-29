CREATE TABLE [SiC].[CaseDetail] (
    [CaseDetailId]     INT            IDENTITY (1, 1) NOT NULL,
    [CaseId]           INT            NULL,
    [EBTAccount]       NVARCHAR (9)   NULL,
    [CaseNumber]       NVARCHAR (11)  NULL,
    [CaseKey]          NVARCHAR (2)   NULL,
    [IncaBK]           INT            NULL,
    [EntityId_Inca]    INT            NULL,
    [CheckCaseBK]      INT            NULL,
    [EntityId_Check]   INT            NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([CaseDetailId] ASC),
    CONSTRAINT [FK_CaseDetail_Case] FOREIGN KEY ([CaseId]) REFERENCES [SiC].[Case] ([CaseId])
);


GO
CREATE NONCLUSTERED INDEX [NCLIX_CaseId_EBTAccount]
    ON [SiC].[CaseDetail]([CaseId] ASC)
    INCLUDE([EBTAccount]);

