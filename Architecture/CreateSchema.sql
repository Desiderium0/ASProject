USE [dbo].[projectb]
GO

CREATE TABLE [dbo].[Users] (
	[ID] INT PRIMARY KEY Identity(1,1),
	[Login] VARCHAR(50) UNIQUE NOT NULL,
	[PasswordHash] VARBINARY(MAX) NOT NULL,
	[Salt] VARBINARY(MAX) NOT NULL,
	[Email] VARCHAR(255) NULL,
	[FirstName] VARCHAR(50) NULL,
	[LastName] VARCHAR(50) NULL,
	[ProfilePicture] VARBINARY(MAX) NULL,
	[LastSeen] DATETIME NULL,
	[Status] VARCHAR(50) NULL,
	[IsOnline] BIT DEFAULT 0,
	[IsActive] BIT DEFAULT 1
)

CREATE TABLE [dbo].[Posts] (
	[Id] INT PRIMARY KEY IDENTITY (1, 1),
	[Author] VARCHAR (120) NOT NULL,
	[Name] NVARCHAR (120) NOT NULL,	
	[Content] NVARCHAR (MAX) NOT NULL,
	[TimeCreated] DateTime2 NOT NULL
);

