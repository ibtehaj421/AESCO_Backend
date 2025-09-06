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

        public CrewService(CrewRepository crewRepository, UserRepository userRepository)
        {
            _crewRepository = crewRepository;
            _userRepository = userRepository;
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

    }
    
}