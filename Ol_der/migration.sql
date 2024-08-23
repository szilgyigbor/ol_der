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

BEGIN TRANSACTION;
GO

CREATE TABLE [Notes] (
    [NoteId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    [IsCompleted] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Notes] PRIMARY KEY ([NoteId])
);
GO

CREATE TABLE [Warranty] (
    [WarrantyId] int NOT NULL IDENTITY,
    [CustomerName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PhoneNumber] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    [FulfilledDate] datetime2 NOT NULL,
    [ProductId] int NOT NULL,
    [SupplierId] int NOT NULL,
    [IsCompleted] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Warranty] PRIMARY KEY ([WarrantyId]),
    CONSTRAINT [FK_Warranty_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Warranty_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Warranty_ProductId] ON [Warranty] ([ProductId]);
GO

CREATE INDEX [IX_Warranty_SupplierId] ON [Warranty] ([SupplierId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240706070120_AddWarriantyAndNotes', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [WarrantyStatus] (
    [WarrantyStatusId] int NOT NULL IDENTITY,
    [WarrantyId] int NOT NULL,
    [StatusDescription] nvarchar(max) NOT NULL,
    [StatusDate] datetime2 NOT NULL,
    CONSTRAINT [PK_WarrantyStatus] PRIMARY KEY ([WarrantyStatusId]),
    CONSTRAINT [FK_WarrantyStatus_Warranty_WarrantyId] FOREIGN KEY ([WarrantyId]) REFERENCES [Warranty] ([WarrantyId])
);
GO

CREATE INDEX [IX_WarrantyStatus_WarrantyId] ON [WarrantyStatus] ([WarrantyId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240823070552_AddWarrantyStatus', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Warranty] DROP CONSTRAINT [FK_Warranty_Products_ProductId];
GO

ALTER TABLE [Warranty] DROP CONSTRAINT [FK_Warranty_Suppliers_SupplierId];
GO

ALTER TABLE [WarrantyStatus] DROP CONSTRAINT [FK_WarrantyStatus_Warranty_WarrantyId];
GO

ALTER TABLE [WarrantyStatus] DROP CONSTRAINT [PK_WarrantyStatus];
GO

ALTER TABLE [Warranty] DROP CONSTRAINT [PK_Warranty];
GO

EXEC sp_rename N'[WarrantyStatus]', N'WarrantyStatuses';
GO

EXEC sp_rename N'[Warranty]', N'Warranties';
GO

EXEC sp_rename N'[WarrantyStatuses].[IX_WarrantyStatus_WarrantyId]', N'IX_WarrantyStatuses_WarrantyId', N'INDEX';
GO

EXEC sp_rename N'[Warranties].[IX_Warranty_SupplierId]', N'IX_Warranties_SupplierId', N'INDEX';
GO

EXEC sp_rename N'[Warranties].[IX_Warranty_ProductId]', N'IX_Warranties_ProductId', N'INDEX';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Warranties]') AND [c].[name] = N'PhoneNumber');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Warranties] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Warranties] ALTER COLUMN [PhoneNumber] nvarchar(max) NOT NULL;
GO

ALTER TABLE [WarrantyStatuses] ADD CONSTRAINT [PK_WarrantyStatuses] PRIMARY KEY ([WarrantyStatusId]);
GO

ALTER TABLE [Warranties] ADD CONSTRAINT [PK_Warranties] PRIMARY KEY ([WarrantyId]);
GO

ALTER TABLE [Warranties] ADD CONSTRAINT [FK_Warranties_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE CASCADE;
GO

ALTER TABLE [Warranties] ADD CONSTRAINT [FK_Warranties_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId]) ON DELETE CASCADE;
GO

ALTER TABLE [WarrantyStatuses] ADD CONSTRAINT [FK_WarrantyStatuses_Warranties_WarrantyId] FOREIGN KEY ([WarrantyId]) REFERENCES [Warranties] ([WarrantyId]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240823101859_ChangeWarrantyPhoneNumberType', N'8.0.4');
GO

COMMIT;
GO

