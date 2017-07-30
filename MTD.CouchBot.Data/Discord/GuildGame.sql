CREATE TABLE [Discord].[GuildGame]
(
	[GuildId] INT NOT NULL,
	[GameId] INT NOT NULL,
	CONSTRAINT [FK_GuildGame_Guild] FOREIGN KEY ([GuildId]) REFERENCES [Discord].[Guild] ([Id]),
	CONSTRAINT [FK_GuildGame_Team] FOREIGN KEY ([GameId]) REFERENCES [Platform].[Game] ([Id])
)
