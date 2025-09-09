-- AESCO - Seed All Data (FK-safe order)
-- Usage: connect to ASCOServices then run: \i db_seed_all.sql

BEGIN;

-- 1) Core principals
INSERT INTO "Users" (
    "Name","Surname","Nationality","IdenNumber","DateOfBirth","BirthPlace","Gender","Status","JobType","Rank",
    "MaritalStatus","MilitaryStatus","EducationLevel","GraduationYear","School","Competency","OrganizationUnit","Email","PasswordHash",
    "CreatedAt","WorkEndDate","FatherName","LastLoginAt","EmailConfirmed","EmailConfirmationToken","PasswordResetToken","PasswordResetTokenExpiry",
    "FailedLoginAttempts","LockoutEnd"
) VALUES
('John','Anderson','American',123456789,'1975-03-15','New York, USA','M','active','full time','Fleet Manager','married','completed','master',2000,'Maritime Academy of New York','Fleet Operations Management','Shore Operations','john.anderson@aesco.com','x',NOW(),NULL,'Robert Anderson',NULL,true,NULL,NULL,NULL,0,NULL),
('Maria','Rodriguez','Spanish',987654321,'1980-07-22','Barcelona, Spain','F','active','full time','HSEQ Manager','single','not applicable','master',2005,'UPC','HSEQ','HSEQ Department','maria.rodriguez@aesco.com','x',NOW(),NULL,'Carlos Rodriguez',NULL,true,NULL,NULL,NULL,0,NULL);

INSERT INTO "Roles" ("Name","Description","CreatedAt","IsActive") VALUES
('Super Admin','Full system access',NOW(),true),
('Fleet Manager','Manage fleet',NOW(),true),
('HSEQ Manager','HSEQ management',NOW(),true);

INSERT INTO "Permissions" ("Name","Description","Module") VALUES
('ViewUsers','View user information','Users'),
('ManageUsers','Create/edit/delete users','Users'),
('ViewVessels','View vessel information','Vessels'),
('ManageVessels','Create/edit/delete vessels','Vessels');

-- 2) Role links
-- Resolve IDs dynamically to avoid assuming sequences
INSERT INTO "UserRoles" ("UserId","RoleId","AssignedAt","AssignedByUserId","IsActive")
SELECT u1."Id", r."Id", NOW(), u1."Id", true
FROM "Users" u1 CROSS JOIN "Roles" r WHERE u1."Email"='john.anderson@aesco.com' AND r."Name"='Fleet Manager'
ON CONFLICT DO NOTHING;

INSERT INTO "RolePermissions" ("RoleId","PermissionId","GrantedAt")
SELECT r."Id", p."Id", NOW() FROM "Roles" r JOIN "Permissions" p ON true WHERE r."Name"='Super Admin'
ON CONFLICT DO NOTHING;

-- 3) Reference data
INSERT INTO "Ships" ("Name","IMONumber","CallSign","RegistrationNumber","ShipType","Flag","Builder","BuildDate","LaunchDate","LengthOverall","Beam","Draft","GrossTonnage","NetTonnage","DeadweightTonnage","PassengerCapacity","CrewCapacity","MaxSpeed","ServiceSpeed","EngineType","EnginePower","HomePort","Status","Description","CreatedAt") VALUES
('MV Atlantic Pioneer','9234567','HBCA','REG001','Container Ship','Panama','Hyundai','2015-03-15','2015-06-20',299.9,48.2,14.5,98500,58200,124800,0,25,24.5,22.0,'MAN',32400,'Hamburg','active','Trans-Atlantic',NOW());

INSERT INTO "Ports" ("Name","Code","Country","Region","Latitude","Longitude","TimeZone","Facilities","MaxDraft","MaxLength","Status","Notes","CreatedAt") VALUES
('Port of Hamburg','DEHAM','Germany','Northern Europe',53.5511,9.9937,'CET','Container, Bulk, RoRo',15.1,400,'active','Third largest in EU',NOW());

-- 4) Operational entities
INSERT INTO "Voyages" ("ShipId","VoyageNumber","DeparturePort","ArrivalPort","PlannedDeparture","PlannedArrival","ActualDeparture","ActualArrival","Status","CargoType","CargoWeight","Distance","Notes","CreatedAt")
SELECT s."Id", 'AP001','Hamburg','New York','2024-01-15 08:00:00','2024-01-28 14:00:00','2024-01-15 08:30:00','2024-01-28 13:45:00','completed','Containers',85000,3625,'Atlantic crossing',NOW()
FROM "Ships" s WHERE s."Name"='MV Atlantic Pioneer';

