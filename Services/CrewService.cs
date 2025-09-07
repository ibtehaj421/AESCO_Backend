using ASCO.DTOs;
using ASCO.Models;
using ASCO.Repositories;
using ASCP.Repositories;


namespace ASCO.Services
{
    public class CrewService
    {
        private readonly CrewRepository _crewRepository;
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;

        public CrewService(CrewRepository crewRepository, UserRepository userRepository, UserService userService)
        {
            _crewRepository = crewRepository;
            _userRepository = userRepository;
            _userService = userService;
        }


        //updations
        public async Task<int> UpdateCrewAsync(UpdateUserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userDto.Id);
            //store all values in a user object
            if (user != null)
            {
                // Basic Information - stored directly in Users table
                user.Name = userDto.Name;
                user.Surname = userDto.Surname;
                user.Nationality = userDto.Nationality;
                user.IdenNumber = Convert.ToInt64(userDto.IdenNumber);
                user.DateOfBirth = userDto.DateOfBirth;
                user.BirthPlace = userDto.BirthPlace;
                user.Gender = userDto.Gender;

                //employment details
                user.JobType = userDto.JobType;
                user.Rank = userDto.Rank;
                user.MaritalStatus = userDto.MaritalStatus;
                user.MilitaryStatus = userDto.MilitaryStatus;

                //education details
                user.EducationLevel = userDto.EducationLevel;
                user.GraduationYear = Convert.ToInt32(userDto.GraduationYear);
                user.School = userDto.School;

                //professsional details
                user.Competency = userDto.Competency;
                user.OrganizationUnit = userDto.OrganizationUnit;

                //contact details
                user.Email = userDto.Email;
                //PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password), // Hash the password before

                //family details
                user.FatherName = userDto.FatherName;

                //Status and timestamps
                user.Status = userDto.Status ?? "pending"; // Default status or admin assigned
                user.CreatedAt = DateTime.UtcNow;
                user.WorkEndDate = userDto.WorkEndDate;

                //security logs
                //FailedLoginAttempts = 0,
                // = false

            }
            ;
            int value = await _crewRepository.UpdateCrewAsync(user!);
            return value;
        }

        //assign a crew member to a vessel.
        public async Task<int> AssignCrewToVesselAsync(AssignmentDTO assignDto)
        {
            // Check if the crew member exists
            var crewMember = await _userRepository.GetUserByIdAsync(assignDto.CrewId);
            if (crewMember == null)
            {
                return 0; // Crew member not found
            }

            // Create a new assignment
            var assignment = new ShipAssignment
            {
                ShipId = assignDto.VesselId,
                UserId = assignDto.CrewId,
                Position = crewMember.Rank ?? "Crew Member", // Default position if rank is null
                AssignedAt = DateTime.UtcNow,
                UnassignedAt = assignDto.EndDate ?? null, //no value is assigned yet.
                Status = "active", // Default status
                AssignedByUserId = assignDto.AssignedByUserId,
                Notes = assignDto.Notes
            };

            int result = await _crewRepository.AssignCrewToVesselAsync(assignment);
            return result;
        }

        //unassing a crew member from a vessel.
        public async Task<int> UnassignCrewFromVesselAsync(int id, DateTime unassignDto)
        {
            // Find the active assignment for the crew member
            var activeAssignment = await _crewRepository.GetActiveAssignmentById(id);
            if (activeAssignment == null)
            {
                Console.WriteLine("No active assignment found for the given ID.");
                return 0; // No active assignment found
            }

            // Update the assignment to mark it as unassigned
            activeAssignment.UnassignedAt = unassignDto;
            activeAssignment.Status = "active";
            if (activeAssignment.UnassignedAt > DateTime.UtcNow)
            {
                activeAssignment.Status = "inactive"; // Set to provided end date if it's earlier
            }
            //activeAssignment.Status = "inactive"; // Mark as inactive

            int result = await _crewRepository.UnassignCrewFromVesselAsync(activeAssignment);
            return result;
        }

