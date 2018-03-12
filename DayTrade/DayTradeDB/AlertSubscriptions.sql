CREATE TABLE [dbo].[AlertSubscriptions]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1), 
    [Symbol] NVARCHAR(20) NOT NULL, 
    [Interval] NVARCHAR(3) NOT NULL, 
    [PercentChanged] DECIMAL(5,2) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL DEFAULT GetDate(), 
    [ModifiedDate] DATETIME NOT NULL DEFAULT GetDate(), 
    [Active] BIT NOT NULL DEFAULT 1, 
    [SubscriberId] INT NOT NULL, 
    CONSTRAINT [FK_AlertSubscription_ToAlertSubscriber] FOREIGN KEY (SubscriberId) REFERENCES AlertSubscribers(Id)
)

GO

CREATE INDEX [IX_AlertSubscription_SubscriberId] ON [dbo].[AlertSubscriptions] ([SubscriberId])
