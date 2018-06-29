CREATE TABLE [WebApp].[Log4Net_Log] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Date]          DATETIME        NOT NULL,
    [Thread]        NVARCHAR (255)  NOT NULL,
    [Level]         NVARCHAR (50)   NOT NULL,
    [Logger]        NVARCHAR (255)  NOT NULL,
    [User]          NVARCHAR (50)   NULL,
    [Message]       NVARCHAR (4000) NOT NULL,
    [Exception]     NVARCHAR (2000) NULL,
    [MachineName]   NVARCHAR (255)  NULL,
    [ApplicationId] INT             NULL,
    [IpAddress]     NVARCHAR (100)  NULL,
    [PageUrl]       NVARCHAR (100)  NULL,
    [ReferrerUrl]   NVARCHAR (100)  NULL
);

