CREATE TABLE [Live].[Queue]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ChannelId] int NOT NULL,
	[PlatformId] int NOT NULL,
	[GuildId] INT NOT NULL,
	[MessageId] NVARCHAR(24) NOT NULL,
	[DeleteOffline] BIT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [FK_LiveQueue_Guild] FOREIGN KEY ([GuildId]) REFERENCES [Discord].[Guild] ([Id]),
	CONSTRAINT [FK_LiveQueue_ChannelId] FOREIGN KEY ([ChannelId]) REFERENCES [Platform].[Channel] ([Id]),
	CONSTRAINT [FK_LiveQueue_PlatformId] FOREIGN KEY ([PlatformId]) REFERENCES [Platform].[Platform] ([Id])
)
