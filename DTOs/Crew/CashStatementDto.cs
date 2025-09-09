using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs.Crew
{
    public class CashStatementDto
    {
        public int Id { get; set; }
        public int VesselId { get; set; }
        public string VesselName { get; set; } = string.Empty;
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Inflow { get; set; }
        public decimal Outflow { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateCashStatementDto
    {
        [Required]
        public int VesselId { get; set; }
        
        [Required]
        public int CreatedById { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [Required]
        public DateTime TransactionDate { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Inflow { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Outflow { get; set; }
        
        [Required]
        public decimal Balance { get; set; }
    }
}
