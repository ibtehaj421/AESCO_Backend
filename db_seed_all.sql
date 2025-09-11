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

-- Ensure passports for key seed users with realistic expiry
INSERT INTO "CrewPassports" ("UserId","PassportNumber","Nationality","IssueDate","ExpiryDate","IssuedBy","Notes","CreatedAt")
SELECT u."Id", 'PPT-SEED-ANDERSON','American', CURRENT_DATE - interval '5 years', CURRENT_DATE + interval '5 years', 'Govt', 'Seed passport', NOW()
FROM "Users" u WHERE u."Email"='john.anderson@aesco.com'
ON CONFLICT DO NOTHING;

INSERT INTO "CrewPassports" ("UserId","PassportNumber","Nationality","IssueDate","ExpiryDate","IssuedBy","Notes","CreatedAt")
SELECT u."Id", 'PPT-SEED-RODRIGUEZ','Spanish', CURRENT_DATE - interval '5 years', CURRENT_DATE + interval '5 years', 'Govt', 'Seed passport', NOW()
FROM "Users" u WHERE u."Email"='maria.rodriguez@aesco.com'
ON CONFLICT DO NOTHING;

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

-- Ensure the vessel Zangazur exists
INSERT INTO "Ships" ("Name","IMONumber","CallSign","RegistrationNumber","ShipType","Flag","Builder","BuildDate","LaunchDate","LengthOverall","Beam","Draft","GrossTonnage","NetTonnage","DeadweightTonnage","PassengerCapacity","CrewCapacity","MaxSpeed","ServiceSpeed","EngineType","EnginePower","HomePort","Status","Description","CreatedAt")
VALUES ('Zangazur','9999999','4JUD','REG-ZAN-001','Tanker','Azerbaijan','Baku Shipyard','2010-01-01','2010-06-01',200,32,10,35000,20000,50000,0,25,18,15,'Wartsila',20000,'Baku','active','Seeded vessel Zangazur',NOW())
ON CONFLICT DO NOTHING;

