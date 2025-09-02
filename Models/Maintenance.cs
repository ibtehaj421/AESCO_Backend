using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("MaintenanceRecords")]
    public class MaintenanceRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MaintenanceType { get; set; } = string.Empty; // routine, emergency, repair, upgrade

        [Required]
        [MaxLength(100)]
        public string Component { get; set; } = string.Empty; // engine, hull, navigation, etc.

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "scheduled"; // scheduled, in_progress, completed, cancelled

        [Range(0, double.MaxValue)]
        public double? EstimatedCost { get; set; }

        [Range(0, double.MaxValue)]
        public double? ActualCost { get; set; }

        [MaxLength(100)]
        public string? PerformedBy { get; set; } // contractor/company name

        [MaxLength(100)]
        public string? Location { get; set; } // port/shipyard where maintenance performed

        [MaxLength(50)]
        public string Priority { get; set; } = "medium"; // low, medium, high, critical

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Ship Ship { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }

    [Table("Inspections")]
    public class Inspection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        [MaxLength(50)]
        public string InspectionType { get; set; } = string.Empty; // safety, security, environmental, classification

        [Required]
        [MaxLength(100)]
        public string InspectedBy { get; set; } = string.Empty; // authority/organization

        [Required]
        public DateTime InspectionDate { get; set; }

        public DateTime? NextInspectionDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string Result { get; set; } = string.Empty; // passed, failed, conditional, pending

        [MaxLength(1000)]
        public string? Findings { get; set; }

        [MaxLength(1000)]
        public string? Recommendations { get; set; }

        [MaxLength(50)]
        public string? CertificateNumber { get; set; }

        public DateTime? CertificateExpiry { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Ship Ship { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }
}