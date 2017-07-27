CREATE TABLE [Live].[Queue]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ChannelId] int NOT NULL,
	[PlatformId] int NOT NULL,
	[ServerId] NUMERIC(20) NOT NULL,
	[MessageId] NUMERIC(20) NOT NULL,
	[DeleteOffline] BIT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [FK_LiveQueue_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([Id]),
	CONSTRAINT [FK_LiveQueue_ChannelId] FOREIGN KEY ([ChannelId]) REFERENCES [Platform].[Channel] ([Id]),
	CONSTRAINT [FK_LiveQueue_PlatformId] FOREIGN KEY ([PlatformId]) REFERENCES [Platform].[Platform] ([Id])
)
