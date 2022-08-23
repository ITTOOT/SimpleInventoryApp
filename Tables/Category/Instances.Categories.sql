-- if you try to create a category with the same name, SQL will reject it.
-- However, for products/items it would not.

CREATE TABLE [Instances].[Categories]
(
	[InstanceId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(64) NOT NULL,
	[Description] VARCHAR(256) NOT NULL,
	[SystemOwned] BIT NOT NULL DEFAULT 0,
	[CreatedTimestamp] DATETIME(7) NOT NULL DEFAULT SYSUTCDATETIME()
)

GO

CREATE UNIQUE INDEX [UQ_Categories_Name] ON [Instances].[Categories] ([Name])
