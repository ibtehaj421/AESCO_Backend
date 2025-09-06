using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("CrewWorkRestHours")]
    public class CrewWorkRestHours
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public int VesselId { get; set; }

        [ForeignKey(nameof(VesselId))]
        public virtual Ship Vessel { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 24, ErrorMessage = "Work hours must be between 0 and 24")]
        public decimal WorkHours { get; set; }

        [Required]
        [Range(0, 24, ErrorMessage = "Rest hours must be between 0 and 24")]
        public decimal RestHours { get; set; }

        [Required]
        [Range(0, 24, ErrorMessage = "Total hours must be between 0 and 24")]
        public decimal TotalHours { get; set; }

        [StringLength(200)]
        public string? WorkDescription { get; set; }

        [StringLength(200)]
        public string? RestDescription { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User CreatedBy { get; set; } = null!;
        public int CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User CreatedByUser { get; set; } = null!;
    }
}