        //update assignment details - end date and notes, contains more info
        public async Task<int> UpdateAssignmentDetailsAsync(AssignmentDTO updateDto)
        {
            // Find the active assignment for the crew member
            var activeAssignment = await _crewRepository.GetActiveAssignmentById(updateDto.id);
            if (activeAssignment == null)
            {
                return 0; // No active assignment found
            }

            // Update the assignment details
            activeAssignment.AssignedAt = updateDto.AssignmentDate ?? activeAssignment.AssignedAt;
            activeAssignment.UnassignedAt = updateDto.EndDate ?? activeAssignment.UnassignedAt;
            activeAssignment.Notes = updateDto.Notes ?? activeAssignment.Notes;
            if (activeAssignment.UnassignedAt.HasValue && activeAssignment.UnassignedAt <= DateTime.UtcNow)
            {
                activeAssignment.Status = "inactive"; // Set to inactive if end date has passed
            }
            else
            {
                activeAssignment.Status = "active"; // Otherwise, keep it active
            }


            int result = await _crewRepository.UnassignCrewFromVesselAsync(activeAssignment);
            return result;
        }

        // crew medical
        public async Task<int> CreateMedicalAsync(CreateCrewMedicalDto dto)
        {
            var rec = new CrewMedicalRecord
            {
                UserId = dto.UserId,
                ProviderName = dto.ProviderName,
                BloodGroup = dto.BloodGroup,
                ExaminationDate = dto.ExaminationDate,
                ExpiryDate = dto.ExpiryDate,
                Notes = dto.Notes
            };
            return await _crewRepository.AddMedicalAsync(rec);
        }

        public async Task<int> UpdateMedicalAsync(CrewMedicalDto dto)
        {
            var existing = await _crewRepository.GetMedicalByIdAsync(dto.Id);
            if (existing == null) return 0;
            existing.ProviderName = dto.ProviderName;
            existing.BloodGroup = dto.BloodGroup;
            existing.ExaminationDate = dto.ExaminationDate;
            existing.ExpiryDate = dto.ExpiryDate;
            existing.Notes = dto.Notes;
            existing.UpdatedAt = DateTime.UtcNow;
            return await _crewRepository.UpdateMedicalAsync(existing);
        }

        // passports
        public async Task<int> CreatePassportAsync(CreateCrewPassportDto dto)
        {
            var rec = new CrewPassport
            {
                UserId = dto.UserId,
                PassportNumber = dto.PassportNumber,
                Nationality = dto.Nationality,
                IssueDate = dto.IssueDate,
                ExpiryDate = dto.ExpiryDate,
                IssuedBy = dto.IssuedBy,
                Notes = dto.Notes
            };
            return await _crewRepository.AddPassportAsync(rec);
        }

        public async Task<int> UpdatePassportAsync(CrewPassportDto dto)
        {
            var existing = await _crewRepository.GetPassportByIdAsync(dto.Id);
            if (existing == null) return 0;
            existing.PassportNumber = dto.PassportNumber;
            existing.Nationality = dto.Nationality;
            existing.IssueDate = dto.IssueDate;
            existing.ExpiryDate = dto.ExpiryDate;
            existing.IssuedBy = dto.IssuedBy;
            existing.Notes = dto.Notes;
            existing.UpdatedAt = DateTime.UtcNow;
            return await _crewRepository.UpdatePassportAsync(existing);
        }

        // visas
        public async Task<int> CreateVisaAsync(CreateCrewVisaDto dto)
        {
            var rec = new CrewVisa
            {
                UserId = dto.UserId,
                VisaType = dto.VisaType,
                Country = dto.Country,
                IssueDate = dto.IssueDate,
                ExpiryDate = dto.ExpiryDate,
                IssuedBy = dto.IssuedBy,
                Notes = dto.Notes
            };
            return await _crewRepository.AddVisaAsync(rec);
        }

        public async Task<int> UpdateVisaAsync(CrewVisaDto dto)
        {
            var existing = await _crewRepository.GetVisaByIdAsync(dto.Id);
            if (existing == null) return 0;
            existing.VisaType = dto.VisaType;
            existing.Country = dto.Country;
            existing.IssueDate = dto.IssueDate;
            existing.ExpiryDate = dto.ExpiryDate;
            existing.IssuedBy = dto.IssuedBy;
            existing.Notes = dto.Notes;
            existing.UpdatedAt = DateTime.UtcNow;
            return await _crewRepository.UpdateVisaAsync(existing);
        }

