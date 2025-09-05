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
        //     public async Task<string> AddCrewAsync(CreateUserDto crewDto)
        //     {
        //         if (crewDto == null)
        //         {
        //             return "Invalid crew data.";
        //         }

        //         var crew = new User
        //         {

        //         };

        //         var result = await _crewRepository.AddCrewAsync(crew);
        //         return result > 0 ? "Crew added successfully." : "Failed to add crew.";
        //     }

        //     public async Task<List<Crew>> GetAllCrewsAsync()
        //     {
        //         return await _crewRepository.GetAllCrewsAsync();
        //     }

        //     public async Task<Crew?> GetCrewByIdAsync(int crewId)
        //     {
        //         return await _crewRepository.GetCrewByIdAsync(crewId);
        //     }

        //     public async Task<string> UpdateCrewAsync(int crewId, UpdateCrewDto crewDto)
        //     {
        //         var existingCrew = await _crewRepository.GetCrewByIdAsync(crewId);
        //         if (existingCrew == null)
        //         {
        //             return "Crew not found.";
        //         }

        //         existingCrew.Name = crewDto.Name ?? existingCrew.Name;
        //         existingCrew.Description = crewDto.Description ?? existingCrew.Description;
        //         existingCrew.UpdatedAt = DateTime.UtcNow;

        //         var result = await _crewRepository.UpdateCrewAsync(existingCrew);
        //         return result > 0 ? "Crew updated successfully." : "Failed to update crew.";
        //     }

        //     public async Task<string> DeleteCrewAsync(int crewId)
        //     {
        //         var existingCrew = await _crewRepository.GetCrewByIdAsync(crewId);
        //         if (existingCrew == null)
        //         {
        //             return "Crew not found.";
        //         }

        //         var result = await _crewRepository.DeleteCrewAsync(existingCrew);
        //         return result > 0 ? "Crew deleted successfully." : "Failed to delete crew.";
        //     }
    }
    
}