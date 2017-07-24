CREATE TABLE [Discord].[ServerUser]
(
	[ServerId] NUMERIC(20) NOT NULL,
	[UserId] NUMERIC(20) NOT NULL,
	PRIMARY KEY ([ServerId], [UserId])
)
