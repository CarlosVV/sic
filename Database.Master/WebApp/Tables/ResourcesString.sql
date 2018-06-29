CREATE TABLE [WebApp].[ResourcesString] (
    [ResourcesStringId] INT            IDENTITY (1, 1) NOT NULL,
    [ResourceName]      NVARCHAR (200) NOT NULL,
    [ResourceValue]     NVARCHAR (MAX) NOT NULL,
    [CreatedBy]         NVARCHAR (150) NOT NULL,
    [CreatedDateTime]   DATETIME       NOT NULL,
    [ModifiedBy]        NVARCHAR (150) NULL,
    [ModifiedDateTime]  DATETIME       NULL,
    CONSTRAINT [PK_ResourcesString] PRIMARY KEY CLUSTERED ([ResourcesStringId] ASC)
);

