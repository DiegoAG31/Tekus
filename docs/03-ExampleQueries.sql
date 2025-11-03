-- =============================================
-- TEKUS Service Management Database
-- Consultas de Ejemplo y Verificación
-- =============================================

USE TekusDb;
GO

PRINT '========================================';
PRINT 'EXAMPLE QUERIES';
PRINT '========================================';
PRINT '';

-- =============================================
-- 1. LISTAR TODOS LOS PROVEEDORES
-- =============================================
PRINT '1️⃣ All Providers with their details:';
PRINT '';
SELECT 
    Id,
    Nit,
    Name,
    Email,
    IsActive,
    CreatedAt,
    UpdatedAt
FROM Providers
ORDER BY Name;
GO

-- =============================================
-- 2. PROVEEDORES CON SUS CAMPOS PERSONALIZADOS
-- =============================================
PRINT '';
PRINT '2️⃣ Providers with Custom Fields:';
PRINT '';
SELECT 
    p.Name AS ProviderName,
    p.Nit,
    p.Email,
    cf.FieldName,
    cf.FieldValue,
    cf.FieldType
FROM Providers p
LEFT JOIN ProviderCustomFields cf ON p.Id = cf.ProviderId
ORDER BY p.Name, cf.FieldName;
GO

-- =============================================
-- 3. SERVICIOS POR PROVEEDOR CON TARIFA
-- =============================================
PRINT '';
PRINT '3️⃣ Services by Provider with Hourly Rate:';
PRINT '';
SELECT 
    p.Name AS ProviderName,
    s.Name AS ServiceName,
    s.HourlyRate_Amount AS HourlyRate,
    s.HourlyRate_Currency AS Currency,
    COUNT(sc.CountryCode) AS CountriesAvailable
FROM Providers p
INNER JOIN Services s ON p.Id = s.ProviderId
LEFT JOIN ServiceCountries sc ON s.Id = sc.ServiceId
GROUP BY p.Name, s.Name, s.HourlyRate_Amount, s.HourlyRate_Currency
ORDER BY p.Name, s.HourlyRate_Amount DESC;
GO

-- =============================================
-- 4. SERVICIOS DISPONIBLES POR PAÍS
-- =============================================
PRINT '';
PRINT '4️⃣ Services available in Colombia (COL):';
PRINT '';
SELECT 
    c.Name AS Country,
    p.Name AS Provider,
    s.Name AS Service,
    s.HourlyRate_Amount AS HourlyRate,
    s.HourlyRate_Currency AS Currency
FROM Countries c
INNER JOIN ServiceCountries sc ON c.Code = sc.CountryCode
INNER JOIN Services s ON sc.ServiceId = s.Id
INNER JOIN Providers p ON s.ProviderId = p.Id
WHERE c.Code = 'COL'
ORDER BY s.HourlyRate_Amount;
GO

-- =============================================
-- 5. PAÍSES MÁS ATENDIDOS
-- =============================================
PRINT '';
PRINT '5️⃣ Top 5 Countries with most services:';
PRINT '';
SELECT TOP 5
    c.Code,
    c.Name AS CountryName,
    COUNT(sc.ServiceId) AS ServicesAvailable
FROM Countries c
INNER JOIN ServiceCountries sc ON c.Code = sc.CountryCode
GROUP BY c.Code, c.Name
ORDER BY COUNT(sc.ServiceId) DESC;
GO

-- =============================================
-- 6. PROVEEDOR MÁS COMPLETO
-- =============================================
PRINT '';
PRINT '6️⃣ Provider with most services:';
PRINT '';
SELECT TOP 1
    p.Name AS ProviderName,
    p.Nit,
    p.Email,
    COUNT(DISTINCT s.Id) AS TotalServices,
    COUNT(DISTINCT sc.CountryCode) AS TotalCountriesCovered,
    AVG(s.HourlyRate_Amount) AS AverageHourlyRate
FROM Providers p
INNER JOIN Services s ON p.Id = s.ProviderId
LEFT JOIN ServiceCountries sc ON s.Id = sc.ServiceId
GROUP BY p.Id, p.Name, p.Nit, p.Email
ORDER BY COUNT(DISTINCT s.Id) DESC;
GO

-- =============================================
-- 7. SERVICIOS MÁS CAROS Y MÁS BARATOS
-- =============================================
PRINT '';
PRINT '7️⃣ Most expensive and cheapest services:';
PRINT '';
SELECT 'Most Expensive' AS Category, 
    p.Name AS Provider,
    s.Name AS Service,
    s.HourlyRate_Amount AS Rate,
    s.HourlyRate_Currency AS Currency
