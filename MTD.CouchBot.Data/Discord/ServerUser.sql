CREATE TABLE [Discord].[ServerUser]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ServerId] NUMERIC(20) NOT NULL,
	[UserId] NUMERIC(20) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [FK_ServerUser_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([Id]),
)