-- 5) Assign crew to ship using existing users
INSERT INTO "ShipAssignments" ("ShipId","UserId","Position","AssignedAt","Status","AssignedByUserId","Notes")
SELECT s."Id", u."Id", 'Master','2023-12-01','active', u."Id", 'Initial master assignment'
FROM "Ships" s CROSS JOIN "Users" u WHERE s."Name"='MV Atlantic Pioneer' AND u."Email"='john.anderson@aesco.com';

COMMIT;

SELECT 'Seeding completed successfully' AS status;

-- =============================
-- Extended bulk seeding section
-- Run these after the core commit to add lots of realistic data
-- Re-open a transaction for bulk inserts
BEGIN;

-- Ensure UUID extension exists for document tables
DO $$ BEGIN
    PERFORM 1 FROM pg_extension WHERE extname = 'uuid-ossp';
    IF NOT FOUND THEN
        CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
    END IF;
END $$;

-- Add more users (50 total)
WITH base AS (
  SELECT generate_series(1,48) AS n
)
INSERT INTO "Users" (
    "Name","Surname","Nationality","IdenNumber","DateOfBirth","BirthPlace","Gender","Status","JobType","Rank",
    "MaritalStatus","MilitaryStatus","EducationLevel","GraduationYear","School","Competency","OrganizationUnit","Email","PasswordHash",
    "CreatedAt","WorkEndDate","FatherName","LastLoginAt","EmailConfirmed","EmailConfirmationToken","PasswordResetToken","PasswordResetTokenExpiry",
    "FailedLoginAttempts","LockoutEnd"
)
SELECT 
    'User'||n, 'Test'||n, 'Mixed', 100000000+n,
    DATE '1985-01-01' + (n||' days')::interval,
    'City '||n, CASE WHEN n%2=0 THEN 'M' ELSE 'F' END,
    'active','full time', CASE WHEN n%6=0 THEN 'Chief Engineer' WHEN n%5=0 THEN 'Chief Officer' WHEN n%4=0 THEN 'Second Officer' WHEN n%3=0 THEN 'Third Officer' ELSE 'Able Seaman' END,
    'single', 'not applicable', CASE WHEN n%3=0 THEN 'bachelor' ELSE 'high school' END,
    2000 + (n%25), 'Maritime School '||n, 'General Competency', 'Dept '||((n%5)+1),
    'user'||n||'@aesco.com', 'x', NOW() - (n||' days')::interval,
    NULL, 'Father'||n, NULL, true, NULL, NULL, NULL, 0, NULL
FROM base;

-- Give all users a role (Crew Member or Deck Officer)
INSERT INTO "UserRoles" ("UserId","RoleId","AssignedAt","AssignedByUserId","IsActive")
SELECT u."Id", r."Id", NOW(), (SELECT MIN("Id") FROM "Users"), true
FROM "Users" u
JOIN "Roles" r ON r."Name" IN ('HSEQ Manager','Fleet Manager')
WHERE u."Email" LIKE 'user%'
ON CONFLICT DO NOTHING;

-- Add more permissions and map to roles
INSERT INTO "Permissions" ("Name","Description","Module")
SELECT 'P_'||g, 'Auto perm '||g, 'General' FROM generate_series(1,16) g;

INSERT INTO "RolePermissions" ("RoleId","PermissionId","GrantedAt")
SELECT r."Id", p."Id", NOW() FROM "Roles" r CROSS JOIN LATERAL (
  SELECT p2."Id" FROM "Permissions" p2 ORDER BY p2."Id" LIMIT 10
) p
ON CONFLICT DO NOTHING;

