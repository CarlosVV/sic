CREATE TABLE [WebApp].[Setting] (
    [SettingId]        INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (200)  NOT NULL,
    [Value]            NVARCHAR (2000) NOT NULL,
    [Description]      NVARCHAR (MAX)  NOT NULL,
    [CreatedBy]        NVARCHAR (150)  NOT NULL,
    [CreatedDateTime]  DATETIME        NOT NULL,
    [ModifiedBy]       NVARCHAR (150)  NULL,
    [ModifiedDateTime] DATETIME        NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([SettingId] ASC)
);

