CREATE TABLE [Discord].[GuildUser]
(
	[GuildId] INT NOT NULL,
	[UserId] INT NOT NULL,
	CONSTRAINT [FK_GuildUser_Guild] FOREIGN KEY ([GuildId]) REFERENCES [Discord].[Guild] ([Id]),
	CONSTRAINT [FK_GuildUser_User] FOREIGN KEY ([UserId]) REFERENCES [Discord].[User] ([Id])
)
