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

        // Granular time breakdown
        [Required]
        [Range(2000, 3000)]
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(1, 31)]
        public int Day { get; set; }

        [Required]
        [Range(0, 23)]
        public int Hour { get; set; }

        [Required]
        public bool IsWorking { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

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