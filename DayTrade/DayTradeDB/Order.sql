CREATE TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [CreatedDate] DATETIME NOT NULL DEFAULT GetDate(), 
    [ExpiredDate] DATETIME NOT NULL, 
    [SubmitAfterDate] DATETIME NOT NULL, 
    [BrokerId] INT NOT NULL, 
	[AccountId] INT nOT NULL,
    [Operation] NVARCHAR(4) NOT NULL, 
    [Symbol] NVARCHAR(50) NOT NULL, 
    [Type] NVARCHAR(50) NOT NULL, 
    [Quantity] DECIMAL(18,8) NOT NULL, 
    [Price] DECIMAL(18,8)  NOT NULL, 
    [StopPrice] DECIMAL(18,8)  NOT NULL, 
    [OpenedDate] DATETIME NULL, 
    [FilledDate] DATETIME NULL, 
    [State] NVARCHAR(50) NOT NULL, 
    [Fee] DECIMAL(18,8) NOT NULL DEFAULT 0, 
    [FeeSymbol] NVARCHAR(50) NOT NULL DEFAULT '', 
    [BrokerOrderId] NVARCHAR(50) NOT NULL DEFAULT '', 
    [TestOnly] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Order_ToBrokers] FOREIGN KEY ([BrokerId]) REFERENCES [Brokers]([Id]), 
    CONSTRAINT [FK_Order_ToAccounts] FOREIGN KEY ([AccountId]) REFERENCES [Accounts]([Id]), 
    CONSTRAINT [CK_Orders_Operation] CHECK (Operation in ('buy', 'sell')), 
    CONSTRAINT [CK_Orders_Type] CHECK (Type in ('Limit', 'Market', 'StopLoss', 'StopLossLimit', 'TakeProfitLimit', 'TakeProfit')), 
    CONSTRAINT [CK_Orders_State] CHECK (State in ('Pending', 'Opened', 'Cancelled', 'Filled')), 
    CONSTRAINT [CK_Orders_Fee] CHECK (Fee <> 0 and State='Filled'),
    CONSTRAINT [CK_Orders_FeeSymbol] CHECK (FeeSymbol <> '' and State='Filled')
)

GO

CREATE UNIQUE INDEX [IX_Orders_NaturalKey] ON [dbo].[Orders] ([BrokerId], [AccountId], [CreatedDate], [Id]) 