        // reports
        public async Task<int> CreateCrewReportAsync(CreateCrewReportDto dto)
        {
            var rec = new CrewReport
            {
                UserId = dto.UserId,
                ReportType = dto.ReportType,
                Title = dto.Title,
                Details = dto.Details,
                ReportDate = dto.ReportDate ?? DateTime.UtcNow
            };
            return await _crewRepository.AddCrewReportAsync(rec);
        }

        public async Task<int> UpdateCrewReportAsync(CrewReportDto dto)
        {
            var existing = await _crewRepository.GetCrewReportByIdAsync(dto.Id);
            if (existing == null) return 0;
            existing.ReportType = dto.ReportType;
            existing.Title = dto.Title;
            existing.Details = dto.Details;
            existing.ReportDate = dto.ReportDate ?? existing.ReportDate;
            existing.UpdatedAt = DateTime.UtcNow;
            return await _crewRepository.UpdateCrewReportAsync(existing);
        }

        // searches
        public Task<List<CrewMedicalRecord>> SearchMedicalAsync(CrewDocumentSearchDto s) => _crewRepository.SearchMedicalAsync(s);
        public Task<List<CrewPassport>> SearchPassportsAsync(CrewDocumentSearchDto s) => _crewRepository.SearchPassportsAsync(s);
        public Task<List<CrewVisa>> SearchVisasAsync(CrewDocumentSearchDto s) => _crewRepository.SearchVisasAsync(s);
        public Task<List<CrewReport>> SearchCrewReportsAsync(int? userId, string? reportType, int page, int pageSize) => _crewRepository.SearchCrewReportsAsync(userId, reportType, page, pageSize);

        public Task<List<ShipAssignment>> GetAssignmentHistoryAsync(int userId) => _crewRepository.GetAssignmentHistoryAsync(userId);
        //vessel manning properties.

        public async Task<int> AddVesselManningAsync(VesselManningDTO vm)
        {
            List<VesselManning> addManning = new List<VesselManning>();
            //for each rank in the list, add a new VesselManning entry
            foreach (string rank in vm.Rank)
            {
                var newManning = new VesselManning
                {
                    VesselId = vm.VesselId,
                    Rank = rank
                };
                addManning.Add(newManning);
            }
            var val = await _crewRepository.AddVesselManningAsync(addManning);
            return val; //if the value is positive, the manning was added successfully.
        }

        public async Task<int> RemoveVesselManningAsync(VesselManningDeleteDTO vessel)
        {
            var val = await _crewRepository.DeleteVesselManningAsync(vessel.VesselId, vessel.Rank);
            return val; //if the value
        }
        public Task<List<VesselManning>> GetVesselManningAsync(int vesselId) => _crewRepository.GetVesselManningAsync(vesselId);

        //payroll records
        public async Task<int> AddPayrollRecordAsync(PayrollDTO dto)
        {
            var rec = new Payroll
            {
                CrewMemberId = dto.CrewMemberId,
                PeriodStart = dto.PeriodStart,
                PeriodEnd = dto.PeriodEnd,
                BaseWage = dto.BaseWage,
                Overtime = dto.Overtime,
                Bonuses = dto.Bonuses,
                Deductions = dto.Deductions,
                Currency = dto.Currency,
                PaymentDate = dto.PaymentDate,
                PaymentMethod = dto.PaymentMethod

            };
            return await _crewRepository.AddPayrollRecordAsync(rec);
        }

        public async Task<List<Payroll>> GetPayrollByCrewAsync(int crewId)
        {
            return await _crewRepository.GetPayrollByCrewAsync(crewId);
        }

