-- =============================================
-- TEKUS Service Management Database
-- Script de Datos de Ejemplo (SEED DATA)
-- =============================================

USE TekusDb;
GO

PRINT '========================================';
PRINT 'INSERTING SAMPLE DATA';
PRINT '========================================';
PRINT '';

-- =============================================
-- 1. COUNTRIES (Países de América Latina)
-- =============================================
PRINT '📍 Inserting Countries...';

IF NOT EXISTS (SELECT 1 FROM Countries WHERE Code = 'COL')
BEGIN
    INSERT INTO Countries (Code, Name, LastSync, CreatedAt, UpdatedAt)
    VALUES 
        ('COL', 'Colombia', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('MEX', 'México', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('ARG', 'Argentina', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('BRA', 'Brasil', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('CHL', 'Chile', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('PER', 'Perú', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('ECU', 'Ecuador', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('URY', 'Uruguay', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('PRY', 'Paraguay', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('BOL', 'Bolivia', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('VEN', 'Venezuela', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('PAN', 'Panamá', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('CRI', 'Costa Rica', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('GTM', 'Guatemala', GETUTCDATE(), GETUTCDATE(), GETUTCDATE()),
        ('DOM', 'República Dominicana', GETUTCDATE(), GETUTCDATE(), GETUTCDATE());

    PRINT '   ✅ 15 countries inserted';
END
ELSE
BEGIN
    PRINT '   ⚠️  Countries already exist';
END
GO

-- =============================================
-- 2. PROVIDERS (Proveedores de Servicios)
-- =============================================
PRINT '';
PRINT '🏢 Inserting Providers...';

DECLARE @Provider1_Id UNIQUEIDENTIFIER = 'A1234567-89AB-CDEF-0123-456789ABCDEF';
DECLARE @Provider2_Id UNIQUEIDENTIFIER = 'B2345678-9ABC-DEF0-1234-56789ABCDEF0';
DECLARE @Provider3_Id UNIQUEIDENTIFIER = 'C3456789-ABCD-EF01-2345-6789ABCDEF01';
DECLARE @Provider4_Id UNIQUEIDENTIFIER = 'D4567890-BCDE-F012-3456-789ABCDEF012';
DECLARE @Provider5_Id UNIQUEIDENTIFIER = 'E5678901-CDEF-0123-4567-89ABCDEF0123';

IF NOT EXISTS (SELECT 1 FROM Providers WHERE Id = @Provider1_Id)
BEGIN
    INSERT INTO Providers (Id, Nit, Name, Email, IsActive, CreatedAt, UpdatedAt)
    VALUES 
        -- Provider 1: TEKUS (Activo)
        (@Provider1_Id, '900123456-1', 'TEKUS S.A.S.', 'info@tekus.com', 1, GETUTCDATE(), GETUTCDATE()),
        
        -- Provider 2: CloudTech Solutions (Activo)
        (@Provider2_Id, '900234567-2', 'CloudTech Solutions Colombia', 'contact@cloudtech.com.co', 1, GETUTCDATE(), GETUTCDATE()),
        
        -- Provider 3: DataCore Systems (Activo)
        (@Provider3_Id, '900345678-3', 'DataCore Systems Ltda.', 'sales@datacore.com', 1, GETUTCDATE(), GETUTCDATE()),
        
        -- Provider 4: InnovateTech (Inactivo)
        (@Provider4_Id, '900456789-4', 'InnovateTech Partners S.A.', 'hello@innovatetech.com', 0, GETUTCDATE(), GETUTCDATE()),
        
        -- Provider 5: Global IT Services (Activo)
        (@Provider5_Id, '900567890-5', 'Global IT Services Colombia', 'info@globalit.com.co', 1, GETUTCDATE(), GETUTCDATE());

    PRINT '   ✅ 5 providers inserted';
END
ELSE
BEGIN
    PRINT '   ⚠️  Providers already exist';
END
GO

-- =============================================
-- 3. PROVIDER CUSTOM FIELDS
-- =============================================
PRINT '';
PRINT '🔖 Inserting Provider Custom Fields...';

DECLARE @Provider1_Id UNIQUEIDENTIFIER = 'A1234567-89AB-CDEF-0123-456789ABCDEF';
DECLARE @Provider2_Id UNIQUEIDENTIFIER = 'B2345678-9ABC-DEF0-1234-56789ABCDEF0';
DECLARE @Provider3_Id UNIQUEIDENTIFIER = 'C3456789-ABCD-EF01-2345-6789ABCDEF01';
DECLARE @Provider4_Id UNIQUEIDENTIFIER = 'D4567890-BCDE-F012-3456-789ABCDEF012';
DECLARE @Provider5_Id UNIQUEIDENTIFIER = 'E5678901-CDEF-0123-4567-89ABCDEF0123';

IF NOT EXISTS (SELECT 1 FROM ProviderCustomFields WHERE ProviderId = @Provider1_Id)
BEGIN
    INSERT INTO ProviderCustomFields (Id, ProviderId, FieldName, FieldValue, FieldType, CreatedAt, UpdatedAt)
    VALUES 
        -- TEKUS Custom Fields
        (NEWID(), @Provider1_Id, 'Phone', '+57 300 1234567', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider1_Id, 'Address', 'Calle 100 #15-20, Bogotá D.C., Colombia', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider1_Id, 'Website', 'https://www.tekus.com', 'url', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider1_Id, 'EmployeeCount', '150', 'number', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider1_Id, 'YearFounded', '2010', 'number', GETUTCDATE(), GETUTCDATE()),
        
        -- CloudTech Custom Fields
        (NEWID(), @Provider2_Id, 'Phone', '+57 315 9876543', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider2_Id, 'Address', 'Carrera 7 #71-52, Bogotá D.C., Colombia', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider2_Id, 'Website', 'https://www.cloudtech.com.co', 'url', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider2_Id, 'Certifications', 'ISO 9001, ISO 27001, AWS Partner', 'string', GETUTCDATE(), GETUTCDATE()),
        
        -- DataCore Custom Fields
        (NEWID(), @Provider3_Id, 'Phone', '+57 320 5551234', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider3_Id, 'Address', 'Av. El Dorado #68-90, Bogotá D.C., Colombia', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider3_Id, 'SupportEmail', 'support@datacore.com', 'email', GETUTCDATE(), GETUTCDATE()),
        
        -- InnovateTech Custom Fields
        (NEWID(), @Provider4_Id, 'Phone', '+57 310 7778888', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider4_Id, 'Status', 'Temporarily Inactive - Restructuring', 'string', GETUTCDATE(), GETUTCDATE()),
        
        -- Global IT Custom Fields
        (NEWID(), @Provider5_Id, 'Phone', '+57 318 4445566', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider5_Id, 'Address', 'Calle 116 #7-15, Bogotá D.C., Colombia', 'string', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Provider5_Id, 'OperatingCountries', '12', 'number', GETUTCDATE(), GETUTCDATE());

    PRINT '   ✅ 17 custom fields inserted';
END
ELSE
BEGIN
    PRINT '   ⚠️  Custom fields already exist';
END
GO

-- =============================================
-- 4. SERVICES (Servicios ofrecidos)
-- =============================================
PRINT '';
PRINT '⚙️  Inserting Services...';

DECLARE @Provider1_Id UNIQUEIDENTIFIER = 'A1234567-89AB-CDEF-0123-456789ABCDEF';
DECLARE @Provider2_Id UNIQUEIDENTIFIER = 'B2345678-9ABC-DEF0-1234-56789ABCDEF0';
DECLARE @Provider3_Id UNIQUEIDENTIFIER = 'C3456789-ABCD-EF01-2345-6789ABCDEF01';
DECLARE @Provider5_Id UNIQUEIDENTIFIER = 'E5678901-CDEF-0123-4567-89ABCDEF0123';

DECLARE @Service1_Id UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @Service2_Id UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';
DECLARE @Service3_Id UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';
DECLARE @Service4_Id UNIQUEIDENTIFIER = '44444444-4444-4444-4444-444444444444';
DECLARE @Service5_Id UNIQUEIDENTIFIER = '55555555-5555-5555-5555-555555555555';
DECLARE @Service6_Id UNIQUEIDENTIFIER = '66666666-6666-6666-6666-666666666666';
DECLARE @Service7_Id UNIQUEIDENTIFIER = '77777777-7777-7777-7777-777777777777';
DECLARE @Service8_Id UNIQUEIDENTIFIER = '88888888-8888-8888-8888-888888888888';
DECLARE @Service9_Id UNIQUEIDENTIFIER = '99999999-9999-9999-9999-999999999999';
DECLARE @Service10_Id UNIQUEIDENTIFIER = 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA';

IF NOT EXISTS (SELECT 1 FROM Services WHERE Id = @Service1_Id)
BEGIN
    INSERT INTO Services (Id, Name, ProviderId, HourlyRate_Amount, HourlyRate_Currency, CreatedAt, UpdatedAt)
    VALUES 
        -- TEKUS Services
        (@Service1_Id, 'Cloud Hosting & Infrastructure', @Provider1_Id, 120.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        (@Service2_Id, 'Software Development', @Provider1_Id, 150.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        (@Service3_Id, 'IT Consulting', @Provider1_Id, 180.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        
        -- CloudTech Services
        (@Service4_Id, 'AWS Cloud Migration', @Provider2_Id, 200.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        (@Service5_Id, 'DevOps Services', @Provider2_Id, 170.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        (@Service6_Id, 'Cloud Security', @Provider2_Id, 190.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        
        -- DataCore Services
        (@Service7_Id, 'Data Analytics & BI', @Provider3_Id, 160.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        (@Service8_Id, 'Machine Learning Solutions', @Provider3_Id, 220.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        
        -- Global IT Services
        (@Service9_Id, 'Managed IT Services', @Provider5_Id, 100.00, 'USD', GETUTCDATE(), GETUTCDATE()),
        (@Service10_Id, 'Cybersecurity Services', @Provider5_Id, 185.00, 'USD', GETUTCDATE(), GETUTCDATE());

    PRINT '   ✅ 10 services inserted';
END
ELSE
BEGIN
    PRINT '   ⚠️  Services already exist';
END
GO

-- =============================================
-- 5. SERVICE COUNTRIES (Relación N:M)
-- =============================================
PRINT '';
PRINT '🌎 Inserting Service-Country relationships...';

DECLARE @Service1_Id UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @Service2_Id UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';
DECLARE @Service3_Id UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';
DECLARE @Service4_Id UNIQUEIDENTIFIER = '44444444-4444-4444-4444-444444444444';
DECLARE @Service5_Id UNIQUEIDENTIFIER = '55555555-5555-5555-5555-555555555555';
DECLARE @Service6_Id UNIQUEIDENTIFIER = '66666666-6666-6666-6666-666666666666';
DECLARE @Service7_Id UNIQUEIDENTIFIER = '77777777-7777-7777-7777-777777777777';
DECLARE @Service8_Id UNIQUEIDENTIFIER = '88888888-8888-8888-8888-888888888888';
DECLARE @Service9_Id UNIQUEIDENTIFIER = '99999999-9999-9999-9999-999999999999';
DECLARE @Service10_Id UNIQUEIDENTIFIER = 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA';

IF NOT EXISTS (SELECT 1 FROM ServiceCountries WHERE ServiceId = @Service1_Id)
BEGIN
    INSERT INTO ServiceCountries (Id, ServiceId, CountryCode, CreatedAt, UpdatedAt)
    VALUES 
        -- Service 1: Cloud Hosting (disponible en 5 países)
        (NEWID(), @Service1_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service1_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service1_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service1_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service1_Id, 'CHL', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 2: Software Development (disponible en 7 países)
        (NEWID(), @Service2_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service2_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service2_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service2_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service2_Id, 'PER', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service2_Id, 'ECU', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service2_Id, 'CHL', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 3: IT Consulting (disponible en 4 países)
        (NEWID(), @Service3_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service3_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service3_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service3_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 4: AWS Cloud Migration (disponible en 3 países)
        (NEWID(), @Service4_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service4_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service4_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 5: DevOps (disponible en 6 países)
        (NEWID(), @Service5_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service5_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service5_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service5_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service5_Id, 'CHL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service5_Id, 'PER', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 6: Cloud Security (disponible en 4 países)
        (NEWID(), @Service6_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service6_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service6_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service6_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 7: Data Analytics (disponible en 5 países)
        (NEWID(), @Service7_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service7_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service7_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service7_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service7_Id, 'CHL', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 8: Machine Learning (disponible en 3 países - premium)
        (NEWID(), @Service8_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service8_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service8_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 9: Managed IT (disponible en 8 países)
        (NEWID(), @Service9_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service9_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service9_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service9_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service9_Id, 'CHL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service9_Id, 'PER', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service9_Id, 'ECU', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service9_Id, 'URY', GETUTCDATE(), GETUTCDATE()),
        
        -- Service 10: Cybersecurity (disponible en 6 países)
        (NEWID(), @Service10_Id, 'COL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service10_Id, 'MEX', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service10_Id, 'BRA', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service10_Id, 'ARG', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service10_Id, 'CHL', GETUTCDATE(), GETUTCDATE()),
        (NEWID(), @Service10_Id, 'PER', GETUTCDATE(), GETUTCDATE());

    PRINT '   ✅ 53 service-country relationships inserted';
END
ELSE
BEGIN
    PRINT '   ⚠️  Service-country relationships already exist';
END
GO

-- =============================================
-- RESUMEN FINAL
-- =============================================
PRINT '';
PRINT '========================================';
PRINT 'SEED DATA SUMMARY';
PRINT '========================================';

PRINT '';
PRINT '📊 Data Count:';
SELECT 'Countries' AS Entity, COUNT(*) AS Total FROM Countries
UNION ALL
SELECT 'Providers', COUNT(*) FROM Providers
UNION ALL
SELECT 'ProviderCustomFields', COUNT(*) FROM ProviderCustomFields
UNION ALL
SELECT 'Services', COUNT(*) FROM Services
UNION ALL
SELECT 'ServiceCountries', COUNT(*) FROM ServiceCountries;

PRINT '';
PRINT '✅ Sample data inserted successfully!';
PRINT '🚀 Database ready for testing!';
PRINT '';
PRINT '📝 Sample Provider IDs:';
PRINT '   TEKUS:      A1234567-89AB-CDEF-0123-456789ABCDEF';
PRINT '   CloudTech:  B2345678-9ABC-DEF0-1234-56789ABCDEF0';
PRINT '   DataCore:   C3456789-ABCD-EF01-2345-6789ABCDEF01';
PRINT '';
GO
