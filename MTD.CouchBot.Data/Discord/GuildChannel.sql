CREATE TABLE [Discord].[GuildChannel]
(
	[GuildId] INT NOT NULL,
	[ChannelId] INT NOT NULL,
	[IsOwner] BIT NOT NULL,
	PRIMARY KEY ([GuildId], [ChannelId]),
	CONSTRAINT [FK_GuildChannel_Guild] FOREIGN KEY ([GuildId]) REFERENCES [Discord].[Guild] ([Id]),
	CONSTRAINT [FK_GuildChannel_Channel] FOREIGN KEY ([ChannelId]) REFERENCES [Platform].[Channel] ([Id])
)
