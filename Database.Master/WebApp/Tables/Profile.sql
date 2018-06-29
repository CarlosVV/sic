CREATE TABLE [WebApp].[Profile] (
    [ProfileId]          INT            IDENTITY (1, 1) NOT NULL,
    [ProfileName]        NVARCHAR (30)  NOT NULL,
    [ProfileDescription] NVARCHAR (100) NULL,
    [CreatedBy]          NVARCHAR (150) NOT NULL,
    [CreatedDateTime]    DATETIME       NOT NULL,
    [ModifiedBy]         NVARCHAR (150) NULL,
    [ModifiedDateTime]   DATETIME       NULL,
    CONSTRAINT [PK_Profiles] PRIMARY KEY CLUSTERED ([ProfileId] ASC)
);

