namespace EnergyBalancesApi.Models.EnergyModels
{
    public class EnergyProduct
    {

        public int Id { get; set; }
        public string Code { get; set; } // np. "G3000"
        public string Description { get; set; } // np. "Natural gas"

        public ICollection<EnergyValue> EnergyValues { get; set; }


    }
}
