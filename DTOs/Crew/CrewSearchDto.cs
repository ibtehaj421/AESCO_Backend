using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs.Crew
{
    public class CrewSearchDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Nationality { get; set; }
        public string? Rank { get; set; }
        public string? JobType { get; set; }
        public string? Status { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
