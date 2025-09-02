using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("Ships")]
    public class Ship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string IMONumber { get; set; } = string.Empty; // International Maritime Organization number

        [MaxLength(20)]
        public string? CallSign { get; set; }

        [Required]
        [MaxLength(20)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ShipType { get; set; } = string.Empty; // cargo, passenger, tanker, etc.

        [Required]
        [MaxLength(50)]
        public string Flag { get; set; } = string.Empty; // flag state/country

        [Required]
        [MaxLength(100)]
        public string Builder { get; set; } = string.Empty; // shipyard/manufacturer

        [Required]
        public DateTime BuildDate { get; set; }

        [Required]
        public DateTime LaunchDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double LengthOverall { get; set; } // in meters

        [Required]
        [Range(0, double.MaxValue)]
        public double Beam { get; set; } // width in meters

        [Required]
        [Range(0, double.MaxValue)]
        public double Draft { get; set; } // depth in meters

        [Required]
        [Range(0, double.MaxValue)]
        public double GrossTonnage { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double NetTonnage { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double DeadweightTonnage { get; set; }

        [Range(0, int.MaxValue)]
        public int? PassengerCapacity { get; set; }

        [Range(0, int.MaxValue)]
        public int? CrewCapacity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double MaxSpeed { get; set; } // in knots

        [Required]
        [Range(0, double.MaxValue)]
        public double ServiceSpeed { get; set; } // in knots

        [Required]
        [MaxLength(50)]
        public string EngineType { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double EnginePower { get; set; } // in kW

        [MaxLength(100)]
        public string? HomePort { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty; // active, inactive, maintenance, decommissioned

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<ShipAssignment> ShipAssignments { get; set; } = new List<ShipAssignment>();
        public virtual ICollection<Voyage> Voyages { get; set; } = new List<Voyage>();
        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
        public virtual ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    }

    [Table("ShipAssignments")]
    public class ShipAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Position { get; set; } = string.Empty; // Captain, Engineer, Navigator, etc.

        [Required]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UnassignedAt { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "active"; // active, completed, transferred

        public int AssignedByUserId { get; set; }

        [MaxLength(200)]
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Ship Ship { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual User AssignedBy { get; set; } = null!;
    }

    [Table("CrewCertifications")]
    public class CrewCertification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CertificationType { get; set; } = string.Empty; // STCW, medical, radio operator, etc.

        [Required]
        [MaxLength(50)]
        public string CertificateNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string IssuedBy { get; set; } = string.Empty;

        [Required]
        public DateTime IssuedDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "valid"; // valid, expired, suspended, revoked

        [MaxLength(100)]
        public string? IssuedAt { get; set; }

        [MaxLength(500)]
        public string? Limitations { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
}