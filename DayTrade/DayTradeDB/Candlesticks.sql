CREATE TABLE [dbo].[Candlesticks]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Symbol] NVARCHAR(20) NOT NULL , 
    [Timestamp] DATETIME2 NOT NULL, 
	[Interval] nvarchar(3) NOT NULL,
    [Open] DECIMAL(18, 8) NOT NULL, 
    [Close] DECIMAL(18, 8) NOT NULL, 
    [ClosePercent] DECIMAL(9, 2) NOT NULL, 
    [Low] DECIMAL(18, 8) NOT NULL, 
    [LowPercent] DECIMAL(9, 2) NOT NULL, 
    [High] DECIMAL(18, 8) NOT NULL, 
    [HighPercent] DECIMAL(9, 2) NOT NULL, 
	[Volume] DECIMAL(18) NOT NULL,
	[Up] bit NOT NULL,
	[NumberOfTrades] DECIMAL (18) NOT NULL,
    [Provider] NVARCHAR(50) NULL, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT Getdate(), 
    PRIMARY KEY ([Symbol], [Timestamp], [Interval])
)

GO

CREATE NONCLUSTERED INDEX [IX_Candlesticks_Id] ON [dbo].[Candlesticks] ([Id])