        //expense records
        public async Task<int> AddCrewExpenseAsync(CreateCrewExpenseDto expense)
        {
            // Calculate AmountInBaseCurrency
            CrewExpense exp = new CrewExpense
            {
                ExpenseReportId = expense.ExpenseReportId,
                CrewMemberId = expense.CrewMemberId,
                Category = expense.Category,
                Amount = expense.Amount,
                Currency = expense.Currency,
                ExpenseDate = expense.ExpenseDate ?? DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = expense.UpdatedAt
            };

            return await _crewRepository.AddCrewExpenseAsync(exp);
        }

        //cash statement
        public async Task<int> AddCashStatementAsync(StatementOfCashCreateDto dto)
        {
            var rec = new StatementOfCash
            {
                VesselId = dto.VesselId,
                CreatedById = dto.CreatedById,
                TransactionDate = dto.TransactionDate,
                status = dto.status ?? "pending",
                CreatedAt = DateTime.UtcNow
            };
            return await _crewRepository.AddCashStatementAsync(rec);
        }

        // Crew Training methods
        public async Task<int> CreateCrewTrainingAsync(CreateCrewTrainingDto dto)
        {
            var training = new CrewTraining
            {
                UserId = dto.UserId,
                VesselId = dto.VesselId,
                TrainingCategory = dto.TrainingCategory,
                Rank = dto.Rank,
                Trainer = dto.Trainer,
                Remark = dto.Remark,
                Training = dto.Training,
                Source = dto.Source,
                TrainingDate = dto.TrainingDate,
                ExpireDate = dto.ExpireDate,
                Status = dto.Status,
                Attachments = dto.Attachments,
                CreatedByUserId = 1, // TODO: Get from current user context
                CreatedAt = DateTime.UtcNow
            };
            return await _crewRepository.AddCrewTrainingAsync(training);
        }

        public async Task<int> UpdateCrewTrainingAsync(UpdateCrewTrainingDto dto)
        {
            var existing = await _crewRepository.GetCrewTrainingByIdAsync(dto.Id);
            if (existing == null) return 0;

            existing.UserId = dto.UserId;
            existing.VesselId = dto.VesselId;
            existing.TrainingCategory = dto.TrainingCategory;
            existing.Rank = dto.Rank;
            existing.Trainer = dto.Trainer;
            existing.Remark = dto.Remark;
            existing.Training = dto.Training;
            existing.Source = dto.Source;
            existing.TrainingDate = dto.TrainingDate;
            existing.ExpireDate = dto.ExpireDate;
            existing.Status = dto.Status;
            existing.Attachments = dto.Attachments;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _crewRepository.UpdateCrewTrainingAsync(existing);
        }

        public Task<List<CrewTraining>> SearchCrewTrainingsAsync(CrewTrainingSearchDto searchDto) => 
            _crewRepository.SearchCrewTrainingsAsync(searchDto);

        // Crew Evaluation methods
        public async Task<int> CreateCrewEvaluationAsync(CreateCrewEvaluationDto dto)
        {
            var evaluation = new CrewEvaluation
            {
                UserId = dto.UserId,
                VesselId = dto.VesselId,
                FormNo = dto.FormNo,
                RevisionNo = dto.RevisionNo,
                RevisionDate = dto.RevisionDate,
                FormName = dto.FormName,
                FormDescription = dto.FormDescription,
                EnteredByUserId = dto.EnteredByUserId,
                EnteredDate = dto.EnteredDate,
                Rank = dto.Rank,
                Name = dto.Name,
                Surname = dto.Surname,
                UniqueId = dto.UniqueId,
                TechnicalCompetence = dto.TechnicalCompetence,
                SafetyAwareness = dto.SafetyAwareness,
                Teamwork = dto.Teamwork,
                Communication = dto.Communication,
                Leadership = dto.Leadership,
                ProblemSolving = dto.ProblemSolving,
                Adaptability = dto.Adaptability,
                WorkEthic = dto.WorkEthic,
                OverallRating = dto.OverallRating,
                Strengths = dto.Strengths,
                AreasForImprovement = dto.AreasForImprovement,
                Comments = dto.Comments,
                CrewMemberComments = dto.CrewMemberComments,
                CrewMemberSignature = dto.CrewMemberSignature,
                CrewMemberSignedDate = dto.CrewMemberSignedDate,
                Status = dto.Status,
                Attachments = dto.Attachments,
                CreatedByUserId = 1, // TODO: Get from current user context
                CreatedAt = DateTime.UtcNow
            };
            return await _crewRepository.AddCrewEvaluationAsync(evaluation);
        }