-- Add ships (10 total)
WITH s AS (
  SELECT generate_series(1,9) AS n
)
INSERT INTO "Ships" ("Name","IMONumber","CallSign","RegistrationNumber","ShipType","Flag","Builder","BuildDate","LaunchDate","LengthOverall","Beam","Draft","GrossTonnage","NetTonnage","DeadweightTonnage","PassengerCapacity","CrewCapacity","MaxSpeed","ServiceSpeed","EngineType","EnginePower","HomePort","Status","Description","CreatedAt")
SELECT 
  'MV Test Ship '||n,
  lpad((9000000+n)::text,7,'0'),
  'CS'||lpad(n::text,3,'0'),
  'REG'||lpad(n::text,3,'0'),
  CASE WHEN n%3=0 THEN 'Bulk Carrier' WHEN n%3=1 THEN 'Container Ship' ELSE 'Tanker' END,
  CASE WHEN n%4=0 THEN 'Panama' WHEN n%4=1 THEN 'Liberia' WHEN n%4=2 THEN 'Malta' ELSE 'Singapore' END,
  'Builder '||n,
  DATE '2015-01-01' + (n||' months')::interval,
  DATE '2015-06-01' + (n||' months')::interval,
  180 + n*10, 25 + n, 7 + (n%10), 30000 + n*1000, 15000 + n*700, 50000 + n*1500,
  CASE WHEN n%5=0 THEN 1000 ELSE 0 END,
  20 + (n%10), 20 + (n%6), 18 + (n%5), 'Engine'||n, 12000 + n*500, 'Port '||n,
  'active', 'Seeded test ship', NOW()
FROM s;

-- Add ports (30 total)
INSERT INTO "Ports" ("Name","Code","Country","Region","Latitude","Longitude","TimeZone","Facilities","MaxDraft","MaxLength","Status","Notes","CreatedAt")
SELECT 
  'Port '||g, 'P'||lpad(g::text,4,'0'),
  CASE WHEN g%5=0 THEN 'USA' WHEN g%5=1 THEN 'Germany' WHEN g%5=2 THEN 'Singapore' WHEN g%5=3 THEN 'China' ELSE 'Spain' END,
  'Region '||((g%4)+1),
  10 + g, 20 + g, 'UTC', 'Container,Bulk,RoRo', 10 + (g%10), 200 + g, 'active', 'Auto seeded', NOW()
FROM generate_series(1,30) g
ON CONFLICT DO NOTHING;

-- Voyages (100 total, tie to existing ships)
INSERT INTO "Voyages" ("ShipId","VoyageNumber","DeparturePort","ArrivalPort","PlannedDeparture","PlannedArrival","ActualDeparture","ActualArrival","Status","CargoType","CargoWeight","Distance","Notes","CreatedAt")
SELECT s."Id",
  'V'||lpad(g::text,5,'0'),
  'Port '||((g%30)+1),
  'Port '||(((g+7)%30)+1),
  NOW() - (g||' days')::interval,
  NOW() - ((g-1)||' days')::interval,
  NOW() - (g||' days')::interval + interval '1 hour',
  NOW() - ((g-1)||' days')::interval - interval '2 hour',
  CASE WHEN g%4=0 THEN 'planned' WHEN g%4=1 THEN 'in_progress' WHEN g%4=2 THEN 'completed' ELSE 'cancelled' END,
  CASE WHEN g%3=0 THEN 'Containers' WHEN g%3=1 THEN 'Grain' ELSE 'Oil' END,
  10000 + (g*100), 500 + g*10, 'Auto voyage', NOW()
FROM generate_series(1,100) g
JOIN LATERAL (
  SELECT "Id" FROM "Ships" ORDER BY "Id" OFFSET (g%10) LIMIT 1
) s ON true;

-- Port calls (200)
INSERT INTO "PortCalls" ("ShipId","PortId","VoyageId","Purpose","PlannedArrival","PlannedDeparture","ActualArrival","ActualDeparture","Status","BerthNumber","CargoLoaded","CargoUnloaded","FuelTaken","PortCharges","Notes","CreatedByUserId","CreatedAt")
SELECT 
  v."ShipId",
  p."Id",
  v."Id",
  CASE WHEN g%2=0 THEN 'Loading' ELSE 'Discharge' END,
  v."PlannedDeparture" - interval '6 hours',
  v."PlannedDeparture" + interval '6 hours',
  v."PlannedDeparture" - interval '5 hours',
  v."PlannedDeparture" + interval '5 hours',
  'departed',
  'B-'||((g%20)+1),
  CASE WHEN g%2=0 THEN 1000+g ELSE 0 END,
  CASE WHEN g%2=1 THEN 1000+g ELSE 0 END,
  50 + (g%30),
  1000 + g*10,
  'Auto port call',
  (SELECT MIN("Id") FROM "Users"),
  NOW()
FROM generate_series(1,200) g
JOIN LATERAL (
  SELECT "Id","ShipId","PlannedDeparture" FROM "Voyages" ORDER BY "Id" OFFSET (g%100) LIMIT 1
) v ON true
JOIN LATERAL (
  SELECT "Id" FROM "Ports" ORDER BY "Id" OFFSET (g%30) LIMIT 1
) p ON true;

