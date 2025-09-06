using Microsoft.EntityFrameworkCore;
using ASCO.Models;
using FluentAssertions;

namespace ASCO.DbContext
{
    public class ASCODbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ASCODbContext(DbContextOptions<ASCODbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Document> Documents { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Voyage> Voyages { get; set; }
        public DbSet<VoyageLog> VoyageLogs { get; set; }

        public DbSet<Port> Ports { get; set; }

        public DbSet<PortCall> PortCalls { get; set; }

        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<ShipAssignment> ShipAssignments { get; set; }
        public DbSet<CrewCertification> CrewCertifications { get; set; }
        public DbSet<CrewMedicalRecord> CrewMedicalRecords { get; set; }
        public DbSet<CrewPassport> CrewPassports { get; set; }
        public DbSet<CrewVisa> CrewVisas { get; set; }
        public DbSet<CrewReport> CrewReports { get; set; }
        public DbSet<VesselManning> VesselMannings { get; set; }

        public DbSet<Payroll> PayrollRecords { get; set; }
        public DbSet<CrewExpenseReport> CrewExpenseReports { get; set; }
        public DbSet<CrewExpense> CrewExpenses { get; set; }
        
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<StatementOfCash> CashStatements { get; set; }

        //on model creating method

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configurations can be added here if needed
            modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.AssignedBy)
            .WithMany()
            .HasForeignKey(ur => ur.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes


            //ship assignment configuration
            modelBuilder.Entity<ShipAssignment>()
            .HasOne(sa => sa.AssignedBy)
            .WithMany()
            .HasForeignKey(sa => sa.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<ShipAssignment>()
            .HasOne(sa => sa.User)
            .WithMany()
            .HasForeignKey(sa => sa.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShipAssignment>()
            .HasOne(sa => sa.Ship)
            .WithMany(s => s.ShipAssignments)
            .HasForeignKey(sa => sa.ShipId)
            .OnDelete(DeleteBehavior.Restrict);

            //inicident configuration
            modelBuilder.Entity<Incident>()
            .HasOne(i => i.ReportedBy)
            .WithMany()
            .HasForeignKey(i => i.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Incident>()
            .HasOne(i => i.InvestigatedBy)
            .WithMany()
            .HasForeignKey(i => i.InvestigatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Incident>()
            .HasOne(i => i.Ship)
            .WithMany()
            .HasForeignKey(i => i.ShipId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Incident>()
            .HasOne(i => i.Voyage)
            .WithMany()
            .HasForeignKey(i => i.VoyageId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes


            //certificate configuration
            modelBuilder.Entity<Certificate>()
            .HasOne(c => c.CreatedBy)
            .WithMany()
            .HasForeignKey(c => c.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Certificate>()
            .HasOne(c => c.Ship)
            .WithMany(s => s.Certificates)
            .HasForeignKey(c => c.ShipId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            //document configuration
            modelBuilder.Entity<Document>()
            .HasOne(d => d.UploadedBy)
            .WithMany()
            .HasForeignKey(d => d.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            //notification configuration
            modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<Notification>()
            .HasOne(n => n.ToUser)
            .WithMany()
            .HasForeignKey(n => n.ToUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            //maintenance record configuration
            modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(m => m.CreatedBy)
            .WithMany()
            .HasForeignKey(m => m.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(m => m.Ship)
            .WithMany(s => s.MaintenanceRecords)
            .HasForeignKey(m => m.ShipId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            //port calls configuration
            modelBuilder.Entity<PortCall>()
            .HasOne(pc => pc.CreatedBy)
            .WithMany()
            .HasForeignKey(pc => pc.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<PortCall>()
            .HasOne(pc => pc.Ship)
            .WithMany()
            .HasForeignKey(pc => pc.ShipId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<PortCall>()
            .HasOne(pc => pc.Port)
            .WithMany(p => p.PortCalls)
            .HasForeignKey(pc => pc.PortId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<PortCall>()
            .HasOne(pc => pc.Voyage)
            .WithMany()
            .HasForeignKey(pc => pc.VoyageId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            //voyage log configuration
            modelBuilder.Entity<VoyageLog>()
            .HasOne(vl => vl.LoggedBy)
            .WithMany()
            .HasForeignKey(vl => vl.LoggedByUserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<VoyageLog>()
            .HasOne(vl => vl.Voyage)
            .WithMany(v => v.VoyageLogs)
            .HasForeignKey(vl => vl.VoyageId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            //ports configuration
            modelBuilder.Entity<Port>()
            .HasIndex(p => p.Name)
            .IsUnique();

            // crew medical
            modelBuilder.Entity<CrewMedicalRecord>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            // crew passport
            modelBuilder.Entity<CrewPassport>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            // crew visa
            modelBuilder.Entity<CrewVisa>()
            .HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            // crew reports
            modelBuilder.Entity<CrewReport>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            //vessel manning
            modelBuilder.Entity<VesselManning>()
            .HasOne(vm => vm.Vessel)
            .WithMany()
            .HasForeignKey(vm => vm.VesselId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes, will look into this later

            //payroll records
            modelBuilder.Entity<Payroll>()
            .HasOne(p => p.CrewMember)
            .WithMany()
            .HasForeignKey(p => p.CrewMemberId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes, will look into this later

            //crew expenses
            modelBuilder.Entity<CrewExpense>()
            .HasOne(ce => ce.CrewMember)
            .WithMany()
            .HasForeignKey(ce => ce.CrewMemberId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes, will look into this later
            
            //cash statements

        }
    }
}