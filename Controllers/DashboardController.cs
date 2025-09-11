using Microsoft.AspNetCore.Mvc;
using ASCO.Services;
using ASCO.DTOs;

namespace ASCO.Controllers
{
    [ApiController]
    [Route("crew/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // ==================== OVERVIEW STATISTICS ====================
        [HttpGet("overview")]
        public async Task<IActionResult> GetOverviewStats()
        {
            try
            {
                var stats = await _dashboardService.GetOverviewStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving overview statistics", error = ex.Message });
            }
        }

        // ==================== EXPIRY STATISTICS ====================
        [HttpGet("expiry-stats")]
        public async Task<IActionResult> GetExpiryStats()
        {
            try
            {
                var stats = await _dashboardService.GetExpiryStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving expiry statistics", error = ex.Message });
            }
        }

        // ==================== VESSEL STATISTICS ====================
        [HttpGet("vessel-stats")]
        public async Task<IActionResult> GetVesselStats()
        {
            try
            {
                var stats = await _dashboardService.GetVesselStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving vessel statistics", error = ex.Message });
            }
        }

        // ==================== TRAINING STATISTICS ====================
        [HttpGet("training-stats")]
        public async Task<IActionResult> GetTrainingStats()
        {
            try
            {
                var stats = await _dashboardService.GetTrainingStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving training statistics", error = ex.Message });
            }
        }

        // ==================== EVALUATION STATISTICS ====================
        [HttpGet("evaluation-stats")]
        public async Task<IActionResult> GetEvaluationStats()
        {
            try
            {
                var stats = await _dashboardService.GetEvaluationStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving evaluation statistics", error = ex.Message });
            }
        }

        // ==================== CREW EXPIRY DATA ====================
        [HttpPost("expiry-data")]
        public async Task<IActionResult> GetCrewExpiryData([FromBody] CrewExpiryFiltersDto filters)
        {
            try
            {
                if (filters == null)
                {
                    filters = new CrewExpiryFiltersDto();
                }

                var data = await _dashboardService.GetCrewExpiryDataAsync(filters);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving crew expiry data", error = ex.Message });
            }
        }

        // ==================== RECENT ACTIVITIES ====================
        [HttpGet("recent-activities")]
        public async Task<IActionResult> GetRecentActivities([FromQuery] int limit = 10)
        {
            try
            {
                var activities = await _dashboardService.GetRecentActivitiesAsync(limit);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving recent activities", error = ex.Message });
            }
        }

        // ==================== CREW DISTRIBUTION ====================
        [HttpGet("distribution/rank")]
        public async Task<IActionResult> GetCrewDistributionByRank()
        {
            try
            {
                var distribution = await _dashboardService.GetCrewDistributionByRankAsync();
                return Ok(distribution);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving crew distribution by rank", error = ex.Message });
            }
        }

        [HttpGet("distribution/nationality")]
        public async Task<IActionResult> GetCrewDistributionByNationality()
        {
            try
            {
                var distribution = await _dashboardService.GetCrewDistributionByNationalityAsync();
                return Ok(distribution);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving crew distribution by nationality", error = ex.Message });
            }
        }

        [HttpGet("distribution/vessel")]
        public async Task<IActionResult> GetCrewDistributionByVessel()
        {
            try
            {
                var distribution = await _dashboardService.GetCrewDistributionByVesselAsync();
                return Ok(distribution);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving crew distribution by vessel", error = ex.Message });
            }
        }

        // ==================== EXPIRY TRENDS ====================
        [HttpGet("expiry-trends")]
        public async Task<IActionResult> GetExpiryTrends([FromQuery] int months = 12)
        {
            try
            {
                var trends = await _dashboardService.GetExpiryTrendsAsync(months);
                return Ok(trends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving expiry trends", error = ex.Message });
            }
        }

        // ==================== CRITICAL ALERTS ====================
        [HttpGet("critical-alerts")]
        public async Task<IActionResult> GetCriticalAlerts()
        {
            try
            {
                var alerts = await _dashboardService.GetCriticalAlertsAsync();
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving critical alerts", error = ex.Message });
            }
        }

        // ==================== UPCOMING EXPIRIES ====================
        [HttpGet("upcoming-expiries")]
        public async Task<IActionResult> GetUpcomingExpiries([FromQuery] int days = 30)
        {
            try
            {
                var expiries = await _dashboardService.GetUpcomingExpiriesAsync(days);
                return Ok(expiries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving upcoming expiries", error = ex.Message });
            }
        }
    }
}
