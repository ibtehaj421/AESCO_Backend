using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ASCO.Models
{
    [Table("Forms")]
    public class Form
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)] //optional description to give more context about the form
        public string? Description { get; set; }


    }

    [Table("FormFields")]
    public class FormField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FormId { get; set; }

        [ForeignKey("FormId")]
        public Form? Form { get; set; }

        [Required]
        [StringLength(100)]
        public string? FieldName { get; set; }

        [Required]
        [StringLength(50)]
        public string? FieldType { get; set; } // e.g., Text, Number, Date, etc.

        [StringLength(500)] //optional help text to guide users on how to fill out the field
        public string? HelpText { get; set; }

        public string? ValidValue { get; set; } //specifies a valid value or pattern for the field
        public bool IsRequired { get; set; } // Indicates if the field is mandatory
    }
}