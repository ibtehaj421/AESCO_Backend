using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs.Crew
{
    public class ExpenseReportDto
    {
        public int Id { get; set; }
        public int CrewMemberId { get; set; }
        public string CrewMemberName { get; set; } = string.Empty;
        public int ShipId { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime ReportDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CrewExpenseDto> Expenses { get; set; } = new();
    }

    public class UpdateExpenseReportDto
    {
        [Required]
        public int CrewMemberId { get; set; }
        
        [Required]
        public int ShipId { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;
        
        [Required]
        public DateTime ReportDate { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
