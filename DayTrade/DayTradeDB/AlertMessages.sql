CREATE TABLE [dbo].[AlertMessages]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [SubscriptionId] INT NOT NULL, 
    [SubscriberId] INT NOT NULL, 
    [Message] NVARCHAR(MAX) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL DEFAULT GetDate(), 
    [SentDate] DATETIME NULL, 
    [Timestamp] DATETIME NOT NULL, 
    [Interval] NVARCHAR(3) NOT NULL, 
    [EndTimestamp] DATETIME NOT NULL, 
    CONSTRAINT [FK_AlertRaised_ToAlertSubscriptions] FOREIGN KEY ([SubscriptionId]) REFERENCES [AlertSubscriptions]([Id]),
    CONSTRAINT [FK_AlertRaised_ToAlertSibscribers] FOREIGN KEY ([SubscriberId]) REFERENCES [AlertSubscribers]([Id])
)

GO

CREATE INDEX [IX_AlertMessages_SubscriptionId] ON [dbo].[AlertMessages] ([SubscriptionId])
GO
CREATE INDEX [IX_AlertMessages_SubscriberId] ON [dbo].[AlertMessages] ([SubscriberId])
