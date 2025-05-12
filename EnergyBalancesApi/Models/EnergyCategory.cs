namespace EnergyBalancesApi.Models
{
    public class EnergyCategory
    {
        public string CategoryName { get; set; }
        public List<EnergyDataEntry> Entries { get; set; } = new();
    }
}
