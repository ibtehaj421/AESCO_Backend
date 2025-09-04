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

        public async Task<string> AddCrewAsync(CreateUserDto crewDto)
        {
            if (crewDto == null)
            {
                return "Invalid crew data.";
            }

            var crew = new User
            {
               
            };

            var result = await _crewRepository.AddCrewAsync(crew);
            return result > 0 ? "Crew added successfully." : "Failed to add crew.";
        }

        public async Task<List<Crew>> GetAllCrewsAsync()
        {
            return await _crewRepository.GetAllCrewsAsync();
        }

        public async Task<Crew?> GetCrewByIdAsync(int crewId)
        {
            return await _crewRepository.GetCrewByIdAsync(crewId);
        }

        public async Task<string> UpdateCrewAsync(int crewId, UpdateCrewDto crewDto)
        {
            var existingCrew = await _crewRepository.GetCrewByIdAsync(crewId);
            if (existingCrew == null)
            {
                return "Crew not found.";
            }

            existingCrew.Name = crewDto.Name ?? existingCrew.Name;
            existingCrew.Description = crewDto.Description ?? existingCrew.Description;
            existingCrew.UpdatedAt = DateTime.UtcNow;

            var result = await _crewRepository.UpdateCrewAsync(existingCrew);
            return result > 0 ? "Crew updated successfully." : "Failed to update crew.";
        }

        public async Task<string> DeleteCrewAsync(int crewId)
        {
            var existingCrew = await _crewRepository.GetCrewByIdAsync(crewId);
            if (existingCrew == null)
            {
                return "Crew not found.";
            }

            var result = await _crewRepository.DeleteCrewAsync(existingCrew);
            return result > 0 ? "Crew deleted successfully." : "Failed to delete crew.";
        }
    }
}