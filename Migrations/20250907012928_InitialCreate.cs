using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendAESCO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrewModuleMains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewModuleMains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Module = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    TimeZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Facilities = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MaxDraft = table.Column<double>(type: "double precision", nullable: true),
                    MaxLength = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IMONumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CallSign = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ShipType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Flag = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Builder = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BuildDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LaunchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LengthOverall = table.Column<double>(type: "double precision", nullable: false),
                    Beam = table.Column<double>(type: "double precision", nullable: false),
                    Draft = table.Column<double>(type: "double precision", nullable: false),
                    GrossTonnage = table.Column<double>(type: "double precision", nullable: false),
                    NetTonnage = table.Column<double>(type: "double precision", nullable: false),
                    DeadweightTonnage = table.Column<double>(type: "double precision", nullable: false),
                    PassengerCapacity = table.Column<int>(type: "integer", nullable: true),
                    CrewCapacity = table.Column<int>(type: "integer", nullable: true),
                    MaxSpeed = table.Column<double>(type: "double precision", nullable: false),
                    ServiceSpeed = table.Column<double>(type: "double precision", nullable: false),
                    EngineType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EnginePower = table.Column<double>(type: "double precision", nullable: false),
                    HomePort = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nationality = table.Column<string>(type: "text", nullable: false),
                    IdenNumber = table.Column<long>(type: "bigint", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BirthPlace = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    JobType = table.Column<string>(type: "text", nullable: false),
                    Rank = table.Column<string>(type: "text", nullable: false),
                    MaritalStatus = table.Column<string>(type: "text", nullable: true),
                    MilitaryStatus = table.Column<string>(type: "text", nullable: true),
                    EducationLevel = table.Column<string>(type: "text", nullable: true),
                    GraduationYear = table.Column<int>(type: "integer", nullable: false),
                    School = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Competency = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OrganizationUnit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FatherName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    EmailConfirmationToken = table.Column<string>(type: "text", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "text", nullable: true),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "integer", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrewModuleDatabanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FieldId = table.Column<int>(type: "integer", nullable: false),
                    SubTypeName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewModuleDatabanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewModuleDatabanks_CrewModuleMains_FieldId",
                        column: x => x.FieldId,
                        principalTable: "CrewModuleMains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", nullable: false),
                    GrantedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VesselMannings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VesselMannings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VesselMannings_Ships_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Voyages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    VoyageNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DeparturePort = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ArrivalPort = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PlannedDeparture = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlannedArrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualDeparture = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualArrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CargoType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CargoWeight = table.Column<double>(type: "double precision", nullable: true),
                    Distance = table.Column<double>(type: "double precision", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voyages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voyages_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    CertificateType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CertificateNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IssuedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IssuedAt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Conditions = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Certificates_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewCertifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CertificationType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CertificateNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IssuedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IssuedAt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Limitations = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewCertifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewCertifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrewEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    FormNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RevisionNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RevisionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FormName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FormDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EnteredByUserId = table.Column<int>(type: "integer", nullable: false),
                    EnteredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Rank = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UniqueId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TechnicalCompetence = table.Column<int>(type: "integer", nullable: true),
                    SafetyAwareness = table.Column<int>(type: "integer", nullable: true),
                    Teamwork = table.Column<int>(type: "integer", nullable: true),
                    Communication = table.Column<int>(type: "integer", nullable: true),
                    Leadership = table.Column<int>(type: "integer", nullable: true),
                    ProblemSolving = table.Column<int>(type: "integer", nullable: true),
                    Adaptability = table.Column<int>(type: "integer", nullable: true),
                    WorkEthic = table.Column<int>(type: "integer", nullable: true),
                    OverallRating = table.Column<int>(type: "integer", nullable: true),
                    Strengths = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AreasForImprovement = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Comments = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CrewMemberComments = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CrewMemberSignature = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CrewMemberSignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Attachments = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewEvaluations_Ships_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewEvaluations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewEvaluations_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewEvaluations_Users_EnteredByUserId",
                        column: x => x.EnteredByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewEvaluations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewExpenseReport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrewMemberId = table.Column<int>(type: "integer", nullable: false),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewExpenseReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewExpenseReport_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewExpenseReport_Users_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrewMedicalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ProviderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BloodGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExaminationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMedicalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewMedicalRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewPassports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PassportNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nationality = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewPassports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewPassports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewPayrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CrewMemberId = table.Column<int>(type: "integer", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseWage = table.Column<decimal>(type: "numeric", nullable: false),
                    Overtime = table.Column<decimal>(type: "numeric", nullable: false),
                    Bonuses = table.Column<decimal>(type: "numeric", nullable: false),
                    Deductions = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewPayrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewPayrolls_Users_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ReportType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Details = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ReportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewReports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    TrainingCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rank = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Trainer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Remark = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Training = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TrainingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Attachments = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewTrainings_Ships_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewTrainings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewTrainings_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewTrainings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewVisas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    VisaType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssuedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewVisas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewVisas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewWorkRestHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WorkHours = table.Column<decimal>(type: "numeric", nullable: false),
                    RestHours = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalHours = table.Column<decimal>(type: "numeric", nullable: false),
                    WorkDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RestDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewWorkRestHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewWorkRestHours_Ships_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewWorkRestHours_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewWorkRestHours_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CrewWorkRestHours_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PhysicalPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    MimeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsFolder = table.Column<bool>(type: "boolean", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Documents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    InspectionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InspectedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NextInspectionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Result = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Findings = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Recommendations = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CertificateNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CertificateExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inspections_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    MaintenanceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Component = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EstimatedCost = table.Column<double>(type: "double precision", nullable: true),
                    ActualCost = table.Column<double>(type: "double precision", nullable: true),
                    PerformedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Priority = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ToUserId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RelatedEntityType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RelatedEntityId = table.Column<int>(type: "integer", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsEmailSent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShipAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnassignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AssignedByUserId = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShipAssignments_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipAssignments_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatementOfCash",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VesselId = table.Column<int>(type: "integer", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Inflow = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Outflow = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementOfCash", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatementOfCash_Ships_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatementOfCash_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AssignedByUserId = table.Column<int>(type: "integer", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_AssignedByUserId",
                        column: x => x.AssignedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    VoyageId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IncidentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IncidentDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ImmediateActions = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RootCause = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PreventiveMeasures = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    AuthoritiesNotified = table.Column<bool>(type: "boolean", nullable: false),
                    AuthoritiesNotifiedDetails = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ReportedByUserId = table.Column<int>(type: "integer", nullable: false),
                    InvestigatedByUserId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidents_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_InvestigatedByUserId",
                        column: x => x.InvestigatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Users_ReportedByUserId",
                        column: x => x.ReportedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Voyages_VoyageId",
                        column: x => x.VoyageId,
                        principalTable: "Voyages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PortCalls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShipId = table.Column<int>(type: "integer", nullable: false),
                    PortId = table.Column<int>(type: "integer", nullable: false),
                    VoyageId = table.Column<int>(type: "integer", nullable: true),
                    Purpose = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PlannedArrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PlannedDeparture = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualArrival = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualDeparture = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    BerthNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CargoLoaded = table.Column<double>(type: "double precision", nullable: true),
                    CargoUnloaded = table.Column<double>(type: "double precision", nullable: true),
                    FuelTaken = table.Column<double>(type: "double precision", nullable: true),
                    PortCharges = table.Column<double>(type: "double precision", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortCalls_Ports_PortId",
                        column: x => x.PortId,
                        principalTable: "Ports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PortCalls_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PortCalls_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PortCalls_Voyages_VoyageId",
                        column: x => x.VoyageId,
                        principalTable: "Voyages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VoyageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VoyageId = table.Column<int>(type: "integer", nullable: false),
                    LogTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LogType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LogEntry = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Speed = table.Column<double>(type: "double precision", nullable: true),
                    Course = table.Column<double>(type: "double precision", nullable: true),
                    WeatherConditions = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    WindSpeed = table.Column<double>(type: "double precision", nullable: true),
                    WindDirection = table.Column<double>(type: "double precision", nullable: true),
                    SeaState = table.Column<double>(type: "double precision", nullable: true),
                    LoggedByUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoyageLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoyageLogs_Users_LoggedByUserId",
                        column: x => x.LoggedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VoyageLogs_Voyages_VoyageId",
                        column: x => x.VoyageId,
                        principalTable: "Voyages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CrewExpenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExpenseReportId = table.Column<long>(type: "bigint", nullable: false),
                    CrewMemberId = table.Column<int>(type: "integer", nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewExpenses_CrewExpenseReport_ExpenseReportId",
                        column: x => x.ExpenseReportId,
                        principalTable: "CrewExpenseReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewExpenses_Users_CrewMemberId",
                        column: x => x.CrewMemberId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActionById = table.Column<int>(type: "integer", nullable: false),
                    ActionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentLogs_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentLogs_Users_ActionById",
                        column: x => x.ActionById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhysicalPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    VersionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<string>(type: "text", nullable: false),
                    ChangeDescription = table.Column<string>(type: "text", nullable: true),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentVersions_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CreatedByUserId",
                table: "Certificates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ShipId",
                table: "Certificates",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewCertifications_UserId",
                table: "CrewCertifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewEvaluations_CreatedById",
                table: "CrewEvaluations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CrewEvaluations_CreatedByUserId",
                table: "CrewEvaluations",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewEvaluations_EnteredByUserId",
                table: "CrewEvaluations",
                column: "EnteredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewEvaluations_UserId",
                table: "CrewEvaluations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewEvaluations_VesselId",
                table: "CrewEvaluations",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenseReport_CrewMemberId",
                table: "CrewExpenseReport",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenseReport_ShipId",
                table: "CrewExpenseReport",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenses_CrewMemberId",
                table: "CrewExpenses",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewExpenses_ExpenseReportId",
                table: "CrewExpenses",
                column: "ExpenseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMedicalRecords_UserId",
                table: "CrewMedicalRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewModuleDatabanks_FieldId",
                table: "CrewModuleDatabanks",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewPassports_UserId",
                table: "CrewPassports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewPayrolls_CrewMemberId",
                table: "CrewPayrolls",
                column: "CrewMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewReports_UserId",
                table: "CrewReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewTrainings_CreatedById",
                table: "CrewTrainings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CrewTrainings_CreatedByUserId",
                table: "CrewTrainings",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewTrainings_UserId",
                table: "CrewTrainings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewTrainings_VesselId",
                table: "CrewTrainings",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewVisas_UserId",
                table: "CrewVisas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewWorkRestHours_CreatedById",
                table: "CrewWorkRestHours",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CrewWorkRestHours_CreatedByUserId",
                table: "CrewWorkRestHours",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewWorkRestHours_UserId",
                table: "CrewWorkRestHours",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewWorkRestHours_VesselId",
                table: "CrewWorkRestHours",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLogs_ActionById",
                table: "DocumentLogs",
                column: "ActionById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLogs_DocumentId",
                table: "DocumentLogs",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedById",
                table: "Documents",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ParentId",
                table: "Documents",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UpdatedById",
                table: "Documents",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_DocumentId",
                table: "DocumentVersions",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_InvestigatedByUserId",
                table: "Incidents",
                column: "InvestigatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ReportedByUserId",
                table: "Incidents",
                column: "ReportedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ShipId",
                table: "Incidents",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_VoyageId",
                table: "Incidents",
                column: "VoyageId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_CreatedById",
                table: "Inspections",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_ShipId",
                table: "Inspections",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_CreatedByUserId",
                table: "MaintenanceRecords",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_ShipId",
                table: "MaintenanceRecords",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ToUserId",
                table: "Notifications",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PortCalls_CreatedByUserId",
                table: "PortCalls",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PortCalls_PortId",
                table: "PortCalls",
                column: "PortId");

            migrationBuilder.CreateIndex(
                name: "IX_PortCalls_ShipId",
                table: "PortCalls",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_PortCalls_VoyageId",
                table: "PortCalls",
                column: "VoyageId");

            migrationBuilder.CreateIndex(
                name: "IX_Ports_Name",
                table: "Ports",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipAssignments_AssignedByUserId",
                table: "ShipAssignments",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipAssignments_ShipId",
                table: "ShipAssignments",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipAssignments_UserId",
                table: "ShipAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatementOfCash_CreatedById",
                table: "StatementOfCash",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_StatementOfCash_VesselId",
                table: "StatementOfCash",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_AssignedByUserId",
                table: "UserRoles",
                column: "AssignedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_VesselMannings_VesselId",
                table: "VesselMannings",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_VoyageLogs_LoggedByUserId",
                table: "VoyageLogs",
                column: "LoggedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoyageLogs_VoyageId",
                table: "VoyageLogs",
                column: "VoyageId");

            migrationBuilder.CreateIndex(
                name: "IX_Voyages_ShipId",
                table: "Voyages",
                column: "ShipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "CrewCertifications");

            migrationBuilder.DropTable(
                name: "CrewEvaluations");

            migrationBuilder.DropTable(
                name: "CrewExpenses");

            migrationBuilder.DropTable(
                name: "CrewMedicalRecords");

            migrationBuilder.DropTable(
                name: "CrewModuleDatabanks");

            migrationBuilder.DropTable(
                name: "CrewPassports");

            migrationBuilder.DropTable(
                name: "CrewPayrolls");

            migrationBuilder.DropTable(
                name: "CrewReports");

            migrationBuilder.DropTable(
                name: "CrewTrainings");

            migrationBuilder.DropTable(
                name: "CrewVisas");

            migrationBuilder.DropTable(
                name: "CrewWorkRestHours");

            migrationBuilder.DropTable(
                name: "DocumentLogs");

            migrationBuilder.DropTable(
                name: "DocumentVersions");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PortCalls");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "ShipAssignments");

            migrationBuilder.DropTable(
                name: "StatementOfCash");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "VesselMannings");

            migrationBuilder.DropTable(
                name: "VoyageLogs");

            migrationBuilder.DropTable(
                name: "CrewExpenseReport");

            migrationBuilder.DropTable(
                name: "CrewModuleMains");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Ports");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Voyages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Ships");
        }
    }
}
