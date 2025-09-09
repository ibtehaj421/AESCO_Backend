-- AESCO - Clear All Data (FK-safe)
-- Usage: connect to ASCOServices then run: \i db_clear_all.sql
ROLLBACK;

BEGIN;

-- Truncate in one shot with CASCADE to satisfy all FK relationships
TRUNCATE TABLE
    "VoyageLogs",
    "PortCalls",
    "CrewWorkRestHours",
    "CrewEvaluations",
    "CrewTrainings",
    "StatementOfCash",
    "Incidents",
    "CrewExpenses",
    "CrewPayrolls",
    "VesselMannings",
    "CrewReports",
    "CrewVisas",
    "CrewPassports",
    "CrewMedicalRecords",
    "CrewCertifications",
    "Certificates",
    "MaintenanceRecords",
    "ShipAssignments",
    "Voyages",
    "Ports",
    "Ships",
    "Notifications",
    "DocumentModuleDatabanks",
    "DocumentModuleMains",
    "FormFields",
    "Forms",
    "DocumentVersions",
    "DocumentLogs",
    "Documents",
    "CrewModuleDatabanks",
    "CrewModuleMains",
    "RolePermissions",
    "UserRoles",
    "Permissions",
    "Roles",
    "Users"
RESTART IDENTITY CASCADE;

COMMIT;

SELECT 'All tables truncated successfully' AS status;


