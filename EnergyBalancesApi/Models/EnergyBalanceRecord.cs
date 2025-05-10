using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergyBalancesApi.Models
{
    [Table("EnergyBalanceRecord")]
    public class EnergyBalanceRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public decimal FossilFuelKtoe { get; set; }

        [Required]
        public decimal RenewableKtoe { get; set; }

        // Foreign Key do Country
        [ForeignKey(nameof(Country))]                   // wskazuje na właściwość Country :contentReference[oaicite:7]{index=7}
        public int CountryId { get; set; }

        [Required]
        public Country Country { get; set; } = null!;
    }
}
