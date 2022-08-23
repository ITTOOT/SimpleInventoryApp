-- standard link table where each category can have any number of other categories. 
-- And each category can have any number custom attributes. 
-- The primary keys are utilized to provide uniqueness on the link tables.

CREATE TABLE [Instances].[CategoryAttributes]
(
	[InstanceId] INT NOT NULL,
	[Key] VARCHAR(64) NOT NULL,
	[Value] VARCHAR(4096) NOT NULL,

	CONSTRAINT [PK_CategoryAttributes]
		PRIMARY KEY ([InstancesId], [Key]),
	CONSTRAINT [FK_CategoryAttributes_Categories]
		FOREIGN KEY ([InstanceId])
		REFERENCES [Instances].[Categories]([InstanceId]) ON DELETE CASCADE
)