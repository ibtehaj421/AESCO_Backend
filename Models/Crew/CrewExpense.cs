using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ASCO.Models
{

    [Table("CrewExpenseReport")]
    public class CrewExpenseReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int CrewMemberId { get; set; } // Foreign key to Crew Member

        public int ShipId { get; set; } // Foreign key to Ship

        public decimal TotalAmount { get; set; }

        public string Currency { get; set; } = string.Empty; // e.g., USD, EUR

        public DateTime ReportDate { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(CrewMemberId))]
        public virtual User CrewMember { get; set; } = null!;
        [ForeignKey(nameof(ShipId))]
        public virtual Ship Ship { get; set; } = null!;
    }


    [Table("CrewExpenses")]
    public class CrewExpense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

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

        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(CrewMemberId))]
        public virtual User CrewMember { get; set; } = null!;
        [ForeignKey(nameof(ExpenseReportId))]
        public virtual CrewExpenseReport ExpenseReport { get; set; } = null!;

    }
}