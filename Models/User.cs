using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Surname { get; set; }
        [Required]
        public string? Nationality { get; set; }
        [Required]
        public long IdenNumber { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }

        public string? BirthPlace { get; set; }
        [Required]
        public string? Gender { get; set; } //M or F.
        [Required]
        public string? Status { get; set; } //active, inactive, pending, banned etc
        [Required]
        public string? JobType { get; set; } //full time, part time, contractor etc
        [Required]
        public string? Rank { get; set; }
        public string? MaritalStatus { get; set; } //single, married, divorced, widowed etc
        public string? MilitaryStatus { get; set; } //completed, exempted, ongoing, not applicable etc
        public string? EducationLevel { get; set; } //high school, bachelor, master, phd etc

        [Range(1900, 3000)]
        public int GraduationYear { get; set; }

        [MaxLength(100)]
        public string? School { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Competency { get; set; }

        [MaxLength(100)]
        public string? OrganizationUnit { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? WorkEndDate { get; set; } //this value can be null if the user is still working.

        [MaxLength(50)]
        public string? FatherName { get; set; }

        // Security fields
        public DateTime? LastLoginAt { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    public class UserRole
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int RoleId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public int AssignedByUserId { get; set; } // Track who assigned the role
        public DateTime? ExpiresAt { get; set; } // Optional: role expiration
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
        public virtual User AssignedBy { get; set; } = null!;
    }
    
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Module { get; set; } = string.Empty; // e.g., "Users", "Reports", etc.

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    [Table("RolePermissions")]
    public class RolePermission
    {
        [Key]
        public int RoleId { get; set; }
        
        [Key]
        public int PermissionId { get; set; }

        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

        public virtual Role Role { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}

