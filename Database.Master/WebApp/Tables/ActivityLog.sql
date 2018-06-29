CREATE TABLE [WebApp].[ActivityLog] (
    [ActivityLogId]     INT             IDENTITY (1, 1) NOT NULL,
    [ObjectTypeId]      INT             NOT NULL,
    [ObjectId]          NVARCHAR (20)   NULL,
    [ActivityLogTypeId] INT             NOT NULL,
    [Comment]           NVARCHAR (4000) NULL,
    [CreatedBy]         NVARCHAR (150)  NOT NULL,
    [CreatedDateTime]   DATETIME        NOT NULL,
    CONSTRAINT [PK_ActivityLog] PRIMARY KEY CLUSTERED ([ActivityLogId] ASC),
    CONSTRAINT [FK_ActivityLog_ActivityLogType] FOREIGN KEY ([ActivityLogTypeId]) REFERENCES [WebApp].[ActivityLogType] ([ActivityLogTypeId]),
    CONSTRAINT [FK_ActivityLog_ObjectType] FOREIGN KEY ([ObjectTypeId]) REFERENCES [WebApp].[ObjectType] ([ValueId])
);

