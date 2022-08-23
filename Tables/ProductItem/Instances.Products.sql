-- The name of the product is not required to be unique,
-- products can have the same name but have different manufacturers.
-- VARCHAR(MAX) bad practice

CREATE TABLE [Instances].[Products]
(
    [InstanceId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Name] VARCHAR(256) NOT NULL,
    [Description] VARCHAR(256) NOT NULL,
    [ProductImageUris] VARCHAR(MAX) NOT NULL,
    [ValidSkus] VARCHAR(MAX) NOT NULL,
    [SystemOwned] BIT NOT NULL DEFAULT 0,
    [CreatedTimestamp] DATETIME2(7) NOT NULL DEFAULT SYSUTCDATETIME()
)

GO

CREATE INDEX [IX_Products_Name] ON [Instances].[Products] ([Name] ASC, [InstanceId] ASC)