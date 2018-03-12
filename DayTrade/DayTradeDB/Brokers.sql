CREATE TABLE [dbo].[Brokers]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL
)

GO

CREATE UNIQUE INDEX [IX_Brokers_Name] ON [dbo].[Brokers] ([Name])
