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

    [HttpGet("assignment/history/{userId}")]
    public async Task<IActionResult> GetAssignmentHistory(int userId)
    {
        if (userId <= 0) return BadRequest("Invalid user id");
        var history = await _crewService.GetAssignmentHistoryAsync(userId);
        return Ok(history);
    }

    // medical
    [HttpPost("medical/create")]
    public async Task<IActionResult> CreateMedical([FromBody] CreateCrewMedicalDto dto)
    {
        var res = await _crewService.CreateMedicalAsync(dto);
        return res > 0 ? Ok("Medical record created") : StatusCode(500, "Failed to create medical record");
    }

    [HttpPost("medical/update")]
    public async Task<IActionResult> UpdateMedical([FromBody] CrewMedicalDto dto)
    {
        var res = await _crewService.UpdateMedicalAsync(dto);
        return res > 0 ? Ok("Medical record updated") : NotFound();
    }

    [HttpPost("medical/search")]
    public async Task<IActionResult> SearchMedical([FromBody] CrewDocumentSearchDto s)
    {
        var res = await _crewService.SearchMedicalAsync(s);
        return Ok(res);
    }

    // passports
    [HttpPost("passport/create")]
    public async Task<IActionResult> CreatePassport([FromBody] CreateCrewPassportDto dto)
    {
        var res = await _crewService.CreatePassportAsync(dto);
        return res > 0 ? Ok("Passport created") : StatusCode(500, "Failed to create passport");
    }

    [HttpPost("passport/update")]
    public async Task<IActionResult> UpdatePassport([FromBody] CrewPassportDto dto)
    {
        var res = await _crewService.UpdatePassportAsync(dto);
        return res > 0 ? Ok("Passport updated") : NotFound();
    }

    [HttpPost("passport/search")]
    public async Task<IActionResult> SearchPassports([FromBody] CrewDocumentSearchDto s)
    {
        var res = await _crewService.SearchPassportsAsync(s);
        return Ok(res);
    }

    // visas
    [HttpPost("visa/create")]
    public async Task<IActionResult> CreateVisa([FromBody] CreateCrewVisaDto dto)
    {
        var res = await _crewService.CreateVisaAsync(dto);
        return res > 0 ? Ok("Visa created") : StatusCode(500, "Failed to create visa");
    }

    [HttpPost("visa/update")]
    public async Task<IActionResult> UpdateVisa([FromBody] CrewVisaDto dto)
    {
        var res = await _crewService.UpdateVisaAsync(dto);
        return res > 0 ? Ok("Visa updated") : NotFound();
    }

    [HttpPost("visa/search")]
    public async Task<IActionResult> SearchVisas([FromBody] CrewDocumentSearchDto s)
    {
        var res = await _crewService.SearchVisasAsync(s);
        return Ok(res);
    }

    // crew reports
    [HttpPost("report/create")]
    public async Task<IActionResult> CreateReport([FromBody] CreateCrewReportDto dto)
    {
        var res = await _crewService.CreateCrewReportAsync(dto);
        return res > 0 ? Ok("Report created") : StatusCode(500, "Failed to create report");
    }

    [HttpPost("report/update")]
    public async Task<IActionResult> UpdateReport([FromBody] CrewReportDto dto)
    {
        var res = await _crewService.UpdateCrewReportAsync(dto);
        return res > 0 ? Ok("Report updated") : NotFound();
    }

    [HttpGet("report/search")]
    public async Task<IActionResult> SearchReports([FromQuery] int? userId, [FromQuery] string? reportType, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var res = await _crewService.SearchCrewReportsAsync(userId, reportType, page, pageSize);
        return Ok(res);
    }

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