-- Crew list for Zangazur (users, passports, assignments)
-- Create users
WITH upsert_user AS (
  INSERT INTO "Users" ("Name","Surname","Nationality","IdenNumber","DateOfBirth","BirthPlace","Gender","Status","JobType","Rank","MaritalStatus","MilitaryStatus","EducationLevel","GraduationYear","School","Competency","OrganizationUnit","Email","PasswordHash","CreatedAt","WorkEndDate","FatherName","LastLoginAt","EmailConfirmed","EmailConfirmationToken","PasswordResetToken","PasswordResetTokenExpiry","FailedLoginAttempts","LockoutEnd") VALUES
  ('Georgy','Ustyuzhanin','Russian', 800000001,'1981-07-01','USSR','M','active','full time','Master','married','completed','bachelor',2002,'Maritime School','Deck','Crew','georgy.ustyuzhanin@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Sergey','Polonnikov','Russian', 800000002,'1980-11-18','USSR','M','active','full time','Ch Off','married','completed','bachelor',2001,'Maritime School','Deck','Crew','sergey.polonnikov@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Rustam','Mammadov','Azerbaijan',800000003,'1997-05-21','Azerbaijan','M','active','full time','2nd Off','single','completed','bachelor',2019,'Maritime School','Deck','Crew','rustam.mammadov@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Vusal','Alakbarov','Azerbaijan',800000004,'1986-11-02','Azerbaijan','M','active','full time','3rd Off','single','completed','bachelor',2008,'Maritime School','Deck','Crew','vusal.alakbarov@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Sergey','Azhogin','Russian',800000005,'1970-02-16','USSR','M','active','full time','Ch Eng','married','completed','bachelor',1992,'Maritime School','Engine','Crew','sergey.azhogin@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Denis','Tarasenko','Russian',800000006,'1984-01-11','USSR','M','active','full time','2nd Eng','married','completed','bachelor',2006,'Maritime School','Engine','Crew','denis.tarasenko@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Vasily','Donetskiy','Russian',800000007,'1990-07-10','USSR','M','active','full time','3rd Eng','single','completed','bachelor',2012,'Maritime School','Engine','Crew','vasily.donetskiy@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Jabbar','Mammadzada','Azerbaijan',800000008,'2002-06-19','Azerbaijan','M','active','full time','4th Eng','single','completed','bachelor',2024,'Maritime School','Engine','Crew','jabbar.mammadzada@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Dmitry','Volodko','Russian',800000009,'1977-01-17','USSR','M','active','full time','ETO','married','completed','bachelor',1999,'Maritime School','Engine','Crew','dmitry.volodko@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Hervic','Nool','Filipino',800000010,'1983-04-27','Santiago Isabela','M','active','full time','Pumpman','married','completed','high school',2001,'Technical School','Engine','Crew','hervic.nool@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Dave','Landicho','Filipino',800000011,'1975-11-13','Laurel Batangas','M','active','full time','Bosun','married','completed','high school',1993,'Technical School','Deck','Crew','dave.landicho@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Paul','Amper','Filipino',800000012,'1979-06-29','Cebu City','M','active','full time','AB 1','married','completed','high school',1999,'Technical School','Deck','Crew','paul.amper@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Alfred','Banatanto','Filipino',800000013,'1996-11-19','Roseller LIM ZDS','M','active','full time','AB 2','single','completed','high school',2014,'Technical School','Deck','Crew','alfred.banatanto@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Emmanuel','Caolie','Filipino',800000014,'1977-08-19','Lingayen PGN','M','active','full time','AB 3','married','completed','high school',1997,'Technical School','Deck','Crew','emmanuel.caolie@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Ericson','Loreto','Filipino',800000015,'1996-05-03','Bien Unido Bohol','M','active','full time','OS 1','single','completed','high school',2014,'Technical School','Deck','Crew','ericson.loreto@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Bernardino III','Selga','Filipino',800000016,'1998-04-28','Agoo La Union','M','active','full time','OS 2','single','completed','high school',2016,'Technical School','Deck','Crew','bernardino.selga@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Anar','Muradov','Azerbaijan',800000017,'1988-09-23','Russian Federation','M','active','full time','Oiler 1','married','completed','bachelor',2010,'Maritime School','Engine','Crew','anar.muradov@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Fernando JR','Patricio','Filipino',800000018,'1997-11-21','Cabanatuan City','M','active','full time','Oiler 2','single','completed','high school',2015,'Technical School','Engine','Crew','fernando.patricio@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Enrique','Molo','Filipino',800000019,'1970-11-30','Tacloban city','M','active','full time','Fitter','married','completed','high school',1989,'Technical School','Engine','Crew','enrique.molo@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Samir','Alakbarov','Azerbaijan',800000020,'1992-09-20','Azerbaijan','M','active','full time','Cook','married','completed','high school',2011,'Technical School','Catering','Crew','samir.alakbarov@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL),
  ('Reymarc','Tagapan','Filipino',800000021,'1999-05-02','Roxas Isabela','M','active','full time','Messman','single','completed','high school',2017,'Technical School','Catering','Crew','reymarc.tagapan@seed.local','x',NOW(),NULL,NULL,NULL,true,NULL,NULL,NULL,0,NULL)
  ON CONFLICT DO NOTHING
  RETURNING "Id","Email"
)
SELECT 1;

-- Passports for these crew
INSERT INTO "CrewPassports" ("UserId","PassportNumber","Nationality","IssueDate","ExpiryDate","IssuedBy","Notes","CreatedAt")
SELECT u."Id",
       p.passport,
       p.nationality,
       CURRENT_DATE - interval '5 years',
       p.expiry,
       'Authority','Seed passport',NOW()
FROM (
  VALUES
    ('georgy.ustyuzhanin@seed.local','75 8309482','Russian', DATE '2028-06-22'),
    ('sergey.polonnikov@seed.local','75 7020501','Russian', DATE '2028-02-20'),
    ('rustam.mammadov@seed.local','C02680120','Azerbaijan', DATE '2029-07-25'),
    ('vusal.alakbarov@seed.local','C02590736','Azerbaijan', DATE '2029-08-20'),
    ('sergey.azhogin@seed.local','75 6431848','Russian', DATE '2027-11-23'),
    ('denis.tarasenko@seed.local','77 1767847','Russian', DATE '2033-11-07'),
    ('vasily.donetskiy@seed.local','76 7791118','Russian', DATE '2032-06-10'),
    ('jabbar.mammadzada@seed.local','C02893290','Azerbaijan', DATE '2031-06-01'),
    ('dmitry.volodko@seed.local','77 2869666','Russian', DATE '2034-03-19'),
    ('hervic.nool@seed.local','P5576675B','Filipino', DATE '2030-09-29'),
    ('dave.landicho@seed.local','P8357884A','Filipino', DATE '2028-08-14'),
    ('paul.amper@seed.local','P1758106B','Filipino', DATE '2029-05-27'),
    ('alfred.banatanto@seed.local','P5040817B','Filipino', DATE '2030-03-04'),
    ('emmanuel.caolie@seed.local','P7188691B','Filipino', DATE '2031-07-13'),
    ('ericson.loreto@seed.local','P5927657B','Filipino', DATE '2030-12-10'),
    ('bernardino.selga@seed.local','P4878309B','Filipino', DATE '2030-02-18'),
    ('anar.muradov@seed.local','C02774132','Azerbaijan', DATE '2029-11-29'),
    ('fernando.patricio@seed.local','P4414809B','Filipino', DATE '2030-01-16'),
    ('enrique.molo@seed.local','P2087023B','Filipino', DATE '2029-05-03'),
    ('samir.alakbarov@seed.local','C01825903','Azerbaijan', DATE '2028-09-20'),
    ('reymarc.tagapan@seed.local','P0819899C','Filipino', DATE '2032-07-05')
) p(email, passport, nationality, expiry)
JOIN "Users" u ON u."Email" = p.email
ON CONFLICT DO NOTHING;

-- Assign to Zangazur with embark port (Notes) and date
WITH z AS (
  SELECT "Id" AS ship_id FROM "Ships" WHERE "Name"='Zangazur' LIMIT 1
)
INSERT INTO "ShipAssignments" ("ShipId","UserId","Position","AssignedAt","Status","AssignedByUserId","Notes")
SELECT z.ship_id, u."Id", a.rank, a.embarked_at::timestamp, 'active', (SELECT MIN("Id") FROM "Users"), a.embark_port
FROM (
  VALUES
    ('georgy.ustyuzhanin@seed.local','Master','Primorsk, Russia','2025-02-27'),
    ('sergey.polonnikov@seed.local','Ch Off','Aliaga, Turkey','2024-12-18'),
    ('rustam.mammadov@seed.local','2nd Off','Aliaga, Turkey','2024-12-18'),
    ('vusal.alakbarov@seed.local','3rd Off','Aliaga, Turkey','2025-02-04'),
    ('sergey.azhogin@seed.local','Ch Eng','Primorsk, Russia','2025-02-27'),
    ('denis.tarasenko@seed.local','2nd Eng','Aliaga, Turkey','2024-10-31'),
    ('vasily.donetskiy@seed.local','3rd Eng','Aliaga, Turkey','2024-10-31'),
    ('jabbar.mammadzada@seed.local','4th Eng','Primorsk, Russia','2024-10-11'),
    ('dmitry.volodko@seed.local','ETO','Primorsk, Russia','2025-02-27'),
    ('hervic.nool@seed.local','Pumpman','Aliaga, Turkey','2024-07-18'),
    ('dave.landicho@seed.local','Bosun','Aliaga, Turkey','2024-12-18'),
    ('paul.amper@seed.local','AB 1','Aliaga, Turkey','2024-07-18'),
    ('alfred.banatanto@seed.local','AB 2','Aliaga, Turkey','2024-12-18'),
    ('emmanuel.caolie@seed.local','AB 3','Aliaga, Turkey','2024-07-18'),
    ('ericson.loreto@seed.local','OS 1','Aliaga, Turkey','2024-07-18'),
    ('bernardino.selga@seed.local','OS 2','Aliaga, Turkey','2024-07-18'),
    ('anar.muradov@seed.local','Oiler 1','Aliaga, Turkey','2024-10-31'),
    ('fernando.patricio@seed.local','Oiler 2','Aliaga, Turkey','2024-07-18'),
    ('enrique.molo@seed.local','Fitter','Aliaga, Turkey','2024-07-18'),
    ('samir.alakbarov@seed.local','Cook','Aliaga, Turkey','2024-10-31'),
    ('reymarc.tagapan@seed.local','Messman','Aliaga, Turkey','2025-02-04')
) a(email, rank, embark_port, embarked_at)
JOIN "Users" u ON u."Email" = a.email, z
ON CONFLICT DO NOTHING;

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

-- Add embark assignments for a subset of crew to test CrewList Where/When Embarked
-- Assign first 8 crew to alternating ships with staggered dates
WITH ships AS (
  SELECT array_agg("Id" ORDER BY "Id") AS ids FROM "Ships"
), base AS (
  SELECT user_id, row_number() OVER () AS rn FROM tmp_crew ORDER BY user_id LIMIT 8
)
INSERT INTO "ShipAssignments" ("ShipId","UserId","Position","AssignedAt","Status","AssignedByUserId","Notes")
SELECT 
  (SELECT ids[(rn % greatest(array_length(ids,1),1))+1] FROM ships),
  b.user_id,
  CASE WHEN rn%5=0 THEN 'Able Seaman' WHEN rn%5=1 THEN 'Third Officer' WHEN rn%5=2 THEN 'Second Officer' WHEN rn%5=3 THEN 'Chief Officer' ELSE 'Chief Engineer' END,
  (CURRENT_DATE - ((10 - rn))::int)::timestamp,
  'active',
  (SELECT MIN("Id") FROM "Users"),
  'Seed embark'
FROM base b;

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
SELECT uuid_generate_v4(), d."Id", '/uploads/versions/'||d."Id"||'/v1.pdf', 'pdf', 1024, NOW(), (SELECT MIN("Id") FROM "Users"), 'initial version', 1
FROM docs d;

INSERT INTO "DocumentLogs" ("Id","DocumentId","ActionDate","ActionById","ActionType","Notes")
SELECT uuid_generate_v4(), d."Id", NOW(), (SELECT MIN("Id") FROM "Users"), 'Upload', 'auto'
FROM (SELECT "Id" FROM "Documents" ORDER BY "CreatedAt" DESC LIMIT 20) d;

-- Document approvals for first 5 documents
WITH approver AS (
  SELECT MIN("Id") AS user_id FROM "Users"
), picked AS (
  SELECT "Id" FROM "Documents" ORDER BY "CreatedAt" DESC LIMIT 5
)
INSERT INTO "DocumentApprovals" ("Id","DocumentId","ApproverId","RequestedAt","RespondedAt","Status","Notes")
SELECT uuid_generate_v4(), p."Id", (SELECT user_id FROM approver), NOW() - interval '1 day', NOW(), 1, 'Auto-approved seed'
FROM picked p;

-- Document text content for first 5 documents and their versions
WITH dv AS (
  SELECT v."Id" AS version_id, v."DocumentId" AS doc_id
  FROM "DocumentVersions" v
  ORDER BY v."VersionDate" DESC
  LIMIT 5
)
INSERT INTO "DocumentText" ("DocumentId","VersionedDocumentId","Content")
SELECT dv.doc_id, dv.version_id, 'Seeded OCR/full-text content for document '||dv.doc_id::text
FROM dv;

-- Incidents (safety incidents, accidents, near misses)
INSERT INTO "Incidents" ("ShipId","VoyageId","Title","IncidentType","Severity","IncidentDateTime","Location","Latitude","Longitude","Description","ImmediateActions","RootCause","PreventiveMeasures","Status","AuthoritiesNotified","AuthoritiesNotifiedDetails","ReportedByUserId","InvestigatedByUserId","CreatedAt","UpdatedAt","ResolvedAt")
SELECT 
  s."Id",
  CASE WHEN (row_number() OVER())%3 = 0 THEN (SELECT MIN("Id") FROM "Voyages") ELSE NULL END,
  'Incident ' || (row_number() OVER()),
  CASE (row_number() OVER())%5
    WHEN 0 THEN 'accident'
    WHEN 1 THEN 'near_miss'
    WHEN 2 THEN 'equipment_failure'
    WHEN 3 THEN 'safety'
    ELSE 'security'
  END,
  CASE (row_number() OVER())%4
    WHEN 0 THEN 'low'
    WHEN 1 THEN 'medium'
    WHEN 2 THEN 'high'
    ELSE 'critical'
  END,
  NOW() - INTERVAL '1 day' * ((row_number() OVER())%30),
  CASE (row_number() OVER())%4
    WHEN 0 THEN 'Engine Room'
    WHEN 1 THEN 'Bridge'
    WHEN 2 THEN 'Deck'
    ELSE 'Cargo Hold'
  END,
  CASE (row_number() OVER())%2 WHEN 0 THEN 40.7128 + (random() * 0.1) ELSE NULL END,
  CASE (row_number() OVER())%2 WHEN 0 THEN -74.0060 + (random() * 0.1) ELSE NULL END,
  'Auto incident description for ' || s."Name",
  'Immediate actions taken to address the situation',
  'Root cause analysis completed',
  'Preventive measures implemented',
  CASE (row_number() OVER())%4
    WHEN 0 THEN 'open'
    WHEN 1 THEN 'investigating'
    WHEN 2 THEN 'resolved'
    ELSE 'closed'
  END,
  (row_number() OVER())%3 = 0,
  CASE WHEN (row_number() OVER())%3 = 0 THEN 'Coast Guard notified' ELSE NULL END,
  (SELECT MIN("Id") FROM "Users"),
  CASE WHEN (row_number() OVER())%2 = 0 THEN (SELECT MIN("Id") FROM "Users") ELSE NULL END,
  NOW() - INTERVAL '1 day' * ((row_number() OVER())%30),
  CASE WHEN (row_number() OVER())%3 = 0 THEN NOW() - INTERVAL '1 hour' ELSE NULL END,
  CASE WHEN (row_number() OVER())%4 = 0 THEN NOW() - INTERVAL '1 hour' ELSE NULL END
FROM "Ships" s
CROSS JOIN generate_series(1, 3);

-- VesselMannings (crew positions for each vessel)
INSERT INTO "VesselMannings" ("VesselId","Rank")
SELECT 
  s."Id",
  CASE (row_number() OVER())%15
    WHEN 0 THEN 'Captain'
    WHEN 1 THEN 'Chief Officer'
    WHEN 2 THEN 'Second Officer'
    WHEN 3 THEN 'Third Officer'
    WHEN 4 THEN 'Chief Engineer'
    WHEN 5 THEN 'Second Engineer'
    WHEN 6 THEN 'Third Engineer'
    WHEN 7 THEN 'Fourth Engineer'
    WHEN 8 THEN 'Bosun'
    WHEN 9 THEN 'Able Seaman'
    WHEN 10 THEN 'Ordinary Seaman'
    WHEN 11 THEN 'Cook'
    WHEN 12 THEN 'Steward'
    WHEN 13 THEN 'Electrician'
    ELSE 'Pumpman'
  END
FROM "Ships" s
CROSS JOIN generate_series(1, 8);

-- Inspections (vessel inspections)
INSERT INTO "Inspections" ("ShipId","InspectionType","InspectedBy","InspectionDate","NextInspectionDate","Result","Findings","Recommendations","CertificateNumber","CertificateExpiry","Location","CreatedByUserId","CreatedById","CreatedAt")
SELECT 
  s."Id",
  CASE (row_number() OVER())%4
    WHEN 0 THEN 'safety'
    WHEN 1 THEN 'security'
    WHEN 2 THEN 'environmental'
    ELSE 'classification'
  END,
  CASE (row_number() OVER())%6
    WHEN 0 THEN 'Port State Control'
    WHEN 1 THEN 'Flag State'
    WHEN 2 THEN 'Classification Society'
    WHEN 3 THEN 'Coast Guard'
    WHEN 4 THEN 'Maritime Safety Authority'
    ELSE 'International Maritime Organization'
  END,
  NOW() - INTERVAL '1 day' * ((row_number() OVER())%90),
  NOW() + INTERVAL '1 day' * (365 + ((row_number() OVER())%30)),
  CASE (row_number() OVER())%4
    WHEN 0 THEN 'passed'
    WHEN 1 THEN 'failed'
    WHEN 2 THEN 'conditional'
    ELSE 'pending'
  END,
  CASE (row_number() OVER())%3
    WHEN 0 THEN 'No significant findings'
    WHEN 1 THEN 'Minor deficiencies noted'
    ELSE 'Several areas require attention'
  END,
  CASE (row_number() OVER())%3
    WHEN 0 THEN 'Continue current practices'
    WHEN 1 THEN 'Address minor issues within 30 days'
    ELSE 'Major corrective actions required'
  END,
  'CERT-' || (row_number() OVER())::text,
  NOW() + INTERVAL '1 day' * (365 + ((row_number() OVER())%30)),
  CASE (row_number() OVER())%4
    WHEN 0 THEN 'Port of New York'
    WHEN 1 THEN 'Port of Los Angeles'
    WHEN 2 THEN 'Port of Miami'
    ELSE 'Port of Seattle'
  END,
  (SELECT MIN("Id") FROM "Users"),
  (SELECT MIN("Id") FROM "Users"),
  NOW() - INTERVAL '1 day' * ((row_number() OVER())%90)
FROM "Ships" s
CROSS JOIN generate_series(1, 2);

-- CrewModuleMain (crew module field definitions)
INSERT INTO "CrewModuleMains" ("FieldName")
VALUES 
  ('Personal Information'),
  ('Employment Details'),
  ('Medical Records'),
  ('Training Records'),
  ('Certifications'),
  ('Performance Evaluations'),
  ('Payroll Information'),
  ('Expense Reports'),
  ('Work Rest Hours'),
  ('Incident Reports');

-- CrewModuleDatabank (crew module sub-field definitions)
INSERT INTO "CrewModuleDatabanks" ("FieldId","SubTypeName")
SELECT 
  cm."Id",
  CASE cm."FieldName"
    WHEN 'Personal Information' THEN 
      CASE (row_number() OVER())%4
        WHEN 0 THEN 'Name'
        WHEN 1 THEN 'Date of Birth'
        WHEN 2 THEN 'Nationality'
        ELSE 'Contact Information'
      END
    WHEN 'Employment Details' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Position'
        WHEN 1 THEN 'Department'
        ELSE 'Start Date'
      END
    WHEN 'Medical Records' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Medical Certificate'
        WHEN 1 THEN 'Blood Type'
        ELSE 'Allergies'
      END
    WHEN 'Training Records' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Safety Training'
        WHEN 1 THEN 'Technical Training'
        ELSE 'Certification Training'
      END
    WHEN 'Certifications' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'STCW Certificates'
        WHEN 1 THEN 'Medical Certificates'
        ELSE 'Specialized Certificates'
      END
    WHEN 'Performance Evaluations' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Annual Review'
        WHEN 1 THEN 'Probation Review'
        ELSE 'Promotion Review'
      END
    WHEN 'Payroll Information' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Base Salary'
        WHEN 1 THEN 'Overtime'
        ELSE 'Bonuses'
      END
    WHEN 'Expense Reports' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Travel Expenses'
        WHEN 1 THEN 'Meal Allowances'
        ELSE 'Equipment Expenses'
      END
    WHEN 'Work Rest Hours' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Working Hours'
        WHEN 1 THEN 'Rest Hours'
        ELSE 'Overtime Hours'
      END
    ELSE 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Safety Incidents'
        WHEN 1 THEN 'Near Misses'
        ELSE 'Equipment Failures'
      END
  END
FROM "CrewModuleMains" cm
CROSS JOIN generate_series(1, 3);

-- DocumentModuleMain (document module field definitions)
INSERT INTO "DocumentModuleMains" ("FieldName")
VALUES 
  ('Document Management'),
  ('Version Control'),
  ('Access Control'),
  ('Document Types'),
  ('Storage Locations'),
  ('Retention Policies'),
  ('Approval Workflows'),
  ('Audit Trails'),
  ('Document Templates'),
  ('File Formats');

-- Update crew assignments to make some crew members available for assignment
-- Unassign some crew members from Zangazur so they appear in Available Crew

UPDATE "ShipAssignments" 
SET 
    "Status" = 'inactive',
    "UnassignedAt" = NOW()
WHERE "Id" IN (
    SELECT sa."Id"
    FROM "ShipAssignments" sa
    JOIN "Users" u ON sa."UserId" = u."Id"
    JOIN "Ships" s ON sa."ShipId" = s."Id"
    WHERE s."Name" = 'Zangazur' 
    AND sa."Status" = 'active'
    AND u."Email" IN (
        'georgy.ustyuzhanin@seed.local',
        'sergey.polonnikov@seed.local',
        'rustam.mammadov@seed.local',
        'vusal.alakbarov@seed.local'
    )
);

-- Add vessel manning data for Zangazur
-- First, delete any existing manning data for Zangazur to avoid duplicates
DELETE FROM "VesselMannings" 
WHERE "VesselId" IN (
    SELECT "Id" FROM "Ships" WHERE "Name" = 'Zangazur'
);

-- Then insert the new manning data
INSERT INTO "VesselMannings" ("VesselId", "Rank", "RequiredCount", "CurrentCount", "Notes", "CreatedAt")
SELECT 
    s."Id" as "VesselId",
    manning_data.rank,
    manning_data.required_count,
    manning_data.current_count,
    manning_data.notes,
    NOW() as "CreatedAt"
FROM "Ships" s
CROSS JOIN (
    VALUES 
        ('Master', 1, 1, 'Master position for Zangazur'),
        ('Ch Off', 1, 1, 'Chief Officer position'),
        ('2nd Off', 1, 1, 'Second Officer position'),
        ('3rd Off', 1, 1, 'Third Officer position'),
        ('Ch Eng', 1, 1, 'Chief Engineer position'),
        ('2nd Eng', 1, 1, 'Second Engineer position'),
        ('3rd Eng', 1, 1, 'Third Engineer position'),
        ('4th Eng', 1, 1, 'Fourth Engineer position'),
        ('ETO', 1, 1, 'Electro Technical Officer'),
        ('Pumpman', 1, 1, 'Pumpman position'),
        ('Bosun', 1, 1, 'Bosun position'),
        ('AB 1', 2, 2, 'Able Seaman positions'),
        ('AB 2', 1, 1, 'Able Seaman position'),
        ('AB 3', 1, 1, 'Able Seaman position'),
        ('OS 1', 1, 1, 'Ordinary Seaman position'),
        ('OS 2', 1, 1, 'Ordinary Seaman position'),
        ('Oiler 1', 1, 1, 'Oiler position'),
        ('Oiler 2', 1, 1, 'Oiler position'),
        ('Fitter', 1, 1, 'Fitter position'),
        ('Cook', 1, 1, 'Cook position'),
        ('Messman', 1, 1, 'Messman position')
) AS manning_data(rank, required_count, current_count, notes)
WHERE s."Name" = 'Zangazur';

-- DocumentModuleDatabank (document module sub-field definitions)
INSERT INTO "DocumentModuleDatabanks" ("FieldId","SubTypeName")
SELECT 
  dm."Id",
  CASE dm."FieldName"
    WHEN 'Document Management' THEN 
      CASE (row_number() OVER())%4
        WHEN 0 THEN 'Upload Documents'
        WHEN 1 THEN 'Download Documents'
        WHEN 2 THEN 'Delete Documents'
        ELSE 'Search Documents'
      END
    WHEN 'Version Control' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Create Version'
        WHEN 1 THEN 'Compare Versions'
        ELSE 'Restore Version'
      END
    WHEN 'Access Control' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'User Permissions'
        WHEN 1 THEN 'Role Permissions'
        ELSE 'Document Permissions'
      END
    WHEN 'Document Types' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'PDF Documents'
        WHEN 1 THEN 'Word Documents'
        ELSE 'Excel Spreadsheets'
      END
    WHEN 'Storage Locations' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Local Storage'
        WHEN 1 THEN 'Cloud Storage'
        ELSE 'Archive Storage'
      END
    WHEN 'Retention Policies' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Short Term'
        WHEN 1 THEN 'Medium Term'
        ELSE 'Long Term'
      END
    WHEN 'Approval Workflows' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Single Approval'
        WHEN 1 THEN 'Multi-level Approval'
        ELSE 'Automatic Approval'
      END
    WHEN 'Audit Trails' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Access Logs'
        WHEN 1 THEN 'Modification Logs'
        ELSE 'Deletion Logs'
      END
    WHEN 'Document Templates' THEN 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'Standard Templates'
        WHEN 1 THEN 'Custom Templates'
        ELSE 'Form Templates'
      END
    ELSE 
      CASE (row_number() OVER())%3
        WHEN 0 THEN 'PDF Format'
        WHEN 1 THEN 'DOCX Format'
        ELSE 'XLSX Format'
      END
  END
FROM "DocumentModuleMains" dm
CROSS JOIN generate_series(1, 3);

COMMIT;

SELECT 'Extended bulk seeding completed' AS status;

-- =============================
-- Comprehensive Sample Data for First 10 Users
-- =============================

BEGIN;

-- Get the first 10 users for detailed seeding
WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

-- Add comprehensive passport data for first 10 users
INSERT INTO "CrewPassports" ("UserId", "PassportNumber", "Nationality", "IssueDate", "ExpiryDate", "IssuedBy", "Notes", "CreatedAt")
SELECT 
  u."Id",
  'PPT-' || u."Name" || '-' || u."Surname" || '-001',
  CASE 
    WHEN u."Id" % 5 = 0 THEN 'American'
    WHEN u."Id" % 5 = 1 THEN 'British'
    WHEN u."Id" % 5 = 2 THEN 'German'
    WHEN u."Id" % 5 = 3 THEN 'French'
    ELSE 'Spanish'
  END,
  CURRENT_DATE - INTERVAL '3 years' - (u."Id" || ' days')::INTERVAL,
  CURRENT_DATE + INTERVAL '7 years' - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 3 = 0 THEN 'US Department of State'
    WHEN u."Id" % 3 = 1 THEN 'UK Passport Office'
    ELSE 'Ministry of Foreign Affairs'
  END,
  'Primary passport for ' || u."Name" || ' ' || u."Surname",
  NOW() - (u."Id" || ' days')::INTERVAL
FROM first_ten_users u;

COMMIT;

SELECT 'Passport data for first 10 users completed' AS status;

-- Add visa data
BEGIN;

WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

INSERT INTO "CrewVisas" ("UserId", "VisaType", "Country", "IssueDate", "ExpiryDate", "IssuedBy", "Notes", "CreatedAt")
SELECT 
  u."Id",
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Work Visa'
    WHEN u."Id" % 4 = 1 THEN 'Transit Visa'
    WHEN u."Id" % 4 = 2 THEN 'Tourist Visa'
    ELSE 'Business Visa'
  END,
  CASE 
    WHEN u."Id" % 6 = 0 THEN 'United States'
    WHEN u."Id" % 6 = 1 THEN 'United Kingdom'
    WHEN u."Id" % 6 = 2 THEN 'Germany'
    WHEN u."Id" % 6 = 3 THEN 'France'
    WHEN u."Id" % 6 = 4 THEN 'Singapore'
    ELSE 'Japan'
  END,
  CURRENT_DATE - INTERVAL '6 months' - (u."Id" || ' days')::INTERVAL,
  CURRENT_DATE + INTERVAL '2 years' - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 3 = 0 THEN 'US Embassy'
    WHEN u."Id" % 3 = 1 THEN 'UK Consulate'
    ELSE 'Local Embassy'
  END,
  'Visa for maritime operations - ' || u."Name" || ' ' || u."Surname",
  NOW() - (u."Id" || ' days')::INTERVAL
FROM first_ten_users u;

COMMIT;

SELECT 'Visa data for first 10 users completed' AS status;

-- Add medical records
BEGIN;

WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

INSERT INTO "CrewMedicalRecords" ("UserId", "ProviderName", "BloodGroup", "ExaminationDate", "ExpiryDate", "Notes", "CreatedAt")
SELECT 
  u."Id",
  CASE 
    WHEN u."Id" % 3 = 0 THEN 'Maritime Medical Center'
    WHEN u."Id" % 3 = 1 THEN 'International Seafarer Clinic'
    ELSE 'Port Medical Services'
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'A+'
    WHEN u."Id" % 4 = 1 THEN 'B+'
    WHEN u."Id" % 4 = 2 THEN 'O+'
    ELSE 'AB+'
  END,
  CURRENT_DATE - INTERVAL '6 months' - (u."Id" || ' days')::INTERVAL,
  CURRENT_DATE + INTERVAL '2 years' - (u."Id" || ' days')::INTERVAL,
  'Annual medical examination for ' || u."Name" || ' ' || u."Surname" || '. All tests passed. Fit for sea duty.',
  NOW() - (u."Id" || ' days')::INTERVAL
FROM first_ten_users u;

COMMIT;

SELECT 'Medical data for first 10 users completed' AS status;

-- Add payroll data
BEGIN;

WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

INSERT INTO "CrewPayrolls" ("CrewMemberId", "PeriodStart", "PeriodEnd", "BaseWage", "Overtime", "Bonuses", "Deductions", "Currency", "PaymentDate", "PaymentMethod")
SELECT 
  u."Id",
  CURRENT_DATE - INTERVAL '1 month' - (u."Id" || ' days')::INTERVAL,
  CURRENT_DATE - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 8500.00
    WHEN u."Id" % 5 = 1 THEN 7200.00
    WHEN u."Id" % 5 = 2 THEN 6800.00
    WHEN u."Id" % 5 = 3 THEN 7500.00
    ELSE 8000.00
  END,
  CASE 
    WHEN u."Id" % 3 = 0 THEN 1200.00
    WHEN u."Id" % 3 = 1 THEN 1500.00
    ELSE 1000.00
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 450.00
    WHEN u."Id" % 4 = 1 THEN 380.00
    WHEN u."Id" % 4 = 2 THEN 420.00
    ELSE 400.00
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 450.00
    WHEN u."Id" % 4 = 1 THEN 380.00
    WHEN u."Id" % 4 = 2 THEN 420.00
    ELSE 400.00
  END,
  'USD',
  CURRENT_DATE - (u."Id" || ' days')::INTERVAL,
  'Bank Transfer'
FROM first_ten_users u;

COMMIT;

SELECT 'Payroll data for first 10 users completed' AS status;

-- Add training data
BEGIN;

WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

INSERT INTO "CrewTrainings" ("UserId", "VesselId", "TrainingCategory", "Rank", "Trainer", "Training", "Source", "TrainingDate", "ExpireDate", "Status", "Remark", "CreatedAt", "CreatedByUserId", "CreatedById")
SELECT 
  u."Id",
  (SELECT "Id" FROM "Ships" ORDER BY "Id" LIMIT 1 OFFSET (u."Id" - 1) % 10),
  CASE 
    WHEN u."Id" % 5 = 0 THEN 'Safety Training'
    WHEN u."Id" % 5 = 1 THEN 'Fire Fighting'
    WHEN u."Id" % 5 = 2 THEN 'Medical Training'
    WHEN u."Id" % 5 = 3 THEN 'Survival Training'
    ELSE 'Security Training'
  END,
  CASE 
    WHEN u."Id" % 6 = 0 THEN 'Master'
    WHEN u."Id" % 6 = 1 THEN 'Chief Officer'
    WHEN u."Id" % 6 = 2 THEN 'Second Officer'
    WHEN u."Id" % 6 = 3 THEN 'Third Officer'
    WHEN u."Id" % 6 = 4 THEN 'Chief Engineer'
    ELSE 'Able Seaman'
  END,
  CASE 
    WHEN u."Id" % 3 = 0 THEN 'Maritime Training Institute'
    WHEN u."Id" % 3 = 1 THEN 'International Maritime Academy'
    ELSE 'Port Training Center'
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 'STCW Basic Safety Training'
    WHEN u."Id" % 5 = 1 THEN 'Advanced Fire Fighting'
    WHEN u."Id" % 5 = 2 THEN 'Medical First Aid'
    WHEN u."Id" % 5 = 3 THEN 'Personal Survival Techniques'
    ELSE 'Security Awareness Training'
  END,
  'Certificate',
  CURRENT_DATE - INTERVAL '1 year' - (u."Id" || ' days')::INTERVAL,
  CURRENT_DATE + INTERVAL '4 years' - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 3 = 0 THEN 'Completed'
    WHEN u."Id" % 3 = 1 THEN 'In Progress'
    ELSE 'Pending'
  END,
  'Mandatory training completed by ' || u."Name" || ' ' || u."Surname" || '. Certificate valid for 5 years.',
  NOW() - (u."Id" || ' days')::INTERVAL,
  (SELECT MIN("Id") FROM "Users"),
  (SELECT MIN("Id") FROM "Users")
FROM first_ten_users u;

COMMIT;

SELECT 'Training data for first 10 users completed' AS status;

-- Add reports data
BEGIN;

WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

INSERT INTO "CrewReports" ("UserId", "ReportType", "Title", "Details", "ReportDate", "CreatedAt")
SELECT 
  u."Id",
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Performance Review'
    WHEN u."Id" % 4 = 1 THEN 'Incident Report'
    WHEN u."Id" % 4 = 2 THEN 'Training Report'
    ELSE 'Safety Report'
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Annual Performance Evaluation - ' || u."Name" || ' ' || u."Surname"
    WHEN u."Id" % 4 = 1 THEN 'Incident Report - ' || u."Name" || ' ' || u."Surname"
    WHEN u."Id" % 4 = 2 THEN 'Training Completion Report - ' || u."Name" || ' ' || u."Surname"
    ELSE 'Safety Inspection Report - ' || u."Name" || ' ' || u."Surname"
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Annual performance evaluation for ' || u."Name" || ' ' || u."Surname" || '. Excellent work performance and safety record.'
    WHEN u."Id" % 4 = 1 THEN 'Minor incident report involving ' || u."Name" || ' ' || u."Surname" || '. No injuries, proper procedures followed.'
    WHEN u."Id" % 4 = 2 THEN 'Training completion report for ' || u."Name" || ' ' || u."Surname" || '. All required certifications updated.'
    ELSE 'Safety inspection report for ' || u."Name" || ' ' || u."Surname" || '. All safety protocols followed correctly.'
  END,
  CURRENT_DATE - INTERVAL '2 months' - (u."Id" || ' days')::INTERVAL,
  NOW() - (u."Id" || ' days')::INTERVAL
FROM first_ten_users u;

COMMIT;

SELECT 'Reports data for first 10 users completed' AS status;

-- Add expenses data
BEGIN;

WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

-- Create expense reports first
INSERT INTO "CrewExpenseReport" ("CrewMemberId", "ShipId", "TotalAmount", "Currency", "ReportDate", "Notes", "CreatedAt")
SELECT 
  u."Id",
  (SELECT "Id" FROM "Ships" ORDER BY "Id" LIMIT 1 OFFSET (u."Id" - 1) % 10),
  0, -- Will be updated later
  'USD',
  CURRENT_DATE - INTERVAL '1 month' - (u."Id" || ' days')::INTERVAL,
  'Expense report for ' || u."Name" || ' ' || u."Surname",
  NOW() - (u."Id" || ' days')::INTERVAL
FROM first_ten_users u;

-- Add individual expenses
WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)
INSERT INTO "CrewExpenses" ("ExpenseReportId", "CrewMemberId", "Category", "Amount", "Currency", "ExpenseDate", "Notes", "CreatedAt")
SELECT 
  r."Id",
  r."CrewMemberId",
  CASE 
    WHEN u."Id" % 5 = 0 THEN 'Travel'
    WHEN u."Id" % 5 = 1 THEN 'Accommodation'
    WHEN u."Id" % 5 = 2 THEN 'Meals'
    WHEN u."Id" % 5 = 3 THEN 'Transportation'
    ELSE 'Medical'
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 250.00
    WHEN u."Id" % 4 = 1 THEN 180.00
    WHEN u."Id" % 4 = 2 THEN 320.00
    ELSE 150.00
  END,
  'USD',
  CURRENT_DATE - INTERVAL '1 month' - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 'Travel expenses for ' || u."Name" || ' ' || u."Surname" || ' to join vessel'
    WHEN u."Id" % 5 = 1 THEN 'Hotel accommodation for ' || u."Name" || ' ' || u."Surname" || ' during port stay'
    WHEN u."Id" % 5 = 2 THEN 'Meal allowance for ' || u."Name" || ' ' || u."Surname" || ' during shore leave'
    WHEN u."Id" % 5 = 3 THEN 'Transportation costs for ' || u."Name" || ' ' || u."Surname" || ' to/from port'
    ELSE 'Medical expenses for ' || u."Name" || ' ' || u."Surname" || ' health checkup'
  END,
  NOW() - (u."Id" || ' days')::INTERVAL
