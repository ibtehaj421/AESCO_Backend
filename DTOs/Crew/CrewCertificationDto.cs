using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs.Crew
{
    public class CrewCertificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string CertificationType { get; set; } = string.Empty;
        public string CertificateNumber { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;
        public bool IsExpiringSoon => ExpiryDate.HasValue && (ExpiryDate.Value - DateTime.UtcNow).TotalDays <= 30;
        public int DaysUntilExpiry => ExpiryDate.HasValue ? (int)(ExpiryDate.Value - DateTime.UtcNow).TotalDays : int.MaxValue;
    }

    public class CreateCrewCertificationDto
    {
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CertificationType { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string CertificateNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string IssuedBy { get; set; } = string.Empty;
        
        [Required]
        public DateTime IssuedDate { get; set; }
        
        public DateTime ExpiryDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdateCrewCertificationDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CertificationType { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string CertificateNumber { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string IssuedBy { get; set; } = string.Empty;
        
        [Required]
        public DateTime IssuedDate { get; set; }
        
        public DateTime ExpiryDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
