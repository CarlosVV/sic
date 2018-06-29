CREATE TABLE [Reports].[PaymentHistory] (
    [CheckNum]          NVARCHAR (9)   NULL,
    [ConceptCode]       NVARCHAR (2)   NULL,
    [Concept]           NVARCHAR (50)  NULL,
    [PaymentClassCode]  NVARCHAR (1)   NULL,
    [Asegurado]         NVARCHAR (50)  NULL,
    [EBT]               NVARCHAR (50)  NULL,
    [DateChangeStatus]  DATETIME       NULL,
    [DateProgramed]     DATETIME       NULL,
    [DateFrom]          DATETIME       NULL,
    [DateTo]            DATETIME       NULL,
    [Amount]            MONEY          NULL,
    [PaymentStatusCode] NVARCHAR (1)   NULL,
    [Region]            NVARCHAR (80)  NULL,
    [PaymentClass]      NVARCHAR (50)  NULL,
    [FullName]          NVARCHAR (200) NULL,
    [CaseNumber]        NVARCHAR (11)  NULL,
    [PolicyNo]          NVARCHAR (11)  NULL,
    [SSN]               NVARCHAR (11)  NULL,
    [IsInjured]         BIT            NULL,
    [InjuredFullName]   NVARCHAR (200) NULL
);

