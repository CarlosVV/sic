CREATE TABLE [WebApp].[ActivityLogType] (
    [ActivityLogTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [SystemKeyword]     NVARCHAR (50)  NOT NULL,
    [Name]              NVARCHAR (100) NOT NULL,
    [IsEnabled]         BIT            NOT NULL,
    [CreatedBy]         NVARCHAR (150) NOT NULL,
    [CreatedDateTime]   DATETIME       NOT NULL,
    [ModifiedBy]        NVARCHAR (150) NULL,
    [ModifiedDateTime]  DATETIME       NULL,
    CONSTRAINT [PK_ActivityLogType] PRIMARY KEY CLUSTERED ([ActivityLogTypeId] ASC)
);

