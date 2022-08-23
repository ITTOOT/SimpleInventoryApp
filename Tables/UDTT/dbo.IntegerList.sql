-- With dbo.CustomAttributeList.sql, we are able to upload a single entity with one INSERT statement.

CREATE TYPE [dbo].[IntegerList] AS TABLE
(
	[Value] INT NOT NULL
)