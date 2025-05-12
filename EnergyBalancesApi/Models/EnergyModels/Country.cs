namespace EnergyBalancesApi.Models.EnergyModels
{
    public class Country
    {
        public int Id { get; set; }
        public string Code { get; set; } // np. "PL"
        public string Name { get; set; } // np. "Poland"

        public ICollection<EnergyValue> EnergyValues { get; set; }
    }
}
