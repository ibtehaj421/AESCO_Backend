using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // Certificate DTOs
    public class CreateCertificateDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        [Required(ErrorMessage = "Certificate type is required")]
        [StringLength(100, ErrorMessage = "Certificate type cannot exceed 100 characters")]
        public string CertificateType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Certificate number is required")]
        [StringLength(50, ErrorMessage = "Certificate number cannot exceed 50 characters")]
        public string CertificateNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Issued by is required")]
        [StringLength(100, ErrorMessage = "Issued by cannot exceed 100 characters")]
        public string IssuedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Issue date is required")]
        public DateTime IssuedDate { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        public DateTime ExpiryDate { get; set; }

        [StringLength(100, ErrorMessage = "Issued at cannot exceed 100 characters")]
        public string? IssuedAt { get; set; }

        [StringLength(500, ErrorMessage = "Conditions cannot exceed 500 characters")]
        public string? Conditions { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }

    public class UpdateCertificateDto : CreateCertificateDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters")]
        public string? Status { get; set; }
    }

    public class CertificateDto
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public string CertificateType { get; set; } = string.Empty;
        public string CertificateNumber { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? IssuedAt { get; set; }
        public string? Conditions { get; set; }
        public string? Notes { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsExpiringSoon { get; set; } // calculated property
        public int DaysUntilExpiry { get; set; } // calculated property
    }

    // Document DTOs
    public class CreateDocumentDto
    {
        [Required(ErrorMessage = "Document type is required")]
        [StringLength(50, ErrorMessage = "Document type cannot exceed 50 characters")]
        public string DocumentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Entity ID is required")]
        public int EntityId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Tags cannot exceed 100 characters")]
        public string? Tags { get; set; }

        public bool IsConfidential { get; set; } = false;

        [Required(ErrorMessage = "Created by user ID is required")]
        public int CreatedByUserId { get; set; }
    }

    public class DocumentDto
    {
        public int Id { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public bool IsConfidential { get; set; }
        public string UploadedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string FileSizeFormatted { get; set; } = string.Empty; // calculated property
    }

    // Notification DTOs
    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        public string Type { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Related entity type cannot exceed 50 characters")]
        public string? RelatedEntityType { get; set; }

        public int? RelatedEntityId { get; set; }
    }

    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsEmailSent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TimeAgo { get; set; } = string.Empty; // calculated property
    }
}