        public async Task<int> UpdateCrewEvaluationAsync(UpdateCrewEvaluationDto dto)
        {
            var existing = await _crewRepository.GetCrewEvaluationByIdAsync(dto.Id);
            if (existing == null) return 0;

            existing.UserId = dto.UserId;
            existing.VesselId = dto.VesselId;
            existing.FormNo = dto.FormNo;
            existing.RevisionNo = dto.RevisionNo;
            existing.RevisionDate = dto.RevisionDate;
            existing.FormName = dto.FormName;
            existing.FormDescription = dto.FormDescription;
            existing.EnteredByUserId = dto.EnteredByUserId;
            existing.EnteredDate = dto.EnteredDate;
            existing.Rank = dto.Rank;
            existing.Name = dto.Name;
            existing.Surname = dto.Surname;
            existing.UniqueId = dto.UniqueId;
            existing.TechnicalCompetence = dto.TechnicalCompetence;
            existing.SafetyAwareness = dto.SafetyAwareness;
            existing.Teamwork = dto.Teamwork;
            existing.Communication = dto.Communication;
            existing.Leadership = dto.Leadership;
            existing.ProblemSolving = dto.ProblemSolving;
            existing.Adaptability = dto.Adaptability;
            existing.WorkEthic = dto.WorkEthic;
            existing.OverallRating = dto.OverallRating;
            existing.Strengths = dto.Strengths;
            existing.AreasForImprovement = dto.AreasForImprovement;
            existing.Comments = dto.Comments;
            existing.CrewMemberComments = dto.CrewMemberComments;
            existing.CrewMemberSignature = dto.CrewMemberSignature;
            existing.CrewMemberSignedDate = dto.CrewMemberSignedDate;
            existing.Status = dto.Status;
            existing.Attachments = dto.Attachments;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _crewRepository.UpdateCrewEvaluationAsync(existing);
        }

        public Task<List<CrewEvaluation>> SearchCrewEvaluationsAsync(CrewEvaluationSearchDto searchDto) => 
            _crewRepository.SearchCrewEvaluationsAsync(searchDto);

        // Crew Work Rest Hours methods
        public async Task<int> CreateCrewWorkRestHoursAsync(CreateCrewWorkRestHoursDto dto)
        {
            var workRestHours = new CrewWorkRestHours
            {
                UserId = dto.UserId,
                VesselId = dto.VesselId,
                Date = dto.Date,
                WorkHours = dto.WorkHours,
                RestHours = dto.RestHours,
                TotalHours = dto.TotalHours,
                WorkDescription = dto.WorkDescription,
                RestDescription = dto.RestDescription,
                Notes = dto.Notes,
                CreatedByUserId = 1, // TODO: Get from current user context
                CreatedAt = DateTime.UtcNow
            };
            return await _crewRepository.AddCrewWorkRestHoursAsync(workRestHours);
        }

        public async Task<int> UpdateCrewWorkRestHoursAsync(UpdateCrewWorkRestHoursDto dto)
        {
            var existing = await _crewRepository.GetCrewWorkRestHoursByIdAsync(dto.Id);
            if (existing == null) return 0;

            existing.UserId = dto.UserId;
            existing.VesselId = dto.VesselId;
            existing.Date = dto.Date;
            existing.WorkHours = dto.WorkHours;
            existing.RestHours = dto.RestHours;
            existing.TotalHours = dto.TotalHours;
            existing.WorkDescription = dto.WorkDescription;
            existing.RestDescription = dto.RestDescription;
            existing.Notes = dto.Notes;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _crewRepository.UpdateCrewWorkRestHoursAsync(existing);
        }

        public Task<List<CrewWorkRestHours>> SearchCrewWorkRestHoursAsync(CrewWorkRestHoursSearchDto searchDto) => 
            _crewRepository.SearchCrewWorkRestHoursAsync(searchDto);

