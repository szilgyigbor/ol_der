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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE TABLE [Suppliers] (
        [SupplierId] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Suppliers] PRIMARY KEY ([SupplierId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE TABLE [Products] (
        [ProductId] int NOT NULL IDENTITY,
        [ItemNumber] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [SupplierId] int NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([ProductId]),
        CONSTRAINT [FK_Products_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OrderItems_ProductId] ON [OrderItems] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE INDEX [IDX_Orders_SupplierId_OrderDate_IsOpen] ON [Orders] ([SupplierId], [OrderDate], [IsOpen]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_SupplierId] ON [Products] ([SupplierId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SaleItems_ProductId] ON [SaleItems] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SaleItems_SaleId] ON [SaleItems] ([SaleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240705155425_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240705155425_InitialCreate', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240706070120_AddWarriantyAndNotes'
)
BEGIN
    CREATE TABLE [Notes] (
        [NoteId] int NOT NULL IDENTITY,
        [Content] nvarchar(max) NOT NULL,
        [CreationDate] datetime2 NOT NULL,
        [IsCompleted] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Notes] PRIMARY KEY ([NoteId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240706070120_AddWarriantyAndNotes'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240706070120_AddWarriantyAndNotes'
)
BEGIN
    CREATE INDEX [IX_Warranty_ProductId] ON [Warranty] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240706070120_AddWarriantyAndNotes'
)
BEGIN
    CREATE INDEX [IX_Warranty_SupplierId] ON [Warranty] ([SupplierId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240706070120_AddWarriantyAndNotes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240706070120_AddWarriantyAndNotes', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823070552_AddWarrantyStatus'
)
BEGIN
    CREATE TABLE [WarrantyStatus] (
        [WarrantyStatusId] int NOT NULL IDENTITY,
        [WarrantyId] int NOT NULL,
        [StatusDescription] nvarchar(max) NOT NULL,
        [StatusDate] datetime2 NOT NULL,
        CONSTRAINT [PK_WarrantyStatus] PRIMARY KEY ([WarrantyStatusId]),
        CONSTRAINT [FK_WarrantyStatus_Warranty_WarrantyId] FOREIGN KEY ([WarrantyId]) REFERENCES [Warranty] ([WarrantyId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823070552_AddWarrantyStatus'
)
BEGIN
    CREATE INDEX [IX_WarrantyStatus_WarrantyId] ON [WarrantyStatus] ([WarrantyId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823070552_AddWarrantyStatus'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240823070552_AddWarrantyStatus', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [Warranty] DROP CONSTRAINT [FK_Warranty_Products_ProductId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [Warranty] DROP CONSTRAINT [FK_Warranty_Suppliers_SupplierId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [WarrantyStatus] DROP CONSTRAINT [FK_WarrantyStatus_Warranty_WarrantyId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [WarrantyStatus] DROP CONSTRAINT [PK_WarrantyStatus];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [Warranty] DROP CONSTRAINT [PK_Warranty];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    EXEC sp_rename N'[WarrantyStatus]', N'WarrantyStatuses';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    EXEC sp_rename N'[Warranty]', N'Warranties';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    EXEC sp_rename N'[WarrantyStatuses].[IX_WarrantyStatus_WarrantyId]', N'IX_WarrantyStatuses_WarrantyId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    EXEC sp_rename N'[Warranties].[IX_Warranty_SupplierId]', N'IX_Warranties_SupplierId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    EXEC sp_rename N'[Warranties].[IX_Warranty_ProductId]', N'IX_Warranties_ProductId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Warranties]') AND [c].[name] = N'PhoneNumber');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Warranties] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Warranties] ALTER COLUMN [PhoneNumber] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [WarrantyStatuses] ADD CONSTRAINT [PK_WarrantyStatuses] PRIMARY KEY ([WarrantyStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [Warranties] ADD CONSTRAINT [PK_Warranties] PRIMARY KEY ([WarrantyId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [Warranties] ADD CONSTRAINT [FK_Warranties_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([ProductId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [Warranties] ADD CONSTRAINT [FK_Warranties_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([SupplierId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    ALTER TABLE [WarrantyStatuses] ADD CONSTRAINT [FK_WarrantyStatuses_Warranties_WarrantyId] FOREIGN KEY ([WarrantyId]) REFERENCES [Warranties] ([WarrantyId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823101859_ChangeWarrantyPhoneNumberType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240823101859_ChangeWarrantyPhoneNumberType', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828074552_AddCustomerOrders'
)
BEGIN
    CREATE TABLE [CustomerOrders] (
        [CustomerOrderId] int NOT NULL IDENTITY,
        [CustomerName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(max) NOT NULL,
        [OrderDescription] nvarchar(max) NOT NULL,
        [CreationDate] datetime2 NOT NULL,
        [FulfilledDate] datetime2 NOT NULL,
        [IsCompleted] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_CustomerOrders] PRIMARY KEY ([CustomerOrderId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828074552_AddCustomerOrders'
)
BEGIN
    CREATE TABLE [CustomerOrderStatuses] (
        [CustomerOrderStatusId] int NOT NULL IDENTITY,
        [CustomerOrderId] int NOT NULL,
        [StatusDescription] nvarchar(max) NOT NULL,
        [StatusDate] datetime2 NOT NULL,
        CONSTRAINT [PK_CustomerOrderStatuses] PRIMARY KEY ([CustomerOrderStatusId]),
        CONSTRAINT [FK_CustomerOrderStatuses_CustomerOrders_CustomerOrderId] FOREIGN KEY ([CustomerOrderId]) REFERENCES [CustomerOrders] ([CustomerOrderId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828074552_AddCustomerOrders'
)
BEGIN
    CREATE INDEX [IX_CustomerOrderStatuses_CustomerOrderId] ON [CustomerOrderStatuses] ([CustomerOrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828074552_AddCustomerOrders'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240828074552_AddCustomerOrders', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250827060833_AddCustomerClass'
)
BEGIN
    ALTER TABLE [Sales] ADD [CustomerId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250827060833_AddCustomerClass'
)
BEGIN
    CREATE TABLE [Customers] (
        [CustomerId] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [TaxNumber] nvarchar(max) NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250827060833_AddCustomerClass'
)
BEGIN
    CREATE INDEX [IX_Sales_CustomerId] ON [Sales] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250827060833_AddCustomerClass'
)
BEGIN
    ALTER TABLE [Sales] ADD CONSTRAINT [FK_Sales_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250827060833_AddCustomerClass'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250827060833_AddCustomerClass', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250827163715_AddCustomers'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250827163715_AddCustomers', N'8.0.4');
END;
GO

COMMIT;
GO

