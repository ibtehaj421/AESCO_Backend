using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;
using ASCO.DTOs;
using ASCO.DTOs.Crew;

namespace ASCO.Repositories
{
    public class VesselRepository
    {
        private readonly ASCODbContext _context;

        public VesselRepository(ASCODbContext context)
        {
            _context = context;
        }

        public async Task<Ship> CreateVesselAsync(Ship vessel)
        {
            await _context.Ships.AddAsync(vessel);
            await _context.SaveChangesAsync();
            return vessel;
        }

        public async Task<List<Ship>> GetAllVesselsAsync()
        {
            return await _context.Ships
                .Include(s => s.ShipAssignments)
                    .ThenInclude(sa => sa.User)
                .Include(s => s.ShipAssignments)
                    .ThenInclude(sa => sa.AssignedBy)
                .Include(s => s.Voyages)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Ship?> GetVesselByIdAsync(int id)
        {
            return await _context.Ships
                .Include(s => s.ShipAssignments)
                    .ThenInclude(sa => sa.User)
                .Include(s => s.ShipAssignments)
                    .ThenInclude(sa => sa.AssignedBy)
                .Include(s => s.Voyages)
                .Include(s => s.MaintenanceRecords)
                .Include(s => s.Certificates)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Ship> UpdateVesselAsync(Ship vessel)
        {
            _context.Ships.Update(vessel);
            await _context.SaveChangesAsync();
            return vessel;
        }

        public async Task<bool> DeleteVesselAsync(int id)
        {
            var vessel = await _context.Ships.FindAsync(id);
            if (vessel == null)
            {
                return false;
            }

            _context.Ships.Remove(vessel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ship>> SearchVesselsAsync(ShipSearchDto searchDto)
        {
            var query = _context.Ships
                .Include(s => s.ShipAssignments)
                    .ThenInclude(sa => sa.User)
                .Include(s => s.Voyages)
                .AsQueryable();

            // Apply search filters
            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                var searchTerm = searchDto.SearchTerm.ToLower();
                query = query.Where(s => 
                    s.Name.ToLower().Contains(searchTerm) ||
                    s.IMONumber.ToLower().Contains(searchTerm) ||
                    s.CallSign != null && s.CallSign.ToLower().Contains(searchTerm) ||
                    s.RegistrationNumber.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(searchDto.ShipType))
            {
                query = query.Where(s => s.ShipType == searchDto.ShipType);
            }

            if (!string.IsNullOrEmpty(searchDto.Flag))
            {
                query = query.Where(s => s.Flag == searchDto.Flag);
            }

            if (!string.IsNullOrEmpty(searchDto.Status))
            {
                query = query.Where(s => s.Status == searchDto.Status);
            }

            if (!string.IsNullOrEmpty(searchDto.HomePort))
            {
                query = query.Where(s => s.HomePort == searchDto.HomePort);
            }

            if (searchDto.BuildDateFrom.HasValue)
            {
                query = query.Where(s => s.BuildDate >= searchDto.BuildDateFrom.Value);
            }

            if (searchDto.BuildDateTo.HasValue)
            {
                query = query.Where(s => s.BuildDate <= searchDto.BuildDateTo.Value);
            }

            if (searchDto.MinTonnage.HasValue)
            {
                query = query.Where(s => s.GrossTonnage >= searchDto.MinTonnage.Value);
            }

            if (searchDto.MaxTonnage.HasValue)
            {
                query = query.Where(s => s.GrossTonnage <= searchDto.MaxTonnage.Value);
            }

            // Apply sorting
            query = searchDto.SortBy.ToLower() switch
            {
                "name" => searchDto.SortDescending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "imono" => searchDto.SortDescending ? query.OrderByDescending(s => s.IMONumber) : query.OrderBy(s => s.IMONumber),
                "shiptype" => searchDto.SortDescending ? query.OrderByDescending(s => s.ShipType) : query.OrderBy(s => s.ShipType),
                "flag" => searchDto.SortDescending ? query.OrderByDescending(s => s.Flag) : query.OrderBy(s => s.Flag),
                "status" => searchDto.SortDescending ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status),
                "grosstonnage" => searchDto.SortDescending ? query.OrderByDescending(s => s.GrossTonnage) : query.OrderBy(s => s.GrossTonnage),
                "builddate" => searchDto.SortDescending ? query.OrderByDescending(s => s.BuildDate) : query.OrderBy(s => s.BuildDate),
                "createdat" => searchDto.SortDescending ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
                _ => query.OrderBy(s => s.Name)
            };

            // Apply pagination
            if (searchDto.Page > 0 && searchDto.PageSize > 0)
            {
                query = query.Skip((searchDto.Page - 1) * searchDto.PageSize).Take(searchDto.PageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<PagedResult<Ship>> GetVesselsPagedAsync(int page, int pageSize, string? searchTerm = null)
        {
            var query = _context.Ships
                .Include(s => s.ShipAssignments)
                    .ThenInclude(sa => sa.User)
                .Include(s => s.Voyages)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(s => 
                    s.Name.ToLower().Contains(search) ||
                    s.IMONumber.ToLower().Contains(search) ||
                    s.CallSign != null && s.CallSign.ToLower().Contains(search) ||
                    s.RegistrationNumber.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Ship>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> VesselExistsAsync(int id)
        {
            return await _context.Ships.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> IMOExistsAsync(string imoNumber, int? excludeId = null)
        {
            var query = _context.Ships.Where(s => s.IMONumber == imoNumber);
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        public async Task<bool> RegistrationNumberExistsAsync(string registrationNumber, int? excludeId = null)
        {
            var query = _context.Ships.Where(s => s.RegistrationNumber == registrationNumber);
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }
            return await query.AnyAsync();
        }

        public async Task<List<string>> GetDistinctShipTypesAsync()
        {
            return await _context.Ships
                .Select(s => s.ShipType)
                .Distinct()
                .OrderBy(st => st)
                .ToListAsync();
        }

        public async Task<List<string>> GetDistinctFlagsAsync()
        {
            return await _context.Ships
                .Select(s => s.Flag)
                .Distinct()
                .OrderBy(f => f)
                .ToListAsync();
        }

        public async Task<List<string>> GetDistinctHomePortsAsync()
        {
            return await _context.Ships
                .Where(s => !string.IsNullOrEmpty(s.HomePort))
                .Select(s => s.HomePort!)
                .Distinct()
                .OrderBy(hp => hp)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetVesselStatisticsAsync()
        {
            var totalVessels = await _context.Ships.CountAsync();
            var activeVessels = await _context.Ships.CountAsync(s => s.Status == "active");
            var inactiveVessels = await _context.Ships.CountAsync(s => s.Status == "inactive");
            var maintenanceVessels = await _context.Ships.CountAsync(s => s.Status == "maintenance");
            var decommissionedVessels = await _context.Ships.CountAsync(s => s.Status == "decommissioned");

            return new Dictionary<string, int>
            {
                { "Total", totalVessels },
                { "Active", activeVessels },
                { "Inactive", inactiveVessels },
                { "Maintenance", maintenanceVessels },
                { "Decommissioned", decommissionedVessels }
            };
        }
    }
}