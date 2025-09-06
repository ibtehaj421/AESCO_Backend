//each module will have its own databank class.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//this class will contain all the models related to that module.

namespace ASCO.Models
{
    public class CrewModuleMain
    {
        public int Id { get; set; }
        public string? FieldName { get; set; } // Made nullable

        // Navigation property for related databank entries
        public ICollection<CrewModuleDatabank> Databanks { get; set; } = new List<CrewModuleDatabank>(); // Initialized collection
    }

    public class CrewModuleDatabank
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string? SubTypeName { get; set; } // Made nullable

        // Navigation property for the main field
        public CrewModuleMain? Field { get; set; } // Made nullable
    }
}