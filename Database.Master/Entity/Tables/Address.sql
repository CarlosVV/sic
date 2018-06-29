CREATE TABLE [Entity].[Address] (
    [AddressId]        INT            IDENTITY (1, 1) NOT NULL,
    [EntityId]         INT            NOT NULL,
    [EntityBk]         BIGINT         NOT NULL,
    [SourceId]         INT            NOT NULL,
    [AddressBk]        INT            NULL,
    [UniqueHashId]     NVARCHAR (50)  NULL,
    [FullAddress]      NVARCHAR (230) NULL,
    [Line1]            NVARCHAR (60)  NULL,
    [Line2]            NVARCHAR (60)  NULL,
    [CityId]           INT            NULL,
    [OtherCity]        NVARCHAR (50)  NULL,
    [StateId]          INT            NULL,
    [CountryId]        INT            NULL,
    [ZipCode]          NVARCHAR (5)   NULL,
    [ZipCodeExt]       NVARCHAR (4)   NULL,
    [AddressTypeId]    INT            NULL,
    [XCord]            NVARCHAR (20)  NULL,
    [YCord]            NVARCHAR (20)  NULL,
    [Status]           NVARCHAR (30)  NULL,
    [DeliverPl]        NVARCHAR (2)   NULL,
    [CkDig]            NVARCHAR (1)   NULL,
    [Letterman]        NVARCHAR (8)   NULL,
    [Deleted]          BIT            NULL,
    [DeletedDate]      DATETIME       NULL,
    [ETLFingerprint]   NVARCHAR (50)  NULL,
    [CreatedBy]        NVARCHAR (150) NULL,
    [CreatedDateTime]  DATETIME       NULL,
    [ModifiedBy]       NVARCHAR (150) NULL,
    [ModifiedDateTime] DATETIME       NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressId] ASC),
    CONSTRAINT [FK_Address_AddressType] FOREIGN KEY ([AddressTypeId]) REFERENCES [Entity].[AddressType] ([AddressTypeId]),
    CONSTRAINT [FK_Address_City] FOREIGN KEY ([CityId]) REFERENCES [Location].[City] ([CityId]),
    CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryId]) REFERENCES [Location].[Country] ([CountryId]),
    CONSTRAINT [FK_Address_Entity] FOREIGN KEY ([EntityId]) REFERENCES [Entity].[Entity] ([EntityId]),
    CONSTRAINT [FK_Address_State] FOREIGN KEY ([StateId]) REFERENCES [Location].[State] ([StateId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la tabla', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'AddressId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[Entity] que contiene información de la persona', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'EntityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id en la fuente de la persona', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'EntityBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id de la fuente de la persona', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'SourceId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Si existe un Id de la dirección se guarda en esta columna', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'AddressBk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id creado por el sistema para la dirección', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'UniqueHashId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dirección completa', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'FullAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Línea 1 de la dirección', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'Line1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Línea 2 de la dirección', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'Line2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[City] que contiene la ciudad', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'CityId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[State] que contiene el estado', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'StateId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Location].[Country] que contiene el país', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'CountryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Primeros 5 dígitos del código postal', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'ZipCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Últimos 4 dígitos del código postal', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'ZipCodeExt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id a la tabla [Entity].[AddressType] que contiene el tipo de dirección', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'AddressTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Latitud', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'XCord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Longitud', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'YCord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estatus de la dirección', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Punto de entrega', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'DeliverPl';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cartero', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'Letterman';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si la dirección fue borrada en la fuente', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'Deleted';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha en que se encontró que fue borrada', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'DeletedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Huella del record', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'ETLFingerprint';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'CreatedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de creación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'CreatedDateTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Modificado por', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'ModifiedBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de modificación', @level0type = N'SCHEMA', @level0name = N'Entity', @level1type = N'TABLE', @level1name = N'Address', @level2type = N'COLUMN', @level2name = N'ModifiedDateTime';

