using System.ComponentModel.DataAnnotations;


public class VesselManningDTO
{
    public int id { get; set; } //for updates

    [Required]
    public int VesselId { get; set; } // Foreign key to Vessel

    [Required]
    public List<string> Rank { get; set; } = new List<string>();

    public int count { get; set; } //number of crew required for this rank. This is mainly for removals.
}

public class VesselManningDeleteDTO
{
    [Required]
    public int VesselId { get; set; } // Foreign key to Vessel

    [Required]
    public List<string> Rank { get; set; } = new List<string>();
}