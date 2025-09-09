using ASCO.DTOs;
using ASCO.Models;
using ASCO.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ASCO.Services
{
    public class VesselService
    {
        private readonly VesselRepository _vesselRepository;

        public VesselService(VesselRepository vesselRepository)
        {
            _vesselRepository = vesselRepository;
        }

        public async Task<ShipDto> CreateVesselAsync(CreateShipDto vesselDto)
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
                Status = vesselDto.Status ?? "active",
                CreatedAt = DateTime.UtcNow
            };

            var createdVessel = await _vesselRepository.CreateVesselAsync(vessel);
            return MapToShipDto(createdVessel);
        }

        public async Task<List<ShipSummaryDto>> GetAllVesselsAsync()
        {
            var vessels = await _vesselRepository.GetAllVesselsAsync();
            return vessels.Select(MapToShipSummaryDto).ToList();
        }

        public async Task<ShipDto?> GetVesselByIdAsync(int id)
        {
            var vessel = await _vesselRepository.GetVesselByIdAsync(id);
            return vessel != null ? MapToShipDto(vessel) : null;
        }

        public async Task<ShipDto?> UpdateVesselAsync(int id, UpdateShipDto vesselDto)
        {
            var existingVessel = await _vesselRepository.GetVesselByIdAsync(id);
            if (existingVessel == null)
            {
                return null;
            }

            // Update properties
            existingVessel.Name = vesselDto.Name;
            existingVessel.IMONumber = vesselDto.IMONumber;
            existingVessel.CallSign = vesselDto.CallSign;
            existingVessel.RegistrationNumber = vesselDto.RegistrationNumber;
            existingVessel.ShipType = vesselDto.ShipType;
            existingVessel.Flag = vesselDto.Flag;
            existingVessel.Builder = vesselDto.Builder;
            existingVessel.BuildDate = vesselDto.BuildDate;
            existingVessel.LaunchDate = vesselDto.LaunchDate;
            existingVessel.LengthOverall = vesselDto.LengthOverall;
            existingVessel.Beam = vesselDto.Beam;
            existingVessel.Draft = vesselDto.Draft;
            existingVessel.GrossTonnage = vesselDto.GrossTonnage;
            existingVessel.NetTonnage = vesselDto.NetTonnage;
            existingVessel.DeadweightTonnage = vesselDto.DeadweightTonnage;
            existingVessel.PassengerCapacity = vesselDto.PassengerCapacity;
            existingVessel.CrewCapacity = vesselDto.CrewCapacity;
            existingVessel.MaxSpeed = vesselDto.MaxSpeed;
            existingVessel.ServiceSpeed = vesselDto.ServiceSpeed;
            existingVessel.EngineType = vesselDto.EngineType;
            existingVessel.EnginePower = vesselDto.EnginePower;
            existingVessel.HomePort = vesselDto.HomePort;
            existingVessel.Description = vesselDto.Description;
            existingVessel.Status = vesselDto.Status ?? existingVessel.Status;
            existingVessel.UpdatedAt = DateTime.UtcNow;

            var updatedVessel = await _vesselRepository.UpdateVesselAsync(existingVessel);
            return MapToShipDto(updatedVessel);
        }

        public async Task<bool> DeleteVesselAsync(int id)
        {
            return await _vesselRepository.DeleteVesselAsync(id);
        }

        public async Task<List<ShipSummaryDto>> SearchVesselsAsync(ShipSearchDto searchDto)
        {
            var vessels = await _vesselRepository.SearchVesselsAsync(searchDto);
            return vessels.Select(MapToShipSummaryDto).ToList();
        }

        public async Task<PagedResult<ShipSummaryDto>> GetVesselsPagedAsync(int page, int pageSize, string? searchTerm = null)
        {
            var result = await _vesselRepository.GetVesselsPagedAsync(page, pageSize, searchTerm);
            return new PagedResult<ShipSummaryDto>
            {
                Items = result.Items.Select(MapToShipSummaryDto).ToList(),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }

        public async Task<Dictionary<string, int>> GetVesselStatisticsAsync()
        {
            return await _vesselRepository.GetVesselStatisticsAsync();
        }

        public async Task<List<string>> GetDistinctShipTypesAsync()
        {
            return await _vesselRepository.GetDistinctShipTypesAsync();
        }

        public async Task<List<string>> GetDistinctFlagsAsync()
        {
            return await _vesselRepository.GetDistinctFlagsAsync();
        }

        public async Task<List<string>> GetDistinctHomePortsAsync()
        {
            return await _vesselRepository.GetDistinctHomePortsAsync();
        }

        // Mapping methods
        private ShipDto MapToShipDto(Ship vessel)
        {
            return new ShipDto
            {
                Id = vessel.Id,
                Name = vessel.Name,
                IMONumber = vessel.IMONumber,
                CallSign = vessel.CallSign,
                RegistrationNumber = vessel.RegistrationNumber,
                ShipType = vessel.ShipType,
                Flag = vessel.Flag,
                Builder = vessel.Builder,
                BuildDate = vessel.BuildDate,
                LaunchDate = vessel.LaunchDate,
                LengthOverall = vessel.LengthOverall,
                Beam = vessel.Beam,
                Draft = vessel.Draft,
                GrossTonnage = vessel.GrossTonnage,
                NetTonnage = vessel.NetTonnage,
                DeadweightTonnage = vessel.DeadweightTonnage,
                PassengerCapacity = vessel.PassengerCapacity,
                CrewCapacity = vessel.CrewCapacity,
                MaxSpeed = vessel.MaxSpeed,
                ServiceSpeed = vessel.ServiceSpeed,
                EngineType = vessel.EngineType,
                EnginePower = vessel.EnginePower,
                HomePort = vessel.HomePort,
                Status = vessel.Status,
                Description = vessel.Description,
                CreatedAt = vessel.CreatedAt,
                UpdatedAt = vessel.UpdatedAt,
                CurrentAssignments = vessel.ShipAssignments?.Where(sa => sa.Status == "active")
                    .Select(sa => new ShipAssignmentDto
                    {
                        Id = sa.Id,
                        ShipId = sa.ShipId,
                        ShipName = vessel.Name,
                        UserId = sa.UserId,
                        UserName = sa.User?.Name + " " + sa.User?.Surname,
                        UserEmail = sa.User?.Email ?? "",
                        Position = sa.Position,
                        AssignedAt = sa.AssignedAt ?? DateTime.UtcNow,
                        UnassignedAt = sa.UnassignedAt,
                        Status = sa.Status,
                        AssignedByName = sa.AssignedBy?.Name + " " + sa.AssignedBy?.Surname,
                        Notes = sa.Notes
                    }).ToList() ?? new List<ShipAssignmentDto>(),
                CurrentVoyage = vessel.Voyages?.Where(v => v.Status == "active").FirstOrDefault() != null ?
                    new VoyageDto
                    {
                        Id = vessel.Voyages.Where(v => v.Status == "active").First().Id,
                        ShipId = vessel.Id,
                        ShipName = vessel.Name,
                        DeparturePort = vessel.Voyages.Where(v => v.Status == "active").First().DeparturePort,
                        ArrivalPort = vessel.Voyages.Where(v => v.Status == "active").First().ArrivalPort,
                        PlannedDeparture = vessel.Voyages.Where(v => v.Status == "active").First().PlannedDeparture,
                        PlannedArrival = vessel.Voyages.Where(v => v.Status == "active").First().PlannedArrival,
                        ActualDeparture = vessel.Voyages.Where(v => v.Status == "active").First().ActualDeparture,
                        ActualArrival = vessel.Voyages.Where(v => v.Status == "active").First().ActualArrival,
                        Status = vessel.Voyages.Where(v => v.Status == "active").First().Status,
                        VoyageNumber = vessel.Voyages.Where(v => v.Status == "active").First().VoyageNumber,
                        CargoType = vessel.Voyages.Where(v => v.Status == "active").First().CargoType,
                        CargoWeight = vessel.Voyages.Where(v => v.Status == "active").First().CargoWeight,
                        Distance = vessel.Voyages.Where(v => v.Status == "active").First().Distance,
                        Notes = vessel.Voyages.Where(v => v.Status == "active").First().Notes,
                        CreatedAt = vessel.Voyages.Where(v => v.Status == "active").First().CreatedAt,
                        UpdatedAt = vessel.Voyages.Where(v => v.Status == "active").First().UpdatedAt
                    } : null
            };
        }

        private ShipSummaryDto MapToShipSummaryDto(Ship vessel)
        {
            return new ShipSummaryDto
            {
                Id = vessel.Id,
                Name = vessel.Name,
                IMONumber = vessel.IMONumber,
                ShipType = vessel.ShipType,
                Flag = vessel.Flag,
                Status = vessel.Status,
                HomePort = vessel.HomePort,
                GrossTonnage = vessel.GrossTonnage,
                CreatedAt = vessel.CreatedAt,
                LastVoyageDate = vessel.Voyages?.OrderByDescending(v => v.PlannedDeparture).FirstOrDefault()?.PlannedDeparture,
                CurrentLocation = vessel.Voyages?.Where(v => v.Status == "active").FirstOrDefault()?.ArrivalPort,
                AssignedCrew = vessel.ShipAssignments?.Where(sa => sa.Status == "active")
                    .Select(sa => sa.User?.Name + " " + sa.User?.Surname).ToList() ?? new List<string>()
            };
        }
    }
}