using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
	[Table("CrewVisas")]
	public class CrewVisa
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		[MaxLength(50)]
		public string VisaType { get; set; } = string.Empty;

		[MaxLength(50)]
		public string? Country { get; set; }

		public DateTime IssueDate { get; set; }
		public DateTime ExpiryDate { get; set; }

		[MaxLength(100)]
		public string? IssuedBy { get; set; }

		[MaxLength(200)]
		public string? Notes { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; } = null!;
	}
}

