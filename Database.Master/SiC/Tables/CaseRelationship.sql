CREATE TABLE [SiC].[CaseRelationship] (
    [CaseRelationshipId] INT            IDENTITY (1, 1) NOT NULL,
    [CaseId1]            INT            NOT NULL,
    [CaseNumber1]        NVARCHAR (15)  NOT NULL,
    [CaseId2]            INT            NOT NULL,
    [CaseNumber2]        NVARCHAR (15)  NOT NULL,
    [Hidden]             BIT            NULL,
    [CreatedBy]          NVARCHAR (150) NULL,
    [CreatedDateTime]    DATETIME       NULL,
    [ModifiedBy]         NVARCHAR (150) NULL,
    [ModifiedDateTime]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([CaseRelationshipId] ASC)
);

