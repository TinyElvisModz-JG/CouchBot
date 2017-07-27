CREATE TABLE [Activity].[LiveNotification]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ChannelId] INT NOT NULL,
	[PlatformId] int NOT NULL,
	[ServerId] NUMERIC(20) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [FK_LiveNotification_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([Id]),
	CONSTRAINT [FK_LiveNotification_Channel] FOREIGN KEY ([ChannelId]) REFERENCES [Platform].[Channel] ([Id]),
	CONSTRAINT [FK_FunEvent_Platform] FOREIGN KEY ([PlatformId]) REFERENCES [Platform].[Platform] ([Id])
)
