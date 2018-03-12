CREATE TABLE [dbo].[Accounts]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
	[BrokerId] INT NOT NULL,
    [Number] NVARCHAR(200) NOT NULL, 
    [ApiKey] NVARCHAR(200) NOT NULL DEFAULT '', 
    [ApiSecret] NVARCHAR(200) NOT NULL DEFAULT '', 
    [AddressDeposit] NVARCHAR(200) NOT NULL DEFAULT '', 
    [Symbol] NVARCHAR(50) NULL
)

GO

CREATE INDEX [IX_Accounts_NaturalKey] ON [dbo].[Accounts] ([BrokerId],[Number])