FROM Services s
INNER JOIN Providers p ON s.ProviderId = p.Id
WHERE s.HourlyRate_Amount = (SELECT MAX(HourlyRate_Amount) FROM Services)

UNION ALL

SELECT 'Cheapest' AS Category,
    p.Name AS Provider,
    s.Name AS Service,
    s.HourlyRate_Amount AS Rate,
    s.HourlyRate_Currency AS Currency
FROM Services s
INNER JOIN Providers p ON s.ProviderId = p.Id
WHERE s.HourlyRate_Amount = (SELECT MIN(HourlyRate_Amount) FROM Services);
GO

-- =============================================
-- 8. COBERTURA GEOGRÁFICA POR SERVICIO
-- =============================================
PRINT '';
PRINT '8️⃣ Geographic coverage by service:';
PRINT '';
SELECT 
    s.Name AS ServiceName,
    p.Name AS ProviderName,
    STRING_AGG(c.Name, ', ') AS AvailableCountries,
    COUNT(sc.CountryCode) AS CountriesCount
FROM Services s
INNER JOIN Providers p ON s.ProviderId = p.Id
LEFT JOIN ServiceCountries sc ON s.Id = sc.ServiceId
LEFT JOIN Countries c ON sc.CountryCode = c.Code
GROUP BY s.Id, s.Name, p.Name
ORDER BY COUNT(sc.CountryCode) DESC, s.Name;
GO

-- =============================================
-- 9. PROVEEDORES ACTIVOS VS INACTIVOS
-- =============================================
PRINT '';
PRINT '9️⃣ Active vs Inactive Providers:';
PRINT '';
SELECT 
    CASE 
        WHEN IsActive = 1 THEN 'Active'
        ELSE 'Inactive'
    END AS Status,
    COUNT(*) AS Total,
    STRING_AGG(Name, ', ') AS ProviderNames
FROM Providers
GROUP BY IsActive;
GO

-- =============================================
-- 10. ESTADÍSTICAS GENERALES
-- =============================================
PRINT '';
PRINT '🔟 General Statistics:';
PRINT '';
SELECT 
    'Total Providers' AS Metric,
    COUNT(*) AS Value
FROM Providers
UNION ALL
SELECT 
    'Active Providers',
    COUNT(*)
FROM Providers
WHERE IsActive = 1
UNION ALL
SELECT 
    'Total Services',
    COUNT(*)
FROM Services
UNION ALL
SELECT 
    'Countries Supported',
    COUNT(DISTINCT CountryCode)
FROM ServiceCountries
UNION ALL
SELECT 
    'Custom Fields',
    COUNT(*)
FROM ProviderCustomFields
UNION ALL
SELECT 
    'Service-Country Relationships',
    COUNT(*)
FROM ServiceCountries
UNION ALL
SELECT 
    'Average Service Rate (USD)',
    CAST(AVG(HourlyRate_Amount) AS DECIMAL(10,2))
FROM Services;
GO

-- =============================================
-- 11. SERVICIOS SIN PAÍS ASIGNADO (VALIDACIÓN)
-- =============================================
PRINT '';
PRINT '1️⃣1️⃣ Services without countries assigned (should be empty):';
PRINT '';
SELECT 
    p.Name AS ProviderName,
    s.Name AS ServiceName,
    s.HourlyRate_Amount AS HourlyRate
FROM Services s
INNER JOIN Providers p ON s.ProviderId = p.Id
LEFT JOIN ServiceCountries sc ON s.Id = sc.ServiceId
WHERE sc.Id IS NULL;
GO

-- =============================================
-- 12. BÚSQUEDA POR NOMBRE DE SERVICIO
-- =============================================
PRINT '';
PRINT '1️⃣2️⃣ Search services containing "Cloud":';
PRINT '';
SELECT 
    p.Name AS ProviderName,
    s.Name AS ServiceName,
    s.HourlyRate_Amount AS HourlyRate,
    COUNT(sc.CountryCode) AS CountriesAvailable
FROM Services s
INNER JOIN Providers p ON s.ProviderId = p.Id
LEFT JOIN ServiceCountries sc ON s.Id = sc.ServiceId
WHERE s.Name LIKE '%Cloud%'
GROUP BY p.Name, s.Name, s.HourlyRate_Amount
ORDER BY s.HourlyRate_Amount DESC;
GO

PRINT '';
PRINT '========================================';
PRINT '✅ All example queries executed!';
PRINT '========================================';
GO
