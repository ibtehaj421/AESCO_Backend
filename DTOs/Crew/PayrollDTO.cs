using System.ComponentModel.DataAnnotations;
using ASCO.Models;

namespace ASCO.DTOs.Crew
{
    public class PayrollDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal Allowances { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreatePayrollDto
    {
        [Required]
        public int CrewMemberId { get; set; }
        
        [Required]
        public DateTime PeriodStart { get; set; }
        
        [Required]
        public DateTime PeriodEnd { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal BaseWage { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Overtime { get; set; } = 0;
        
        [Range(0, double.MaxValue)]
        public decimal Bonuses { get; set; } = 0;
        
        [Range(0, double.MaxValue)]
        public decimal Deductions { get; set; } = 0;
        
        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "USD";
        
        [Required]
        public DateTime PaymentDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class UpdatePayrollDto
    {
        [Required]
        public int CrewMemberId { get; set; }

        [Required]
        public DateTime PeriodStart { get; set; }
        
        [Required]
        public DateTime PeriodEnd { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal BaseWage { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Overtime { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Bonuses { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Deductions { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = "USD";
        
        [Required]
        public DateTime PaymentDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "Bank Transfer";
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}