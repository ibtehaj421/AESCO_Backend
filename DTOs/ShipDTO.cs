using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // Base DTO for common ship properties
    public abstract class BaseShipDto
    {
        [Required(ErrorMessage = "Ship name is required")]
        [StringLength(100, ErrorMessage = "Ship name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "IMO number is required")]
        [StringLength(20, ErrorMessage = "IMO number cannot exceed 20 characters")]
        public string IMONumber { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Call sign cannot exceed 20 characters")]
        public string? CallSign { get; set; }

        [Required(ErrorMessage = "Registration number is required")]
        [StringLength(20, ErrorMessage = "Registration number cannot exceed 20 characters")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ship type is required")]
        [StringLength(50, ErrorMessage = "Ship type cannot exceed 50 characters")]
        public string ShipType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Flag is required")]
        [StringLength(50, ErrorMessage = "Flag cannot exceed 50 characters")]
        public string Flag { get; set; } = string.Empty;

        [Required(ErrorMessage = "Builder is required")]
        [StringLength(100, ErrorMessage = "Builder cannot exceed 100 characters")]
        public string Builder { get; set; } = string.Empty;

        [Required(ErrorMessage = "Build date is required")]
        public DateTime BuildDate { get; set; }

        [Required(ErrorMessage = "Launch date is required")]
        public DateTime LaunchDate { get; set; }

        [Required(ErrorMessage = "Length overall is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Length overall must be greater than 0")]
        public double LengthOverall { get; set; }

        [Required(ErrorMessage = "Beam is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Beam must be greater than 0")]
        public double Beam { get; set; }

        [Required(ErrorMessage = "Draft is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Draft must be greater than 0")]
        public double Draft { get; set; }

        [Required(ErrorMessage = "Gross tonnage is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Gross tonnage must be greater than 0")]
        public double GrossTonnage { get; set; }

        [Required(ErrorMessage = "Net tonnage is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Net tonnage must be greater than 0")]
        public double NetTonnage { get; set; }

        [Required(ErrorMessage = "Deadweight tonnage is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Deadweight tonnage must be greater than 0")]
        public double DeadweightTonnage { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Passenger capacity must be 0 or greater")]
        public int? PassengerCapacity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Crew capacity must be 0 or greater")]
        public int? CrewCapacity { get; set; }

        [Required(ErrorMessage = "Max speed is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Max speed must be greater than 0")]
        public double MaxSpeed { get; set; }

        [Required(ErrorMessage = "Service speed is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Service speed must be greater than 0")]
        public double ServiceSpeed { get; set; }

        [Required(ErrorMessage = "Engine type is required")]
        [StringLength(50, ErrorMessage = "Engine type cannot exceed 50 characters")]
        public string EngineType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Engine power is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Engine power must be greater than 0")]
        public double EnginePower { get; set; }

        [StringLength(100, ErrorMessage = "Home port cannot exceed 100 characters")]
        public string? HomePort { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    // DTO for creating a new ship
    public class CreateShipDto : BaseShipDto
    {
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = "active";
    }

    // DTO for updating ship information
    public class UpdateShipDto : BaseShipDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string? Status { get; set; }
    }

    // DTO for returning ship information
    public class ShipDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IMONumber { get; set; } = string.Empty;
        public string? CallSign { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string ShipType { get; set; } = string.Empty;
        public string Flag { get; set; } = string.Empty;
        public string Builder { get; set; } = string.Empty;
        public DateTime BuildDate { get; set; }
        public DateTime LaunchDate { get; set; }
        public double LengthOverall { get; set; }
        public double Beam { get; set; }
        public double Draft { get; set; }
        public double GrossTonnage { get; set; }
        public double NetTonnage { get; set; }
        public double DeadweightTonnage { get; set; }
        public int? PassengerCapacity { get; set; }
        public int? CrewCapacity { get; set; }
        public double MaxSpeed { get; set; }
        public double ServiceSpeed { get; set; }
        public string EngineType { get; set; } = string.Empty;
        public double EnginePower { get; set; }
        public string? HomePort { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Current assignments and voyage info
        public List<ShipAssignmentDto> CurrentAssignments { get; set; } = new List<ShipAssignmentDto>();
        public VoyageDto? CurrentVoyage { get; set; }
    }

    // DTO for ship summary/listing
    public class ShipSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IMONumber { get; set; } = string.Empty;
        public string ShipType { get; set; } = string.Empty;
        public string Flag { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? HomePort { get; set; }
        public double GrossTonnage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastVoyageDate { get; set; }
        public string? CurrentLocation { get; set; }
        public List<string> AssignedCrew { get; set; } = new List<string>();
    }

    // Ship Assignment DTOs
    public class CreateShipAssignmentDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Assignment date is required")]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
        public string? Notes { get; set; }
    }

    public class UpdateShipAssignmentDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters")]
        public string? Position { get; set; }

        public DateTime? UnassignedAt { get; set; }

        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string? Status { get; set; }

        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
        public string? Notes { get; set; }
    }

    public class ShipAssignmentDto
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
        public DateTime? UnassignedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AssignedByName { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    // Search/Filter DTOs
    public class ShipSearchDto
    {
        public string? SearchTerm { get; set; }
        public string? ShipType { get; set; }
        public string? Flag { get; set; }
        public string? Status { get; set; }
        public string? HomePort { get; set; }
        public DateTime? BuildDateFrom { get; set; }
        public DateTime? BuildDateTo { get; set; }
        public double? MinTonnage { get; set; }
        public double? MaxTonnage { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "Name";
        public bool SortDescending { get; set; } = false;
    }

    // Bulk operation DTOs
    public class BulkAssignCrewDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        [Required(ErrorMessage = "Assignments are required")]
        [MinLength(1, ErrorMessage = "At least one assignment is required")]
        public List<CrewAssignmentItemDto> Assignments { get; set; } = new List<CrewAssignmentItemDto>();
    }

    public class CrewAssignmentItemDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters")]
        public string Position { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
        public string? Notes { get; set; }
    }
}