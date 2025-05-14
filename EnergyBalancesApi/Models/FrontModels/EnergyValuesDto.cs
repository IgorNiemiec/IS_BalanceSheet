namespace EnergyBalancesApi.Models.FrontModels
{
    public class EnergyValuesDto
    {

        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public int Year { get; set; }
        public string FlowCode { get; set; }
        public string FlowDescription { get; set; }

        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }

        public string Unit { get; set; }
        public double Amount { get; set; }


    }
}
