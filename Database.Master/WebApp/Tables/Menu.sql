CREATE TABLE [WebApp].[Menu] (
    [MenuId]           INT            NOT NULL,
    [MenuName]         NVARCHAR (50)  NOT NULL,
    [MenuDescription]  NVARCHAR (200) NULL,
    [MenuUrl]          NVARCHAR (100) DEFAULT (N'#') NOT NULL,
    [IsActive]         BIT            DEFAULT ((0)) NOT NULL,
    [Rendered]         BIT            DEFAULT ((1)) NOT NULL,
    [ParentId]         INT            NULL,
    [FunctionalityId]  INT            NULL,
    [DisplayOrder]     INT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]        NVARCHAR (150) NOT NULL,
    [CreatedDateTime]  DATETIME       NOT NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([MenuId] ASC),
    CONSTRAINT [FK_Menu_Functionality] FOREIGN KEY ([FunctionalityId]) REFERENCES [WebApp].[Functionality] ([FunctionalityId]),
    CONSTRAINT [FK_Menu_Menu] FOREIGN KEY ([ParentId]) REFERENCES [WebApp].[Menu] ([MenuId])
);

