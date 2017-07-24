CREATE TABLE [Activity].[LiveNotification]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ChannelId] nvarchar(64) NOT NULL,
	[PlatformId] int NOT NULL,
	[ServerId] NUMERIC(20) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [FK_LiveNotification_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([ID]),
	CONSTRAINT [FK_FunEvent_Platform] FOREIGN KEY ([PlatformId]) REFERENCES [Platform].[Platform] ([ID])
)
