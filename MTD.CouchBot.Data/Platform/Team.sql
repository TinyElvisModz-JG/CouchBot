﻿CREATE TABLE [Platform].[Team]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[TeamId] INT NOT NULL,
	[Name] NVARCHAR(64) NOT NULL
)