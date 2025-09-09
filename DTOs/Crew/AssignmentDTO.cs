using System.ComponentModel.DataAnnotations;

namespace ASCO.DTOs.Crew
{
    public class AssignmentDTO
{
    public int id { get; set; } //assignment id for updates

    [Required]
    public int CrewId { get; set; }

    [Required]
    public int VesselId { get; set; }

    [Required]
    public int AssignedByUserId { get; set; } //the user who is making the assignment.
    public DateTime? AssignmentDate { get; set; } //use current time when creating assignment.

    public DateTime? EndDate { get; set; } //nullable for ongoing assignments. but it is editable.
    
    public string Notes { get; set; } = string.Empty;
    }
}