-- Voyage logs (300)
INSERT INTO "VoyageLogs" ("VoyageId","LogTime","LogType","LogEntry","Latitude","Longitude","Speed","Course","WeatherConditions","WindSpeed","WindDirection","SeaState","LoggedByUserId")
SELECT 
  v."Id",
  v."PlannedDeparture" + ((g%48)||' hours')::interval,
  CASE WHEN g%3=0 THEN 'position' WHEN g%3=1 THEN 'weather' ELSE 'note' END,
  'Log entry '||g,
  10 + (g%80), 10 + (g%160), 10 + (g%12), (g%360), 'OK', 5+(g%40), (g%360), (g%10),
  (SELECT MIN("Id") FROM "Users")
FROM generate_series(1,300) g
JOIN LATERAL (
  SELECT "Id","PlannedDeparture" FROM "Voyages" ORDER BY "Id" OFFSET (g%100) LIMIT 1
) v ON true;

-- Maintenance records (50)
INSERT INTO "MaintenanceRecords" (
  "ShipId","MaintenanceType","Component","Description","ScheduledDate","CompletedDate","Status",
  "EstimatedCost","ActualCost","PerformedBy","Location","Priority","Notes","CreatedByUserId","CreatedAt"
)
SELECT 
  s."Id",
  CASE WHEN g%2=0 THEN 'routine' ELSE 'repair' END,
  CASE WHEN g%3=0 THEN 'engine' WHEN g%3=1 THEN 'hull' ELSE 'navigation' END,
  'Maintenance job '||g,
  NOW() - ((g+10)||' days')::interval,
  CASE WHEN g%3=0 THEN NULL ELSE NOW() - ((g+9)||' days')::interval END,
  CASE WHEN g%3=0 THEN 'scheduled' WHEN g%3=1 THEN 'in_progress' ELSE 'completed' END,
  (1000 + g*200),
  CASE WHEN g%3=2 THEN (900 + g*180) ELSE NULL END,
  'Contractor '||((g%7)+1),
  'Port '||((g%10)+1),
  CASE WHEN g%4=0 THEN 'low' WHEN g%4=1 THEN 'medium' WHEN g%4=2 THEN 'high' ELSE 'critical' END,
  'Auto note '||g,
  (SELECT MIN("Id") FROM "Users"),
  NOW()
FROM generate_series(1,50) g
JOIN LATERAL (
  SELECT "Id" FROM "Ships" ORDER BY "Id" OFFSET (g%10) LIMIT 1
) s ON true;

-- Certificates (50)
INSERT INTO "Certificates" ("CertificateType","CertificateNumber","IssuedBy","IssuedDate","ExpiryDate","Status","ShipId","CreatedByUserId","CreatedAt")
SELECT 
  'CertType '||g, 'CERT-'||lpad(g::text,6,'0'), 'Authority '||((g%5)+1),
  CURRENT_DATE - (g||' days')::interval,
  CURRENT_DATE + ((365 + g)%900||' days')::interval,
  CASE WHEN g%5=0 THEN 'expired' ELSE 'valid' END,
  s."Id",
  (SELECT MIN("Id") FROM "Users"),
  NOW()
FROM generate_series(1,50) g
JOIN LATERAL (
  SELECT "Id" FROM "Ships" ORDER BY "Id" OFFSET (g%10) LIMIT 1
) s ON true;

-- Create a temp list of 30 crew users to reuse across statements
CREATE TEMP TABLE IF NOT EXISTS tmp_crew(user_id int) ON COMMIT DROP;
TRUNCATE tmp_crew;
INSERT INTO tmp_crew(user_id)
SELECT "Id" FROM "Users" WHERE "Email" LIKE 'user%' ORDER BY "Id" LIMIT 30;

INSERT INTO "CrewPayrolls" ("CrewMemberId","PeriodStart","PeriodEnd","BaseWage","Overtime","Bonuses","Deductions","Currency","PaymentDate","PaymentMethod")
SELECT c.user_id, DATE '2024-01-01', DATE '2024-01-31', 4000+(row_number() OVER())*50, 500, 200, 100, 'USD', DATE '2024-02-05', 'Bank Transfer'
FROM tmp_crew c;

-- Create one expense report per crew member
INSERT INTO "CrewExpenseReport" ("CrewMemberId","ShipId","TotalAmount","Currency","ReportDate","Notes","CreatedAt")
SELECT c.user_id, (SELECT MIN("Id") FROM "Ships"), 0, 'USD', CURRENT_DATE, 'Auto report', NOW()
FROM tmp_crew c;

