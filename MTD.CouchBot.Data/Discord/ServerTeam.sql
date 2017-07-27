CREATE TABLE [Discord].[ServerTeam]
(
	[ServerId] NUMERIC(20) NOT NULL,
	[TeamId] INT NOT NULL,
	PRIMARY KEY ([ServerId], [TeamId]),
	CONSTRAINT [FK_ServerTeam_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([Id]),
	CONSTRAINT [FK_ServerTeam_Team] FOREIGN KEY ([TeamId]) REFERENCES [Platform].[Team] ([Id])
)
