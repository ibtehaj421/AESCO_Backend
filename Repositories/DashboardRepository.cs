using Microsoft.EntityFrameworkCore;
using ASCO.DbContext;
using ASCO.DTOs;

namespace ASCO.Repositories
{
    public class DashboardRepository
    {
        private readonly ASCODbContext _context;

        public DashboardRepository(ASCODbContext context)
        {
            _context = context;
        }

        // ==================== OVERVIEW STATISTICS ====================
        public async Task<DashboardOverviewStatsDto> GetOverviewStatsAsync()
        {
            var totalCrew = await _context.Users.CountAsync();
            var activeCrew = await _context.Users.CountAsync(u => u.Status == "active");
            var onLeaveCrew = await _context.Users.CountAsync(u => u.Status == "on leave");
            
            var totalVessels = await _context.Ships.CountAsync();
            var activeVessels = await _context.Ships.CountAsync(s => s.Status == "active");
            
            var totalCertifications = await _context.Certificates.CountAsync();
            var expiringCertifications = await _context.Certificates
                .CountAsync(c => c.ExpiryDate <= DateTime.UtcNow.AddDays(30));
            
            var totalTrainings = await _context.CrewTrainings.CountAsync();
            var pendingTrainings = await _context.CrewTrainings
                .CountAsync(t => t.Status == "Pending");
            
            var totalEvaluations = await _context.CrewEvaluations.CountAsync();
            var pendingEvaluations = await _context.CrewEvaluations
                .CountAsync(e => e.Status == "Pending");

            return new DashboardOverviewStatsDto
            {
                TotalCrewMembers = totalCrew,
                ActiveCrewMembers = activeCrew,
                OnLeaveCrewMembers = onLeaveCrew,
                TotalVessels = totalVessels,
                ActiveVessels = activeVessels,
                TotalCertifications = totalCertifications,
                ExpiringCertifications = expiringCertifications,
                TotalTrainings = totalTrainings,
                PendingTrainings = pendingTrainings,
                TotalEvaluations = totalEvaluations,
                PendingEvaluations = pendingEvaluations
            };
        }

        // ==================== EXPIRY STATISTICS ====================
        public async Task<DashboardExpiryStatsDto> GetExpiryStatsAsync()
        {
            var now = DateTime.UtcNow;
            var thisWeek = now.AddDays(7);
            var thisMonth = now.AddDays(30);
            var nextMonth = now.AddDays(60);

            var totalExpiring = await _context.Certificates
                .CountAsync(c => c.ExpiryDate <= thisMonth);
            
            var expiringThisWeek = await _context.Certificates
                .CountAsync(c => c.ExpiryDate <= thisWeek && c.ExpiryDate > now);
            
            var expiringThisMonth = await _context.Certificates
                .CountAsync(c => c.ExpiryDate <= thisMonth && c.ExpiryDate > thisWeek);
            
            var expiringNextMonth = await _context.Certificates
                .CountAsync(c => c.ExpiryDate <= nextMonth && c.ExpiryDate > thisMonth);
            
            var alreadyExpired = await _context.Certificates
                .CountAsync(c => c.ExpiryDate < now);

            var totalCertificates = await _context.Certificates.CountAsync();
            var expiryRate = totalCertificates > 0 ? (double)totalExpiring / totalCertificates * 100 : 0;

            return new DashboardExpiryStatsDto
            {
                TotalExpiring = totalExpiring,
                ExpiringThisWeek = expiringThisWeek,
                ExpiringThisMonth = expiringThisMonth,
                ExpiringNextMonth = expiringNextMonth,
                AlreadyExpired = alreadyExpired,
                ExpiryRate = Math.Round(expiryRate, 2)
            };
        }

        // ==================== VESSEL STATISTICS ====================
        public async Task<DashboardVesselStatsDto> GetVesselStatsAsync()
        {
            var totalVessels = await _context.Ships.CountAsync();
            var activeVessels = await _context.Ships.CountAsync(s => s.Status == "active");
            var inactiveVessels = await _context.Ships.CountAsync(s => s.Status == "Inactive");
            
            // For now, we'll use mock data for vessels in port/at sea
            var vesselsInPort = activeVessels / 2; // Mock: half of active vessels
            var vesselsAtSea = activeVessels - vesselsInPort;

            var totalCrew = await _context.Users.CountAsync();
            var averageCrewPerVessel = totalVessels > 0 ? (double)totalCrew / totalVessels : 0;

            return new DashboardVesselStatsDto
            {
                TotalVessels = totalVessels,
                ActiveVessels = activeVessels,
                InactiveVessels = inactiveVessels,
                VesselsInPort = vesselsInPort,
                VesselsAtSea = vesselsAtSea,
                AverageCrewPerVessel = Math.Round(averageCrewPerVessel, 1)
            };
        }

