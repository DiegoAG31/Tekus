-- =============================================
-- TEKUS Service Management Database
-- Script de Creación de Base de Datos (DDL)
-- =============================================

USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TekusDb')
BEGIN
    CREATE DATABASE TekusDb;
    PRINT '✅ Database TekusDb created successfully';
END
ELSE
BEGIN
    PRINT '⚠️  Database TekusDb already exists';
END
GO

USE TekusDb;
GO

-- =============================================
-- 1. COUNTRIES TABLE
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Countries]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Countries] (
        [Code] NVARCHAR(3) NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [LastSync] DATETIME2(7) NULL DEFAULT GETUTCDATE(),
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Code] ASC)
    );

    CREATE NONCLUSTERED INDEX [IX_Countries_Name] 
        ON [dbo].[Countries]([Name] ASC);

    CREATE NONCLUSTERED INDEX [IX_Countries_LastSync] 
        ON [dbo].[Countries]([LastSync] ASC);

    PRINT '✅ Table Countries created successfully';
END
ELSE
BEGIN
    PRINT '⚠️  Table Countries already exists';
END
GO

-- =============================================
-- 2. PROVIDERS TABLE
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Providers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Providers] (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [Nit] NVARCHAR(20) NOT NULL,
        [Name] NVARCHAR(200) NOT NULL,
        [Email] NVARCHAR(255) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE UNIQUE NONCLUSTERED INDEX [IX_Providers_Nit] 
        ON [dbo].[Providers]([Nit] ASC);

    CREATE NONCLUSTERED INDEX [IX_Providers_Email] 
        ON [dbo].[Providers]([Email] ASC);

    CREATE NONCLUSTERED INDEX [IX_Providers_IsActive] 
        ON [dbo].[Providers]([IsActive] ASC);

    PRINT '✅ Table Providers created successfully';
END
ELSE
BEGIN
    PRINT '⚠️  Table Providers already exists';
END
GO

-- =============================================
-- 3. PROVIDER CUSTOM FIELDS TABLE
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProviderCustomFields]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ProviderCustomFields] (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [ProviderId] UNIQUEIDENTIFIER NOT NULL,
        [FieldName] NVARCHAR(100) NOT NULL,
        [FieldValue] NVARCHAR(500) NOT NULL,
        [FieldType] NVARCHAR(50) NOT NULL DEFAULT 'string',
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT [PK_ProviderCustomFields] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_ProviderCustomFields_Providers] 
            FOREIGN KEY ([ProviderId]) 
            REFERENCES [dbo].[Providers]([Id]) 
            ON DELETE CASCADE
    );

    CREATE UNIQUE NONCLUSTERED INDEX [IX_ProviderCustomFields_ProviderId_FieldName] 
        ON [dbo].[ProviderCustomFields]([ProviderId] ASC, [FieldName] ASC);

    PRINT '✅ Table ProviderCustomFields created successfully';
END
ELSE
BEGIN
    PRINT '⚠️  Table ProviderCustomFields already exists';
END
GO

-- =============================================
-- 4. SERVICES TABLE
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Services]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Services] (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [Name] NVARCHAR(200) NOT NULL,
        [ProviderId] UNIQUEIDENTIFIER NOT NULL,
        [HourlyRate_Amount] DECIMAL(18,2) NOT NULL,
        [HourlyRate_Currency] NVARCHAR(3) NOT NULL DEFAULT 'USD',
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Services_Providers] 
            FOREIGN KEY ([ProviderId]) 
            REFERENCES [dbo].[Providers]([Id]) 
            ON DELETE NO ACTION
    );

    CREATE NONCLUSTERED INDEX [IX_Services_ProviderId] 
        ON [dbo].[Services]([ProviderId] ASC);

    CREATE NONCLUSTERED INDEX [IX_Services_Name] 
        ON [dbo].[Services]([Name] ASC);

    PRINT '✅ Table Services created successfully';
END
ELSE
BEGIN
    PRINT '⚠️  Table Services already exists';
END
GO

-- =============================================
-- 5. SERVICE COUNTRIES TABLE (Junction)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServiceCountries]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ServiceCountries] (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [ServiceId] UNIQUEIDENTIFIER NOT NULL,
        [CountryCode] NVARCHAR(3) NOT NULL,
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT [PK_ServiceCountries] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_ServiceCountries_Services] 
            FOREIGN KEY ([ServiceId]) 
            REFERENCES [dbo].[Services]([Id]) 
            ON DELETE CASCADE,
        CONSTRAINT [FK_ServiceCountries_Countries] 
            FOREIGN KEY ([CountryCode]) 
            REFERENCES [dbo].[Countries]([Code]) 
            ON DELETE NO ACTION
    );

    CREATE UNIQUE NONCLUSTERED INDEX [IX_ServiceCountries_ServiceId_CountryCode] 
        ON [dbo].[ServiceCountries]([ServiceId] ASC, [CountryCode] ASC);

    CREATE NONCLUSTERED INDEX [IX_ServiceCountries_CountryCode] 
        ON [dbo].[ServiceCountries]([CountryCode] ASC);

    PRINT '✅ Table ServiceCountries created successfully';
END
ELSE
BEGIN
    PRINT '⚠️  Table ServiceCountries already exists';
END
GO

-- =============================================
-- VERIFICACIÓN FINAL
-- =============================================
PRINT '';
PRINT '========================================';
PRINT 'DATABASE CREATION SUMMARY';
PRINT '========================================';
SELECT 
    name AS TableName,
    create_date AS CreatedDate
FROM sys.tables
WHERE name IN ('Countries', 'Providers', 'ProviderCustomFields', 'Services', 'ServiceCountries')
ORDER BY name;
PRINT '';
PRINT '✅ Database schema created successfully!';
PRINT '📋 Next step: Run 02-SeedData.sql to populate with sample data';
GO
