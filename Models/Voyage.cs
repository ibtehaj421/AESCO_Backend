using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("Voyages")]
    public class Voyage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        [MaxLength(10)]
        public string VoyageNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string DeparturePort { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ArrivalPort { get; set; } = string.Empty;

        [Required]
        public DateTime PlannedDeparture { get; set; }

        [Required]
        public DateTime PlannedArrival { get; set; }

        public DateTime? ActualDeparture { get; set; }

        public DateTime? ActualArrival { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "planned"; // planned, in_progress, completed, cancelled, delayed

        [MaxLength(50)]
        public string? CargoType { get; set; }

        [Range(0, double.MaxValue)]
        public double? CargoWeight { get; set; } // in tons

        [Range(0, double.MaxValue)]
        public double? Distance { get; set; } // in nautical miles

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Ship Ship { get; set; } = null!;
        public virtual ICollection<VoyageLog> VoyageLogs { get; set; } = new List<VoyageLog>();
    }

    [Table("VoyageLogs")]
    public class VoyageLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VoyageId { get; set; }

        [Required]
        public DateTime LogTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string LogType { get; set; } = string.Empty; // position, weather, incident, maintenance, etc.

        [Required]
        [MaxLength(1000)]
        public string LogEntry { get; set; } = string.Empty;

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }

        [Range(0, double.MaxValue)]
        public double? Speed { get; set; } // in knots

        [Range(0, 360)]
        public double? Course { get; set; } // in degrees

        [MaxLength(50)]
        public string? WeatherConditions { get; set; }

        [Range(0, 200)]
        public double? WindSpeed { get; set; } // in knots

        [Range(0, 360)]
        public double? WindDirection { get; set; } // in degrees

        [Range(0, 10)]
        public double? SeaState { get; set; } // Beaufort scale

        public int LoggedByUserId { get; set; }

        // Navigation properties
        public virtual Voyage Voyage { get; set; } = null!;
        public virtual User LoggedBy { get; set; } = null!;
    }
}