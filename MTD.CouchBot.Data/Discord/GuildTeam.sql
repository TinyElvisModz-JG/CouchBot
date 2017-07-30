CREATE TABLE [Discord].[GuildTeam]
(
	[GuildId] INT NOT NULL,
	[TeamId] INT NOT NULL,
	CONSTRAINT [FK_GuildTeam_Guild] FOREIGN KEY ([GuildId]) REFERENCES [Discord].[Guild] ([Id]),
	CONSTRAINT [FK_GuildTeam_Team] FOREIGN KEY ([TeamId]) REFERENCES [Platform].[Team] ([Id])
)
