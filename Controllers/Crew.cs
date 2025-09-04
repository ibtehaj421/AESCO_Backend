using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASCO.Services;
//[Authorize]
[ApiController]
[Route("crew")]
public class CrewController : ControllerBase
{
    private readonly CrewService _crewService;

    public CrewController(CrewService crewService)
    {
        _crewService = crewService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCrews()
    {
        var crews = await _crewService.GetAllCrewsAsync();
        return Ok(crews);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCrewById(int id)
    {
        var crew = await _crewService.GetCrewByIdAsync(id);
        if (crew == null)
        {
            return NotFound();
        }
        return Ok(crew);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCrew([FromBody] CreateCrewDto crewDto)
    {
        if (crewDto == null)
        {
            return BadRequest("Invalid crew data.");
        }

        var createdCrew = await _crewService.CreateCrewAsync(crewDto);
        return CreatedAtAction(nameof(GetCrewById), new { id = createdCrew.Id }, createdCrew);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateCrew(int id, [FromBody] UpdateCrewDto crewDto)
    {
        if (crewDto == null || id != crewDto.Id)
        {
            return BadRequest("Invalid crew data.");
        }

        var updatedCrew = await _crewService.UpdateCrewAsync(id, crewDto);
        if (updatedCrew == null)
        {
            return NotFound();
        }

        return Ok(updatedCrew);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCrew(int id)
    {
        var result = await _crewService.DeleteCrewAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}