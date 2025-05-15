import { useEffect, useState } from "react";
import {
LineChart,
Line,
XAxis,
YAxis,
Tooltip,
CartesianGrid,
ResponsiveContainer,
} from "recharts";
import {
Select,
SelectTrigger,
SelectValue,
SelectContent,
SelectItem,
} from "./ui/select";
import { Card, CardContent } from "./ui/card";
import { Loader } from "./ui/loader";

type EnergyData = {
countryCode: string;
countryName: string;
year: number;
flowCode: string;
flowDescription: string;
productCode: string;
productDescription: string;
unit: string;
amount: number;
};

type ChartPoint = {
year: number;
value: number;
};

const countries = ["PL", "DE", "FR", "IT"];

export default function EnergyChart() {
const [selectedCountry, setSelectedCountry] = useState("PL");
const [data, setData] = useState<ChartPoint[]>([]);
const [loading, setLoading] = useState(false);

useEffect(() => {
const fetchData = async () => {
setLoading(true);
try {
const res = await fetch(`http://localhost:5244/api/energy/values?country=${selectedCountry}`);
const json: EnergyData[] = await res.json();

    const grouped: Record<string, EnergyData[]> = {};

    for (const curr of json) {
      if (!grouped[curr.year]) {
        grouped[curr.year] = [];
      }
      grouped[curr.year].push(curr);
    }

    const chartData: ChartPoint[] = Object.entries(grouped)
      .map(([year, entries]) => ({
        year: Number(year),
        value: entries.reduce((sum, item) => sum + item.amount, 0),
      }))
      .sort((a, b) => a.year - b.year);

    setData(chartData);
  } catch (err) {
    console.error("Błąd podczas pobierania danych:", err);
    setData([]);
  } finally {
    setLoading(false);
  }
};

fetchData();
}, [selectedCountry]);

return (
<Card className="p-6 mt-6 w-full max-w-4xl mx-auto">
<CardContent className="space-y-4">
<div className="flex justify-between items-center">
<h2 className="text-xl font-semibold">Zużycie energii: {selectedCountry}</h2>
<Select
value={selectedCountry}
onValueChange={(val) => setSelectedCountry(val)}
>
<SelectTrigger className="w-32">
<SelectValue placeholder="Wybierz kraj" />
</SelectTrigger>
<SelectContent>
{countries.map((code) => (
<SelectItem key={code} value={code}>
{code}
</SelectItem>
))}
</SelectContent>
</Select>
</div>
  {loading ? (
      <div className="flex justify-center p-6">
        <Loader />
      </div>
    ) : (
      <ResponsiveContainer width="100%" height={400}>
        <LineChart data={data}>
          <CartesianGrid stroke="#ccc" />
          <XAxis dataKey="year" />
          <YAxis />
          <Tooltip />
          <Line type="monotone" dataKey="value" stroke="#2563eb" />
        </LineChart>
      </ResponsiveContainer>
    )}
  </CardContent>
</Card>
);
}