        public Task<List<CrewWorkRestHours>> GetCrewWorkRestHoursByUserAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null) => 
            _crewRepository.GetCrewWorkRestHoursByUserAsync(userId, fromDate, toDate);

        // Aggregated crew profile
        public async Task<CrewProfileDto?> GetCrewProfileAsync(int userId)
        {
            var user = await _crewRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            var profile = new CrewProfileDto
            {
                PersonalInfo = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name ?? string.Empty,
                    Surname = user.Surname ?? string.Empty,
                    Nationality = user.Nationality ?? string.Empty,
                    IdenNumber = user.IdenNumber.ToString(),
                    DateOfBirth = user.DateOfBirth ?? DateTime.MinValue,
                    BirthPlace = user.BirthPlace,
                    Gender = user.Gender ?? string.Empty,
                    Status = user.Status ?? string.Empty,
                    JobType = user.JobType ?? string.Empty,
                    Rank = user.Rank ?? string.Empty,
                    MaritalStatus = user.MaritalStatus,
                    MilitaryStatus = user.MilitaryStatus,
                    EducationLevel = user.EducationLevel,
                    GraduationYear = user.GraduationYear,
                    School = user.School,
                    Competency = user.Competency ?? string.Empty,
                    OrganizationUnit = user.OrganizationUnit,
                    Email = user.Email ?? string.Empty,
                    FatherName = user.FatherName,
                    WorkEndDate = user.WorkEndDate,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = null,
                    LastLoginAt = user.LastLoginAt,
                    EmailConfirmed = user.EmailConfirmed
                }
            };

            // parallel fetch
            var medicalTask = _crewRepository.GetMedicalByUserAsync(userId);
            var passportTask = _crewRepository.GetPassportsByUserAsync(userId);
            var visaTask = _crewRepository.GetVisasByUserAsync(userId);
            var reportTask = _crewRepository.GetReportsByUserAsync(userId);
            var payrollTask = _crewRepository.GetPayrollsByUserAsync(userId);
            var expenseTask = _crewRepository.GetExpensesByCrewAsync(userId);
            var assignmentTask = _crewRepository.GetAssignmentsByUserAsync(userId);
            var trainingTask = _crewRepository.GetTrainingsByUserAsync(userId);
            var evaluationTask = _crewRepository.GetEvaluationsByUserAsync(userId);

            await Task.WhenAll(medicalTask, passportTask, visaTask, reportTask, payrollTask, expenseTask, assignmentTask, trainingTask, evaluationTask);

            profile.MedicalRecords = medicalTask.Result;
            profile.Passports = passportTask.Result;
            profile.Visas = visaTask.Result;
            profile.Reports = reportTask.Result;
            profile.Payrolls = payrollTask.Result;
            profile.Expenses = expenseTask.Result;
            profile.AssignmentHistory = assignmentTask.Result;

            // map trainings to DTO projection with minimal fields already defined
            profile.Trainings = trainingTask.Result.Select(ct => new CrewTrainingDto
            {
                Id = ct.Id,
                UserId = ct.UserId,
                UserName = user.Name ?? string.Empty,
                UserSurname = user.Surname ?? string.Empty,
                VesselId = ct.VesselId,
                VesselName = string.Empty,
                TrainingCategory = ct.TrainingCategory,
                Rank = ct.Rank,
                Trainer = ct.Trainer,
                Remark = ct.Remark,
                Training = ct.Training,
                Source = ct.Source,
                TrainingDate = ct.TrainingDate,
                ExpireDate = ct.ExpireDate,
                Status = ct.Status,
                Attachments = ct.Attachments,
                CreatedByName = string.Empty,
                CreatedAt = ct.CreatedAt,
                UpdatedAt = ct.UpdatedAt,
                IsExpiringSoon = ct.ExpireDate.HasValue && (ct.ExpireDate.Value - DateTime.UtcNow).TotalDays <= 30,
                DaysUntilExpiry = ct.ExpireDate.HasValue ? (int)(ct.ExpireDate.Value - DateTime.UtcNow).TotalDays : int.MaxValue
            }).ToList();

            profile.Evaluations = evaluationTask.Result;

            return profile;
        }

        // Create an aggregated crew profile
        public async Task<CreateCrewProfileResultDto> CreateCrewProfileAsync(CreateCrewProfileDto request)
        {
            // 1) Create user via existing user service to preserve hashing/roles
            var registerResp = await _userService.RegisterUserAsync(request.PersonalInfo);

            // Retrieve created user id by email
            var userIdLong = await _userRepository.GetUserID(request.PersonalInfo.Email);
            if (userIdLong <= 0)
            {
                throw new InvalidOperationException("User creation failed.");
            }
            var userId = (int)userIdLong;

            int createdPassports = 0, createdVisas = 0, createdMedical = 0, createdReports = 0,
                createdPayrolls = 0, createdExpenses = 0, createdAssignments = 0, createdTrainings = 0, createdEvaluations = 0;

            // 2) Passports
            if (request.Passports != null)
            {
                foreach (var p in request.Passports)
                {
                    var rec = new CrewPassport
                    {
                        UserId = userId,
                        PassportNumber = p.PassportNumber,
                        Nationality = p.Nationality,
                        IssueDate = p.IssueDate,
                        ExpiryDate = p.ExpiryDate,
                        IssuedBy = p.IssuedBy,
                        Notes = p.Notes
                    };
                    createdPassports += await _crewRepository.AddPassportAsync(rec) > 0 ? 1 : 0;
                }
            }

            // 3) Visas
            if (request.Visas != null)
            {
                foreach (var v in request.Visas)
                {
                    var rec = new CrewVisa
                    {
                        UserId = userId,
                        VisaType = v.VisaType,
                        Country = v.Country,
                        IssueDate = v.IssueDate,
                        ExpiryDate = v.ExpiryDate,
                        IssuedBy = v.IssuedBy,
                        Notes = v.Notes
                    };
                    createdVisas += await _crewRepository.AddVisaAsync(rec) > 0 ? 1 : 0;
                }
            }

            // 4) Medical
            if (request.MedicalRecords != null)
            {
                foreach (var m in request.MedicalRecords)
                {
                    var rec = new CrewMedicalRecord
                    {
                        UserId = userId,
                        ProviderName = m.ProviderName,
                        BloodGroup = m.BloodGroup,
                        ExaminationDate = m.ExaminationDate,
                        ExpiryDate = m.ExpiryDate,
                        Notes = m.Notes
                    };
                    createdMedical += await _crewRepository.AddMedicalAsync(rec) > 0 ? 1 : 0;
                }
            }

            // 5) Reports
            if (request.Reports != null)
            {
                foreach (var r in request.Reports)
                {
                    var rec = new CrewReport
                    {
                        UserId = userId,
                        ReportType = r.ReportType,
                        Title = r.Title,
                        Details = r.Details,
                        ReportDate = r.ReportDate ?? DateTime.UtcNow
                    };
                    createdReports += await _crewRepository.AddCrewReportAsync(rec) > 0 ? 1 : 0;
                }
            }

            // 6) Payrolls
            if (request.Payrolls != null)
            {
                foreach (var pr in request.Payrolls)
                {
                    var rec = new Payroll
                    {
                        CrewMemberId = userId,
                        PeriodStart = pr.PeriodStart,
                        PeriodEnd = pr.PeriodEnd,
                        BaseWage = pr.BaseWage,
                        Overtime = pr.Overtime,
                        Bonuses = pr.Bonuses,
                        Deductions = pr.Deductions,
                        Currency = pr.Currency,
                        PaymentDate = pr.PaymentDate,
                        PaymentMethod = pr.PaymentMethod
                    };
                    createdPayrolls += await _crewRepository.AddPayrollRecordAsync(rec) > 0 ? 1 : 0;
                }
            }

            // 7) Expenses
            if (request.Expenses != null)
            {
                foreach (var e in request.Expenses)
                {
                    var rec = new CrewExpense
                    {
                        ExpenseReportId = e.ExpenseReportId,
                        CrewMemberId = userId,
                        Category = e.Category,
                        Amount = e.Amount,
                        Currency = e.Currency,
                        ExpenseDate = e.ExpenseDate ?? DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = e.UpdatedAt
                    };
                    createdExpenses += await _crewRepository.AddCrewExpenseAsync(rec) > 0 ? 1 : 0;
                }
            }

            // 8) Assignments
            if (request.Assignments != null)
            {
                foreach (var a in request.Assignments)
                {
                    var assignment = new ShipAssignment
                    {
                        ShipId = a.VesselId,
                        UserId = userId,
                        //Position = a.Position ?? a.Rank ?? "Crew Member",
                        AssignedAt = a.AssignmentDate ?? DateTime.UtcNow,
                        UnassignedAt = a.EndDate,
                        Status = "active",
                        AssignedByUserId = a.AssignedByUserId,
                        Notes = a.Notes
                    };
                    createdAssignments += await _crewRepository.AssignCrewToVesselAsync(assignment) > 0 ? 1 : 0;
                }
            }

            // 9) Trainings
            if (request.Trainings != null)
            {
                foreach (var t in request.Trainings)
                {
                    var training = new CrewTraining
                    {
                        UserId = userId,
                        VesselId = t.VesselId,
                        TrainingCategory = t.TrainingCategory,
                        Rank = t.Rank,
                        Trainer = t.Trainer,
                        Remark = t.Remark,
                        Training = t.Training,
                        Source = t.Source,
                        TrainingDate = t.TrainingDate,
                        ExpireDate = t.ExpireDate,
                        Status = t.Status,
                        Attachments = t.Attachments,
                        CreatedByUserId = 1,
                        CreatedAt = DateTime.UtcNow
                    };
                    createdTrainings += await _crewRepository.AddCrewTrainingAsync(training) > 0 ? 1 : 0;
                }
            }

            // 10) Evaluations
            if (request.Evaluations != null)
            {
                foreach (var ev in request.Evaluations)
                {
                    var evaluation = new CrewEvaluation
                    {
                        UserId = userId,
                        VesselId = ev.VesselId,
                        FormNo = ev.FormNo,
                        RevisionNo = ev.RevisionNo,
                        RevisionDate = ev.RevisionDate,
                        FormName = ev.FormName,
                        FormDescription = ev.FormDescription,
                        EnteredByUserId = ev.EnteredByUserId,
                        EnteredDate = ev.EnteredDate,
                        Rank = ev.Rank,
                        Name = ev.Name,
                        Surname = ev.Surname,
                        UniqueId = ev.UniqueId,
                        TechnicalCompetence = ev.TechnicalCompetence,
                        SafetyAwareness = ev.SafetyAwareness,
                        Teamwork = ev.Teamwork,
                        Communication = ev.Communication,
                        Leadership = ev.Leadership,
                        ProblemSolving = ev.ProblemSolving,
                        Adaptability = ev.Adaptability,
                        WorkEthic = ev.WorkEthic,
                        OverallRating = ev.OverallRating,
                        Strengths = ev.Strengths,
                        AreasForImprovement = ev.AreasForImprovement,
                        Comments = ev.Comments,
                        CrewMemberComments = ev.CrewMemberComments,
                        CrewMemberSignature = ev.CrewMemberSignature,
                        CrewMemberSignedDate = ev.CrewMemberSignedDate,
                        Status = ev.Status,
                        Attachments = ev.Attachments,
                        CreatedByUserId = 1,
                        CreatedAt = DateTime.UtcNow
                    };
                    createdEvaluations += await _crewRepository.AddCrewEvaluationAsync(evaluation) > 0 ? 1 : 0;
                }
            }

            return new CreateCrewProfileResultDto
            {
                UserId = userId,
                CreatedPassports = createdPassports,
                CreatedVisas = createdVisas,
                CreatedMedical = createdMedical,
                CreatedReports = createdReports,
                CreatedPayrolls = createdPayrolls,
                CreatedExpenses = createdExpenses,
                CreatedAssignments = createdAssignments,
                CreatedTrainings = createdTrainings,
                CreatedEvaluations = createdEvaluations
            };
        }
    }
    
}