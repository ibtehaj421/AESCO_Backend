using Microsoft.EntityFrameworkCore;
using ASCO.Models;

namespace ASCO.DbContext
{
    public class ASCODbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ASCODbContext(DbContextOptions<ASCODbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }



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

        }
    }
}