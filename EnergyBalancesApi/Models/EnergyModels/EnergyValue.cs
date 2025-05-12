namespace EnergyBalancesApi.Models.EnergyModels
{
    public class EnergyValue
    {

        public int Id { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int ProductId { get; set; }
        public EnergyProduct Product { get; set; }

        public int FlowTypeId { get; set; }
        public EnergyFlowType FlowType { get; set; }

        public int Year { get; set; }

        public string Unit { get; set; } // np. "KTOE"

        public double Amount { get; set; }



    }
}
