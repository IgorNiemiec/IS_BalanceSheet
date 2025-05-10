using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergyBalancesApi.Models
{
    public class Country
    {
        [Key]                                          // klucz główny :contentReference[oaicite:4]{index=4}
        public int Id { get; set; }

        [Required]                                     // nie może być null :contentReference[oaicite:5]{index=5}
        [MaxLength(100)]                               // ograniczenie długości
        public string Name { get; set; } = null!;

        // Relacja 1:N: jeden Country ma wiele EnergyBalanceRecords :contentReference[oaicite:6]{index=6}
        public ICollection<EnergyBalanceRecord> EnergyBalanceRecords { get; set; } = new List<EnergyBalanceRecord>();
    }
}
