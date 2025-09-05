using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
	[Table("CrewMedicalRecords")]
	public class CrewMedicalRecord
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int UserId { get; set; }

		[Required]
		[MaxLength(100)]
		public string ProviderName { get; set; } = string.Empty;

		[MaxLength(100)]
		public string? BloodGroup { get; set; }

		public DateTime ExaminationDate { get; set; }

		public DateTime? ExpiryDate { get; set; }

		[MaxLength(500)]
		public string? Notes { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; } = null!;
	}
}

