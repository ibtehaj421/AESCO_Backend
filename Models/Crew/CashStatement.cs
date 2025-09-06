using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASCO.Models;


    [Table("StatementOfCash")]
    public class StatementOfCash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign Key to Vessel
        public int VesselId { get; set; }

        [ForeignKey("VesselId")]
        public Ship Vessel { get; set; } = null!;  // navigation property
        public int CreatedById { get; set; } // Foreign Key to User who created the statement
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; } = null!; // navigation property
        
        [Required]
        public string? status { get; set; } // e.g., "Draft", "Finalized"
        // Transaction date
        [Required]
        public DateTime TransactionDate { get; set; }

        // Description (e.g., "Cash to Master", "Crew Wages", "Port Charges")
        [MaxLength(200)]
        public string? Description { get; set; }

        // Money coming in
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Inflow { get; set; }

        // Money going out
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Outflow { get; set; }

        // Balance after transaction
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }