using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASCO.Models
{
    [Table("CrewTrainings")]
    public class CrewTraining
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public int VesselId { get; set; }

        [ForeignKey(nameof(VesselId))]
        public virtual Ship Vessel { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string TrainingCategory { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Rank { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Trainer { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Remark { get; set; }

        [Required]
        [StringLength(100)]
        public string Training { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Source { get; set; } = string.Empty;

        [Required]
        public DateTime TrainingDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Expired, Cancelled

        [StringLength(200)]
        public string? Attachments { get; set; } // File paths or URLs

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User CreatedBy { get; set; } = null!;
        public int CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User CreatedByUser { get; set; } = null!;
    }

    [Table("CrewEvaluations")]
    public class CrewEvaluation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public int VesselId { get; set; }

        [ForeignKey(nameof(VesselId))]
        public virtual Ship Vessel { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string FormNo { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string RevisionNo { get; set; } = string.Empty;

        [Required]
        public DateTime RevisionDate { get; set; }

        [Required]
        [StringLength(100)]
        public string FormName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? FormDescription { get; set; }

        [Required]
        public int EnteredByUserId { get; set; }

        [ForeignKey(nameof(EnteredByUserId))]
        public virtual User EnteredBy { get; set; } = null!;

        [Required]
        public DateTime EnteredDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Rank { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Surname { get; set; } = string.Empty;

        [StringLength(50)]
        public string? UniqueId { get; set; }

        // Evaluation criteria scores (1-5 scale)
        [Range(1, 5)]
        public int? TechnicalCompetence { get; set; }

        [Range(1, 5)]
        public int? SafetyAwareness { get; set; }

        [Range(1, 5)]
        public int? Teamwork { get; set; }

        [Range(1, 5)]
        public int? Communication { get; set; }

        [Range(1, 5)]
        public int? Leadership { get; set; }

        [Range(1, 5)]
        public int? ProblemSolving { get; set; }

        [Range(1, 5)]
        public int? Adaptability { get; set; }

        [Range(1, 5)]
        public int? WorkEthic { get; set; }

        [Range(1, 5)]
        public int? OverallRating { get; set; }

        [StringLength(1000)]
        public string? Strengths { get; set; }

        [StringLength(1000)]
        public string? AreasForImprovement { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }

        [StringLength(1000)]
        public string? CrewMemberComments { get; set; }

        [StringLength(100)]
        public string? CrewMemberSignature { get; set; }

        public DateTime? CrewMemberSignedDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Draft"; // Draft, Completed, Signed, Archived

        [StringLength(200)]
        public string? Attachments { get; set; } // File paths or URLs

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual User CreatedBy { get; set; } = null!;
        public int CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User CreatedByUser { get; set; } = null!;
    }
}
