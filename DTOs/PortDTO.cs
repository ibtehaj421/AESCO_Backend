using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // Port DTOs
    public class CreatePortDto
    {
        [Required(ErrorMessage = "Port name is required")]
        [StringLength(100, ErrorMessage = "Port name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Port code is required")]
        [StringLength(10, ErrorMessage = "Port code cannot exceed 10 characters")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Region cannot exceed 50 characters")]
        public string? Region { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double? Longitude { get; set; }

        [StringLength(50, ErrorMessage = "Time zone cannot exceed 50 characters")]
        public string? TimeZone { get; set; }

        [StringLength(500, ErrorMessage = "Facilities cannot exceed 500 characters")]
        public string? Facilities { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Max draft must be 0 or greater")]
        public double? MaxDraft { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Max length must be 0 or greater")]
        public int? MaxLength { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }

    public class UpdatePortDto : CreatePortDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string? Status { get; set; }
    }

    public class PortDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Region { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? TimeZone { get; set; }
        public string? Facilities { get; set; }
        public double? MaxDraft { get; set; }
        public int? MaxLength { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Port Call DTOs
    public class CreatePortCallDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        [Required(ErrorMessage = "Port ID is required")]
        public int PortId { get; set; }

        public int? VoyageId { get; set; }

        [Required(ErrorMessage = "Purpose is required")]
        [StringLength(50, ErrorMessage = "Purpose cannot exceed 50 characters")]
        public string Purpose { get; set; } = string.Empty;

        [Required(ErrorMessage = "Planned arrival is required")]
        public DateTime PlannedArrival { get; set; }

        [Required(ErrorMessage = "Planned departure is required")]
        public DateTime PlannedDeparture { get; set; }

        [StringLength(50, ErrorMessage = "Berth number cannot exceed 50 characters")]
        public string? BerthNumber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cargo loaded must be 0 or greater")]
        public double? CargoLoaded { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cargo unloaded must be 0 or greater")]
        public double? CargoUnloaded { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Fuel taken must be 0 or greater")]
        public double? FuelTaken { get; set; }

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Created by user ID is required")]
        public int CreatedByUserId { get; set; }    
    }

    public class UpdatePortCallDto : CreatePortCallDto
    {
        [Required]
        public int Id { get; set; }

        public DateTime? ActualArrival { get; set; }
        public DateTime? ActualDeparture { get; set; }

        [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters")]
        public string? Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Port charges must be 0 or greater")]
        public double? PortCharges { get; set; }
    }

    public class PortCallDto
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public int PortId { get; set; }
        public string PortName { get; set; } = string.Empty;
        public string PortCode { get; set; } = string.Empty;
        public int? VoyageId { get; set; }
        public string? VoyageNumber { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public DateTime PlannedArrival { get; set; }
        public DateTime PlannedDeparture { get; set; }
        public DateTime? ActualArrival { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? BerthNumber { get; set; }
        public double? CargoLoaded { get; set; }
        public double? CargoUnloaded { get; set; }
        public double? FuelTaken { get; set; }
        public double? PortCharges { get; set; }
        public string? Notes { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}