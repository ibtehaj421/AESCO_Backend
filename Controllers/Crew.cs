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


    //updations
    [HttpPost("update")]
    public async Task<IActionResult> UpdateCrew([FromBody] UpdateUserDto crewDto)
    {
        if (crewDto == null)
        {
            return BadRequest("Invalid crew data.");
        }

        var updatedCrew = await _crewService.UpdateCrewAsync(crewDto);
        //if value not positive or less than 1, something went wrong

        if (updatedCrew < 1)
        {
            return NotFound();
        }

        return Ok("Value has been updated successfully.");
    }

    //viewing and fetch requests.


    //deletions


    //assignments and management
    [HttpPost("vessel/assign")]
    public async Task<IActionResult> AssignCrewToVessel([FromBody] AssignmentDTO assignDto)
    {
        if (assignDto == null)
        {
            return BadRequest("Invalid assignment data.");
        }

        var result = await _crewService.AssignCrewToVesselAsync(assignDto);
        if (result > 0)
        {
            return Ok("Crew assigned to vessel successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while assigning the crew to the vessel.");
        }
    }

    [HttpPatch("vessel/unassign/{id}")] //adds the unassignment endpoint date. and when the date exceeeds, we will cater to that later.
    public async Task<IActionResult> UnassignCrewFromVessel(int id, [FromBody] DateTime unassignDto)
    {
        if (id == 0)
        {
            return BadRequest("Invalid unassignment data.");
        }

        var result = await _crewService.UnassignCrewFromVesselAsync(id, unassignDto);
        if (result > 0)
        {
            return Ok("Unassignment date added to crew member from vessel successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while unassigning the crew from the vessel.");
        }
    }

    [HttpPost("vessel/update-assignment")]
    public async Task<IActionResult> UpdateAssignmentDetails([FromBody] AssignmentDTO updateDto)
    {
        if (updateDto == null || updateDto.CrewId <= 0 || updateDto.VesselId <= 0)
        {
            return BadRequest("Invalid assignment update data.");
        }

        var result = await _crewService.UpdateAssignmentDetailsAsync(updateDto);
        if (result > 0)
        {
            return Ok("Assignment details updated successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while updating the assignment details.");
        }
    }
}