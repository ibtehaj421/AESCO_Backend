using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // Maintenance DTOs
    public class CreateMaintenanceDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        [Required(ErrorMessage = "Maintenance type is required")]
        [StringLength(50, ErrorMessage = "Maintenance type cannot exceed 50 characters")]
        public string MaintenanceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Component is required")]
        [StringLength(100, ErrorMessage = "Component cannot exceed 100 characters")]
        public string Component { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Scheduled date is required")]
        public DateTime ScheduledDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Estimated cost must be 0 or greater")]
        public double? EstimatedCost { get; set; }

        [StringLength(100, ErrorMessage = "Performed by cannot exceed 100 characters")]
        public string? PerformedBy { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        [StringLength(50, ErrorMessage = "Priority cannot exceed 50 characters")]
        public string Priority { get; set; } = "medium";

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }
    }

    public class UpdateMaintenanceDto : CreateMaintenanceDto
    {
        [Required]
        public int Id { get; set; }

        public DateTime? CompletedDate { get; set; }

        [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters")]
        public string? Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Actual cost must be 0 or greater")]
        public double? ActualCost { get; set; }
    }

    public class MaintenanceDto
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public string MaintenanceType { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public double? EstimatedCost { get; set; }
        public double? ActualCost { get; set; }
        public string? PerformedBy { get; set; }
        public string? Location { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Inspection DTOs
    public class CreateInspectionDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        [Required(ErrorMessage = "Inspection type is required")]
        [StringLength(50, ErrorMessage = "Inspection type cannot exceed 50 characters")]
        public string InspectionType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Inspected by is required")]
        [StringLength(100, ErrorMessage = "Inspected by cannot exceed 100 characters")]
        public string InspectedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Inspection date is required")]
        public DateTime InspectionDate { get; set; }

        public DateTime? NextInspectionDate { get; set; }

        [Required(ErrorMessage = "Result is required")]
        [StringLength(30, ErrorMessage = "Result cannot exceed 30 characters")]
        public string Result { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Findings cannot exceed 1000 characters")]
        public string? Findings { get; set; }

        [StringLength(1000, ErrorMessage = "Recommendations cannot exceed 1000 characters")]
        public string? Recommendations { get; set; }

        [StringLength(50, ErrorMessage = "Certificate number cannot exceed 50 characters")]
        public string? CertificateNumber { get; set; }

        public DateTime? CertificateExpiry { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Created by user ID is required")]
        public int CreatedByUserId { get; set; }
    }

    public class InspectionDto
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public string InspectionType { get; set; } = string.Empty;
        public string InspectedBy { get; set; } = string.Empty;
        public DateTime InspectionDate { get; set; }
        public DateTime? NextInspectionDate { get; set; }
        public string Result { get; set; } = string.Empty;
        public string? Findings { get; set; }
        public string? Recommendations { get; set; }
        public string? CertificateNumber { get; set; }
        public DateTime? CertificateExpiry { get; set; }
        public string? Location { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class MaintenanceSearchDto
    {
        public string? SearchTerm { get; set; }
        public int? ShipId { get; set; }
        public string? MaintenanceType { get; set; }
        public string? Component { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? ScheduledDateFrom { get; set; }
        public DateTime? ScheduledDateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "ScheduledDate";
        public bool SortDescending { get; set; } = false;
    }
}