FROM "CrewExpenseReport" r
JOIN first_ten_users u ON u."Id" = r."CrewMemberId";

-- Update report totals
UPDATE "CrewExpenseReport" r
SET "TotalAmount" = s.sum_amount
FROM (
  SELECT "ExpenseReportId", SUM("Amount") AS sum_amount
  FROM "CrewExpenses"
  GROUP BY "ExpenseReportId"
) s
WHERE r."Id" = s."ExpenseReportId";

COMMIT;

SELECT 'Expenses data for first 10 users completed' AS status;

-- Add assignments data
BEGIN;

WITH first_ten_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id" 
  LIMIT 10
)

INSERT INTO "ShipAssignments" ("ShipId", "UserId", "Position", "AssignedAt", "Status", "AssignedByUserId", "Notes")
SELECT 
  (SELECT "Id" FROM "Ships" ORDER BY "Id" LIMIT 1 OFFSET (u."Id" - 1) % 10),
  u."Id",
  CASE 
    WHEN u."Id" % 6 = 0 THEN 'Master'
    WHEN u."Id" % 6 = 1 THEN 'Chief Officer'
    WHEN u."Id" % 6 = 2 THEN 'Second Officer'
    WHEN u."Id" % 6 = 3 THEN 'Third Officer'
    WHEN u."Id" % 6 = 4 THEN 'Chief Engineer'
    ELSE 'Able Seaman'
  END,
  CURRENT_DATE - INTERVAL '6 months' - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 3 = 0 THEN 'active'
    WHEN u."Id" % 3 = 1 THEN 'completed'
    ELSE 'pending'
  END,
  (SELECT MIN("Id") FROM "Users"),
  'Current assignment for ' || u."Name" || ' ' || u."Surname" || ' on vessel'
