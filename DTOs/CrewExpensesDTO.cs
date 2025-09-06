using System.ComponentModel.DataAnnotations;


namespace ASCO.DTOs
{
    public class CreateCrewExpenseReportDto
    {
        [Required]
        public int CrewMemberId { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;


        [MaxLength(200)]
        public string? Description { get; set; }

        //public DateTime? ExpenseDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


    public class CreateCrewExpenseDto
    {
        [Required]
        public long ExpenseReportId { get; set; } // Foreign key to CrewExpenseReport

        [Required]
        public int CrewMemberId { get; set; } // Foreign key to Crew Member

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty; // e.g., Travel, Medical, Miscellaneous

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty; // e.g., USD, EUR

        public DateTime? ExpenseDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


    public class StatementOfCashCreateDto
    {
        public int VesselId { get; set; }
        public int CreatedById { get; set; }
        public DateTime TransactionDate { get; set; }

        public string? status;
        public string? Description { get; set; }
        public decimal? Inflow { get; set; }
        public decimal? Outflow { get; set; }

    }

    public class StatementOfCashUpdateDto
    {
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public decimal? Inflow { get; set; }
        public decimal? Outflow { get; set; }
        public decimal Balance { get; set; }
        public string? Status { get; set; }
    }
    
    public class StatementOfCashReadDto
    {
        public int Id { get; set; }
        public int VesselId { get; set; }
        public string? VesselName { get; set; }   // Optional: map from Vessel entity
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public decimal? Inflow { get; set; }
        public decimal? Outflow { get; set; }
        public decimal Balance { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}