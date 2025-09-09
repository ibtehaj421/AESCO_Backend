using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASCO.Services;
using ASCO.DTOs;
//[Authorize]
[ApiController]
[Route("vessel")]
public class VesselController : ControllerBase
{
    private readonly VesselService _vesselService;

    public VesselController(VesselService vesselService)
    {
        _vesselService = vesselService;
    }


    //creation method under admin.
    [HttpPost("create")]
    public async Task<IActionResult> CreateVessel([FromBody] CreateShipDto vesselDto)
    {
        if (vesselDto == null)
        {
            return BadRequest("Invalid vessel data.");
        }

        var createdVessel = await _vesselService.CreateVesselAsync(vesselDto);
        return CreatedAtAction(nameof(GetVesselById), new { id = createdVessel.Id }, createdVessel);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllVessels()
    {
        var vessels = await _vesselService.GetAllVesselsAsync();
        return Ok(vessels);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVesselById(int id)
    {
        var vessel = await _vesselService.GetVesselByIdAsync(id);
        if (vessel == null)
        {
            return NotFound();
        }
        return Ok(vessel);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateVessel(int id, [FromBody] UpdateShipDto vesselDto)
    {
        if (vesselDto == null || id != vesselDto.Id)
        {
            return BadRequest("Invalid vessel data.");
        }

        var updatedVessel = await _vesselService.UpdateVesselAsync(id, vesselDto);
        if (updatedVessel == null)
        {
            return NotFound();
        }

        return Ok(updatedVessel);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteVessel(int id)
    {
        var result = await _vesselService.DeleteVesselAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return Ok("Vessel deleted successfully.");
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchVessels([FromBody] ShipSearchDto searchDto)
    {
        if (searchDto == null)
        {
            return BadRequest("Invalid search criteria.");
        }

        var vessels = await _vesselService.SearchVesselsAsync(searchDto);
        return Ok(vessels);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetVesselsPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? searchTerm = null)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Page and page size must be greater than 0.");
        }

        var result = await _vesselService.GetVesselsPagedAsync(page, pageSize, searchTerm);
        return Ok(result);
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetVesselStatistics()
    {
        var statistics = await _vesselService.GetVesselStatisticsAsync();
        return Ok(statistics);
    }

    [HttpGet("ship-types")]
    public async Task<IActionResult> GetShipTypes()
    {
        var shipTypes = await _vesselService.GetDistinctShipTypesAsync();
        return Ok(shipTypes);
    }

    [HttpGet("flags")]
    public async Task<IActionResult> GetFlags()
    {
        var flags = await _vesselService.GetDistinctFlagsAsync();
        return Ok(flags);
    }

    [HttpGet("home-ports")]
    public async Task<IActionResult> GetHomePorts()
    {
        var homePorts = await _vesselService.GetDistinctHomePortsAsync();
        return Ok(homePorts);
    }

}