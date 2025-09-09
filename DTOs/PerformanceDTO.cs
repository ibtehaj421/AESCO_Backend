using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs
{
    // Crew Training DTOs
    public class CreateCrewTrainingDto
    {
        [Required(ErrorMessage = "Crew member is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vessel is required")]
        public int VesselId { get; set; }

        [Required(ErrorMessage = "Training category is required")]
        [StringLength(100, ErrorMessage = "Training category cannot exceed 100 characters")]
        public string TrainingCategory { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rank is required")]
        [StringLength(50, ErrorMessage = "Rank cannot exceed 50 characters")]
        public string Rank { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trainer is required")]
        [StringLength(100, ErrorMessage = "Trainer cannot exceed 100 characters")]
        public string Trainer { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Remark cannot exceed 500 characters")]
        public string? Remark { get; set; }

        [Required(ErrorMessage = "Training is required")]
        [StringLength(100, ErrorMessage = "Training cannot exceed 100 characters")]
        public string Training { get; set; } = string.Empty;

        [Required(ErrorMessage = "Source is required")]
        [StringLength(50, ErrorMessage = "Source cannot exceed 50 characters")]
        public string Source { get; set; } = string.Empty;

        [Required(ErrorMessage = "Training date is required")]
        public DateTime TrainingDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters")]
        public string Status { get; set; } = "Pending";

        [StringLength(200, ErrorMessage = "Attachments cannot exceed 200 characters")]
        public string? Attachments { get; set; }
    }

    public class UpdateCrewTrainingDto : CreateCrewTrainingDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CrewTrainingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string FullName => $"{UserName} {UserSurname}";
        public int VesselId { get; set; }
        public string VesselName { get; set; } = string.Empty;
        public string TrainingCategory { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public string Trainer { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public string Training { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime TrainingDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Attachments { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsExpiringSoon { get; set; } // calculated property
        public int DaysUntilExpiry { get; set; } // calculated property
    }

    // Crew Evaluation DTOs
    public class CreateCrewEvaluationDto
    {
        [Required(ErrorMessage = "Crew member is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vessel is required")]
        public int VesselId { get; set; }

        [Required(ErrorMessage = "Form number is required")]
        [StringLength(50, ErrorMessage = "Form number cannot exceed 50 characters")]
        public string FormNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Revision number is required")]
        [StringLength(20, ErrorMessage = "Revision number cannot exceed 20 characters")]
        public string RevisionNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Revision date is required")]
        public DateTime RevisionDate { get; set; }

        [Required(ErrorMessage = "Form name is required")]
        [StringLength(100, ErrorMessage = "Form name cannot exceed 100 characters")]
        public string FormName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Form description cannot exceed 1000 characters")]
        public string? FormDescription { get; set; }

        [Required(ErrorMessage = "Entered by user is required")]
        public int EnteredByUserId { get; set; }

        [Required(ErrorMessage = "Entered date is required")]
        public DateTime EnteredDate { get; set; }

        [Required(ErrorMessage = "Rank is required")]
        [StringLength(50, ErrorMessage = "Rank cannot exceed 50 characters")]
        public string Rank { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        public string Surname { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Unique ID cannot exceed 50 characters")]
        public string? UniqueId { get; set; }

        // Evaluation criteria scores (1-5 scale)
        [Range(1, 5, ErrorMessage = "Technical competence must be between 1 and 5")]
        public int? TechnicalCompetence { get; set; }

        [Range(1, 5, ErrorMessage = "Safety awareness must be between 1 and 5")]
        public int? SafetyAwareness { get; set; }

        [Range(1, 5, ErrorMessage = "Teamwork must be between 1 and 5")]
        public int? Teamwork { get; set; }

        [Range(1, 5, ErrorMessage = "Communication must be between 1 and 5")]
        public int? Communication { get; set; }

        [Range(1, 5, ErrorMessage = "Leadership must be between 1 and 5")]
        public int? Leadership { get; set; }

        [Range(1, 5, ErrorMessage = "Problem solving must be between 1 and 5")]
        public int? ProblemSolving { get; set; }

        [Range(1, 5, ErrorMessage = "Adaptability must be between 1 and 5")]
        public int? Adaptability { get; set; }

        [Range(1, 5, ErrorMessage = "Work ethic must be between 1 and 5")]
        public int? WorkEthic { get; set; }

        [Range(1, 5, ErrorMessage = "Overall rating must be between 1 and 5")]
        public int? OverallRating { get; set; }

        [StringLength(1000, ErrorMessage = "Strengths cannot exceed 1000 characters")]
        public string? Strengths { get; set; }

        [StringLength(1000, ErrorMessage = "Areas for improvement cannot exceed 1000 characters")]
        public string? AreasForImprovement { get; set; }

        [StringLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters")]
        public string? Comments { get; set; }

        [StringLength(1000, ErrorMessage = "Crew member comments cannot exceed 1000 characters")]
        public string? CrewMemberComments { get; set; }

        [StringLength(100, ErrorMessage = "Crew member signature cannot exceed 100 characters")]
        public string? CrewMemberSignature { get; set; }

        public DateTime? CrewMemberSignedDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(30, ErrorMessage = "Status cannot exceed 30 characters")]
        public string Status { get; set; } = "Draft";

        [StringLength(200, ErrorMessage = "Attachments cannot exceed 200 characters")]
        public string? Attachments { get; set; }
    }

    public class UpdateCrewEvaluationDto : CreateCrewEvaluationDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CrewEvaluationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string FullName => $"{UserName} {UserSurname}";
        public int VesselId { get; set; }
        public string VesselName { get; set; } = string.Empty;
        public string FormNo { get; set; } = string.Empty;
        public string RevisionNo { get; set; } = string.Empty;
        public DateTime RevisionDate { get; set; }
        public string FormName { get; set; } = string.Empty;
        public string? FormDescription { get; set; }
        public string EnteredByName { get; set; } = string.Empty;
        public DateTime EnteredDate { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? UniqueId { get; set; }
        public int? TechnicalCompetence { get; set; }
        public int? SafetyAwareness { get; set; }
        public int? Teamwork { get; set; }
        public int? Communication { get; set; }
        public int? Leadership { get; set; }
        public int? ProblemSolving { get; set; }
        public int? Adaptability { get; set; }
        public int? WorkEthic { get; set; }
        public int? OverallRating { get; set; }
        public string? Strengths { get; set; }
        public string? AreasForImprovement { get; set; }
        public string? Comments { get; set; }
        public string? CrewMemberComments { get; set; }
        public string? CrewMemberSignature { get; set; }
        public DateTime? CrewMemberSignedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Attachments { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Crew Work Rest Hours DTOs
    public class CreateCrewWorkRestHoursDto
    {
        [Required(ErrorMessage = "Crew member is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vessel is required")]
        public int VesselId { get; set; }

        [Required]
        [Range(2000, 3000)]
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(1, 31)]
        public int Day { get; set; }

        [Required]
        [Range(0, 23)]
        public int Hour { get; set; }

        [Required]
        public bool IsWorking { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }

    public class UpdateCrewWorkRestHoursDto : CreateCrewWorkRestHoursDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CrewWorkRestHoursDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string FullName => $"{UserName} {UserSurname}";
        public int VesselId { get; set; }
        public string VesselName { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public bool IsWorking { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Search DTOs
    public class CrewTrainingSearchDto
    {
        public string? SearchTerm { get; set; }
        public int? UserId { get; set; }
        public int? VesselId { get; set; }
        public string? TrainingCategory { get; set; }
        public string? Status { get; set; }
        public DateTime? TrainingDateFrom { get; set; }
        public DateTime? TrainingDateTo { get; set; }
        public DateTime? ExpireDateFrom { get; set; }
        public DateTime? ExpireDateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "TrainingDate";
        public bool SortDescending { get; set; } = true;
    }

    public class CrewEvaluationSearchDto
    {
        public string? SearchTerm { get; set; }
        public int? UserId { get; set; }
        public int? VesselId { get; set; }
        public string? Status { get; set; }
        public DateTime? EnteredDateFrom { get; set; }
        public DateTime? EnteredDateTo { get; set; }
        public int? MinOverallRating { get; set; }
        public int? MaxOverallRating { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "EnteredDate";
        public bool SortDescending { get; set; } = true;
    }

    public class CrewWorkRestHoursSearchDto
    {
        public int? UserId { get; set; }
        public int? VesselId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? DayFrom { get; set; }
        public int? DayTo { get; set; }
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "Year,Month,Day,Hour";
        public bool SortDescending { get; set; } = true;
    }
}
