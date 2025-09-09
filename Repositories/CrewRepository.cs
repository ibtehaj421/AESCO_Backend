using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;
using ASCO.DTOs;

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

        //vessel manning thingies
        public async Task<List<VesselManning>> GetVesselManningAsync(int vesselId)
        {
            return await _context.VesselMannings
                .Where(vm => vm.VesselId == vesselId)
                .OrderByDescending(vm => vm.Rank)
                .ToListAsync();
        }

        public async Task<int> AddVesselManningAsync(List<VesselManning> vm)
        {
            foreach (var v in vm)
            {
                await _context.VesselMannings.AddAsync(v);
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteVesselManningAsync(int vesselId, List<string> ranks)
        {
            var toDelete = await _context.VesselMannings
                .Where(vm => vm.VesselId == vesselId && ranks.Contains(vm.Rank))
                .ToListAsync();
            if (toDelete.Count == 0) return 0; //nothing to delete
            _context.VesselMannings.RemoveRange(toDelete);
            return await _context.SaveChangesAsync();
        }


        //payroll thingies
        public async Task<int> AddPayrollRecordAsync(Payroll rec)
        {
            await _context.PayrollRecords.AddAsync(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Payroll>> GetPayrollByCrewAsync(int crewId)
        {
            return await _context.PayrollRecords
                .Where(p => p.CrewMemberId == crewId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        //expense thingies
        public async Task<int> AddCrewExpenseAsync(CrewExpense rec)
        {
            await _context.CrewExpenses.AddAsync(rec);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CrewExpense>> GetExpensesByCrewAsync(int crewId)
        {
            return await _context.CrewExpenses
                .Where(e => e.CrewMemberId == crewId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }
        
        public async Task<int> AddCashStatementAsync(StatementOfCash soc)
        {
            await _context.CashStatements.AddAsync(soc);
            return await _context.SaveChangesAsync();
        }

        // Crew Training methods
        public async Task<int> AddCrewTrainingAsync(CrewTraining training)
        {
            await _context.CrewTrainings.AddAsync(training);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCrewTrainingAsync(CrewTraining training)
        {
            _context.CrewTrainings.Update(training);
            return await _context.SaveChangesAsync();
        }

        public async Task<CrewTraining?> GetCrewTrainingByIdAsync(int id)
        {
            return await _context.CrewTrainings
                .Include(ct => ct.User)
                .Include(ct => ct.Vessel)
                .Include(ct => ct.CreatedByUser)
                .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task<List<CrewTraining>> SearchCrewTrainingsAsync(CrewTrainingSearchDto searchDto)
        {
            var query = _context.CrewTrainings
                .Include(ct => ct.User)
                .Include(ct => ct.Vessel)
                .Include(ct => ct.CreatedByUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                query = query.Where(ct => 
                    ct.Training.Contains(searchDto.SearchTerm) ||
                    ct.TrainingCategory.Contains(searchDto.SearchTerm) ||
                    ct.Trainer.Contains(searchDto.SearchTerm) ||
                    (ct.User != null && 
                        (ct.User.Name != null && ct.User.Name.Contains(searchDto.SearchTerm) ||
                        ct.User.Surname != null && ct.User.Surname.Contains(searchDto.SearchTerm))) ||
                    ct.Vessel.Name.Contains(searchDto.SearchTerm));
            }

            if (searchDto.UserId.HasValue)
                query = query.Where(ct => ct.UserId == searchDto.UserId.Value);

            if (searchDto.VesselId.HasValue)
                query = query.Where(ct => ct.VesselId == searchDto.VesselId.Value);

            if (!string.IsNullOrEmpty(searchDto.TrainingCategory))
                query = query.Where(ct => ct.TrainingCategory == searchDto.TrainingCategory);

            if (!string.IsNullOrEmpty(searchDto.Status))
                query = query.Where(ct => ct.Status == searchDto.Status);

            if (searchDto.TrainingDateFrom.HasValue)
                query = query.Where(ct => ct.TrainingDate >= searchDto.TrainingDateFrom.Value);

            if (searchDto.TrainingDateTo.HasValue)
                query = query.Where(ct => ct.TrainingDate <= searchDto.TrainingDateTo.Value);

            if (searchDto.ExpireDateFrom.HasValue)
                query = query.Where(ct => ct.ExpireDate >= searchDto.ExpireDateFrom.Value);

            if (searchDto.ExpireDateTo.HasValue)
                query = query.Where(ct => ct.ExpireDate <= searchDto.ExpireDateTo.Value);

            // Apply sorting
            query = searchDto.SortBy.ToLower() switch
            {
                "trainingdate" => searchDto.SortDescending ? query.OrderByDescending(ct => ct.TrainingDate) : query.OrderBy(ct => ct.TrainingDate),
                "expiredate" => searchDto.SortDescending ? query.OrderByDescending(ct => ct.ExpireDate) : query.OrderBy(ct => ct.ExpireDate),
                "training" => searchDto.SortDescending ? query.OrderByDescending(ct => ct.Training) : query.OrderBy(ct => ct.Training),
                "status" => searchDto.SortDescending ? query.OrderByDescending(ct => ct.Status) : query.OrderBy(ct => ct.Status),
                _ => searchDto.SortDescending ? query.OrderByDescending(ct => ct.CreatedAt) : query.OrderBy(ct => ct.CreatedAt)
            };

            return await query
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();
        }

        // Crew Evaluation methods
        public async Task<int> AddCrewEvaluationAsync(CrewEvaluation evaluation)
        {
            await _context.CrewEvaluations.AddAsync(evaluation);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCrewEvaluationAsync(CrewEvaluation evaluation)
        {
            _context.CrewEvaluations.Update(evaluation);
            return await _context.SaveChangesAsync();
        }

        public async Task<CrewEvaluation?> GetCrewEvaluationByIdAsync(int id)
        {
            return await _context.CrewEvaluations
                .Include(ce => ce.User)
                .Include(ce => ce.Vessel)
                .Include(ce => ce.EnteredBy)
                .Include(ce => ce.CreatedByUser)
                .FirstOrDefaultAsync(ce => ce.Id == id);
        }

        public async Task<List<CrewEvaluation>> SearchCrewEvaluationsAsync(CrewEvaluationSearchDto searchDto)
        {
            var query = _context.CrewEvaluations
                .Include(ce => ce.User)
                .Include(ce => ce.Vessel)
                .Include(ce => ce.EnteredBy)
                .Include(ce => ce.CreatedByUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                query = query.Where(ce => 
                    ce.FormName.Contains(searchDto.SearchTerm) ||
                    ce.FormNo.Contains(searchDto.SearchTerm) ||
                    (ce.User != null && 
                        (ce.User.Name != null && ce.User.Name.Contains(searchDto.SearchTerm) ||
                        ce.User.Surname != null && ce.User.Surname.Contains(searchDto.SearchTerm))) ||
                    ce.Vessel.Name.Contains(searchDto.SearchTerm));
            }

            if (searchDto.UserId.HasValue)
                query = query.Where(ce => ce.UserId == searchDto.UserId.Value);

            if (searchDto.VesselId.HasValue)
                query = query.Where(ce => ce.VesselId == searchDto.VesselId.Value);

            if (!string.IsNullOrEmpty(searchDto.Status))
                query = query.Where(ce => ce.Status == searchDto.Status);

            if (searchDto.EnteredDateFrom.HasValue)
                query = query.Where(ce => ce.EnteredDate >= searchDto.EnteredDateFrom.Value);

            if (searchDto.EnteredDateTo.HasValue)
                query = query.Where(ce => ce.EnteredDate <= searchDto.EnteredDateTo.Value);

            if (searchDto.MinOverallRating.HasValue)
                query = query.Where(ce => ce.OverallRating >= searchDto.MinOverallRating.Value);

            if (searchDto.MaxOverallRating.HasValue)
                query = query.Where(ce => ce.OverallRating <= searchDto.MaxOverallRating.Value);

            // Apply sorting
            query = searchDto.SortBy.ToLower() switch
            {
                "entereddate" => searchDto.SortDescending ? query.OrderByDescending(ce => ce.EnteredDate) : query.OrderBy(ce => ce.EnteredDate),
                "overallrating" => searchDto.SortDescending ? query.OrderByDescending(ce => ce.OverallRating) : query.OrderBy(ce => ce.OverallRating),
                "formname" => searchDto.SortDescending ? query.OrderByDescending(ce => ce.FormName) : query.OrderBy(ce => ce.FormName),
                "status" => searchDto.SortDescending ? query.OrderByDescending(ce => ce.Status) : query.OrderBy(ce => ce.Status),
                _ => searchDto.SortDescending ? query.OrderByDescending(ce => ce.CreatedAt) : query.OrderBy(ce => ce.CreatedAt)
            };

            return await query
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();
        }

        // Crew Work Rest Hours methods
        public async Task<int> AddCrewWorkRestHoursAsync(CrewWorkRestHours workRestHours)
        {
            await _context.CrewWorkRestHours.AddAsync(workRestHours);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCrewWorkRestHoursAsync(CrewWorkRestHours workRestHours)
        {
            _context.CrewWorkRestHours.Update(workRestHours);
            return await _context.SaveChangesAsync();
        }

        public async Task<CrewWorkRestHours?> GetCrewWorkRestHoursByIdAsync(int id)
        {
            return await _context.CrewWorkRestHours
                .Include(cwrh => cwrh.User)
                .Include(cwrh => cwrh.Vessel)
                .Include(cwrh => cwrh.CreatedByUser)
                .FirstOrDefaultAsync(cwrh => cwrh.Id == id);
        }

        public async Task<List<CrewWorkRestHours>> SearchCrewWorkRestHoursAsync(CrewWorkRestHoursSearchDto searchDto)
        {
            var query = _context.CrewWorkRestHours
                .Include(cwrh => cwrh.User)
                .Include(cwrh => cwrh.Vessel)
                .Include(cwrh => cwrh.CreatedByUser)
                .AsQueryable();

            if (searchDto.UserId.HasValue)
                query = query.Where(cwrh => cwrh.UserId == searchDto.UserId.Value);

            if (searchDto.VesselId.HasValue)
                query = query.Where(cwrh => cwrh.VesselId == searchDto.VesselId.Value);

            if (searchDto.Year.HasValue)
                query = query.Where(cwrh => cwrh.Year == searchDto.Year.Value);
            if (searchDto.Month.HasValue)
                query = query.Where(cwrh => cwrh.Month == searchDto.Month.Value);
            if (searchDto.DayFrom.HasValue)
                query = query.Where(cwrh => cwrh.Day >= searchDto.DayFrom.Value);
            if (searchDto.DayTo.HasValue)
                query = query.Where(cwrh => cwrh.Day <= searchDto.DayTo.Value);
            if (searchDto.HourFrom.HasValue)
                query = query.Where(cwrh => cwrh.Hour >= searchDto.HourFrom.Value);
            if (searchDto.HourTo.HasValue)
                query = query.Where(cwrh => cwrh.Hour <= searchDto.HourTo.Value);

            // Apply sorting
            query = searchDto.SortBy.ToLower() switch
            {
                "year,month,day,hour" => searchDto.SortDescending ? 
                    query.OrderByDescending(cwrh => cwrh.Year)
                         .ThenByDescending(cwrh => cwrh.Month)
                         .ThenByDescending(cwrh => cwrh.Day)
                         .ThenByDescending(cwrh => cwrh.Hour)
                    :
                    query.OrderBy(cwrh => cwrh.Year)
                         .ThenBy(cwrh => cwrh.Month)
                         .ThenBy(cwrh => cwrh.Day)
                         .ThenBy(cwrh => cwrh.Hour),
                _ => searchDto.SortDescending ? query.OrderByDescending(cwrh => cwrh.CreatedAt) : query.OrderBy(cwrh => cwrh.CreatedAt)
            };

            return await query
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();
        }

        public async Task<List<CrewWorkRestHours>> GetCrewWorkRestHoursByUserAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.CrewWorkRestHours
                .Include(cwrh => cwrh.User)
                .Include(cwrh => cwrh.Vessel)
                .Include(cwrh => cwrh.CreatedByUser)
                .Where(cwrh => cwrh.UserId == userId);

            // keep legacy range support by mapping to Year/Month/Day
            if (fromDate.HasValue)
            {
                var fd = fromDate.Value;
                query = query.Where(cwrh => 
                    (cwrh.Year > fd.Year) ||
                    (cwrh.Year == fd.Year && cwrh.Month > fd.Month) ||
                    (cwrh.Year == fd.Year && cwrh.Month == fd.Month && cwrh.Day >= fd.Day));
            }

            if (toDate.HasValue)
            {
                var td = toDate.Value;
                query = query.Where(cwrh => 
                    (cwrh.Year < td.Year) ||
                    (cwrh.Year == td.Year && cwrh.Month < td.Month) ||
                    (cwrh.Year == td.Year && cwrh.Month == td.Month && cwrh.Day <= td.Day));
            }

            return await query
                .OrderByDescending(cwrh => cwrh.Year)
                .ThenByDescending(cwrh => cwrh.Month)
                .ThenByDescending(cwrh => cwrh.Day)
                .ThenByDescending(cwrh => cwrh.Hour)
                .ToListAsync();
        }

        // Aggregation helpers for CrewProfile
        public Task<User?> GetUserByIdAsync(int userId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<List<CrewMedicalRecord>> GetMedicalByUserAsync(int userId)
        {
            return _context.CrewMedicalRecords
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ExaminationDate)
                .ToListAsync();
        }

        public Task<List<CrewPassport>> GetPassportsByUserAsync(int userId)
        {
            return _context.CrewPassports
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ExpiryDate)
                .ToListAsync();
        }

        public Task<List<CrewVisa>> GetVisasByUserAsync(int userId)
        {
            return _context.CrewVisas
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ExpiryDate)
                .ToListAsync();
        }

        public Task<List<CrewReport>> GetReportsByUserAsync(int userId)
        {
            return _context.CrewReports
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.ReportDate)
                .ToListAsync();
        }

        public Task<List<Payroll>> GetPayrollsByUserAsync(int userId)
        {
            return _context.PayrollRecords
                .Where(p => p.CrewMemberId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public Task<List<ShipAssignment>> GetAssignmentsByUserAsync(int userId)
        {
            return _context.ShipAssignments
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }

        public Task<List<CrewTraining>> GetTrainingsByUserAsync(int userId)
        {
            return _context.CrewTrainings
                .Where(ct => ct.UserId == userId)
                .OrderByDescending(ct => ct.TrainingDate)
                .ToListAsync();
        }

        public Task<List<CrewEvaluation>> GetEvaluationsByUserAsync(int userId)
        {
            return _context.CrewEvaluations
                .Where(ce => ce.UserId == userId)
                .OrderByDescending(ce => ce.EnteredDate)
                .ToListAsync();
        }
    }

    //rest of the stuff goes here.

    
   
    
}