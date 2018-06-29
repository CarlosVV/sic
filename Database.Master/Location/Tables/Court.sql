CREATE TABLE [Location].[Court] (
    [CourtId]      INT            IDENTITY (1, 1) NOT NULL,
    [CourtName]    NVARCHAR (180) NULL,
    [S_ROWID]      DECIMAL (18)   NULL,
    [AddressLine1] NVARCHAR (30)  NULL,
    [AddressLine2] NVARCHAR (30)  NULL,
    [City]         NVARCHAR (30)  NULL,
    [Region]       NVARCHAR (35)  NULL,
    [ZipCode]      NVARCHAR (5)   NULL,
    [ZipCodeExt]   NVARCHAR (4)   NULL,
    PRIMARY KEY CLUSTERED ([CourtId] ASC)
);

