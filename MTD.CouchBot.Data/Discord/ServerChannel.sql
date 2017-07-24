CREATE TABLE [Discord].[ServerChannel]
(
	[ServerId] NUMERIC(20) NOT NULL,
	[ChannelId] INT NOT NULL,
	[IsOwner] BIT NOT NULL,
	PRIMARY KEY ([ServerId], [ChannelId])
)
