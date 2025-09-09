//choose a specific vessel manning for a specific vessel.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASCO.Models
{
    [Table("VesselMannings")]
    public class VesselManning
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VesselId { get; set; } // Foreign key to Vessel

        [Required]
        [MaxLength(100)]
        public string Rank { get; set; } = string.Empty;

        public int RequiredCount { get; set; } = 0;
        
        public int CurrentCount { get; set; } = 0;
        
        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(VesselId))]
        public virtual Ship Vessel { get; set; } = null!;
    }
}