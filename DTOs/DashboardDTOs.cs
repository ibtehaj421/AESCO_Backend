using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // ==================== DASHBOARD OVERVIEW STATS ====================
    public class DashboardOverviewStatsDto
    {
        public int TotalCrewMembers { get; set; }
        public int ActiveCrewMembers { get; set; }
        public int OnLeaveCrewMembers { get; set; }
        public int TotalVessels { get; set; }
        public int ActiveVessels { get; set; }
        public int TotalCertifications { get; set; }
        public int ExpiringCertifications { get; set; }
        public int TotalTrainings { get; set; }
        public int PendingTrainings { get; set; }
        public int TotalEvaluations { get; set; }
        public int PendingEvaluations { get; set; }
    }

    // ==================== EXPIRY STATISTICS ====================
    public class DashboardExpiryStatsDto
    {
        public int TotalExpiring { get; set; }
        public int ExpiringThisWeek { get; set; }
        public int ExpiringThisMonth { get; set; }
        public int ExpiringNextMonth { get; set; }
        public int AlreadyExpired { get; set; }
        public double ExpiryRate { get; set; }
    }

    // ==================== VESSEL STATISTICS ====================
    public class DashboardVesselStatsDto
    {
        public int TotalVessels { get; set; }
        public int ActiveVessels { get; set; }
        public int InactiveVessels { get; set; }
        public int VesselsInPort { get; set; }
        public int VesselsAtSea { get; set; }
        public double AverageCrewPerVessel { get; set; }
    }

    // ==================== TRAINING STATISTICS ====================
    public class DashboardTrainingStatsDto
    {
        public int TotalTrainings { get; set; }
        public int CompletedTrainings { get; set; }
        public int PendingTrainings { get; set; }
        public int OverdueTrainings { get; set; }
        public int ScheduledTrainings { get; set; }
        public double CompletionRate { get; set; }
    }

    // ==================== EVALUATION STATISTICS ====================
    public class DashboardEvaluationStatsDto
    {
        public int TotalEvaluations { get; set; }
        public int CompletedEvaluations { get; set; }
        public int PendingEvaluations { get; set; }
        public int OverdueEvaluations { get; set; }
        public int ScheduledEvaluations { get; set; }
        public double CompletionRate { get; set; }
    }

    // ==================== CREW EXPIRY DATA ====================
    public class CrewExpiryItemDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string VesselName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentName { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
    }

    public class CrewExpiryDataDto
    {
        public List<CrewExpiryItemDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    // ==================== RECENT ACTIVITIES ====================
    public class RecentActivityDto
    {
        public int Id { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string VesselName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // ==================== CREW DISTRIBUTION ====================
    public class CrewDistributionDto
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    // ==================== EXPIRY TRENDS ====================
    public class ExpiryTrendDto
    {
        public string Month { get; set; } = string.Empty;
        public int ExpiringCount { get; set; }
        public int ExpiredCount { get; set; }
        public int RenewedCount { get; set; }
    }

    // ==================== CRITICAL ALERTS ====================
    public class CriticalAlertDto
    {
        public int Id { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string RelatedEntity { get; set; } = string.Empty;
        public int RelatedEntityId { get; set; }
    }

    // ==================== UPCOMING EXPIRIES ====================
    public class UpcomingExpiryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string VesselName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentName { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string Priority { get; set; } = string.Empty;
    }

    // ==================== FILTER DTOs ====================
    public class CrewExpiryFiltersDto
    {
        public string? SearchTerm { get; set; }
        public string? VesselId { get; set; }
        public string? Rank { get; set; }
        public string? DocumentType { get; set; }
        public string? Status { get; set; }
        public int? DaysUntilExpiry { get; set; }
        public DateTime? ExpiryDateFrom { get; set; }
        public DateTime? ExpiryDateTo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
