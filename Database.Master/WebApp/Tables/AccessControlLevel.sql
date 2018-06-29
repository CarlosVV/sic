CREATE TABLE [WebApp].[AccessControlLevel] (
    [AccessControlLevelId] INT            NOT NULL,
    [FunctionalityId]      INT            NOT NULL,
    [ProfileId]            INT            NOT NULL,
    [Allow]                BIT            NOT NULL,
    [CreatedBy]            NVARCHAR (150) NOT NULL,
    [CreatedDateTime]      DATETIME       NOT NULL,
    [ModifiedBy]           NVARCHAR (150) NULL,
    [ModifiedDateTime]     DATETIME       NULL,
    CONSTRAINT [PK_AccessControlLevel] PRIMARY KEY CLUSTERED ([AccessControlLevelId] ASC),
    CONSTRAINT [FK_AccessControlLevel_Functionality] FOREIGN KEY ([FunctionalityId]) REFERENCES [WebApp].[Functionality] ([FunctionalityId]),
    CONSTRAINT [FK_AccessControlLevel_Profile] FOREIGN KEY ([ProfileId]) REFERENCES [WebApp].[Profile] ([ProfileId]),
    CONSTRAINT [IX_accessControlLevel] UNIQUE NONCLUSTERED ([FunctionalityId] ASC, [ProfileId] ASC)
);