        // ==================== TRAINING STATISTICS ====================
        public async Task<DashboardTrainingStatsDto> GetTrainingStatsAsync()
        {
            var totalTrainings = await _context.CrewTrainings.CountAsync();
            var completedTrainings = await _context.CrewTrainings
                .CountAsync(t => t.Status == "Completed");
            var pendingTrainings = await _context.CrewTrainings
                .CountAsync(t => t.Status == "Pending");
            var overdueTrainings = await _context.CrewTrainings
                .CountAsync(t => t.Status == "Pending" && t.ExpireDate < DateTime.UtcNow);
            var scheduledTrainings = await _context.CrewTrainings
                .CountAsync(t => t.Status == "Scheduled");

            var completionRate = totalTrainings > 0 ? (double)completedTrainings / totalTrainings * 100 : 0;

            return new DashboardTrainingStatsDto
            {
                TotalTrainings = totalTrainings,
                CompletedTrainings = completedTrainings,
                PendingTrainings = pendingTrainings,
                OverdueTrainings = overdueTrainings,
                ScheduledTrainings = scheduledTrainings,
                CompletionRate = Math.Round(completionRate, 2)
            };
        }

        // ==================== EVALUATION STATISTICS ====================
        public async Task<DashboardEvaluationStatsDto> GetEvaluationStatsAsync()
        {
            var totalEvaluations = await _context.CrewEvaluations.CountAsync();
            var completedEvaluations = await _context.CrewEvaluations
                .CountAsync(e => e.Status == "Completed");
            var pendingEvaluations = await _context.CrewEvaluations
                .CountAsync(e => e.Status == "Pending");
            var overdueEvaluations = await _context.CrewEvaluations
                .CountAsync(e => e.Status == "Pending" && e.EnteredDate < DateTime.UtcNow.AddDays(-30));
            var scheduledEvaluations = await _context.CrewEvaluations
                .CountAsync(e => e.Status == "Scheduled");

            var completionRate = totalEvaluations > 0 ? (double)completedEvaluations / totalEvaluations * 100 : 0;

            return new DashboardEvaluationStatsDto
            {
                TotalEvaluations = totalEvaluations,
                CompletedEvaluations = completedEvaluations,
                PendingEvaluations = pendingEvaluations,
                OverdueEvaluations = overdueEvaluations,
                ScheduledEvaluations = scheduledEvaluations,
                CompletionRate = Math.Round(completionRate, 2)
            };
        }

        // ==================== CREW EXPIRY DATA ====================
        public async Task<CrewExpiryDataDto> GetCrewExpiryDataAsync(CrewExpiryFiltersDto filters)
        {
            var query = from c in _context.Certificates
                        join s in _context.Ships on c.ShipId equals s.Id
                        select new CrewExpiryItemDto
                        {
                            Id = c.Id,
                            FullName = $"Ship Certificate", // Since certificates are ship-related, not crew-related
                            Rank = "N/A",
                            VesselName = s.Name,
                            DocumentType = "Certificate",
                            DocumentName = c.CertificateType,
                            ExpiryDate = c.ExpiryDate,
                            DaysUntilExpiry = (int)(c.ExpiryDate - DateTime.UtcNow).TotalDays,
                            Status = c.ExpiryDate < DateTime.UtcNow ? "Expired" : 
                                    c.ExpiryDate <= DateTime.UtcNow.AddDays(7) ? "Critical" :
                                    c.ExpiryDate <= DateTime.UtcNow.AddDays(30) ? "Warning" : "Good",
                            Priority = c.ExpiryDate < DateTime.UtcNow ? "High" :
                                      c.ExpiryDate <= DateTime.UtcNow.AddDays(7) ? "High" :
                                      c.ExpiryDate <= DateTime.UtcNow.AddDays(30) ? "Medium" : "Low"
                        };

            // Apply filters
            if (!string.IsNullOrEmpty(filters.SearchTerm))
            {
                query = query.Where(x => x.FullName.Contains(filters.SearchTerm) || 
                                        x.Rank.Contains(filters.SearchTerm) ||
                                        x.VesselName.Contains(filters.SearchTerm));
            }

            if (!string.IsNullOrEmpty(filters.VesselId))
            {
                query = query.Where(x => x.VesselName == filters.VesselId);
            }

            if (!string.IsNullOrEmpty(filters.Rank))
            {
                query = query.Where(x => x.Rank == filters.Rank);
            }

            if (!string.IsNullOrEmpty(filters.Status))
            {
                query = query.Where(x => x.Status == filters.Status);
            }

            if (filters.DaysUntilExpiry.HasValue)
            {
                query = query.Where(x => x.DaysUntilExpiry <= filters.DaysUntilExpiry.Value);
            }

            if (filters.ExpiryDateFrom.HasValue)
            {
                query = query.Where(x => x.ExpiryDate >= filters.ExpiryDateFrom.Value);
            }

            if (filters.ExpiryDateTo.HasValue)
            {
                query = query.Where(x => x.ExpiryDate <= filters.ExpiryDateTo.Value);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / filters.PageSize);

            var items = await query
                .OrderBy(x => x.ExpiryDate)
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return new CrewExpiryDataDto
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalPages = totalPages
            };
        }