FROM first_ten_users u;

COMMIT;

SELECT 'Assignments data for first 10 users completed' AS status;

-- =============================
-- Additional Comprehensive Training Data
-- =============================

BEGIN;

-- Add more training records for all users
WITH all_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id"
)

INSERT INTO "CrewTrainings" ("UserId", "VesselId", "TrainingCategory", "Rank", "Trainer", "Training", "Source", "TrainingDate", "ExpireDate", "Status", "Remark", "CreatedAt", "CreatedByUserId", "CreatedById")
SELECT 
  u."Id",
  (SELECT "Id" FROM "Ships" ORDER BY "Id" LIMIT 1),
  CASE 
    WHEN u."Id" % 8 = 0 THEN 'Fire Fighting'
    WHEN u."Id" % 8 = 1 THEN 'Medical First Aid'
    WHEN u."Id" % 8 = 2 THEN 'Personal Survival Techniques'
    WHEN u."Id" % 8 = 3 THEN 'Security Awareness'
    WHEN u."Id" % 8 = 4 THEN 'Bridge Resource Management'
    WHEN u."Id" % 8 = 5 THEN 'Engine Room Resource Management'
    WHEN u."Id" % 8 = 6 THEN 'Advanced Fire Fighting'
    ELSE 'Radar Navigation'
  END,
  CASE 
    WHEN u."Id" % 7 = 0 THEN 'Master'
    WHEN u."Id" % 7 = 1 THEN 'Chief Officer'
    WHEN u."Id" % 7 = 2 THEN 'Second Officer'
    WHEN u."Id" % 7 = 3 THEN 'Third Officer'
    WHEN u."Id" % 7 = 4 THEN 'Chief Engineer'
    WHEN u."Id" % 7 = 5 THEN 'Second Engineer'
    ELSE 'Able Seaman'
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Maritime Training Center'
    WHEN u."Id" % 4 = 1 THEN 'International Maritime Academy'
    WHEN u."Id" % 4 = 2 THEN 'Port Training Institute'
    ELSE 'Safety Training Center'
  END,
  CASE 
    WHEN u."Id" % 8 = 0 THEN 'Advanced Fire Fighting Course'
    WHEN u."Id" % 8 = 1 THEN 'Medical First Aid at Sea'
    WHEN u."Id" % 8 = 2 THEN 'Personal Survival Techniques'
    WHEN u."Id" % 8 = 3 THEN 'Security Awareness Training'
    WHEN u."Id" % 8 = 4 THEN 'Bridge Resource Management'
    WHEN u."Id" % 8 = 5 THEN 'Engine Room Resource Management'
    WHEN u."Id" % 8 = 6 THEN 'Advanced Fire Fighting Refresher'
    ELSE 'Radar Navigation and ARPA'
  END,
  'Certificate',
  CURRENT_DATE - INTERVAL '6 months' - (u."Id" || ' days')::INTERVAL,
  CURRENT_DATE + INTERVAL '4 years' - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Completed'
    WHEN u."Id" % 4 = 1 THEN 'In Progress'
    WHEN u."Id" % 4 = 2 THEN 'Pending'
    ELSE 'Completed'
  END,
  'Professional training completed by ' || u."Name" || ' ' || u."Surname" || '. Certificate valid for 5 years from completion date.',
  NOW() - (u."Id" || ' days')::INTERVAL,
  (SELECT MIN("Id") FROM "Users"),
  (SELECT MIN("Id") FROM "Users")
