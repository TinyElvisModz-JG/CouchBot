CREATE TABLE [Platform].[Channel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[PlatformId] int NOT NULL,
	[ChannelId] NVARCHAR(64) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
    CONSTRAINT [FK_Channel_Platform] FOREIGN KEY ([PlatformId]) REFERENCES [Platform].[Platform] ([Id])
)
