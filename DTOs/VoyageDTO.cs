using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // Voyage DTOs
    public class CreateVoyageDto
    {
        [Required(ErrorMessage = "Ship ID is required")]
        public int ShipId { get; set; }

        [Required(ErrorMessage = "Voyage number is required")]
        [StringLength(10, ErrorMessage = "Voyage number cannot exceed 10 characters")]
        public string VoyageNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departure port is required")]
        [StringLength(100, ErrorMessage = "Departure port cannot exceed 100 characters")]
        public string DeparturePort { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arrival port is required")]
        [StringLength(100, ErrorMessage = "Arrival port cannot exceed 100 characters")]
        public string ArrivalPort { get; set; } = string.Empty;

        [Required(ErrorMessage = "Planned departure is required")]
        public DateTime PlannedDeparture { get; set; }

        [Required(ErrorMessage = "Planned arrival is required")]
        public DateTime PlannedArrival { get; set; }

        [StringLength(50, ErrorMessage = "Cargo type cannot exceed 50 characters")]
        public string? CargoType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cargo weight must be 0 or greater")]
        public double? CargoWeight { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Distance must be 0 or greater")]
        public double? Distance { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }

    public class UpdateVoyageDto : CreateVoyageDto
    {
        [Required]
        public int Id { get; set; }

        public DateTime? ActualDeparture { get; set; }
        public DateTime? ActualArrival { get; set; }

        [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters")]
        public string? Status { get; set; }
    }

    public class VoyageDto
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public string VoyageNumber { get; set; } = string.Empty;
        public string DeparturePort { get; set; } = string.Empty;
        public string ArrivalPort { get; set; } = string.Empty;
        public DateTime PlannedDeparture { get; set; }
        public DateTime PlannedArrival { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public DateTime? ActualArrival { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? CargoType { get; set; }
        public double? CargoWeight { get; set; }
        public double? Distance { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<VoyageLogDto> Logs { get; set; } = new List<VoyageLogDto>();
    }

    // Voyage Log DTOs
    public class CreateVoyageLogDto
    {
        [Required(ErrorMessage = "Voyage ID is required")]
        public int VoyageId { get; set; }

        [Required(ErrorMessage = "Log time is required")]
        public DateTime LogTime { get; set; }

        [Required(ErrorMessage = "Log type is required")]
        [StringLength(50, ErrorMessage = "Log type cannot exceed 50 characters")]
        public string LogType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Log entry is required")]
        [StringLength(1000, ErrorMessage = "Log entry cannot exceed 1000 characters")]
        public string LogEntry { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double? Longitude { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Speed must be 0 or greater")]
        public double? Speed { get; set; }

        [Range(0, 360, ErrorMessage = "Course must be between 0 and 360")]
        public double? Course { get; set; }

        [StringLength(50, ErrorMessage = "Weather conditions cannot exceed 50 characters")]
        public string? WeatherConditions { get; set; }

        [Range(0, 200, ErrorMessage = "Wind speed must be between 0 and 200")]
        public double? WindSpeed { get; set; }

        [Range(0, 360, ErrorMessage = "Wind direction must be between 0 and 360")]
        public double? WindDirection { get; set; }

        [Range(0, 10, ErrorMessage = "Sea state must be between 0 and 10")]
        public double? SeaState { get; set; }

        [Required(ErrorMessage = "Logged by user ID is required")]
        public int LoggedByUserId { get; set; }
    }

    public class VoyageLogDto
    {
        public int Id { get; set; }
        public int VoyageId { get; set; }
        public DateTime LogTime { get; set; }
        public string LogType { get; set; } = string.Empty;
        public string LogEntry { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Speed { get; set; }
        public double? Course { get; set; }
        public string? WeatherConditions { get; set; }
        public double? WindSpeed { get; set; }
        public double? WindDirection { get; set; }
        public double? SeaState { get; set; }
        public string LoggedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // Search/Filter DTOs
    public class VoyageSearchDto
    {
        public string? SearchTerm { get; set; }
        public int? ShipId { get; set; }
        public string? DeparturePort { get; set; }
        public string? ArrivalPort { get; set; }
        public string? Status { get; set; }
        public DateTime? DepartureDateFrom { get; set; }
        public DateTime? DepartureDateTo { get; set; }
        public string? CargoType { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "PlannedDeparture";
        public bool SortDescending { get; set; } = true;
    }
}