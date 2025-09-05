//leave this mty for now.
//will configure this later.
using System.ComponentModel.DataAnnotations;
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