-- Add expense lines under each report
INSERT INTO "CrewExpenses" ("ExpenseReportId","CrewMemberId","Category","Amount","Currency","ExpenseDate","Notes","CreatedAt")
SELECT r."Id", r."CrewMemberId",
  CASE WHEN (row_number() OVER())%3=0 THEN 'Travel' WHEN (row_number() OVER())%3=1 THEN 'Medical' ELSE 'Training' END,
  100 + (row_number() OVER())*5,
  'USD', DATE '2024-02-05' - ((row_number() OVER())%15)::int, 'Auto expense', NOW()
FROM "CrewExpenseReport" r
JOIN tmp_crew c2 ON c2.user_id = r."CrewMemberId";

-- Update report totals
UPDATE "CrewExpenseReport" r
SET "TotalAmount" = s.sum_amount
FROM (
  SELECT "ExpenseReportId", SUM("Amount") AS sum_amount
  FROM "CrewExpenses"
  GROUP BY "ExpenseReportId"
) s
WHERE r."Id" = s."ExpenseReportId";

-- StatementOfCash uses vessel and creator
WITH v AS (SELECT MIN("Id") AS vessel_id FROM "Ships"), creator AS (SELECT MIN("Id") AS user_id FROM "Users")
INSERT INTO "StatementOfCash" ("VesselId","CreatedById","status","TransactionDate","Description","Inflow","Outflow","Balance","CreatedAt")
SELECT v.vessel_id, creator.user_id, 'Finalized', CURRENT_DATE - ((row_number() OVER())%10)::int, 'Auto transaction', 100, 50, 1000, NOW()
FROM tmp_crew c, v, creator;

INSERT INTO "CrewReports" ("UserId","ReportType","Title","Details","ReportDate","CreatedAt")
SELECT c.user_id, 'Safety', 'Auto subject', 'Auto description', CURRENT_DATE - interval '3 days', NOW()
FROM tmp_crew c;

-- Crew certifications, medical, passport, visa
INSERT INTO "CrewCertifications" ("UserId","CertificationType","CertificateNumber","IssuedBy","IssuedDate","ExpiryDate","Status","IssuedAt","Notes","CreatedAt")
SELECT c.user_id, 'STCW', 'STCW-'||lpad((c.user_id)::text,6,'0'), 'Maritime Authority', CURRENT_DATE-interval '60 days', CURRENT_DATE+interval '2 years', 'valid', 'HQ', NULL, NOW()
FROM tmp_crew c;

INSERT INTO "CrewMedicalRecords" ("UserId","ProviderName","BloodGroup","ExaminationDate","ExpiryDate","Notes","CreatedAt")
SELECT c.user_id, 'AME', 'O+', CURRENT_DATE-interval '30 days', CURRENT_DATE+interval '11 months', 'Auto medical', NOW()
FROM tmp_crew c;

INSERT INTO "CrewPassports" ("UserId","PassportNumber","Nationality","IssueDate","ExpiryDate","IssuedBy","Notes","CreatedAt")
SELECT c.user_id, 'PPT'||lpad((c.user_id)::text,8,'0'), 'Mixed', CURRENT_DATE-interval '2 years', CURRENT_DATE+interval '8 years', 'Govt', 'Auto passport', NOW()
FROM tmp_crew c;

INSERT INTO "CrewVisas" ("UserId","VisaType","Country","IssueDate","ExpiryDate","IssuedBy","Notes","CreatedAt")
SELECT c.user_id, 'C1/D', 'USA', CURRENT_DATE-interval '90 days', CURRENT_DATE+interval '1 years', 'Embassy', 'Transit', NOW()
FROM tmp_crew c;

-- Training, Evaluations, Work/Rest
-- Pick one vessel and distribute
WITH v AS (
  SELECT MIN("Id") AS vessel_id FROM "Ships"
)
INSERT INTO "CrewTrainings" ("UserId","VesselId","TrainingCategory","Rank","Trainer","Remark","Training","Source","TrainingDate","ExpireDate","Status","CreatedByUserId","CreatedById","CreatedAt")
SELECT c.user_id, v.vessel_id, 'Safety', 'Able Seaman', 'Trainer', 'Auto', 'PST', 'Center', CURRENT_DATE-interval '20 days', CURRENT_DATE+interval '2 years', 'Completed', (SELECT MIN("Id") FROM "Users"), (SELECT MIN("Id") FROM "Users"), NOW()
FROM tmp_crew c, (SELECT MIN("Id") AS vessel_id FROM "Ships") v;

