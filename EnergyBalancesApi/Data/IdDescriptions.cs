namespace EnergyBalancesApi.Helpers
{
    public static class IdDescriptions
    {
        public static readonly Dictionary<string, string> ProductMap = new()
        {
            ["0"] = "Total",
            ["1"] = "Solid fossil fuels",
            ["2"] = "Anthracite",
            ["3"] = "Coking coal",
            ["4"] = "Other bituminous coal",
            ["5"] = "Sub-bituminous coal",
            ["6"] = "Lignite",
            ["17"] = "Patent fuel",
            ["18"] = "Coke oven coke",
            ["21"] = "Natural gas",
            ["22"] = "Crude oil",
            ["23"] = "Oil shale and oil sands",
            ["24"] = "Refinery feedstocks",
            ["26"] = "Refinery gas",
            ["27"] = "Ethane",
            ["45"] = "Renewables and biofuels",
            ["46"] = "Hydro",
            ["47"] = "Geothermal",
            ["48"] = "Solar",
            ["49"] = "Tide, wave, ocean",
            ["50"] = "Wind",
            ["51"] = "Biofuels",
            ["52"] = "Waste",
            ["53"] = "Electricity",
            ["55"] = "Heat",
            ["57"] = "Nuclear heat",
            ["59"] = "Electricity output",
            ["61"] = "Heat output",
            ["62"] = "Production of derived heat",
            ["63"] = "Production of derived electricity",
            ["64"] = "Imports",
            ["65"] = "Exports",
            ["66"] = "Stock changes",
            ["67"] = "Total energy supply",
            ["69"] = "Statistical differences",
            ["70"] = "Bioenergy",
            ["71"] = "Total primary energy production"
        };
    }
}
