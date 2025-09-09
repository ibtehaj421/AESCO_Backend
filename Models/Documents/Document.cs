using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


//all models to be used under the document system go here.

namespace ASCO.Models
{
    [Table("Documents")]
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty; // display name

        [MaxLength(10)]
        public string? Extension { get; set; } // .pdf, .docx, etc.

        [Required]
        [MaxLength(500)]
        public string PhysicalPath { get; set; } = string.Empty; // server file system path

        public long SizeInBytes { get; set; }

        [MaxLength(100)]
        public string? MimeType { get; set; } // application/pdf, image/png, etc.

        public bool IsFolder { get; set; } = false;

        // Hierarchy (self-reference)
        public Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Document? Parent { get; set; }

        public ICollection<Document> Children { get; set; } = new List<Document>();

        // Tracking
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CreatedById { get; set; } //id of the user who created the document.

        public DateTime? UpdatedAt { get; set; } //date and time of when the document was last updated.
        public int UpdatedById { get; set; } //id of the user who last updated the document.

        // Versions
        public ICollection<DocumentVersion> Versions { get; set; } = new List<DocumentVersion>();

        // Logs
        public ICollection<DocumentLog> Logs { get; set; } = new List<DocumentLog>();


        //List of approvals.
        public ICollection<DocumentApproval> Approvals { get; set; } = new List<DocumentApproval>();



        [ForeignKey(nameof(CreatedById))]
        public virtual User CreatedBy { get; set; } = null!;

        [ForeignKey(nameof(UpdatedById))]
        public virtual User UpdatedBy { get; set; } = null!;

    }

    [Table("DocumentText")]
    public class DocumentText
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document? Document { get; set; } = null!;
        public Guid? DocumentId { get; set; } //reference document id.

        public Guid? VersionedDocumentId { get; set; }

        [ForeignKey("VersionedDocumentId")]
        
        public virtual DocumentVersion? DocumentVersion { get; set; } = null!; 

        public string Content { get; set; } = string.Empty;

        
    }


    [Table("DocumentVersions")]
    public class DocumentVersion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string PhysicalPath { get; set; } = string.Empty; // versioned path

        [MaxLength(10)]
        public string? Extension { get; set; }

        public long SizeInBytes { get; set; }

        public DateTime VersionDate { get; set; } = DateTime.UtcNow;
        public int ChangedBy { get; set; }

        [ForeignKey("ChangedBy")]
        public virtual User Changed { get; set; } = null!;
        public string? ChangeDescription { get; set; } // e.g., "Updated contract terms"

        public int VersionNumber { get; set; }
    }

    [Table("DocumentLogs")]
    public class DocumentLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; } = null!;

        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
        public int ActionById { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActionType { get; set; } = string.Empty;
        // e.g., "Upload", "Update", "Delete", "View", "Restore"

        [MaxLength(500)]
        public string? Notes { get; set; }

        [ForeignKey(nameof(ActionById))]
        public virtual User ActionBy { get; set; } = null!;
    }

    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }
    [Table("DocumentApprovals")]
    public class DocumentApproval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; } = null!;

        [Required]
        public int ApproverId { get; set; }
        [ForeignKey(nameof(ApproverId))]
        public User Approver { get; set; } = null!;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}