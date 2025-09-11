using ASCO.Repositories;
using ASCO.DTOs;

namespace ASCO.Services
{
    public class DashboardService
    {
        private readonly DashboardRepository _dashboardRepository;

        public DashboardService(DashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        // ==================== OVERVIEW STATISTICS ====================
        public async Task<DashboardOverviewStatsDto> GetOverviewStatsAsync()
        {
            return await _dashboardRepository.GetOverviewStatsAsync();
        }

        // ==================== EXPIRY STATISTICS ====================
        public async Task<DashboardExpiryStatsDto> GetExpiryStatsAsync()
        {
            return await _dashboardRepository.GetExpiryStatsAsync();
        }

        // ==================== VESSEL STATISTICS ====================
        public async Task<DashboardVesselStatsDto> GetVesselStatsAsync()
        {
            return await _dashboardRepository.GetVesselStatsAsync();
        }

        // ==================== TRAINING STATISTICS ====================
        public async Task<DashboardTrainingStatsDto> GetTrainingStatsAsync()
        {
            return await _dashboardRepository.GetTrainingStatsAsync();
        }

        // ==================== EVALUATION STATISTICS ====================
        public async Task<DashboardEvaluationStatsDto> GetEvaluationStatsAsync()
        {
            return await _dashboardRepository.GetEvaluationStatsAsync();
        }

        // ==================== CREW EXPIRY DATA ====================
        public async Task<CrewExpiryDataDto> GetCrewExpiryDataAsync(CrewExpiryFiltersDto filters)
        {
            return await _dashboardRepository.GetCrewExpiryDataAsync(filters);
        }

        // ==================== RECENT ACTIVITIES ====================
        public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int limit = 10)
        {
            return await _dashboardRepository.GetRecentActivitiesAsync(limit);
        }

        // ==================== CREW DISTRIBUTION ====================
        public async Task<List<CrewDistributionDto>> GetCrewDistributionByRankAsync()
        {
            return await _dashboardRepository.GetCrewDistributionByRankAsync();
        }

        public async Task<List<CrewDistributionDto>> GetCrewDistributionByNationalityAsync()
        {
            return await _dashboardRepository.GetCrewDistributionByNationalityAsync();
        }

        public async Task<List<CrewDistributionDto>> GetCrewDistributionByVesselAsync()
        {
            return await _dashboardRepository.GetCrewDistributionByVesselAsync();
        }

        // ==================== EXPIRY TRENDS ====================
        public async Task<List<ExpiryTrendDto>> GetExpiryTrendsAsync(int months = 12)
        {
            return await _dashboardRepository.GetExpiryTrendsAsync(months);
        }

        // ==================== CRITICAL ALERTS ====================
        public async Task<List<CriticalAlertDto>> GetCriticalAlertsAsync()
        {
            return await _dashboardRepository.GetCriticalAlertsAsync();
        }

        // ==================== UPCOMING EXPIRIES ====================
        public async Task<List<UpcomingExpiryDto>> GetUpcomingExpiriesAsync(int days = 30)
        {
            return await _dashboardRepository.GetUpcomingExpiriesAsync(days);
        }
    }
}
