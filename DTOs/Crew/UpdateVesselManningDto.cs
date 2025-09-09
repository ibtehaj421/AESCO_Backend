using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs.Crew
{
    public class UpdateVesselManningDto
    {
        [Required]
        public int VesselId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Rank { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue)]
        public int RequiredCount { get; set; } = 0;
        
        [Range(0, int.MaxValue)]
        public int CurrentCount { get; set; } = 0;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