INSERT INTO "CrewEvaluations" ("UserId","VesselId","FormNo","RevisionNo","RevisionDate","FormName","FormDescription","EnteredByUserId","EnteredDate","Rank","Name","Surname","TechnicalCompetence","SafetyAwareness","Teamwork","Communication","Leadership","ProblemSolving","Adaptability","WorkEthic","OverallRating","Strengths","AreasForImprovement","Comments","Status","CreatedByUserId","CreatedById","CreatedAt")
SELECT c.user_id, v.vessel_id, 'E-'||lpad(c.user_id::text,6,'0'), '1.0', CURRENT_DATE-interval '10 days', 'Evaluation', NULL,
       (SELECT MIN("Id") FROM "Users"), CURRENT_DATE-interval '9 days', 'Able Seaman', 'User','Test', 4,4,4,4,3,4,4,4,4,
       'Good','Improve cargo ops','OK','Completed',(SELECT MIN("Id") FROM "Users"), (SELECT MIN("Id") FROM "Users"), NOW()
FROM tmp_crew c, (SELECT MIN("Id") AS vessel_id FROM "Ships") v;

-- Seed hourly schema: 24 rows per user for a single day snapshot
INSERT INTO "CrewWorkRestHours" ("UserId","VesselId","Year","Month","Day","Hour","IsWorking","Description","Notes","CreatedByUserId","CreatedById","CreatedAt")
SELECT c.user_id, v.vessel_id,
       EXTRACT(YEAR FROM CURRENT_DATE)::int,
       EXTRACT(MONTH FROM CURRENT_DATE)::int,
       EXTRACT(DAY FROM CURRENT_DATE)::int,
       h.hr,
       (h.hr BETWEEN 8 AND 19),
       'Auto W/R',
       NULL,
       (SELECT MIN("Id") FROM "Users"),
       (SELECT MIN("Id") FROM "Users"),
       NOW()
FROM tmp_crew c,
     (SELECT MIN("Id") AS vessel_id FROM "Ships") v,
     LATERAL (SELECT generate_series(0,23) AS hr) h;

-- Notifications (100)
INSERT INTO "Notifications" ("UserId","ToUserId","Title","Message","Type","RelatedEntityType","RelatedEntityId","IsRead","ReadAt","IsEmailSent","CreatedAt")
SELECT u1."Id", u2."Id", 'Info', 'Auto message', 'general', NULL, NULL, (g%2=0), NULL, false, NOW()
FROM generate_series(1,100) g
JOIN LATERAL (
  SELECT "Id" FROM "Users" ORDER BY "Id" OFFSET (g%30) LIMIT 1
) u1 ON true
JOIN LATERAL (
  SELECT "Id" FROM "Users" ORDER BY "Id" OFFSET ((g+5)%30) LIMIT 1
) u2 ON true
WHERE u1."Id"<>u2."Id";

-- Documents and Versions/Logs (20 docs)
WITH creators AS (
  SELECT MIN("Id") AS creator, MIN("Id") AS updater FROM "Users"
), docs AS (
  INSERT INTO "Documents" ("Id","Name","Extension","PhysicalPath","SizeInBytes","MimeType","IsFolder","ParentId","CreatedAt","CreatedById","UpdatedAt","UpdatedById")
  SELECT uuid_generate_v4(), 'Doc '||g, 'pdf', '/uploads/doc'||g||'.pdf', 1024*g, 'application/pdf', false, NULL, NOW(), (SELECT creator FROM creators), NOW(), (SELECT updater FROM creators)
  FROM generate_series(1,20) g
  RETURNING "Id"
)
INSERT INTO "DocumentVersions" ("Id","DocumentId","PhysicalPath","Extension","SizeInBytes","VersionDate","ChangedBy","ChangeDescription","VersionNumber")
SELECT uuid_generate_v4(), d."Id", '/uploads/versions/'||d."Id"||'/v1.pdf', 'pdf', 1024, NOW(), 'system', 'initial version', 1
FROM docs d;

INSERT INTO "DocumentLogs" ("Id","DocumentId","ActionDate","ActionById","ActionType","Notes")
SELECT uuid_generate_v4(), d."Id", NOW(), (SELECT MIN("Id") FROM "Users"), 'Upload', 'auto'
FROM (SELECT "Id" FROM "Documents" ORDER BY "CreatedAt" DESC LIMIT 20) d;

COMMIT;

SELECT 'Extended bulk seeding completed' AS status;


