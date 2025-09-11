using System.ComponentModel.DataAnnotations;
using ASCO.DTOs;
using ASCO.DTOs.Crew;
using ASCO.Models;
public abstract class BaseUserDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(30, ErrorMessage = "Surname cannot exceed 30 characters")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nationality is required")]
    [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters")]
    public string Nationality { get; set; } = string.Empty;

    [Required(ErrorMessage = "Identity number is required")]
    [StringLength(20, ErrorMessage = "Identity number cannot exceed 20 characters")]
    public string IdenNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }

    [StringLength(100, ErrorMessage = "Birth place cannot exceed 100 characters")]
    public string? BirthPlace { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    [RegularExpression("^[MF]$", ErrorMessage = "Gender must be M or F")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job type is required")]
    [StringLength(20, ErrorMessage = "Job type cannot exceed 20 characters")]
    public string JobType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rank is required")]
    [StringLength(50, ErrorMessage = "Rank cannot exceed 50 characters")]
    public string Rank { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Marital status cannot exceed 20 characters")]
    public string? MaritalStatus { get; set; }

    [StringLength(30, ErrorMessage = "Military status cannot exceed 30 characters")]
    public string? MilitaryStatus { get; set; }

    [StringLength(30, ErrorMessage = "Education level cannot exceed 30 characters")]
    public string? EducationLevel { get; set; }

    [Range(1900, 3000, ErrorMessage = "Graduation year must be between 1900 and 3000")]
    public int? GraduationYear { get; set; }

    [StringLength(100, ErrorMessage = "School name cannot exceed 100 characters")]
    public string? School { get; set; }

    [Required(ErrorMessage = "Competency is required")]
    [StringLength(100, ErrorMessage = "Competency cannot exceed 100 characters")]
    public string Competency { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Organization unit cannot exceed 100 characters")]
    public string? OrganizationUnit { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Father name cannot exceed 50 characters")]
    public string? FatherName { get; set; }

    public DateTime? WorkEndDate { get; set; }
}

// DTO for creating a new user (from frontend)
public class CreateUserDto : BaseUserDto
{
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$", 
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Roles to assign (role IDs)
    public List<int> RoleIds { get; set; } = new List<int>();

    // Initial status (optional, defaults to "pending")
    public string? Status { get; set; } = "pending";
}

// DTO for updating user information
public class UpdateUserDto : BaseUserDto
{
    [Required]
    public int Id { get; set; }

    // Optional password change
    // [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&+$]", 
    //     ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character")]
    // public string? Password { get; set; }


    public List<int> RoleIds { get; set; } = new List<int>();

    // Status can be updated
    [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
    public string? Status { get; set; }
}

// DTO for returning user information (to frontend)
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string FullName => $"{Name} {Surname}";
    public string Nationality { get; set; } = string.Empty;
    public string IdenNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? BirthPlace { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string Rank { get; set; } = string.Empty;
    public string? MaritalStatus { get; set; }
    public string? MilitaryStatus { get; set; }
    public string? EducationLevel { get; set; }
    public int? GraduationYear { get; set; }
    public string? School { get; set; }
    public string Competency { get; set; } = string.Empty;
    public string? OrganizationUnit { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FatherName { get; set; }
    public DateTime? WorkEndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool EmailConfirmed { get; set; }

    // Role information
    public List<UserRoleDto> Roles { get; set; } = new List<UserRoleDto>();

    // CrewList extras
    public string? PassportNumber { get; set; }
    public DateTime? PassportExpiry { get; set; }
    public string? WhereEmbarked { get; set; }
    public DateTime? WhenEmbarked { get; set; }
    public int? AssignmentId { get; set; } // For assignment operations
    
    // Assignment-related properties
    public string? Position { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? UnassignedAt { get; set; }
    public string? Notes { get; set; }
}

// DTO for user roles
public class UserRoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? RoleDescription { get; set; }
    public DateTime AssignedAt { get; set; }
    public string AssignedByName { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
}

// DTO for user listing/search results
public class UserSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string FullName => $"{Name} {Surname}";
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string Rank { get; set; } = string.Empty;
    public string? OrganizationUnit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public List<string> RoleNames { get; set; } = new List<string>();
}

// DTO for login requests
public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}

// DTO for login response
public class LoginResponseDto
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiry { get; set; }
    public UserDto? User { get; set; }
    public string? Message { get; set; }
}

// DTO for password reset request
public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
}

// DTO for password reset
public class ResetPasswordDto
{
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

// DTO for changing password (authenticated user)
public class ChangePasswordDto
{
    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]", 
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

// DTO for role assignment/management
public class AssignRoleDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public List<int> RoleIds { get; set; } = new List<int>();

    public DateTime? ExpiresAt { get; set; }
}

// DTO for API responses
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}

// DTO for paginated results
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}

// DTO for user search/filter parameters
public class UserSearchDto
{
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public string? JobType { get; set; }
    public string? OrganizationUnit { get; set; }
    public List<int>? RoleIds { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "CreatedAt";
    public bool SortDescending { get; set; } = true;
}

public class CrewDocumentSearchDto
{
    public int? UserId { get; set; }
    public string? Keyword { get; set; }
    public DateTime? ExpiryFrom { get; set; }
    public DateTime? ExpiryTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

// Aggregated Crew Profile DTOs
public class CrewProfileDto
{
    public UserDto PersonalInfo { get; set; } = new UserDto();
    public List<CrewPassport> Passports { get; set; } = new List<CrewPassport>();
    public List<CrewVisa> Visas { get; set; } = new List<CrewVisa>();
    public List<CrewMedicalRecord> MedicalRecords { get; set; } = new List<CrewMedicalRecord>();
    public List<CrewReport> Reports { get; set; } = new List<CrewReport>();
    public List<Payroll> Payrolls { get; set; } = new List<Payroll>();
    public List<CrewExpense> Expenses { get; set; } = new List<CrewExpense>();
    public List<ShipAssignment> AssignmentHistory { get; set; } = new List<ShipAssignment>();
    public List<CrewTrainingDto> Trainings { get; set; } = new List<CrewTrainingDto>();
    public List<CrewEvaluation> Evaluations { get; set; } = new List<CrewEvaluation>();

    // Placeholders for sections not explicitly modeled yet
    // Supplies, Leaves, Attachments can be wired when models are available
}

public class CrewFilterDto
{
    public string? Keyword { get; set; }
    public string? RankGroup { get; set; }
    public string? Rank { get; set; }
    public string? Competency { get; set; }
    public string? Nationality { get; set; }
    public int? YearsWithOperatorMin { get; set; }
    public int? YearsWithOperatorMax { get; set; }
    public int? YearsInRankMin { get; set; }
    public int? YearsInRankMax { get; set; }
    public bool? CurrentExperience { get; set; }
    public bool? PastExperience { get; set; }
    public string? CrewStatus { get; set; } // Active, Contingent, Left
    public string? JobType { get; set; } // Contract base, Fixed Term, etc.
    public string? AssignmentStatus { get; set; } // On Board, On Vacation
    public int? VesselId { get; set; }
    public DateTime? WorkStartDateFrom { get; set; }
    public DateTime? WorkStartDateTo { get; set; }
    public DateTime? WorkEndDateFrom { get; set; }
    public DateTime? WorkEndDateTo { get; set; }
    public int? AgeFrom { get; set; }
    public int? AgeTo { get; set; }
    public string? BloodGroup { get; set; }
    public string? EducationLevel { get; set; }
    public string? School { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "CreatedAt";
    public bool SortDescending { get; set; } = true;
}

public class CreateCrewMedicalDto
{
    [Required]
    public int UserId { get; set; }
    [Required, StringLength(100)]
    public string ProviderName { get; set; } = string.Empty;
    [StringLength(100)]
    public string? BloodGroup { get; set; }
    [Required]
    public DateTime ExaminationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    [StringLength(500)]
    public string? Notes { get; set; }
}

public class CrewMedicalDto : CreateCrewMedicalDto
{
    [Required]
    public int Id { get; set; }
}

public class CreateCrewPassportDto
{
    [Required]
    public int UserId { get; set; }
    [Required, StringLength(50)]
    public string PassportNumber { get; set; } = string.Empty;
    [StringLength(50)]
    public string? Nationality { get; set; }
    [Required]
    public DateTime IssueDate { get; set; }
    [Required]
    public DateTime ExpiryDate { get; set; }
    [StringLength(100)]
    public string? IssuedBy { get; set; }
    [StringLength(200)]
    public string? Notes { get; set; }
}

public class CrewPassportDto : CreateCrewPassportDto
{
    [Required]
    public int Id { get; set; }
}

public class CreateCrewVisaDto
{
    [Required]
    public int UserId { get; set; }
    [Required, StringLength(50)]
    public string VisaType { get; set; } = string.Empty;
    [StringLength(50)]
    public string? Country { get; set; }
    [Required]
    public DateTime IssueDate { get; set; }
    [Required]
    public DateTime ExpiryDate { get; set; }
    [StringLength(100)]
    public string? IssuedBy { get; set; }
    [StringLength(200)]
    public string? Notes { get; set; }
}

public class CrewVisaDto : CreateCrewVisaDto
{
    [Required]
    public int Id { get; set; }
}

public class CreateCrewReportDto
{
    [Required]
    public int UserId { get; set; }
    [Required, StringLength(50)]
    public string ReportType { get; set; } = string.Empty;
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;
    [StringLength(1000)]
    public string? Details { get; set; }
    public DateTime? ReportDate { get; set; }
}

public class CrewReportDto : CreateCrewReportDto
{
    [Required]
    public int Id { get; set; }
}

// Create aggregated crew profile (input from frontend)
public class CreateCrewProfileDto
{
    [Required]
    public CreateUserDto PersonalInfo { get; set; } = new CreateUserDto();

    public List<CreateCrewPassportDto>? Passports { get; set; }
    public List<CreateCrewVisaDto>? Visas { get; set; }
    public List<CreateCrewMedicalDto>? MedicalRecords { get; set; }
    public List<CreateCrewReportDto>? Reports { get; set; }
    public List<CreatePayrollDto>? Payrolls { get; set; }
    public List<CreateCrewExpenseDto>? Expenses { get; set; }
    public List<AssignmentDTO>? Assignments { get; set; }
    public List<CreateCrewTrainingDto>? Trainings { get; set; }
    public List<CreateCrewEvaluationDto>? Evaluations { get; set; }
}

public class CreateCrewProfileResultDto
{
    public int UserId { get; set; }
    public int CreatedPassports { get; set; }
    public int CreatedVisas { get; set; }
    public int CreatedMedical { get; set; }
    public int CreatedReports { get; set; }
    public int CreatedPayrolls { get; set; }
    public int CreatedExpenses { get; set; }
    public int CreatedAssignments { get; set; }
    public int CreatedTrainings { get; set; }
    public int CreatedEvaluations { get; set; }
}