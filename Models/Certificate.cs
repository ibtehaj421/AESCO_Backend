using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("Certificates")]
    public class Certificate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CertificateType { get; set; } = string.Empty; // safety, radio, tonnage, etc.

        [Required]
        [MaxLength(50)]
        public string CertificateNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string IssuedBy { get; set; } = string.Empty; // authority/organization

        [Required]
        public DateTime IssuedDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "valid"; // valid, expired, suspended, revoked

        [MaxLength(100)]
        public string? IssuedAt { get; set; } // port/location

        [MaxLength(500)]
        public string? Conditions { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Ship Ship { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }

    [Table("Documents")]
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocumentType { get; set; } = string.Empty; // ship, voyage, maintenance, incident, certificate

        [Required]
        public int EntityId { get; set; } // ID of the related entity (ship, voyage, etc.)

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string FileType { get; set; } = string.Empty; // pdf, doc, jpg, etc.

        [Required]
        [Range(0, long.MaxValue)]
        public long FileSize { get; set; } // in bytes

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Tags { get; set; } // comma-separated tags

        public bool IsConfidential { get; set; } = false;

        public int UploadedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User UploadedBy { get; set; } = null!;
    }

    [Table("Notifications")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty; // info, warning, error, reminder

        [MaxLength(50)]
        public string? RelatedEntityType { get; set; } // ship, voyage, maintenance, etc.

        public int? RelatedEntityId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        public bool IsEmailSent { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
}