FROM all_users u;

COMMIT;

SELECT 'Additional training data completed' AS status;

-- =============================
-- Additional Comprehensive Evaluation Data
-- =============================

BEGIN;

-- Add more evaluation records for all users
WITH all_users AS (
  SELECT "Id", "Name", "Surname", "Email" 
  FROM "Users" 
  ORDER BY "Id"
)

INSERT INTO "CrewEvaluations" ("UserId", "VesselId", "FormNo", "RevisionNo", "RevisionDate", "FormName", "FormDescription", "EnteredByUserId", "EnteredDate", "Rank", "Name", "Surname", "TechnicalCompetence", "SafetyAwareness", "Teamwork", "Communication", "Leadership", "ProblemSolving", "Adaptability", "WorkEthic", "OverallRating", "Strengths", "AreasForImprovement", "Comments", "Status", "CreatedByUserId", "CreatedById", "CreatedAt")
SELECT 
  u."Id",
  (SELECT "Id" FROM "Ships" ORDER BY "Id" LIMIT 1),
  'EVAL-' || lpad(u."Id"::text, 4, '0') || '-2024',
  '2.0',
  CURRENT_DATE - INTERVAL '3 months' - (u."Id" || ' days')::INTERVAL,
  'Annual Performance Evaluation',
  'Comprehensive annual performance evaluation covering all aspects of crew performance',
  (SELECT MIN("Id") FROM "Users"),
  CURRENT_DATE - INTERVAL '3 months' - (u."Id" || ' days')::INTERVAL,
  CASE 
    WHEN u."Id" % 7 = 0 THEN 'Master'
    WHEN u."Id" % 7 = 1 THEN 'Chief Officer'
    WHEN u."Id" % 7 = 2 THEN 'Second Officer'
    WHEN u."Id" % 7 = 3 THEN 'Third Officer'
    WHEN u."Id" % 7 = 4 THEN 'Chief Engineer'
    WHEN u."Id" % 7 = 5 THEN 'Second Engineer'
    ELSE 'Able Seaman'
  END,
  u."Name",
  u."Surname",
  CASE 
    WHEN u."Id" % 5 = 0 THEN 5
    WHEN u."Id" % 5 = 1 THEN 4
    WHEN u."Id" % 5 = 2 THEN 3
    WHEN u."Id" % 5 = 3 THEN 4
    ELSE 5
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 5
    WHEN u."Id" % 5 = 1 THEN 4
    WHEN u."Id" % 5 = 2 THEN 4
    WHEN u."Id" % 5 = 3 THEN 3
    ELSE 5
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 4
    WHEN u."Id" % 5 = 1 THEN 5
    WHEN u."Id" % 5 = 2 THEN 3
    WHEN u."Id" % 5 = 3 THEN 4
    ELSE 4
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 4
    WHEN u."Id" % 5 = 1 THEN 4
    WHEN u."Id" % 5 = 2 THEN 3
    WHEN u."Id" % 5 = 3 THEN 5
    ELSE 4
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 5
    WHEN u."Id" % 5 = 1 THEN 3
    WHEN u."Id" % 5 = 2 THEN 4
    WHEN u."Id" % 5 = 3 THEN 4
    ELSE 5
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 4
    WHEN u."Id" % 5 = 1 THEN 4
    WHEN u."Id" % 5 = 2 THEN 3
    WHEN u."Id" % 5 = 3 THEN 4
    ELSE 5
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 5
    WHEN u."Id" % 5 = 1 THEN 4
    WHEN u."Id" % 5 = 2 THEN 4
    WHEN u."Id" % 5 = 3 THEN 3
    ELSE 4
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 4
    WHEN u."Id" % 5 = 1 THEN 4
    WHEN u."Id" % 5 = 2 THEN 3
    WHEN u."Id" % 5 = 3 THEN 4
    ELSE 5
  END,
  CASE 
    WHEN u."Id" % 5 = 0 THEN 5
    WHEN u."Id" % 5 = 1 THEN 4
    WHEN u."Id" % 5 = 2 THEN 3
    WHEN u."Id" % 5 = 3 THEN 4
    ELSE 5
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Excellent technical skills and leadership abilities. Strong safety record.'
    WHEN u."Id" % 4 = 1 THEN 'Good performance with room for improvement in communication skills.'
    WHEN u."Id" % 4 = 2 THEN 'Satisfactory performance, needs development in problem-solving.'
    ELSE 'Outstanding performance across all evaluation criteria.'
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Continue current performance level. Consider advanced training opportunities.'
    WHEN u."Id" % 4 = 1 THEN 'Focus on improving communication and teamwork skills.'
    WHEN u."Id" % 4 = 2 THEN 'Develop problem-solving and decision-making capabilities.'
    ELSE 'Maintain excellent standards and mentor junior crew members.'
  END,
  CASE 
    WHEN u."Id" % 4 = 0 THEN 'Crew member demonstrates exceptional performance and is recommended for promotion consideration.'
    WHEN u."Id" % 4 = 1 THEN 'Good overall performance with specific areas identified for improvement.'
    WHEN u."Id" % 4 = 2 THEN 'Satisfactory performance meeting basic requirements.'
    ELSE 'Outstanding crew member who exceeds expectations in all areas.'
  END,
  CASE 
    WHEN u."Id" % 3 = 0 THEN 'Completed'
    WHEN u."Id" % 3 = 1 THEN 'Draft'
    ELSE 'Signed'
  END,
  (SELECT MIN("Id") FROM "Users"),
  (SELECT MIN("Id") FROM "Users"),
  NOW() - (u."Id" || ' days')::INTERVAL
FROM all_users u;

COMMIT;

SELECT 'Additional evaluation data completed' AS status;


