-- standard link table where each category can have any number of other categories. 
-- And each category can have any number custom attributes. 
-- The primary keys are utilized to provide uniqueness on the link tables.

CREATE TABLE [Instances].[CategoryCategories]
(
	[InstanceId] INT NOT NULL,
	[CatogoryInstanceId] INT NOT NULL,

	CONSTRAINT [PK_CategoryCategories]
		PRIMARY KEY ([InstanceId], [CategoryInstanceId]),
	CONSTRAINT [PK_CategoryCategories_Categories]
		FOREIGN KEY ([InstanceId])
		REFERENCES [Instances].[Categories]([InstancesId]) ON DELETE CASCADE,
	CONSTRAINT [PK_CategoryCategories_Categories_Categories]
		FOREIGN KEY ([CategoryInstanceId])
		REFERENCES [Instances].[Categories]([InstanceId])
)
