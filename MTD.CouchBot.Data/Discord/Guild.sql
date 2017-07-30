﻿CREATE TABLE [Discord].[Guild]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[GuildId] NVARCHAR(24) NOT NULL,
	[OwnerId] NVARCHAR(24) NOT NULL,
	[AnnouncementsChannel] NVARCHAR(24) NULL,
	[LiveChannel] NVARCHAR(24) NULL,
	[GreetingsChannel] NVARCHAR(24) NULL,
	[OwnerLiveChannel] NVARCHAR(24) NULL,
	[OwnerPublishedChannel] NVARCHAR(24) NULL,
	[PublishedChannel] NVARCHAR(24) NULL,
	[OwnerTwitchFeedChannel] NVARCHAR(24) NULL,
	[TwitchFeedChannel] NVARCHAR(24) NULL,
	[AllowEveryone] BIT NOT NULL,
	[AllowThumbnails] BIT NOT NULL,
	[AllowGreetings] BIT NOT NULL,
	[AllowGoodbyes] BIT NOT NULL,
	[AllowPublished] BIT NOT NULL,
	[AllowLive] BIT NOT NULL,
	[GreetingMessage] NVARCHAR(1024) NULL,
	[GoodbyeMessage] NVARCHAR(1024) NULL,
	[PublishedMessage] NVARCHAR(1024) NULL,
	[LiveMessage] NVARCHAR(1024) NULL,
	[TimeZoneOffset] FLOAT NOT NULL DEFAULT (0),
	[YtgDomainPublished] BIT NOT NULL,
	[UseTextAnnouncements] BIT NOT NULL,
	[DeleteWhenOffline] BIT NOT NULL,
	[MentionRole] NVARCHAR(24) NULL,
	[AllowChannelFeed] BIT NOT NULL,
	[AllowOwnerChannelFeed] BIT NOT NULL,
	[StreamOfflineMessage] NVARCHAR(512) NULL
)