        // ==================== RECENT ACTIVITIES ====================
        public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(int limit = 10)
        {
            var activities = new List<RecentActivityDto>();

            // Get recent training activities
            var recentTrainings = await _context.CrewTrainings
                .Include(t => t.User)
                .Include(t => t.Vessel)
                .OrderByDescending(t => t.CreatedAt)
                .Take(limit / 2)
                .Select(t => new RecentActivityDto
                {
                    Id = t.Id,
                    ActivityType = "Training",
                    Description = $"{t.Training} - {t.Status}",
                    UserName = $"{t.User.Name} {t.User.Surname}",
                    VesselName = t.Vessel != null ? t.Vessel.Name : "Unassigned",
                    Timestamp = t.CreatedAt,
                    Status = t.Status
                })
                .ToListAsync();

            // Get recent evaluation activities
            var recentEvaluations = await _context.CrewEvaluations
                .Include(e => e.User)
                .Include(e => e.Vessel)
                .OrderByDescending(e => e.CreatedAt)
                .Take(limit / 2)
                .Select(e => new RecentActivityDto
                {
                    Id = e.Id,
                    ActivityType = "Evaluation",
                    Description = $"{e.FormName} - {e.Status}",
                    UserName = $"{e.User.Name} {e.User.Surname}",
                    VesselName = e.Vessel != null ? e.Vessel.Name : "Unassigned",
                    Timestamp = e.CreatedAt,
                    Status = e.Status
                })
                .ToListAsync();

            activities.AddRange(recentTrainings);
            activities.AddRange(recentEvaluations);

            return activities
                .OrderByDescending(a => a.Timestamp)
                .Take(limit)
                .ToList();
        }

        // ==================== CREW DISTRIBUTION ====================
        public async Task<List<CrewDistributionDto>> GetCrewDistributionByRankAsync()
        {
            var distribution = await _context.Users
                .Where(u => !string.IsNullOrEmpty(u.Rank))
                .GroupBy(u => u.Rank)
                .Select(g => new CrewDistributionDto
                {
                    Category = g.Key ?? "Unknown",
                    Count = g.Count()
                })
                .OrderByDescending(d => d.Count)
                .ToListAsync();

            var totalCount = distribution.Sum(d => d.Count);
            foreach (var item in distribution)
            {
                item.Percentage = totalCount > 0 ? Math.Round((double)item.Count / totalCount * 100, 1) : 0;
            }

            return distribution;
        }

        public async Task<List<CrewDistributionDto>> GetCrewDistributionByNationalityAsync()
        {
            var distribution = await _context.Users
                .Where(u => !string.IsNullOrEmpty(u.Nationality))
                .GroupBy(u => u.Nationality)
                .Select(g => new CrewDistributionDto
                {
                    Category = g.Key ?? "Unknown",
                    Count = g.Count()
                })
                .OrderByDescending(d => d.Count)
                .ToListAsync();

            var totalCount = distribution.Sum(d => d.Count);
            foreach (var item in distribution)
            {
                item.Percentage = totalCount > 0 ? Math.Round((double)item.Count / totalCount * 100, 1) : 0;
            }

            return distribution;
        }

        public async Task<List<CrewDistributionDto>> GetCrewDistributionByVesselAsync()
        {
            // Since User doesn't have a direct Ship relationship, we'll use CrewTrainings to get vessel distribution
            var distribution = await _context.CrewTrainings
                .Include(t => t.Vessel)
                .Where(t => t.Vessel != null)
                .GroupBy(t => t.Vessel.Name)
                .Select(g => new CrewDistributionDto
                {
                    Category = g.Key,
                    Count = g.Select(x => x.UserId).Distinct().Count()
                })
                .OrderByDescending(d => d.Count)
                .ToListAsync();

            var totalCount = distribution.Sum(d => d.Count);
            foreach (var item in distribution)
            {
                item.Percentage = totalCount > 0 ? Math.Round((double)item.Count / totalCount * 100, 1) : 0;
            }

            return distribution;
        }

