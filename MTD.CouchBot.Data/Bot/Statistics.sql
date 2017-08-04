CREATE TABLE [Bot].[Statistics]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[YouTubeAlertAlertCount] INT NOT NULL,
	[TwitchAlertCount] INT NOT NULL,
	[MixerAlertCount] INT NOT NULL,
	[SmashcastAlertCount] INT NOT NULL,
	[PicartoAlertCount] INT NOT NULL,
	[VidMeAlertCount] INT NOT NULL,
	[UptimeMinutes] INT NOT NULL,
	[HaiBaiCount] INT NOT NULL,
	[FlipCount] INT NOT NULL,
	[UnflipCount] INT NOT NULL,
	[LoggingStartDate] DATETIME NOT NULL,
	[LastRestartDate] DATETIME NOT NULL
)
