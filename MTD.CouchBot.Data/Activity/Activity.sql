CREATE TABLE [Activity].[Activity]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[GuildId] int NOT NULL,
	[UserId] int NOT NULL,
	[ActivityTypeId] INT NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [FK_Activity_Type] FOREIGN KEY ([ActivityTypeId]) REFERENCES [Activity].[ActivityType] ([Id]),
	CONSTRAINT [FK_Activity_Guild] FOREIGN KEY ([GuildId]) REFERENCES [Discord].[Guild] ([Id]),
	CONSTRAINT [FK_Activity_User] FOREIGN KEY ([UserId]) REFERENCES [Discord].[User] ([Id])
)
