CREATE TABLE [WebApp].[Functionality] (
    [FunctionalityId]   INT            IDENTITY (1, 1) NOT NULL,
    [FunctionalityName] NVARCHAR (100) NOT NULL,
    [IsActive]          BIT            NOT NULL,
    [CreatedBy]         NVARCHAR (150) NOT NULL,
    [CreatedDateTime]   DATETIME       NOT NULL,
    [ModifiedBy]        NVARCHAR (150) NULL,
    [ModifiedDateTime]  DATETIME       NULL,
    CONSTRAINT [PK_Funcionality] PRIMARY KEY CLUSTERED ([FunctionalityId] ASC)
);

