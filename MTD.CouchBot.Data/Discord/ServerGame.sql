CREATE TABLE [Discord].[ServerGame]
(
	[ServerId] NUMERIC(20) NOT NULL,
	[GameId] INT NOT NULL,
	PRIMARY KEY ([ServerId], [GameId]),
	CONSTRAINT [FK_ServerGame_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([Id]),
	CONSTRAINT [FK_ServerGame_Team] FOREIGN KEY ([GameId]) REFERENCES [Platform].[Game] ([Id])
)
