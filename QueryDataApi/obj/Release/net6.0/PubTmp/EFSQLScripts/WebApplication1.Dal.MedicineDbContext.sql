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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211209152447_intitalmigration')
BEGIN
    CREATE TABLE [medicines] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Notes] nvarchar(max) NULL,
        [ExpiryDate] datetime2 NOT NULL,
        [Quantity] int NOT NULL,
        [Brand] nvarchar(max) NULL,
        CONSTRAINT [PK_medicines] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211209152447_intitalmigration')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Brand', N'ExpiryDate', N'Name', N'Notes', N'Quantity') AND [object_id] = OBJECT_ID(N'[medicines]'))
        SET IDENTITY_INSERT [medicines] ON;
    EXEC(N'INSERT INTO [medicines] ([Id], [Brand], [ExpiryDate], [Name], [Notes], [Quantity])
    VALUES (1, NULL, ''0001-01-01T00:00:00.0000000'', N''Mary'', NULL, 0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Brand', N'ExpiryDate', N'Name', N'Notes', N'Quantity') AND [object_id] = OBJECT_ID(N'[medicines]'))
        SET IDENTITY_INSERT [medicines] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211209152447_intitalmigration')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Brand', N'ExpiryDate', N'Name', N'Notes', N'Quantity') AND [object_id] = OBJECT_ID(N'[medicines]'))
        SET IDENTITY_INSERT [medicines] ON;
    EXEC(N'INSERT INTO [medicines] ([Id], [Brand], [ExpiryDate], [Name], [Notes], [Quantity])
    VALUES (2, NULL, ''0001-01-01T00:00:00.0000000'', N''John'', NULL, 0)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Brand', N'ExpiryDate', N'Name', N'Notes', N'Quantity') AND [object_id] = OBJECT_ID(N'[medicines]'))
        SET IDENTITY_INSERT [medicines] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211209152447_intitalmigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211209152447_intitalmigration', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211213153257_intial -2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211213153257_intial -2', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211214133258_1')
BEGIN
    EXEC(N'UPDATE [medicines] SET [Name] = N''John1''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211214133258_1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211214133258_1', N'6.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211214143459_name')
BEGIN
    EXEC(N'UPDATE [medicines] SET [Name] = N''Sudhir''
    WHERE [Id] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211214143459_name')
BEGIN
    EXEC(N'UPDATE [medicines] SET [Name] = N''Arnav''
    WHERE [Id] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211214143459_name')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211214143459_name', N'6.0.0');
END;
GO

COMMIT;
GO

