-- AESCO Database Validation Queries
-- Run these queries to verify that data was inserted correctly

-- Connect to the database (make sure you're connected to ASCOServices database)
-- This script assumes you're already connected to the ASCOServices database

-- =============================================================================
-- BASIC TABLE COUNT VALIDATION
-- =============================================================================

SELECT '=== TABLE RECORD COUNTS ===' as info;

SELECT 
    'Users' as table_name, COUNT(*) as record_count FROM "Users"
UNION ALL
SELECT 'Roles', COUNT(*) FROM "Roles"
UNION ALL
SELECT 'Permissions', COUNT(*) FROM "Permissions"
UNION ALL
SELECT 'UserRoles', COUNT(*) FROM "UserRoles"
UNION ALL
SELECT 'RolePermissions', COUNT(*) FROM "RolePermissions"
UNION ALL
SELECT 'Ships', COUNT(*) FROM "Ships"
UNION ALL
SELECT 'Ports', COUNT(*) FROM "Ports"
UNION ALL
SELECT 'Voyages', COUNT(*) FROM "Voyages"
UNION ALL
SELECT 'VoyageLogs', COUNT(*) FROM "VoyageLogs"
UNION ALL
SELECT 'ShipAssignments', COUNT(*) FROM "ShipAssignments"
UNION ALL
SELECT 'PortCalls', COUNT(*) FROM "PortCalls"
UNION ALL
SELECT 'MaintenanceRecords', COUNT(*) FROM "MaintenanceRecords"
UNION ALL
SELECT 'Certificates', COUNT(*) FROM "Certificates"
UNION ALL
SELECT 'CrewCertifications', COUNT(*) FROM "CrewCertifications"
UNION ALL
SELECT 'CrewMedicalRecords', COUNT(*) FROM "CrewMedicalRecords"
UNION ALL
SELECT 'CrewPassports', COUNT(*) FROM "CrewPassports"
UNION ALL
SELECT 'CrewVisas', COUNT(*) FROM "CrewVisas"
UNION ALL
SELECT 'CrewReports', COUNT(*) FROM "CrewReports"
UNION ALL
SELECT 'VesselMannings', COUNT(*) FROM "VesselMannings"
UNION ALL
SELECT 'CrewPayrolls', COUNT(*) FROM "CrewPayrolls"
UNION ALL
SELECT 'CrewExpenses', COUNT(*) FROM "CrewExpenses"
UNION ALL
SELECT 'Incidents', COUNT(*) FROM "Incidents"
UNION ALL
SELECT 'StatementOfCash', COUNT(*) FROM "StatementOfCash"
UNION ALL
SELECT 'CrewTrainings', COUNT(*) FROM "CrewTrainings"
UNION ALL
SELECT 'CrewEvaluations', COUNT(*) FROM "CrewEvaluations"
UNION ALL
SELECT 'CrewWorkRestHours', COUNT(*) FROM "CrewWorkRestHours"
UNION ALL
SELECT 'Inspections', COUNT(*) FROM "Inspections"
UNION ALL
SELECT 'Notifications', COUNT(*) FROM "Notifications"
ORDER BY record_count DESC;

-- =============================================================================
-- DATA QUALITY VALIDATION
-- =============================================================================

SELECT '=== DATA QUALITY CHECKS ===' as info;

-- Check for users with valid email formats
SELECT 
    'Users with valid emails' as check_name,
    COUNT(*) as count
FROM "Users" 
WHERE "Email" LIKE '%@%.%';

-- Check for ships with valid IMO numbers (should be 7 digits)
SELECT 
    'Ships with valid IMO numbers' as check_name,
    COUNT(*) as count
FROM "Ships" 
WHERE LENGTH("IMONumber") = 7;

-- Check for active voyages
SELECT 
    'Active/In-Progress voyages' as check_name,
    COUNT(*) as count
FROM "Voyages" 
WHERE "Status" IN ('in_progress', 'planned');

-- Check for completed voyages
SELECT 
    'Completed voyages' as check_name,
    COUNT(*) as count
FROM "Voyages" 
WHERE "Status" = 'completed';

-- Check for users with ship assignments
SELECT 
    'Users with ship assignments' as check_name,
    COUNT(DISTINCT "UserId") as count
FROM "ShipAssignments" 
WHERE "Status" = 'active';

-- =============================================================================
-- SAMPLE DATA VERIFICATION
-- =============================================================================

SELECT '=== SAMPLE DATA PREVIEW ===' as info;

-- Sample Users
SELECT 'USERS SAMPLE:' as category;
SELECT 
    "Id",
    "Name" || ' ' || "Surname" as full_name,
    "Email",
    "Rank",
    "Nationality",
    "Status"
FROM "Users" 
ORDER BY "Id"
LIMIT 10;

-- Sample Ships
SELECT 'SHIPS SAMPLE:' as category;
SELECT 
    "Id",
    "Name",
    "IMONumber",
    "ShipType",
    "Flag",
    "Status",
    "GrossTonnage"
FROM "Ships" 
ORDER BY "Id"
LIMIT 8;

-- Sample Ports
SELECT 'PORTS SAMPLE:' as category;
SELECT 
    "Id",
    "Name",
    "Code",
    "Country",
    "Status"
FROM "Ports" 
ORDER BY "Id"
LIMIT 10;

-- Sample Voyages
SELECT 'VOYAGES SAMPLE:' as category;
SELECT 
    v."Id",
    v."VoyageNumber",
    s."Name" as ship_name,
    v."DeparturePort",
    v."ArrivalPort",
    v."Status",
    v."CargoType"
FROM "Voyages" v
JOIN "Ships" s ON v."ShipId" = s."Id"
ORDER BY v."Id"
LIMIT 10;

-- =============================================================================
-- RELATIONSHIP VALIDATION
-- =============================================================================

SELECT '=== RELATIONSHIP VALIDATION ===' as info;

-- Verify User-Role relationships
SELECT 
    'User-Role assignments' as relationship,
    COUNT(*) as count
FROM "UserRoles" ur
JOIN "Users" u ON ur."UserId" = u."Id"
JOIN "Roles" r ON ur."RoleId" = r."Id";

-- Verify Ship-Assignment relationships
SELECT 
    'Ship-User assignments' as relationship,
    COUNT(*) as count
FROM "ShipAssignments" sa
JOIN "Ships" s ON sa."ShipId" = s."Id"
JOIN "Users" u ON sa."UserId" = u."Id";

-- Verify Voyage-Ship relationships
SELECT 
    'Voyage-Ship relationships' as relationship,
    COUNT(*) as count
FROM "Voyages" v
JOIN "Ships" s ON v."ShipId" = s."Id";

-- Verify Port-PortCall relationships
SELECT 
    'Port-PortCall relationships' as relationship,
    COUNT(*) as count
FROM "PortCalls" pc
JOIN "Ports" p ON pc."PortId" = p."Id"
JOIN "Ships" s ON pc."ShipId" = s."Id";

-- =============================================================================
-- BUSINESS LOGIC VALIDATION
-- =============================================================================

SELECT '=== BUSINESS LOGIC VALIDATION ===' as info;

-- Check for ships with crew assignments
SELECT 
    s."Name" as ship_name,
    COUNT(sa."UserId") as crew_count,
    STRING_AGG(u."Name" || ' ' || u."Surname" || ' (' || sa."Position" || ')', ', ' ORDER BY sa."Position") as crew_list
FROM "Ships" s
LEFT JOIN "ShipAssignments" sa ON s."Id" = sa."ShipId" AND sa."Status" = 'active'
LEFT JOIN "Users" u ON sa."UserId" = u."Id"
GROUP BY s."Id", s."Name"
ORDER BY crew_count DESC;

-- Check voyage status distribution
SELECT 
    "Status",
    COUNT(*) as voyage_count
FROM "Voyages"
GROUP BY "Status"
ORDER BY voyage_count DESC;

-- Check ship type distribution
SELECT 
    "ShipType",
    COUNT(*) as ship_count
FROM "Ships"
GROUP BY "ShipType"
ORDER BY ship_count DESC;

-- Check nationality distribution of crew
SELECT 
    "Nationality",
    COUNT(*) as crew_count
FROM "Users"
WHERE "Rank" != 'Fleet Manager' AND "Rank" != 'HSEQ Manager' AND "Rank" != 'HR Manager'
GROUP BY "Nationality"
ORDER BY crew_count DESC;

-- =============================================================================
-- FINANCIAL DATA VALIDATION
-- =============================================================================

SELECT '=== FINANCIAL DATA VALIDATION ===' as info;

-- Payroll summary
SELECT 
    'Total monthly payroll' as metric,
    SUM("BaseWage" + "Overtime" + "Bonuses" - "Deductions") as amount,
    'USD' as currency
FROM "CrewPayrolls"
WHERE "PeriodStart" >= '2024-01-01' AND "PeriodEnd" <= '2024-01-31';

-- Expense summary
SELECT 
    'Total crew expenses' as metric,
    SUM("Amount") as amount,
    'USD' as currency
FROM "CrewExpenses"
WHERE "ExpenseDate" >= '2024-01-01' AND "ExpenseDate" <= '2024-02-29';

-- Maintenance cost summary
SELECT 
    'Total maintenance costs' as metric,
    SUM("Cost") as amount,
    'USD' as currency
FROM "MaintenanceRecords"
WHERE "ScheduledDate" >= '2024-01-01';

-- =============================================================================
-- OPERATIONAL DATA VALIDATION
-- =============================================================================

SELECT '=== OPERATIONAL DATA VALIDATION ===' as info;

-- Active incidents
SELECT 
    'Active incidents' as metric,
    COUNT(*) as count
FROM "Incidents"
WHERE "Status" IN ('open', 'under_investigation');

-- Expired certificates
SELECT 
    'Expired certificates' as metric,
    COUNT(*) as count
FROM "Certificates"
WHERE "ExpiryDate" < CURRENT_DATE;

-- Upcoming certificate expirations (next 90 days)
SELECT 
    'Certificates expiring in 90 days' as metric,
    COUNT(*) as count
FROM "Certificates"
WHERE "ExpiryDate" BETWEEN CURRENT_DATE AND CURRENT_DATE + INTERVAL '90 days';

-- Ships requiring maintenance
SELECT 
    'Ships with scheduled maintenance' as metric,
    COUNT(DISTINCT "ShipId") as count
FROM "MaintenanceRecords"
WHERE "Status" IN ('scheduled', 'in_progress');

-- =============================================================================
-- PERFORMANCE METRICS
-- =============================================================================

SELECT '=== PERFORMANCE METRICS ===' as info;

-- Average voyage duration
SELECT 
    'Average voyage duration (days)' as metric,
    ROUND(AVG(EXTRACT(DAY FROM ("ActualArrival" - "ActualDeparture"))), 2) as value
FROM "Voyages"
WHERE "ActualDeparture" IS NOT NULL AND "ActualArrival" IS NOT NULL;

-- Fleet utilization
SELECT 
    'Ships with active voyages' as metric,
    COUNT(DISTINCT "ShipId") as active_ships,
    (SELECT COUNT(*) FROM "Ships" WHERE "Status" = 'active') as total_active_ships
FROM "Voyages"
WHERE "Status" IN ('in_progress', 'planned');

-- Crew certification compliance
SELECT 
    'Crew members with valid certifications' as metric,
    COUNT(DISTINCT "UserId") as certified_crew
FROM "CrewCertifications"
WHERE "Status" = 'valid' AND "ExpiryDate" > CURRENT_DATE;

-- =============================================================================
-- DATA INTEGRITY CHECKS
-- =============================================================================

SELECT '=== DATA INTEGRITY CHECKS ===' as info;

-- Check for orphaned records
SELECT 
    'Orphaned ship assignments' as check_name,
    COUNT(*) as count
FROM "ShipAssignments" sa
LEFT JOIN "Ships" s ON sa."ShipId" = s."Id"
LEFT JOIN "Users" u ON sa."UserId" = u."Id"
WHERE s."Id" IS NULL OR u."Id" IS NULL;

-- Check for invalid date ranges
SELECT 
    'Voyages with invalid date ranges' as check_name,
    COUNT(*) as count
FROM "Voyages"
WHERE "PlannedDeparture" >= "PlannedArrival"
   OR ("ActualDeparture" IS NOT NULL AND "ActualArrival" IS NOT NULL AND "ActualDeparture" >= "ActualArrival");

-- Check for users without roles
SELECT 
    'Users without roles' as check_name,
    COUNT(*) as count
FROM "Users" u
LEFT JOIN "UserRoles" ur ON u."Id" = ur."UserId"
WHERE ur."UserId" IS NULL;

-- =============================================================================
-- SUMMARY REPORT
-- =============================================================================

SELECT '=== VALIDATION SUMMARY ===' as info;

SELECT 
    'Database validation completed successfully!' as message,
    CURRENT_TIMESTAMP as validation_time;

-- Final verification query
SELECT 
    'Total records across all tables' as summary,
    (
        (SELECT COUNT(*) FROM "Users") +
        (SELECT COUNT(*) FROM "Ships") +
        (SELECT COUNT(*) FROM "Voyages") +
        (SELECT COUNT(*) FROM "Ports") +
        (SELECT COUNT(*) FROM "ShipAssignments") +
        (SELECT COUNT(*) FROM "MaintenanceRecords") +
        (SELECT COUNT(*) FROM "Certificates") +
        (SELECT COUNT(*) FROM "CrewCertifications") +
        (SELECT COUNT(*) FROM "CrewPayrolls") +
        (SELECT COUNT(*) FROM "Incidents")
    ) as total_records;