        // ==================== EXPIRY TRENDS ====================
        public async Task<List<ExpiryTrendDto>> GetExpiryTrendsAsync(int months = 12)
        {
            var trends = new List<ExpiryTrendDto>();
            var startDate = DateTime.UtcNow.AddMonths(-months);

            for (int i = 0; i < months; i++)
            {
                var monthStart = startDate.AddMonths(i);
                var monthEnd = monthStart.AddMonths(1);

                var expiringCount = await _context.Certificates
                    .CountAsync(c => c.ExpiryDate >= monthStart && c.ExpiryDate < monthEnd);

                var expiredCount = await _context.Certificates
                    .CountAsync(c => c.ExpiryDate < monthStart && c.ExpiryDate >= monthStart.AddDays(-30));

                var renewedCount = await _context.Certificates
                    .CountAsync(c => c.CreatedAt >= monthStart && c.CreatedAt < monthEnd);

                trends.Add(new ExpiryTrendDto
                {
                    Month = monthStart.ToString("MMM yyyy"),
                    ExpiringCount = expiringCount,
                    ExpiredCount = expiredCount,
                    RenewedCount = renewedCount
                });
            }

            return trends;
        }

        // ==================== CRITICAL ALERTS ====================
        public async Task<List<CriticalAlertDto>> GetCriticalAlertsAsync()
        {
            var alerts = new List<CriticalAlertDto>();

            // Expired certificates
            var expiredCertificates = await _context.Certificates
                .Include(c => c.Ship)
                .Where(c => c.ExpiryDate < DateTime.UtcNow)
                .Take(5)
                .Select(c => new CriticalAlertDto
                {
                    Id = c.Id,
                    AlertType = "Certificate Expired",
                    Title = $"Certificate Expired: {c.CertificateType}",
                    Description = $"{c.Ship.Name}'s {c.CertificateType} certificate has expired",
                    Priority = "High",
                    Status = "Active",
                    CreatedAt = c.ExpiryDate,
                    RelatedEntity = "Certificate",
                    RelatedEntityId = c.Id
                })
                .ToListAsync();

            // Certificates expiring soon
            var expiringSoon = await _context.Certificates
                .Include(c => c.Ship)
                .Where(c => c.ExpiryDate <= DateTime.UtcNow.AddDays(7) && c.ExpiryDate > DateTime.UtcNow)
                .Take(5)
                .Select(c => new CriticalAlertDto
                {
                    Id = c.Id,
                    AlertType = "Certificate Expiring Soon",
                    Title = $"Certificate Expiring: {c.CertificateType}",
                    Description = $"{c.Ship.Name}'s {c.CertificateType} certificate expires in {(int)(c.ExpiryDate - DateTime.UtcNow).TotalDays} days",
                    Priority = "High",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    RelatedEntity = "Certificate",
                    RelatedEntityId = c.Id
                })
                .ToListAsync();

            alerts.AddRange(expiredCertificates);
            alerts.AddRange(expiringSoon);

            return alerts
                .OrderBy(a => a.CreatedAt)
                .Take(10)
                .ToList();
        }

        // ==================== UPCOMING EXPIRIES ====================
        public async Task<List<UpcomingExpiryDto>> GetUpcomingExpiriesAsync(int days = 30)
        {
            var endDate = DateTime.UtcNow.AddDays(days);

            var expiries = await _context.Certificates
                .Include(c => c.Ship)
                .Where(c => c.ExpiryDate <= endDate && c.ExpiryDate > DateTime.UtcNow)
                .OrderBy(c => c.ExpiryDate)
                .Take(20)
                .Select(c => new UpcomingExpiryDto
                {
                    Id = c.Id,
                    FullName = $"Ship Certificate",
                    Rank = "N/A",
                    VesselName = c.Ship != null ? c.Ship.Name : "Unassigned",
                    DocumentType = "Certificate",
                    DocumentName = c.CertificateType,
                    ExpiryDate = c.ExpiryDate,
                    DaysUntilExpiry = (int)(c.ExpiryDate - DateTime.UtcNow).TotalDays,
                    Priority = c.ExpiryDate <= DateTime.UtcNow.AddDays(7) ? "High" :
                              c.ExpiryDate <= DateTime.UtcNow.AddDays(30) ? "Medium" : "Low"
                })
                .ToListAsync();

            return expiries;
        }
    }
}
