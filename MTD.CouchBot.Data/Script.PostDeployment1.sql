INSERT INTO [Platform].[Platform] VALUES ('Mixer')
INSERT INTO [Platform].[Platform] VALUES ('Picarto')
INSERT INTO [Platform].[Platform] VALUES ('Smashcast')
INSERT INTO [Platform].[Platform] VALUES ('Twitch')
INSERT INTO [Platform].[Platform] VALUES ('Vidme')
INSERT INTO [Platform].[Platform] VALUES ('YouTube')
INSERT INTO [Platform].[Platform] VALUES ('YouTube Gaming')

INSERT INTO [Activity].[ActivityType] VALUES ('Flip')
INSERT INTO [Activity].[ActivityType] VALUES ('Unflip')
INSERT INTO [Activity].[ActivityType] VALUES ('Haibai')
INSERT INTO [Activity].[ActivityType] VALUES ('Greeting')
INSERT INTO [Activity].[ActivityType] VALUES ('Goodbye')
INSERT INTO [Activity].[ActivityType] VALUES ('Live')
INSERT INTO [Activity].[ActivityType] VALUES ('Offline')

INSERT INTO [Bot].[Configuration] VALUES ('DISCORDTOKEN', 'YouTubeApiKey', 'TwitchClientId', 'ApiAiKey', 'BotId', 
										  'PREFIX', 1, 'OwnerId', 1, 1, 1, 1, 1, 1, 120, 120, 
										  120, 120, 300, 900, 900)
INSERT INTO [Bot].[Statistics] VALUES (1,1,1,1,1,1,1,1,1,1,GETDATE(),GETDATE())