IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Sales] (
    [SaleId] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [PaymentType] int NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [IsCardTransactionProcessed] bit NOT NULL,
    [CustomerName] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NOT NULL,
    [IsPackage] bit NOT NULL,
    CONSTRAINT [PK_Sales] PRIMARY KEY ([SaleId])
);
GO

CREATE TABLE [Suppliers] (
    [SupplierId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Suppliers] PRIMARY KEY ([SupplierId])
);
GO

CREATE TABLE [Orders] (
    [OrderId] int NOT NULL IDENTITY,
    [OrderDate] datetime2 NOT NULL,
    [SupplierId] int NOT NULL,
    [IsColored] bit NOT NULL,
    [IsOpen] bit NOT NULL,
    [ReOrdered] bit NOT NULL,
    [Comment] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_Orders_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId])
);
GO

CREATE TABLE [Products] (
    [ProductId] int NOT NULL IDENTITY,
    [ItemNumber] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [SupplierId] int NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK_Products_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId])
);
GO

CREATE TABLE [OrderItems] (
    [OrderItemId] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [QuantityOrdered] int NOT NULL,
    [QuantityReceived] int NOT NULL,
    [Comment] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([OrderItemId]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]),
    CONSTRAINT [FK_OrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId])
);
GO

CREATE TABLE [SaleItems] (
    [SaleItemId] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [SaleId] int NOT NULL,
    [Quantity] int NOT NULL,
    [NeedToOrder] bit NOT NULL,
    [IsOrdered] bit NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_SaleItems] PRIMARY KEY ([SaleItemId]),
    CONSTRAINT [FK_SaleItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]),
    CONSTRAINT [FK_SaleItems_Sales_SaleId] FOREIGN KEY ([SaleId]) REFERENCES [Sales] ([SaleId])
);
GO

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
GO

CREATE INDEX [IX_OrderItems_ProductId] ON [OrderItems] ([ProductId]);
GO

CREATE INDEX [IDX_Orders_SupplierId_OrderDate_IsOpen] ON [Orders] ([SupplierId], [OrderDate], [IsOpen]);
GO

CREATE INDEX [IX_Products_SupplierId] ON [Products] ([SupplierId]);
GO

CREATE INDEX [IX_SaleItems_ProductId] ON [SaleItems] ([ProductId]);
GO

CREATE INDEX [IX_SaleItems_SaleId] ON [SaleItems] ([SaleId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240705155425_InitialCreate', N'8.0.4');
GO

COMMIT;
GO

