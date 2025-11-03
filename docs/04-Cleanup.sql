-- =============================================
-- TEKUS Service Management Database
-- Script de Limpieza (CLEANUP)
-- ⚠️  WARNING: This will DELETE ALL DATA
-- =============================================

USE TekusDb;
GO

PRINT '⚠️  ========================================';
PRINT '⚠️  WARNING: DATA CLEANUP IN PROGRESS';
PRINT '⚠️  ========================================';
PRINT '';

-- =============================================
-- OPCIÓN 1: Limpiar solo los datos (mantener estructura)
-- =============================================
PRINT '🗑️  Option 1: Cleaning data (keeping table structure)...';
PRINT '';

-- Deshabilitar verificación de Foreign Keys temporalmente
ALTER TABLE ServiceCountries NOCHECK CONSTRAINT ALL;
ALTER TABLE Services NOCHECK CONSTRAINT ALL;
ALTER TABLE ProviderCustomFields NOCHECK CONSTRAINT ALL;
GO

-- Eliminar datos en orden inverso a las dependencias
DELETE FROM ServiceCountries;
PRINT '   ✅ ServiceCountries cleaned';

DELETE FROM Services;
PRINT '   ✅ Services cleaned';

DELETE FROM ProviderCustomFields;
PRINT '   ✅ ProviderCustomFields cleaned';

DELETE FROM Providers;
PRINT '   ✅ Providers cleaned';

DELETE FROM Countries;
PRINT '   ✅ Countries cleaned';

-- Habilitar verificación de Foreign Keys nuevamente
ALTER TABLE ServiceCountries CHECK CONSTRAINT ALL;
ALTER TABLE Services CHECK CONSTRAINT ALL;
ALTER TABLE ProviderCustomFields CHECK CONSTRAINT ALL;
GO

PRINT '';
PRINT '✅ All data cleaned successfully!';
PRINT '📋 Tables structure preserved';
PRINT '';

-- Verificar que las tablas estén vacías
PRINT '📊 Verification - Row counts:';
SELECT 'Countries' AS TableName, COUNT(*) AS RowCount FROM Countries
UNION ALL
SELECT 'Providers', COUNT(*) FROM Providers
UNION ALL
SELECT 'ProviderCustomFields', COUNT(*) FROM ProviderCustomFields
UNION ALL
SELECT 'Services', COUNT(*) FROM Services
UNION ALL
SELECT 'ServiceCountries', COUNT(*) FROM ServiceCountries;

PRINT '';
PRINT '💡 Next steps:';
PRINT '   1. Run 02-SeedData.sql to repopulate with sample data';
PRINT '   2. Or start fresh with your own data';
GO

-- =============================================
-- OPCIÓN 2: Eliminar toda la base de datos (COMENTADO)
-- =============================================
/*
-- ⚠️  DESCOMENTAR SOLO SI QUIERES ELIMINAR LA BASE DE DATOS COMPLETA

USE master;
GO

PRINT '';
PRINT '⚠️  ========================================';
PRINT '⚠️  DROPPING ENTIRE DATABASE';
PRINT '⚠️  ========================================';

-- Cerrar todas las conexiones activas a la base de datos
ALTER DATABASE TekusDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Eliminar la base de datos
DROP DATABASE TekusDb;
GO

PRINT '✅ Database TekusDb dropped successfully!';
PRINT '📋 Next step: Run 01-CreateDatabase.sql to recreate';
GO
*/
