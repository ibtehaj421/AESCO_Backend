using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
	[Table("CrewReports")]
	public class CrewReport
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		[MaxLength(50)]
		public string ReportType { get; set; } = string.Empty; // StatementOfCash, CrewExpense, Payroll, Performance, etc.

		[Required]
		[MaxLength(200)]
		public string Title { get; set; } = string.Empty;

		[MaxLength(1000)]
		public string? Details { get; set; }

		public DateTime ReportDate { get; set; } = DateTime.UtcNow;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; } = null!;
	}
}

