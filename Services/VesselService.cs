using ASCO.DTOs;
using ASCO.Models;
using ASCO.Repositories;
using ASCP.Repositories;


namespace ASCO.Services
{
    public class VesselService
    {
        private readonly VesselRepository _vesselRepository;

        public VesselService(VesselRepository vesselRepository)
        {
            _vesselRepository = vesselRepository;
        }

        // Add methods to interact with vessels, e.g., GetVesselById, AddVessel, UpdateVessel, etc.
        public async Task<int> CreateVesselAsync(CreateShipDto vesselDto)
        {
            var vessel = new Ship
            {
                Name = vesselDto.Name,
                ShipType = vesselDto.ShipType,
                Flag = vesselDto.Flag,
                GrossTonnage = vesselDto.GrossTonnage,
                DeadweightTonnage = vesselDto.DeadweightTonnage,
                LaunchDate = vesselDto.LaunchDate,
                IMONumber = vesselDto.IMONumber,
                RegistrationNumber = vesselDto.RegistrationNumber,
                LengthOverall = vesselDto.LengthOverall,
                Beam = vesselDto.Beam,
                Draft = vesselDto.Draft,
                Builder = vesselDto.Builder,
                BuildDate = vesselDto.BuildDate,
                MaxSpeed = vesselDto.MaxSpeed,
                ServiceSpeed = vesselDto.ServiceSpeed,
                EngineType = vesselDto.EngineType,
                EnginePower = vesselDto.EnginePower,
                HomePort = vesselDto.HomePort,
                NetTonnage = vesselDto.NetTonnage,
                PassengerCapacity = vesselDto.PassengerCapacity,
                CrewCapacity = vesselDto.CrewCapacity,
                Description = vesselDto.Description,
                CallSign = vesselDto.CallSign,
                Status = "active", // Default status
                CreatedAt = DateTime.UtcNow
            };

            int value = await _vesselRepository.CreateVesselAsync(vessel);

            return value;
        }
    }
}