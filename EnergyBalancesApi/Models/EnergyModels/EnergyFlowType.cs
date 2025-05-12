namespace EnergyBalancesApi.Models.EnergyModels
{
    public class EnergyFlowType
    {

        public int Id { get; set; }
        public string Code { get; set; } // np. "PPRD"
        public string Description { get; set; } // np. "Primary production"

        public ICollection<EnergyValue> EnergyValues { get; set; }

    }
}
