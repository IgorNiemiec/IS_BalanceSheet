using System.ComponentModel.DataAnnotations;

namespace EnergyBalancesApi.Models
{


    public class SourceMetadata
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SourceName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
