CREATE TABLE [dbo].[AlertSubscribers]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Name] NVARCHAR(200) NOT NULL Unique, 
    [SMS] NVARCHAR(20) NOT NULL
)
