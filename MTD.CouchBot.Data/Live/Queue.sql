CREATE TABLE [Live].[Queue]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ChannelId] int NOT NULL,
	[PlatformId] int NOT NULL,
	[ServerId] NUMERIC(20) NOT NULL,
	[MessageId] NUMERIC(20) NOT NULL,
	[CreatedDate] DATETIME NOT NULL
)
