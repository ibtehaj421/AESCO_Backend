using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("Ports")]
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty; // UNLOCODE

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Region { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }

        [MaxLength(50)]
        public string? TimeZone { get; set; }

        [MaxLength(500)]
        public string? Facilities { get; set; }

        [Range(0, double.MaxValue)]
        public double? MaxDraft { get; set; } // maximum allowed draft in meters

        [Range(0, int.MaxValue)]
        public int? MaxLength { get; set; } // maximum ship length in meters

        [MaxLength(50)]
        public string Status { get; set; } = "active"; // active, inactive, restricted

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<PortCall> PortCalls { get; set; } = new List<PortCall>();
    }

    [Table("PortCalls")]
    public class PortCall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        public int PortId { get; set; }

        public int? VoyageId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Purpose { get; set; } = string.Empty; // loading, unloading, refueling, maintenance, etc.

        [Required]
        public DateTime PlannedArrival { get; set; }

        [Required]
        public DateTime PlannedDeparture { get; set; }

        public DateTime? ActualArrival { get; set; }

        public DateTime? ActualDeparture { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "scheduled"; // scheduled, arrived, in_port, departed, cancelled

        [MaxLength(50)]
        public string? BerthNumber { get; set; }

        [Range(0, double.MaxValue)]
        public double? CargoLoaded { get; set; } // in tons

        [Range(0, double.MaxValue)]
        public double? CargoUnloaded { get; set; } // in tons

        [Range(0, double.MaxValue)]
        public double? FuelTaken { get; set; } // in tons

        [Range(0, double.MaxValue)]
        public double? PortCharges { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Ship Ship { get; set; } = null!;
        public virtual Port Port { get; set; } = null!;
        public virtual Voyage? Voyage { get; set; }
        public virtual User CreatedBy { get; set; } = null!;
    }
}