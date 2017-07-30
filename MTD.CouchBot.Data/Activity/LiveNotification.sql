CREATE TABLE [Activity].[LiveNotification]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ChannelId] INT NOT NULL,
	[PlatformId] INT NOT NULL,
	[GuildId] INT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [FK_LiveNotification_Guild] FOREIGN KEY ([GuildId]) REFERENCES [Discord].[Guild] ([Id]),
	CONSTRAINT [FK_LiveNotification_Channel] FOREIGN KEY ([ChannelId]) REFERENCES [Platform].[Channel] ([Id]),
	CONSTRAINT [FK_FunEvent_Platform] FOREIGN KEY ([PlatformId]) REFERENCES [Platform].[Platform] ([Id])
)
