CREATE TABLE [SiC].[Diagnostic] (
    [DiagnosticsId]       INT           IDENTITY (1, 1) NOT NULL,
    [CaseId]              INT           NULL,
    [CASENUMBER]          NVARCHAR (15) NULL,
    [Form0395ID]          NUMERIC (18)  NULL,
    [CREATEDDATETIME]     DATETIME      NULL,
    [DiagROWID]           NUMERIC (18)  NULL,
    [ICD9_CODE]           NVARCHAR (50) NULL,
    [DIAGNOSISID]         NUMERIC (18)  NULL,
    [DIAG_STATUS_DDL]     NVARCHAR (40) NULL,
    [DIAG_DECISION_TYPE]  NVARCHAR (80) NULL,
    [DIAG_TOC_PRIM_SEC]   NVARCHAR (80) NULL,
    [DIAG_EVALDATE]       DATETIME      NULL,
    [DIAG_TOC_ORGC_EMTNL] NVARCHAR (80) NULL,
    [DIAG_TOC_RELATED]    NVARCHAR (80) NULL,
    [DIAG_TRTMNT_PERIOD]  NVARCHAR (80) NULL,
    [Revised]             SMALLINT      NULL
);

