using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;
using ASCO.DTOs;
using ASCO.DTOs.Crew;

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
        public async Task<int> UpdateVesselManningAsync(int vesselId, List<UpdateVesselManningDto> manningUpdates)
        {
            var existingManning = await _context.VesselMannings
                .Where(vm => vm.VesselId == vesselId)
                .ToListAsync();

            foreach (var update in manningUpdates)
            {
                var existing = existingManning.FirstOrDefault(vm => vm.Rank == update.Rank);
                if (existing != null)
                {
                    existing.RequiredCount = update.RequiredCount;
                    existing.CurrentCount = update.CurrentCount;
                    existing.Notes = update.Notes;
                }
                else
                {
                    var newManning = new VesselManning
                    {
                        VesselId = vesselId,
                        Rank = update.Rank,
                        RequiredCount = update.RequiredCount,
                        CurrentCount = update.CurrentCount,
                        Notes = update.Notes
                    };
                    _context.VesselMannings.Add(newManning);
                }
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

        public async Task<int> GetCrewTrainingsCountAsync(CrewTrainingSearchDto searchDto)
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

            return await query.CountAsync();
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

        public async Task<int> GetCrewEvaluationsCountAsync(CrewEvaluationSearchDto searchDto)
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

            return await query.CountAsync();
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

        public Task<List<ShipAssignment>> GetCrewMembersByVesselAsync(int vesselId)
        {
            return _context.ShipAssignments
                .Include(a => a.User)
                .Where(a => a.ShipId == vesselId && a.Status == "active")
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }

        public async Task<List<UserDto>> GetAvailableCrewMembersAsync()
        {
            // Get all users who are not currently assigned to any vessel
            var availableUsers = await _context.Users
                .Where(u => u.Status == "active" && 
                           !_context.ShipAssignments.Any(sa => sa.UserId == u.Id && sa.Status == "active"))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name ?? "",
                    Surname = u.Surname ?? "",
                    Email = u.Email ?? "",
                    Rank = u.Rank ?? "",
                    Nationality = u.Nationality ?? "",
                    DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                    BirthPlace = u.BirthPlace,
                    Gender = u.Gender ?? "",
                    Status = u.Status ?? "",
                    JobType = u.JobType ?? "",
                    MaritalStatus = u.MaritalStatus,
                    MilitaryStatus = u.MilitaryStatus,
                    EducationLevel = u.EducationLevel,
                    GraduationYear = u.GraduationYear,
                    School = u.School,
                    Competency = u.Competency ?? "",
                    OrganizationUnit = u.OrganizationUnit,
                    FatherName = u.FatherName,
                    WorkEndDate = u.WorkEndDate,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = null, // User model doesn't have UpdatedAt
                    LastLoginAt = u.LastLoginAt,
                    EmailConfirmed = u.EmailConfirmed,
                    // Get passport info
                    PassportNumber = _context.CrewPassports
                        .Where(p => p.UserId == u.Id)
                        .OrderByDescending(p => p.ExpiryDate)
                        .Select(p => p.PassportNumber)
                        .FirstOrDefault(),
                    PassportExpiry = _context.CrewPassports
                        .Where(p => p.UserId == u.Id)
                        .OrderByDescending(p => p.ExpiryDate)
                        .Select(p => (DateTime?)p.ExpiryDate)
                        .FirstOrDefault()
                })
                .OrderBy(u => u.Name)
                .ToListAsync();

            return availableUsers;
        }

        public async Task<List<string>> GetAvailableRanksAsync()
        {
            // Get all ranks that are not currently assigned to any vessel
            var availableRanks = await _context.Users
                .Where(u => u.Status == "active" && u.Rank != null && u.Rank != "")
                .Select(u => u.Rank!)
                .Distinct()
                .Where(rank => !_context.VesselMannings.Any(vm => vm.Rank == rank))
                .OrderBy(rank => rank)
                .ToListAsync();

            return availableRanks;
        }

        public async Task<List<string>> GetAllPossibleRanksAsync()
        {
            // Get all distinct ranks from active users
            var allRanks = await _context.Users
                .Where(u => u.Status == "active" && u.Rank != null && u.Rank != "")
                .Select(u => u.Rank!)
                .Distinct()
                .OrderBy(rank => rank)
                .ToListAsync();

            return allRanks;
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


        //rest of the stuff goes here.

        // ===== USER MANAGEMENT METHODS =====

        public async Task<PagedResult<UserDto>> GetAllCrewMembersAsync(int page = 1, int pageSize = 20, string? status = null, string? jobType = null, string? rank = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(u => u.Status == status);
            if (!string.IsNullOrEmpty(jobType))
                query = query.Where(u => u.JobType == jobType);
            if (!string.IsNullOrEmpty(rank))
                query = query.Where(u => u.Rank == rank);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name ?? string.Empty,
                    Surname = u.Surname ?? string.Empty,
                    Nationality = u.Nationality ?? string.Empty,
                    IdenNumber = u.IdenNumber.ToString(),
                    DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                    BirthPlace = u.BirthPlace,
                    Gender = u.Gender ?? string.Empty,
                    Status = u.Status ?? string.Empty,
                    JobType = u.JobType ?? string.Empty,
                    Rank = u.Rank ?? string.Empty,
                    MaritalStatus = u.MaritalStatus,
                    MilitaryStatus = u.MilitaryStatus,
                    EducationLevel = u.EducationLevel,
                    GraduationYear = u.GraduationYear,
                    School = u.School,
                    Competency = u.Competency ?? string.Empty,
                    OrganizationUnit = u.OrganizationUnit,
                    Email = u.Email ?? string.Empty,
                    FatherName = u.FatherName,
                    WorkEndDate = u.WorkEndDate,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = null,
                    LastLoginAt = u.LastLoginAt,
                    EmailConfirmed = u.EmailConfirmed,
                    PassportNumber = _context.CrewPassports.Where(p => p.UserId == u.Id)
                                           .OrderByDescending(p => p.ExpiryDate)
                                           .Select(p => p.PassportNumber)
                                           .FirstOrDefault(),
                    PassportExpiry = _context.CrewPassports.Where(p => p.UserId == u.Id)
                                           .OrderByDescending(p => p.ExpiryDate)
                                           .Select(p => (DateTime?)p.ExpiryDate)
                                           .FirstOrDefault(),
                    WhereEmbarked = _context.ShipAssignments.Where(a => a.UserId == u.Id)
                                           .OrderByDescending(a => a.AssignedAt)
                                           .Select(a => a.Notes)
                                           .FirstOrDefault(),
                    WhenEmbarked = _context.ShipAssignments.Where(a => a.UserId == u.Id)
                                           .OrderByDescending(a => a.AssignedAt)
                                           .Select(a => a.AssignedAt)
                                           .FirstOrDefault()
                })
                .ToListAsync();

            return new PagedResult<UserDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<UserDto?> GetCrewMemberByIdAsync(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name ?? string.Empty,
                    Surname = u.Surname ?? string.Empty,
                    Nationality = u.Nationality ?? string.Empty,
                    IdenNumber = u.IdenNumber.ToString(),
                    DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                    BirthPlace = u.BirthPlace,
                    Gender = u.Gender ?? string.Empty,
                    Status = u.Status ?? string.Empty,
                    JobType = u.JobType ?? string.Empty,
                    Rank = u.Rank ?? string.Empty,
                    MaritalStatus = u.MaritalStatus,
                    MilitaryStatus = u.MilitaryStatus,
                    EducationLevel = u.EducationLevel,
                    GraduationYear = u.GraduationYear,
                    School = u.School,
                    Competency = u.Competency ?? string.Empty,
                    OrganizationUnit = u.OrganizationUnit,
                    Email = u.Email ?? string.Empty,
                    FatherName = u.FatherName,
                    WorkEndDate = u.WorkEndDate,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = null,
                    LastLoginAt = u.LastLoginAt,
                    EmailConfirmed = u.EmailConfirmed
                })
                .FirstOrDefaultAsync();

            return user;
        }

         public async Task<int> DeleteCrewMemberAsync(int id)
         {
             var user = await _context.Users.FindAsync(id);
             if (user == null) return 0;

             _context.Users.Remove(user);
             return await _context.SaveChangesAsync();
         }

         public async Task<PagedResult<UserDto>> SearchCrewMembersAsync(CrewSearchDto searchDto)
         {
             var query = _context.Users.AsQueryable();

             if (!string.IsNullOrEmpty(searchDto.Name))
                 query = query.Where(u => u.Name!.Contains(searchDto.Name));
             if (!string.IsNullOrEmpty(searchDto.Surname))
                 query = query.Where(u => u.Surname!.Contains(searchDto.Surname));
             if (!string.IsNullOrEmpty(searchDto.Nationality))
                 query = query.Where(u => u.Nationality == searchDto.Nationality);
             if (!string.IsNullOrEmpty(searchDto.Rank))
                 query = query.Where(u => u.Rank == searchDto.Rank);
             if (!string.IsNullOrEmpty(searchDto.JobType))
                 query = query.Where(u => u.JobType == searchDto.JobType);
             if (!string.IsNullOrEmpty(searchDto.Status))
                 query = query.Where(u => u.Status == searchDto.Status);
             if (!string.IsNullOrEmpty(searchDto.Email))
                 query = query.Where(u => u.Email!.Contains(searchDto.Email));
             if (searchDto.DateOfBirthFrom.HasValue)
                 query = query.Where(u => u.DateOfBirth >= searchDto.DateOfBirthFrom);
             if (searchDto.DateOfBirthTo.HasValue)
                 query = query.Where(u => u.DateOfBirth <= searchDto.DateOfBirthTo);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((searchDto.Page - 1) * searchDto.PageSize)
                 .Take(searchDto.PageSize)
                 .Select(u => new UserDto
                 {
                     Id = u.Id,
                     Name = u.Name ?? string.Empty,
                     Surname = u.Surname ?? string.Empty,
                     Nationality = u.Nationality ?? string.Empty,
                     IdenNumber = u.IdenNumber.ToString(),
                     DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                     BirthPlace = u.BirthPlace,
                     Gender = u.Gender ?? string.Empty,
                     Status = u.Status ?? string.Empty,
                     JobType = u.JobType ?? string.Empty,
                     Rank = u.Rank ?? string.Empty,
                     MaritalStatus = u.MaritalStatus,
                     MilitaryStatus = u.MilitaryStatus,
                     EducationLevel = u.EducationLevel,
                     GraduationYear = u.GraduationYear,
                     School = u.School,
                     Competency = u.Competency ?? string.Empty,
                     OrganizationUnit = u.OrganizationUnit,
                     Email = u.Email ?? string.Empty,
                     FatherName = u.FatherName,
                     WorkEndDate = u.WorkEndDate,
                     CreatedAt = u.CreatedAt,
                     UpdatedAt = null,
                     LastLoginAt = u.LastLoginAt,
                     EmailConfirmed = u.EmailConfirmed,
                     PassportNumber = _context.CrewPassports.Where(p => p.UserId == u.Id)
                                            .OrderByDescending(p => p.ExpiryDate)
                                            .Select(p => p.PassportNumber)
                                            .FirstOrDefault(),
                     PassportExpiry = _context.CrewPassports.Where(p => p.UserId == u.Id)
                                            .OrderByDescending(p => p.ExpiryDate)
                                            .Select(p => (DateTime?)p.ExpiryDate)
                                            .FirstOrDefault(),
                     WhereEmbarked = _context.ShipAssignments.Where(a => a.UserId == u.Id)
                                            .OrderByDescending(a => a.AssignedAt)
                                            .Select(a => a.Notes)
                                            .FirstOrDefault(),
                     WhenEmbarked = _context.ShipAssignments.Where(a => a.UserId == u.Id)
                                            .OrderByDescending(a => a.AssignedAt)
                                            .Select(a => a.AssignedAt)
                                            .FirstOrDefault()
                 })
                 .ToListAsync();

             return new PagedResult<UserDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = searchDto.Page,
                 PageSize = searchDto.PageSize
             };
         }

         public async Task<PagedResult<UserDto>> GetCrewMembersByRankAsync(string rank, int page = 1, int pageSize = 20)
         {
             var query = _context.Users.Where(u => u.Rank == rank);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(u => new UserDto
                 {
                     Id = u.Id,
                     Name = u.Name ?? string.Empty,
                     Surname = u.Surname ?? string.Empty,
                     Nationality = u.Nationality ?? string.Empty,
                     IdenNumber = u.IdenNumber.ToString(),
                     DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                     BirthPlace = u.BirthPlace,
                     Gender = u.Gender ?? string.Empty,
                     Status = u.Status ?? string.Empty,
                     JobType = u.JobType ?? string.Empty,
                     Rank = u.Rank ?? string.Empty,
                     MaritalStatus = u.MaritalStatus,
                     MilitaryStatus = u.MilitaryStatus,
                     EducationLevel = u.EducationLevel,
                     GraduationYear = u.GraduationYear,
                     School = u.School,
                     Competency = u.Competency ?? string.Empty,
                     OrganizationUnit = u.OrganizationUnit,
                     Email = u.Email ?? string.Empty,
                     FatherName = u.FatherName,
                     WorkEndDate = u.WorkEndDate,
                     CreatedAt = u.CreatedAt,
                     UpdatedAt = null,
                     LastLoginAt = u.LastLoginAt,
                     EmailConfirmed = u.EmailConfirmed
                 })
                 .ToListAsync();

             return new PagedResult<UserDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

         public async Task<PagedResult<UserDto>> GetCrewMembersByNationalityAsync(string nationality, int page = 1, int pageSize = 20)
         {
             var query = _context.Users.Where(u => u.Nationality == nationality);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(u => new UserDto
                 {
                     Id = u.Id,
                     Name = u.Name ?? string.Empty,
                     Surname = u.Surname ?? string.Empty,
                     Nationality = u.Nationality ?? string.Empty,
                     IdenNumber = u.IdenNumber.ToString(),
                     DateOfBirth = u.DateOfBirth ?? DateTime.MinValue,
                     BirthPlace = u.BirthPlace,
                     Gender = u.Gender ?? string.Empty,
                     Status = u.Status ?? string.Empty,
                     JobType = u.JobType ?? string.Empty,
                     Rank = u.Rank ?? string.Empty,
                     MaritalStatus = u.MaritalStatus,
                     MilitaryStatus = u.MilitaryStatus,
                     EducationLevel = u.EducationLevel,
                     GraduationYear = u.GraduationYear,
                     School = u.School,
                     Competency = u.Competency ?? string.Empty,
                     OrganizationUnit = u.OrganizationUnit,
                     Email = u.Email ?? string.Empty,
                     FatherName = u.FatherName,
                     WorkEndDate = u.WorkEndDate,
                     CreatedAt = u.CreatedAt,
                     UpdatedAt = null,
                     LastLoginAt = u.LastLoginAt,
                     EmailConfirmed = u.EmailConfirmed
                 })
                 .ToListAsync();

             return new PagedResult<UserDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

         public async Task<CrewStatisticsDto> GetCrewStatisticsAsync()
         {
             var totalCrew = await _context.Users.CountAsync();
             var activeCrew = await _context.Users.CountAsync(u => u.Status == "active");
             var inactiveCrew = await _context.Users.CountAsync(u => u.Status == "inactive");

             var crewByNationality = await _context.Users
                 .GroupBy(u => u.Nationality)
                 .Select(g => new { Nationality = g.Key ?? "Unknown", Count = g.Count() })
                 .ToDictionaryAsync(x => x.Nationality, x => x.Count);

             var crewByRank = await _context.Users
                 .GroupBy(u => u.Rank)
                 .Select(g => new { Rank = g.Key ?? "Unknown", Count = g.Count() })
                 .ToDictionaryAsync(x => x.Rank, x => x.Count);

             var crewByJobType = await _context.Users
                 .GroupBy(u => u.JobType)
                 .Select(g => new { JobType = g.Key ?? "Unknown", Count = g.Count() })
                 .ToDictionaryAsync(x => x.JobType, x => x.Count);

             var crewByStatus = await _context.Users
                 .GroupBy(u => u.Status)
                 .Select(g => new { Status = g.Key ?? "Unknown", Count = g.Count() })
                 .ToDictionaryAsync(x => x.Status, x => x.Count);

             var totalCertifications = await _context.CrewCertifications.CountAsync();
             var expiredCertifications = await _context.CrewCertifications
                 .CountAsync(c => c.ExpiryDate < DateTime.UtcNow);
             var expiringSoonCertifications = await _context.CrewCertifications
                 .CountAsync(c => (c.ExpiryDate - DateTime.UtcNow).TotalDays <= 30);

             var totalTrainings = await _context.CrewTrainings.CountAsync();
             var totalEvaluations = await _context.CrewEvaluations.CountAsync();
             var totalPayrollRecords = await _context.PayrollRecords.CountAsync();
             var totalExpenseReports = await _context.CrewExpenseReports.CountAsync();

             return new CrewStatisticsDto
             {
                 TotalCrewMembers = totalCrew,
                 ActiveCrewMembers = activeCrew,
                 InactiveCrewMembers = inactiveCrew,
                 CrewByNationality = crewByNationality,
                 CrewByRank = crewByRank,
                 CrewByJobType = crewByJobType,
                 CrewByStatus = crewByStatus,
                 TotalCertifications = totalCertifications,
                 ExpiredCertifications = expiredCertifications,
                 ExpiringSoonCertifications = expiringSoonCertifications,
                 TotalTrainings = totalTrainings,
                 TotalEvaluations = totalEvaluations,
                 TotalPayrollRecords = totalPayrollRecords,
                 TotalExpenseReports = totalExpenseReports
             };
         }

         // ===== CERTIFICATION METHODS =====
         
         public async Task<PagedResult<CrewCertificationDto>> GetCrewCertificationsAsync(int id, int page = 1, int pageSize = 20, string? status = null)
         {
             var query = _context.CrewCertifications
                 .Include(cc => cc.User)
                 .Where(cc => cc.UserId == id);

             if (!string.IsNullOrEmpty(status))
                 query = query.Where(cc => cc.Status == status);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(cc => new CrewCertificationDto
                 {
                     Id = cc.Id,
                     UserId = cc.UserId,
                     UserName = cc.User.Name ?? string.Empty,
                     UserSurname = cc.User.Surname ?? string.Empty,
                     CertificationType = cc.CertificationType,
                     CertificateNumber = cc.CertificateNumber,
                     IssuedBy = cc.IssuedBy,
                     IssuedDate = cc.IssuedDate,
                     ExpiryDate = cc.ExpiryDate,
                     Status = cc.Status,
                     Notes = cc.Notes,
                     CreatedAt = cc.CreatedAt,
                     UpdatedAt = cc.UpdatedAt
                 })
                 .ToListAsync();

             return new PagedResult<CrewCertificationDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

         public async Task<int> AddCrewCertificationAsync(CrewCertification certification)
         {
             _context.CrewCertifications.Add(certification);
             return await _context.SaveChangesAsync();
         }

         public async Task<CrewCertification?> GetCrewCertificationByIdAsync(int id)
         {
             return await _context.CrewCertifications.FindAsync(id);
         }

         public async Task<int> UpdateCrewCertificationAsync(CrewCertification certification)
         {
             _context.CrewCertifications.Update(certification);
             return await _context.SaveChangesAsync();
         }

         public async Task<int> DeleteCrewCertificationAsync(int id)
         {
             var certification = await _context.CrewCertifications.FindAsync(id);
             if (certification == null) return 0;

             _context.CrewCertifications.Remove(certification);
             return await _context.SaveChangesAsync();
         }

         // ===== PAYROLL METHODS =====
         
         public async Task<PagedResult<PayrollDto>> GetPayrollRecordsAsync(int crewMemberId, int page = 1, int pageSize = 20, string? period = null)
         {
             var query = _context.PayrollRecords
                 .Include(pr => pr.CrewMember)
                 .Where(pr => pr.CrewMemberId == crewMemberId);

             if (!string.IsNullOrEmpty(period))
                 query = query.Where(pr => pr.PeriodStart.ToString("yyyy-MM") == period);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(pr => new PayrollDto
                 {
                     Id = pr.Id,
                     UserId = pr.CrewMemberId,
                     UserName = pr.CrewMember.Name ?? string.Empty,
                     UserSurname = pr.CrewMember.Surname ?? string.Empty,
                     Period = pr.PeriodStart.ToString("yyyy-MM"),
                     BasicSalary = pr.BaseWage,
                     OvertimePay = pr.Overtime,
                     Allowances = pr.Bonuses,
                     Deductions = pr.Deductions,
                     NetSalary = pr.NetPay,
                     Currency = pr.Currency,
                     PaymentDate = pr.PaymentDate,
                     Status = "Paid", // Default status since it's not in the model
                     Notes = null, // Not in the model
                     CreatedAt = DateTime.UtcNow, // Not in the model
                     UpdatedAt = null // Not in the model
                 })
                 .ToListAsync();

             return new PagedResult<PayrollDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

         public async Task<Payroll?> GetPayrollRecordByIdAsync(int id)
         {
             return await _context.PayrollRecords.FindAsync(id);
         }

         public async Task<int> UpdatePayrollRecordAsync(Payroll payroll)
         {
             _context.PayrollRecords.Update(payroll);
             return await _context.SaveChangesAsync();
         }

         public async Task<VesselManning?> GetVesselManningByIdAsync(int id)
         {
             return await _context.VesselMannings.FindAsync(id);
         }

         // ===== ADDITIONAL VESSEL MANNING METHODS =====
         
         public async Task<PagedResult<VesselManningDTO>> GetAllVesselManningsAsync(int page = 1, int pageSize = 20, int? vesselId = null, string? rank = null)
         {
             var query = _context.VesselMannings
                 .Include(vm => vm.Vessel)
                 .AsQueryable();

             if (vesselId.HasValue)
                 query = query.Where(vm => vm.VesselId == vesselId.Value);
             if (!string.IsNullOrEmpty(rank))
                 query = query.Where(vm => vm.Rank == rank);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(vm => new VesselManningDTO
                 {
                     id = vm.Id,
                     VesselId = vm.VesselId,
                     Rank = new List<string> { vm.Rank },
                     count = vm.RequiredCount
                 })
                 .ToListAsync();

             return new PagedResult<VesselManningDTO>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

         public async Task<int> UpdateVesselManningAsync(VesselManning manning)
         {
             manning.UpdatedAt = DateTime.UtcNow;
             _context.VesselMannings.Update(manning);
             return await _context.SaveChangesAsync();
         }

         // ===== CASH STATEMENT METHODS =====
         
         public async Task<PagedResult<CashStatementDto>> GetCashStatementsAsync(int page = 1, int pageSize = 20, int? vesselId = null, DateTime? fromDate = null, DateTime? toDate = null)
         {
             var query = _context.CashStatements
                 .Include(cs => cs.Vessel)
                 .Include(cs => cs.CreatedBy)
                 .AsQueryable();

             if (vesselId.HasValue)
                 query = query.Where(cs => cs.VesselId == vesselId.Value);
             if (fromDate.HasValue)
                 query = query.Where(cs => cs.TransactionDate >= fromDate.Value);
             if (toDate.HasValue)
                 query = query.Where(cs => cs.TransactionDate <= toDate.Value);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(cs => new CashStatementDto
                 {
                     Id = cs.Id,
                     VesselId = cs.VesselId,
                     VesselName = cs.Vessel.Name,
                     CreatedById = cs.CreatedById,
                     CreatedByName = cs.CreatedBy.Name ?? string.Empty,
                     Status = cs.status ?? string.Empty,
                     TransactionDate = cs.TransactionDate,
                     Description = cs.Description ?? string.Empty,
                    Inflow = cs.Inflow ?? 0m,
                    Outflow = cs.Outflow ?? 0m,
                    Balance = cs.Balance,
                     CreatedAt = cs.CreatedAt,
                     UpdatedAt = cs.UpdatedAt
                 })
                 .ToListAsync();

             return new PagedResult<CashStatementDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

         public async Task<CashStatementDto?> GetCashStatementByIdAsync(int id)
         {
             var statement = await _context.CashStatements
                 .Include(cs => cs.Vessel)
                 .Include(cs => cs.CreatedBy)
                 .Where(cs => cs.Id == id)
                 .Select(cs => new CashStatementDto
                 {
                     Id = cs.Id,
                     VesselId = cs.VesselId,
                     VesselName = cs.Vessel.Name,
                     CreatedById = cs.CreatedById,
                     CreatedByName = cs.CreatedBy.Name ?? string.Empty,
                     Status = cs.status ?? string.Empty,
                     TransactionDate = cs.TransactionDate,
                     Description = cs.Description ?? string.Empty,
                    Inflow = cs.Inflow ?? 0m,
                    Outflow = cs.Outflow ?? 0m,
                    Balance = cs.Balance,
                     CreatedAt = cs.CreatedAt,
                     UpdatedAt = cs.UpdatedAt
                 })
                 .FirstOrDefaultAsync();

             return statement;
         }

         public async Task<int> UpdateCashStatementAsync(StatementOfCash statement)
         {
             statement.UpdatedAt = DateTime.UtcNow;
             _context.CashStatements.Update(statement);
             return await _context.SaveChangesAsync();
         }

         public async Task<int> DeleteCashStatementAsync(int id)
         {
             var statement = await _context.CashStatements.FindAsync(id);
             if (statement == null) return 0;

             _context.CashStatements.Remove(statement);
             return await _context.SaveChangesAsync();
         }

         // ===== EXPENSE REPORT METHODS =====
         
         public async Task<PagedResult<ExpenseReportDto>> GetExpenseReportsAsync(int page = 1, int pageSize = 20, int? crewMemberId = null, int? shipId = null, DateTime? fromDate = null, DateTime? toDate = null)
         {
             var query = _context.CrewExpenseReports
                 .Include(er => er.CrewMember)
                 .Include(er => er.Ship)
                 .AsQueryable();

             if (crewMemberId.HasValue)
                 query = query.Where(er => er.CrewMemberId == crewMemberId.Value);
             if (shipId.HasValue)
                 query = query.Where(er => er.ShipId == shipId.Value);
             if (fromDate.HasValue)
                 query = query.Where(er => er.ReportDate >= fromDate.Value);
             if (toDate.HasValue)
                 query = query.Where(er => er.ReportDate <= toDate.Value);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(er => new ExpenseReportDto
                 {
                     Id = (int)er.Id,
                     CrewMemberId = er.CrewMemberId,
                     CrewMemberName = er.CrewMember.Name ?? string.Empty,
                     ShipId = er.ShipId,
                     ShipName = er.Ship.Name,
                     TotalAmount = er.TotalAmount,
                     Currency = er.Currency,
                     ReportDate = er.ReportDate,
                     Notes = er.Notes,
                     CreatedAt = er.CreatedAt,
                     UpdatedAt = er.UpdatedAt
                 })
                 .ToListAsync();

             return new PagedResult<ExpenseReportDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

         public async Task<ExpenseReportDto?> GetExpenseReportByIdAsync(int id)
         {
             var report = await _context.CrewExpenseReports
                 .Include(er => er.CrewMember)
                 .Include(er => er.Ship)
                 .Where(er => er.Id == id)
                 .Select(er => new ExpenseReportDto
                 {
                     Id = (int)er.Id,
                     CrewMemberId = er.CrewMemberId,
                     CrewMemberName = er.CrewMember.Name ?? string.Empty,
                     ShipId = er.ShipId,
                     ShipName = er.Ship.Name,
                     TotalAmount = er.TotalAmount,
                     Currency = er.Currency,
                     ReportDate = er.ReportDate,
                     Notes = er.Notes,
                     CreatedAt = er.CreatedAt,
                     UpdatedAt = er.UpdatedAt
                 })
                 .FirstOrDefaultAsync();

             return report;
         }

         public async Task<int> UpdateExpenseReportAsync(CrewExpenseReport report)
         {
             report.UpdatedAt = DateTime.UtcNow;
             _context.CrewExpenseReports.Update(report);
             return await _context.SaveChangesAsync();
         }

         public async Task<int> DeleteExpenseReportAsync(int id)
         {
             var report = await _context.CrewExpenseReports.FindAsync(id);
             if (report == null) return 0;

             _context.CrewExpenseReports.Remove(report);
             return await _context.SaveChangesAsync();
         }

         // ===== ADDITIONAL PAYROLL METHODS =====
         
         public async Task<PagedResult<PayrollDto>> GetAllPayrollRecordsAsync(int page = 1, int pageSize = 20, int? crewMemberId = null, DateTime? fromDate = null, DateTime? toDate = null)
         {
             var query = _context.PayrollRecords
                 .Include(pr => pr.CrewMember)
                 .AsQueryable();

             if (crewMemberId.HasValue)
                 query = query.Where(pr => pr.CrewMemberId == crewMemberId.Value);
             if (fromDate.HasValue)
                 query = query.Where(pr => pr.PaymentDate >= fromDate.Value);
             if (toDate.HasValue)
                 query = query.Where(pr => pr.PaymentDate <= toDate.Value);

             var totalCount = await query.CountAsync();
             var items = await query
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(pr => new PayrollDto
                 {
                     Id = pr.Id,
                     UserId = pr.CrewMemberId,
                     UserName = pr.CrewMember.Name ?? string.Empty,
                     UserSurname = pr.CrewMember.Surname ?? string.Empty,
                     Period = pr.PeriodStart.ToString("yyyy-MM"),
                     BasicSalary = pr.BaseWage,
                     OvertimePay = pr.Overtime,
                     Allowances = pr.Bonuses,
                     Deductions = pr.Deductions,
                     NetSalary = pr.NetPay,
                     Currency = pr.Currency,
                     PaymentDate = pr.PaymentDate,
                     Status = "Paid",
                     Notes = null,
                     CreatedAt = DateTime.UtcNow,
                     UpdatedAt = null
                 })
                 .ToListAsync();

             return new PagedResult<PayrollDto>
             {
                 Items = items,
                 TotalCount = totalCount,
                 Page = page,
                 PageSize = pageSize
             };
         }

        public async Task<PayrollDto?> GetPayrollRecordDtoByIdAsync(int id)
        {
            var payroll = await _context.PayrollRecords
                .Include(pr => pr.CrewMember)
                .Where(pr => pr.Id == id)
                .Select(pr => new PayrollDto
                {
                    Id = pr.Id,
                    UserId = pr.CrewMemberId,
                    UserName = pr.CrewMember.Name ?? string.Empty,
                    UserSurname = pr.CrewMember.Surname ?? string.Empty,
                    Period = pr.PeriodStart.ToString("yyyy-MM"),
                    BasicSalary = pr.BaseWage,
                    OvertimePay = pr.Overtime,
                    Allowances = pr.Bonuses,
                    Deductions = pr.Deductions,
                    NetSalary = pr.NetPay,
                    Currency = pr.Currency,
                    PaymentDate = pr.PaymentDate,
                    Status = "Paid",
                    Notes = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                })
                .FirstOrDefaultAsync();

            return payroll;
        }

        public async Task<int> UpdatePayrollRecordEntityAsync(Payroll payroll)
        {
            _context.PayrollRecords.Update(payroll);
            return await _context.SaveChangesAsync();
        }

         public async Task<int> DeletePayrollRecordAsync(int id)
         {
             var payroll = await _context.PayrollRecords.FindAsync(id);
             if (payroll == null) return 0;

             _context.PayrollRecords.Remove(payroll);
             return await _context.SaveChangesAsync();
         }

        // Bulk delete methods for profile updates
        public async Task<int> DeletePassportsByUserIdAsync(int userId)
        {
            var passports = await _context.CrewPassports.Where(p => p.UserId == userId).ToListAsync();
            _context.CrewPassports.RemoveRange(passports);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteVisasByUserIdAsync(int userId)
        {
            var visas = await _context.CrewVisas.Where(v => v.UserId == userId).ToListAsync();
            _context.CrewVisas.RemoveRange(visas);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMedicalByUserIdAsync(int userId)
        {
            var medical = await _context.CrewMedicalRecords.Where(m => m.UserId == userId).ToListAsync();
            _context.CrewMedicalRecords.RemoveRange(medical);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteReportsByUserIdAsync(int userId)
        {
            var reports = await _context.CrewReports.Where(r => r.UserId == userId).ToListAsync();
            _context.CrewReports.RemoveRange(reports);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeletePayrollsByUserIdAsync(int userId)
        {
            var payrolls = await _context.PayrollRecords.Where(p => p.CrewMemberId == userId).ToListAsync();
            _context.PayrollRecords.RemoveRange(payrolls);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteTrainingsByUserIdAsync(int userId)
        {
            var trainings = await _context.CrewTrainings.Where(t => t.UserId == userId).ToListAsync();
            _context.CrewTrainings.RemoveRange(trainings);
            return await _context.SaveChangesAsync();
        }
     }   
}