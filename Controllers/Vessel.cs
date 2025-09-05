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

        int check = await _vesselService.CreateVesselAsync(vesselDto);
        if (check > 0)
        {
            return Ok("Vessel created successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while creating the vessel.");
        }
    }

    
    // [HttpGet("all")]
    // public async Task<IActionResult> GetAllVessels()
    // {
    //     var vessels = await _vesselService.GetAllVesselsAsync();
    //     return Ok(vessels);
    // }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetVesselById(int id)
    // {
    //     var vessel = await _vesselService.GetVesselByIdAsync(id);
    //     if (vessel == null)
    //     {
    //         return NotFound();
    //     }
    //     return Ok(vessel);
    // }

    // [HttpPost("create")]
    // public async Task<IActionResult> CreateVessel([FromBody] CreateVesselDto vesselDto)
    // {
    //     if (vesselDto == null)
    //     {
    //         return BadRequest("Invalid vessel data.");
    //     }

    //     var createdVessel = await _vesselService.CreateVesselAsync(vesselDto);
    //     return CreatedAtAction(nameof(GetVesselById), new { id = createdVessel.Id }, createdVessel);
    // }

    // [HttpPut("update/{id}")]
    // public async Task<IActionResult> UpdateVessel(int id, [FromBody] UpdateVesselDto vesselDto)
    // {
    //     if (vesselDto == null || id != vesselDto.Id)
    //     {
    //         return BadRequest("Invalid vessel data.");
    //     }

    //     var updatedVessel = await _vesselService.UpdateVesselAsync(id, vesselDto);
    //     if (updatedVessel == null)
    //     {
    //         return NotFound();
    //     }

    //     return Ok(updatedVessel);
    // }

}