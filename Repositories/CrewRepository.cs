using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;

namespace ASCO.Repositories
{
    public class CrewRepository
    {
        private readonly ASCODbContext _context;

        public CrewRepository(ASCODbContext context)
        {
            _context = context;
        }

        //updations
        public async Task<int> UpdateCrewAsync(User request)
        {

            _context.Users.Update(request);
            return await _context.SaveChangesAsync();
        }

        //assign crew to vessel
        public async Task<int> AssignCrewToVesselAsync(ShipAssignment assignment)
        {
            await _context.ShipAssignments.AddAsync(assignment);
            return await _context.SaveChangesAsync(); //if the value is positive, the assignment was added successfully.
        }

        //get active assignment by assignment id
        public async Task<ShipAssignment?> GetActiveAssignmentById(int id)
        {
            return await _context.ShipAssignments
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        //unassign crew from vessel (for now im keeping to update the unassignment date)
        public async Task<int> UnassignCrewFromVesselAsync(ShipAssignment assignment)
        {
            //update only required fields
            _context.ShipAssignments.Attach(assignment);
            _context.Entry(assignment).Property(a => a.UnassignedAt).IsModified = true;
            _context.Entry(assignment).Property(a => a.Status).IsModified = true;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAssignmentDetailsAsync(ShipAssignment assignment)
        {
            _context.ShipAssignments.Update(assignment);
            return await _context.SaveChangesAsync();
        }

        // crew documents CRUD and queries
        public async Task<int> AddMedicalAsync(CrewMedicalRecord rec)
        {
            await _context.CrewMedicalRecords.AddAsync(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateMedicalAsync(CrewMedicalRecord rec)
        {
            _context.CrewMedicalRecords.Update(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<CrewMedicalRecord?> GetMedicalByIdAsync(int id)
        {
            return await _context.CrewMedicalRecords.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CrewMedicalRecord>> SearchMedicalAsync(CrewDocumentSearchDto s)
        {
            var q = _context.CrewMedicalRecords.AsQueryable();
            if (s.UserId.HasValue) q = q.Where(x => x.UserId == s.UserId);
            if (!string.IsNullOrWhiteSpace(s.Keyword)) q = q.Where(x => x.ProviderName.Contains(s.Keyword));
            if (s.ExpiryFrom.HasValue) q = q.Where(x => x.ExpiryDate >= s.ExpiryFrom);
            if (s.ExpiryTo.HasValue) q = q.Where(x => x.ExpiryDate <= s.ExpiryTo);
            return await q
                .OrderByDescending(x => x.CreatedAt)
                .Skip((s.Page - 1) * s.PageSize)
                .Take(s.PageSize)
                .ToListAsync();
        }

        public async Task<int> AddPassportAsync(CrewPassport rec)
        {
            await _context.CrewPassports.AddAsync(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdatePassportAsync(CrewPassport rec)
        {
            _context.CrewPassports.Update(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<CrewPassport?> GetPassportByIdAsync(int id)
        {
            return await _context.CrewPassports.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CrewPassport>> SearchPassportsAsync(CrewDocumentSearchDto s)
        {
            var q = _context.CrewPassports.AsQueryable();
            if (s.UserId.HasValue) q = q.Where(x => x.UserId == s.UserId);
            if (!string.IsNullOrWhiteSpace(s.Keyword)) q = q.Where(x => x.PassportNumber.Contains(s.Keyword));
            if (s.ExpiryFrom.HasValue) q = q.Where(x => x.ExpiryDate >= s.ExpiryFrom);
            if (s.ExpiryTo.HasValue) q = q.Where(x => x.ExpiryDate <= s.ExpiryTo);
            return await q
                .OrderBy(x => x.ExpiryDate)
                .Skip((s.Page - 1) * s.PageSize)
                .Take(s.PageSize)
                .ToListAsync();
        }

        public async Task<int> AddVisaAsync(CrewVisa rec)
        {
            await _context.CrewVisas.AddAsync(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateVisaAsync(CrewVisa rec)
        {
            _context.CrewVisas.Update(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<CrewVisa?> GetVisaByIdAsync(int id)
        {
            return await _context.CrewVisas.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CrewVisa>> SearchVisasAsync(CrewDocumentSearchDto s)
        {
            var q = _context.CrewVisas.AsQueryable();
            if (s.UserId.HasValue) q = q.Where(x => x.UserId == s.UserId);
            if (!string.IsNullOrWhiteSpace(s.Keyword)) q = q.Where(x => x.VisaType.Contains(s.Keyword));
            if (s.ExpiryFrom.HasValue) q = q.Where(x => x.ExpiryDate >= s.ExpiryFrom);
            if (s.ExpiryTo.HasValue) q = q.Where(x => x.ExpiryDate <= s.ExpiryTo);
            return await q
                .OrderBy(x => x.ExpiryDate)
                .Skip((s.Page - 1) * s.PageSize)
                .Take(s.PageSize)
                .ToListAsync();
        }

        public async Task<int> AddCrewReportAsync(CrewReport rec)
        {
            await _context.CrewReports.AddAsync(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCrewReportAsync(CrewReport rec)
        {
            _context.CrewReports.Update(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<CrewReport?> GetCrewReportByIdAsync(int id)
        {
            return await _context.CrewReports.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CrewReport>> SearchCrewReportsAsync(int? userId, string? reportType, int page, int pageSize)
        {
            var q = _context.CrewReports.AsQueryable();
            if (userId.HasValue) q = q.Where(x => x.UserId == userId);
            if (!string.IsNullOrWhiteSpace(reportType)) q = q.Where(x => x.ReportType == reportType);
            return await q.OrderByDescending(x => x.ReportDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<ShipAssignment>> GetAssignmentHistoryAsync(int userId)
        {
            return await _context.ShipAssignments
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }
    }

    //rest of the stuff goes here.

    
   
    
}