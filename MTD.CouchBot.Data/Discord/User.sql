CREATE TABLE [Discord].[User]
(
	[Id] NUMERIC(20) NOT NULL PRIMARY KEY,
	[Name] varchar(64) NOT NULL,
	[CreatedDate] datetime NOT NULL
)
