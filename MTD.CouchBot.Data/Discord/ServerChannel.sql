CREATE TABLE [Discord].[ServerChannel]
(
	[ServerId] NUMERIC(20) NOT NULL,
	[ChannelId] INT NOT NULL,
	[IsOwner] BIT NOT NULL,
	PRIMARY KEY ([ServerId], [ChannelId]),
	CONSTRAINT [FK_ServerChannel_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([Id]),
	CONSTRAINT [FK_ServerChannel_Channel] FOREIGN KEY ([ChannelId]) REFERENCES [Platform].[Channel] ([Id])
)
