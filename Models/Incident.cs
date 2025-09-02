using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("Incidents")]
    public class Incident
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ShipId { get; set; }

        public int? VoyageId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string IncidentType { get; set; } = string.Empty; // accident, near_miss, equipment_failure, safety, security

        [Required]
        [MaxLength(50)]
        public string Severity { get; set; } = string.Empty; // low, medium, high, critical

        [Required]
        public DateTime IncidentDateTime { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [Range(-90, 90)]
        public double? Latitude { get; set; }

        [Range(-180, 180)]
        public double? Longitude { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? ImmediateActions { get; set; }

        [MaxLength(1000)]
        public string? RootCause { get; set; }

        [MaxLength(1000)]
        public string? PreventiveMeasures { get; set; }

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "open"; // open, investigating, resolved, closed

        public bool AuthoritiesNotified { get; set; } = false;

        [MaxLength(200)]
        public string? AuthoritiesNotifiedDetails { get; set; }

        public int ReportedByUserId { get; set; }
        public int? InvestigatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // Navigation properties
        public virtual Ship Ship { get; set; } = null!;
        public virtual Voyage? Voyage { get; set; }
        public virtual User ReportedBy { get; set; } = null!;
        public virtual User? InvestigatedBy { get; set; }
    }
}