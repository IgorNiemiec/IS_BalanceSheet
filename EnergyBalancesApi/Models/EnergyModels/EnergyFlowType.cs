namespace EnergyBalancesApi.Models.EnergyModels
{
    public class EnergyFlowType
    {

        public int Id { get; set; }
        public string Code { get; set; } 
        public string Description { get; set; } 

        public ICollection<EnergyValue> EnergyValues { get; set; }

    }
}
