using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASCO.Services;

using ASCO.DTOs.Crew;
using ASCO.DTOs.Documents;
using ASCO.DTOs;
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

    // //payroll thingies.
    // [HttpPost("payroll/add")]
    // public async Task<IActionResult> AddPayrollRecord([FromBody] PayrollDto dto)
    // {
    //     if (dto == null || dto.CrewMemberId <= 0 || string.IsNullOrWhiteSpace(dto.Currency) || dto.PaymentDate == default)
    //     {
    //         return BadRequest("Invalid payroll data.");
    //     }

    //     int result = await _crewService.AddPayrollRecordAsync(dto);
    //     if (result > 0)
    //     {
    //         return Ok("Payroll record added successfully.");
    //     }
    //     else
    //     {
    //         return StatusCode(500, "An error occurred while adding the payroll record.");
    //     }
    // }
    [HttpGet("payroll/individual/{crewId}")]
    public async Task<IActionResult> GetPayrollByCrew(int crewId)
    {
        if (crewId <= 0) return BadRequest("Invalid crew id");
        var payrolls = await _crewService.GetPayrollByCrewAsync(crewId);
        return Ok(payrolls); //if the values are empty, it means there is no payroll for that crew member.
    }

    //cash statements and expenses
    [HttpPost("expense/add")]
    public async Task<IActionResult> AddCrewExpense([FromBody] CreateCrewExpenseDto dto)
    {
        if (dto == null || dto.CrewMemberId <= 0 || dto.Amount <= 0 || string.IsNullOrWhiteSpace(dto.Currency))
        {
            return BadRequest("Invalid crew expense data.");
        }

        int result = await _crewService.AddCrewExpenseAsync(dto);
        if (result > 0)
        {
            return Ok("Crew expense record added successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while adding the crew expense record.");
        }
    }

    [HttpPost("statement/add")]
    public async Task<IActionResult> AddCashStatement([FromBody] StatementOfCashCreateDto dto)
    {
        if (dto == null || dto.VesselId <= 0)
        {
            return BadRequest("Invalid cash statement data.");
        }

        int result = await _crewService.AddCashStatementAsync(dto);
        if (result > 0)
        {
            return Ok("Cash statement record added successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while adding the cash statement record.");
        }
    }

    //vessel manning thingies
    [HttpGet("vessel/manning/{vesselId}")]
    public async Task<IActionResult> GetVesselManning(int vesselId)
    {
        if (vesselId <= 0) return BadRequest("Invalid vessel id");
        var manning = await _crewService.GetVesselManningAsync(vesselId);
        return Ok(manning); //if the values are empty, it means there is no manning for that vessel.
    }

    [HttpGet("vessel/{vesselId}")]
    public async Task<IActionResult> GetCrewMembersByVessel(int vesselId)
    {
        if (vesselId <= 0) return BadRequest("Invalid vessel id");
        var crewMembers = await _crewService.GetCrewMembersByVesselAsync(vesselId);
        return Ok(crewMembers);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableCrewMembers()
    {
        var availableCrew = await _crewService.GetAvailableCrewMembersAsync();
        return Ok(availableCrew);
    }

    [HttpGet("ranks/available")]
    public async Task<IActionResult> GetAvailableRanks()
    {
        var availableRanks = await _crewService.GetAvailableRanksAsync();
        return Ok(availableRanks);
    }

    [HttpGet("ranks/all")]
    public async Task<IActionResult> GetAllPossibleRanks()
    {
        var allRanks = await _crewService.GetAllPossibleRanksAsync();
        return Ok(allRanks);
    }

    [HttpPost("vessel/manning/add")]
    public async Task<IActionResult> AddVesselManning([FromBody] VesselManningDTO dto)
    {
        if (dto == null || dto.VesselId <= 0 || dto.Rank == null || dto.Rank.Count == 0)
        {
            return BadRequest("Invalid vessel manning data.");
        }

        int result = await _crewService.AddVesselManningAsync(dto);
        if (result > 0)
        {
            return Ok("Vessel manning record added successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while adding the vessel manning record.");
        }
    }

    [HttpDelete("vessel/manning/delete")]
    public async Task<IActionResult> DeleteVesselManning([FromBody] VesselManningDeleteDTO dto)
    {
        if (dto == null || dto.VesselId <= 0 || dto.Rank == null || dto.Rank.Count == 0)
        {
            return BadRequest("Invalid vessel manning deletion data.");
        }

        int result = await _crewService.RemoveVesselManningAsync(dto);
        if (result > 0)
        {
            return Ok("Vessel manning record(s) deleted successfully.");
        }
        else
        {
            return StatusCode(500, "An error occurred while deleting the vessel manning record(s).");
        }
    }

    // Crew Training endpoints
    [HttpPost("training/create")]
    public async Task<IActionResult> CreateCrewTraining([FromBody] CreateCrewTrainingDto dto)
    {
        if (dto == null || dto.UserId <= 0 || dto.VesselId <= 0)
        {
            return BadRequest("Invalid training data.");
        }

        var result = await _crewService.CreateCrewTrainingAsync(dto);
        return result > 0 ? Ok("Training record created successfully.") : StatusCode(500, "Failed to create training record.");
    }

    [HttpPost("training/update")]
    public async Task<IActionResult> UpdateCrewTraining([FromBody] UpdateCrewTrainingDto dto)
    {
        if (dto == null || dto.Id <= 0)
        {
            return BadRequest("Invalid training update data.");
        }

        var result = await _crewService.UpdateCrewTrainingAsync(dto);
        return result > 0 ? Ok("Training record updated successfully.") : NotFound("Training record not found.");
    }

    [HttpPost("training/search")]
    public async Task<IActionResult> SearchCrewTrainings([FromBody] CrewTrainingSearchDto searchDto)
    {
        var result = await _crewService.SearchCrewTrainingsAsync(searchDto);
        return Ok(result);
    }

    // Crew Evaluation endpoints
    [HttpPost("evaluation/create")]
    public async Task<IActionResult> CreateCrewEvaluation([FromBody] CreateCrewEvaluationDto dto)
    {
        if (dto == null || dto.UserId <= 0 || dto.VesselId <= 0)
        {
            return BadRequest("Invalid evaluation data.");
        }

        var result = await _crewService.CreateCrewEvaluationAsync(dto);
        return result > 0 ? Ok("Evaluation record created successfully.") : StatusCode(500, "Failed to create evaluation record.");
    }

    [HttpPost("evaluation/update")]
    public async Task<IActionResult> UpdateCrewEvaluation([FromBody] UpdateCrewEvaluationDto dto)
    {
        if (dto == null || dto.Id <= 0)
        {
            return BadRequest("Invalid evaluation update data.");
        }

        var result = await _crewService.UpdateCrewEvaluationAsync(dto);
        return result > 0 ? Ok("Evaluation record updated successfully.") : NotFound("Evaluation record not found.");
    }

    [HttpPost("evaluation/search")]
    public async Task<IActionResult> SearchCrewEvaluations([FromBody] CrewEvaluationSearchDto searchDto)
    {
        var result = await _crewService.SearchCrewEvaluationsAsync(searchDto);
        return Ok(result);
    }

    // Crew Work Rest Hours endpoints
    [HttpPost("work-rest-hours/create")]
    public async Task<IActionResult> CreateCrewWorkRestHours([FromBody] CreateCrewWorkRestHoursDto dto)
    {
        if (dto == null || dto.UserId <= 0 || dto.VesselId <= 0)
        {
            return BadRequest("Invalid work rest hours data.");
        }

        var result = await _crewService.CreateCrewWorkRestHoursAsync(dto);
        return result > 0 ? Ok("Work rest hours record created successfully.") : StatusCode(500, "Failed to create work rest hours record.");
    }

    [HttpPost("work-rest-hours/update")]
    public async Task<IActionResult> UpdateCrewWorkRestHours([FromBody] UpdateCrewWorkRestHoursDto dto)
    {
        if (dto == null || dto.Id <= 0)
        {
            return BadRequest("Invalid work rest hours update data.");
        }

        var result = await _crewService.UpdateCrewWorkRestHoursAsync(dto);
        return result > 0 ? Ok("Work rest hours record updated successfully.") : NotFound("Work rest hours record not found.");
    }

    [HttpPost("work-rest-hours/search")]
    public async Task<IActionResult> SearchCrewWorkRestHours([FromBody] CrewWorkRestHoursSearchDto searchDto)
    {
        var result = await _crewService.SearchCrewWorkRestHoursAsync(searchDto);
        return Ok(result);
    }

    [HttpGet("work-rest-hours/user/{userId}")]
    public async Task<IActionResult> GetCrewWorkRestHoursByUser(int userId, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        if (userId <= 0) return BadRequest("Invalid user id");
        var result = await _crewService.GetCrewWorkRestHoursByUserAsync(userId, fromDate, toDate);
        return Ok(result);
    }

    // Aggregated crew profile
    [HttpGet("profile/{userId}")]
    public async Task<IActionResult> GetCrewProfile(int userId)
    {
        if (userId <= 0) return BadRequest("Invalid user id");
        var profile = await _crewService.GetCrewProfileAsync(userId);
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    // Create aggregated crew profile
    [HttpPost("profile/create")]
    public async Task<IActionResult> CreateCrewProfile([FromBody] CreateCrewProfileDto dto)
    {
        if (dto == null || dto.PersonalInfo == null) return BadRequest("Invalid payload");
        var result = await _crewService.CreateCrewProfileAsync(dto);
        return Ok(result);
    }

    // Update aggregated crew profile
    [HttpPut("profile/update/{userId}")]
    public async Task<IActionResult> UpdateCrewProfile(int userId, [FromBody] CreateCrewProfileDto dto)
    {
        if (userId <= 0) return BadRequest("Invalid user id");
        if (dto == null || dto.PersonalInfo == null) return BadRequest("Invalid payload");
        var result = await _crewService.UpdateCrewProfileAsync(userId, dto);
        return Ok(result);
    }

    // ===== USER MANAGEMENT ENDPOINTS =====
    
    // Get all users/crew members
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCrewMembers([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null, [FromQuery] string? jobType = null, [FromQuery] string? rank = null)
    {
        var crewMembers = await _crewService.GetAllCrewMembersAsync(page, pageSize, status, jobType, rank);
        return Ok(crewMembers);
    }

    // Get crew member by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCrewMemberById(int id)
    {
        if (id <= 0) return BadRequest("Invalid crew member ID");
        
        var crewMember = await _crewService.GetCrewMemberByIdAsync(id);
        if (crewMember == null) return NotFound("Crew member not found");
        
        return Ok(crewMember);
    }

    // Delete crew member
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCrewMember(int id)
    {
        if (id <= 0) return BadRequest("Invalid crew member ID");
        
        var result = await _crewService.DeleteCrewMemberAsync(id);
        if (result > 0)
        {
            return Ok("Crew member deleted successfully.");
        }
        else
        {
            return NotFound("Crew member not found or deletion failed.");
        }
    }

    // Search crew members
    [HttpPost("search")]
    public async Task<IActionResult> SearchCrewMembers([FromBody] CrewSearchDto searchDto)
    {
        if (searchDto == null) return BadRequest("Invalid search criteria");
        
        var crewMembers = await _crewService.SearchCrewMembersAsync(searchDto);
        return Ok(crewMembers);
    }

    // Get crew members by rank
    [HttpGet("by-rank/{rank}")]
    public async Task<IActionResult> GetCrewMembersByRank(string rank, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(rank)) return BadRequest("Invalid rank");
        
        var crewMembers = await _crewService.GetCrewMembersByRankAsync(rank, page, pageSize);
        return Ok(crewMembers);
    }

    // Get crew members by nationality
    [HttpGet("by-nationality/{nationality}")]
    public async Task<IActionResult> GetCrewMembersByNationality(string nationality, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(nationality)) return BadRequest("Invalid nationality");
        
        var crewMembers = await _crewService.GetCrewMembersByNationalityAsync(nationality, page, pageSize);
        return Ok(crewMembers);
    }

    // Get crew statistics
    [HttpGet("statistics")]
    public async Task<IActionResult> GetCrewStatistics()
    {
        var stats = await _crewService.GetCrewStatisticsAsync();
        return Ok(stats);
    }

    // ===== CERTIFICATIONS ENDPOINTS =====
    
    // Get crew certifications
    [HttpGet("{id}/certifications")]
    public async Task<IActionResult> GetCrewCertifications(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
    {
        if (id <= 0) return BadRequest("Invalid crew member ID");
        
        var certifications = await _crewService.GetCrewCertificationsAsync(id, page, pageSize, status);
        return Ok(certifications);
    }

    // Create crew certification
    [HttpPost("certification/create")]
    public async Task<IActionResult> CreateCrewCertification([FromBody] CreateCrewCertificationDto dto)
    {
        if (dto == null || dto.UserId <= 0) return BadRequest("Invalid certification data");
        
        var result = await _crewService.CreateCrewCertificationAsync(dto);
        if (result > 0)
        {
            return Ok("Certification created successfully.");
        }
        else
        {
            return StatusCode(500, "Failed to create certification.");
        }
    }

    // Update crew certification
    [HttpPut("certification/update")]
    public async Task<IActionResult> UpdateCrewCertification([FromBody] UpdateCrewCertificationDto dto)
    {
        if (dto == null || dto.Id <= 0) return BadRequest("Invalid certification data");
        
        var result = await _crewService.UpdateCrewCertificationAsync(dto);
        if (result > 0)
        {
            return Ok("Certification updated successfully.");
        }
        else
        {
            return NotFound("Certification not found or update failed.");
        }
    }

    // Delete crew certification
    [HttpDelete("certification/delete/{id}")]
    public async Task<IActionResult> DeleteCrewCertification(int id)
    {
        if (id <= 0) return BadRequest("Invalid certification ID");
        
        var result = await _crewService.DeleteCrewCertificationAsync(id);
        if (result > 0)
        {
            return Ok("Certification deleted successfully.");
        }
        else
        {
            return NotFound("Certification not found or deletion failed.");
        }
    }

    // ===== VESSEL MANNING ENDPOINTS =====
    
    // Get all vessel manning positions
    [HttpGet("vessel-manning/all")]
    public async Task<IActionResult> GetAllVesselMannings([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] int? vesselId = null, [FromQuery] string? rank = null)
    {
        var mannings = await _crewService.GetAllVesselManningsAsync(page, pageSize, vesselId, rank);
        return Ok(mannings);
    }

     // Get vessel manning by ID
     [HttpGet("vessel-manning/{id}")]
     public async Task<IActionResult> GetVesselManningById(int id)
     {
         if (id <= 0) return BadRequest("Invalid vessel manning ID");
         
         var manning = await _crewService.GetVesselManningByIdAsync(id);
         if (manning == null) return NotFound("Vessel manning not found");
         
         return Ok(manning);
     }

    // Update vessel manning
    [HttpPut("vessel-manning/update/{id}")]
    public async Task<IActionResult> UpdateVesselManning(int id, [FromBody] UpdateVesselManningDto dto)
    {
        if (id <= 0 || dto == null) return BadRequest("Invalid data");
        
        var result = await _crewService.UpdateVesselManningAsync(id, dto);
        if (result > 0)
        {
            return Ok("Vessel manning updated successfully.");
        }
        else
        {
            return NotFound("Vessel manning not found or update failed.");
        }
    }

    // ===== CASH STATEMENT ENDPOINTS =====
    
    // Get cash statements
    [HttpGet("cash-statements")]
    public async Task<IActionResult> GetCashStatements([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] int? vesselId = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var statements = await _crewService.GetCashStatementsAsync(page, pageSize, vesselId, fromDate, toDate);
        return Ok(statements);
    }

    // Get cash statement by ID
    [HttpGet("cash-statements/{id}")]
    public async Task<IActionResult> GetCashStatementById(int id)
    {
        if (id <= 0) return BadRequest("Invalid cash statement ID");
        
        var statement = await _crewService.GetCashStatementByIdAsync(id);
        if (statement == null) return NotFound("Cash statement not found");
        
        return Ok(statement);
    }

    // Update cash statement
    [HttpPut("cash-statements/update/{id}")]
    public async Task<IActionResult> UpdateCashStatement(int id, [FromBody] UpdateCashStatementDto dto)
    {
        if (id <= 0 || dto == null) return BadRequest("Invalid data");
        
        var result = await _crewService.UpdateCashStatementAsync(id, dto);
        if (result > 0)
        {
            return Ok("Cash statement updated successfully.");
        }
        else
        {
            return NotFound("Cash statement not found or update failed.");
        }
    }

    // Delete cash statement
    [HttpDelete("cash-statements/delete/{id}")]
    public async Task<IActionResult> DeleteCashStatement(int id)
    {
        if (id <= 0) return BadRequest("Invalid cash statement ID");
        
        var result = await _crewService.DeleteCashStatementAsync(id);
        if (result > 0)
        {
            return Ok("Cash statement deleted successfully.");
        }
        else
        {
            return NotFound("Cash statement not found or deletion failed.");
        }
    }

    // ===== EXPENSE REPORT ENDPOINTS =====
    
    // Get expense reports
    [HttpGet("expense-reports")]
    public async Task<IActionResult> GetExpenseReports([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] int? crewMemberId = null, [FromQuery] int? shipId = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var reports = await _crewService.GetExpenseReportsAsync(page, pageSize, crewMemberId, shipId, fromDate, toDate);
        return Ok(reports);
    }

    // Get expense report by ID
    [HttpGet("expense-reports/{id}")]
    public async Task<IActionResult> GetExpenseReportById(int id)
    {
        if (id <= 0) return BadRequest("Invalid expense report ID");
        
        var report = await _crewService.GetExpenseReportByIdAsync(id);
        if (report == null) return NotFound("Expense report not found");
        
        return Ok(report);
    }

    // Update expense report
    [HttpPut("expense-reports/update/{id}")]
    public async Task<IActionResult> UpdateExpenseReport(int id, [FromBody] UpdateExpenseReportDto dto)
    {
        if (id <= 0 || dto == null) return BadRequest("Invalid data");
        
        var result = await _crewService.UpdateExpenseReportAsync(id, dto);
        if (result > 0)
        {
            return Ok("Expense report updated successfully.");
        }
        else
        {
            return NotFound("Expense report not found or update failed.");
        }
    }

    // Delete expense report
    [HttpDelete("expense-reports/delete/{id}")]
    public async Task<IActionResult> DeleteExpenseReport(int id)
    {
        if (id <= 0) return BadRequest("Invalid expense report ID");
        
        var result = await _crewService.DeleteExpenseReportAsync(id);
        if (result > 0)
        {
            return Ok("Expense report deleted successfully.");
        }
        else
        {
            return NotFound("Expense report not found or deletion failed.");
        }
    }

    // ===== PAYROLL ENDPOINTS =====
    
    // Get all payroll records
    [HttpGet("payroll/all")]
    public async Task<IActionResult> GetAllPayrollRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] int? crewMemberId = null, [FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null)
    {
        var payrolls = await _crewService.GetAllPayrollRecordsAsync(page, pageSize, crewMemberId, fromDate, toDate);
        return Ok(payrolls);
    }

     // Get payroll record by ID
     [HttpGet("payroll/{id}")]
     public async Task<IActionResult> GetPayrollRecordById(int id)
     {
         if (id <= 0) return BadRequest("Invalid payroll record ID");
         
         var payroll = await _crewService.GetPayrollRecordByIdAsync(id);
         if (payroll == null) return NotFound("Payroll record not found");
         
         return Ok(payroll);
     }

     // Update payroll record
     [HttpPut("payroll/update/{id}")]
     public async Task<IActionResult> UpdatePayrollRecord(int id, [FromBody] UpdatePayrollDto dto)
     {
         if (id <= 0 || dto == null) return BadRequest("Invalid data");
         
         var result = await _crewService.UpdatePayrollRecordAsync(id, dto);
         if (result > 0)
         {
             return Ok("Payroll record updated successfully.");
         }
         else
         {
             return NotFound("Payroll record not found or update failed.");
         }
     }

    // Delete payroll record
    [HttpDelete("payroll/delete/{id}")]
    public async Task<IActionResult> DeletePayrollRecord(int id)
    {
        if (id <= 0) return BadRequest("Invalid payroll record ID");
        
        var result = await _crewService.DeletePayrollRecordAsync(id);
        if (result > 0)
        {
            return Ok("Payroll record deleted successfully.");
        }
        else
        {
            return NotFound("Payroll record not found or deletion failed.");
        }
    }
}
