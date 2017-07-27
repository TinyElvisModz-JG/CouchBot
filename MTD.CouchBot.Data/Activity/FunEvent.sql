CREATE TABLE [Activity].[FunEvent]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[ServerId] NUMERIC(20) NOT NULL,
	[UserId] NUMERIC(20) NOT NULL,
	[FunEventTypeId] INT NOT NULL,
    [CreatedDate] INT NOT NULL,
	CONSTRAINT [FK_FunEvent_FunEventType] FOREIGN KEY ([FunEventTypeId]) REFERENCES [Activity].[FunEventType] ([Id]),
	CONSTRAINT [FK_FunEvent_Server] FOREIGN KEY ([ServerId]) REFERENCES [Discord].[Server] ([Id])
)
