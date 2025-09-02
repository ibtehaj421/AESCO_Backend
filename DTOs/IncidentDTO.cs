using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // Incident DTOs
    public class CreateIncidentDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        public int? VoyageId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Incident type is required")]
        [StringLength(50, ErrorMessage = "Incident type cannot exceed 50 characters")]
        public string IncidentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Severity is required")]
        [StringLength(50, ErrorMessage = "Severity cannot exceed 50 characters")]
        public string Severity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Incident date and time is required")]
        public DateTime IncidentDateTime { get; set; }

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string? Location { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double? Longitude { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Immediate actions cannot exceed 1000 characters")]
        public string? ImmediateActions { get; set; }

        public bool AuthoritiesNotified { get; set; } = false;

        [StringLength(200, ErrorMessage = "Authorities notified details cannot exceed 200 characters")]
        public string? AuthoritiesNotifiedDetails { get; set; }
    }

    public class UpdateIncidentDto : CreateIncidentDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(1000, ErrorMessage = "Root cause cannot exceed 1000 characters")]
        public string? RootCause { get; set; }

        [StringLength(1000, ErrorMessage = "Preventive measures cannot exceed 1000 characters")]
        public string? PreventiveMeasures { get; set; }

        [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters")]
        public string? Status { get; set; }

        public int? InvestigatedByUserId { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class IncidentDto
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public int? VoyageId { get; set; }
        public string? VoyageNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string IncidentType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public DateTime IncidentDateTime { get; set; }
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ImmediateActions { get; set; }
        public string? RootCause { get; set; }
        public string? PreventiveMeasures { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool AuthoritiesNotified { get; set; }
        public string? AuthoritiesNotifiedDetails { get; set; }
        public string ReportedByName { get; set; } = string.Empty;
        public string? InvestigatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class IncidentSearchDto
    {
        public string? SearchTerm { get; set; }
        public int? ShipId { get; set; }
        public string? IncidentType { get; set; }
        public string? Severity { get; set; }
        public string? Status { get; set; }
        public DateTime? IncidentDateFrom { get; set; }
        public DateTime? IncidentDateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "IncidentDateTime";
        public bool SortDescending { get; set; } = true;
    }
}