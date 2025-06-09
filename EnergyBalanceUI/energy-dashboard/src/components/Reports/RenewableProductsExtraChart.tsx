import { useEffect, useState } from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  Legend,
} from "recharts";

import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "../ui/select";

type CountryProductData = {
  year: number;
  countryCode: string;
  productCode: string;
  productDescription: string;
  totalAmount: number;
};

const countries = [
  { code: "PL", name: "Polska" },
  { code: "DE", name: "Niemcy" },
  { code: "FR", name: "Francja" },
  { code: "IT", name: "Włochy" },
  { code: "UK", name: "Wielka Brytania" },
  { code: "SE", name: "Szwecja" },
  { code: "ES", name: "Hiszpania" },
  { code: "HR", name: "Chorwacja" },
];

const years = [
  2010, 2011, 2012, 2013, 2014,
  2015, 2016, 2017, 2018, 2019,
  2020, 2021, 2022,
];

export default function YearlyRenewableProductChart() {
  const [data, setData] = useState<CountryProductData[]>([]);
  const [loading, setLoading] = useState(false);
  const [selectedCountry, setSelectedCountry] = useState("PL");
  const [productOptions, setProductOptions] = useState<string[]>([]);
  const [selectedProduct, setSelectedProduct] = useState<string>("");

  useEffect(() => {
    const fetchAll = async () => {
      setLoading(true);
      const allData: CountryProductData[] = [];

      for (const year of years) {
        try {
          const res = await fetch(
            `http://localhost:5244/api/energy/report/by-renewableproduct-filtered?country=${selectedCountry}&year=${year}`
          );
          const json = await res.json();
          json.forEach((item: any) => {
            allData.push({
              year,
              countryCode: selectedCountry,
              productCode: item.code,
              productDescription: item.description,
              totalAmount: item.amount,
            });
          });
        } catch (err) {
          console.error("Błąd pobierania danych:", err);
        }
      }

      setData(allData);

      const unique = Array.from(
        new Set(allData.map((d) => d.productDescription))
      );
      setProductOptions(unique);
      if (!unique.includes(selectedProduct)) {
        setSelectedProduct(unique[0]);
      }

      setLoading(false);
    };

    fetchAll();
  }, [selectedCountry]);

  const chartData = years.map((year) => {
    const entry = data.find(
      (d) => d.year === year && d.productDescription === selectedProduct
    );
    return {
      year,
      amount: entry ? entry.totalAmount : 0,
    };
  });

  return (
    <div className="energy-comparison-card">
      <h2 className="text-xl font-semibold mb-4"> Energia odnawialna - rok po roku  </h2>

      <div className="flex gap-4 mb-6">
        <Select value={selectedCountry} onValueChange={setSelectedCountry}>
          <SelectTrigger className="select-trigger">
            <SelectValue placeholder="Wybierz kraj" />
          </SelectTrigger>
          <SelectContent>
            {countries.map((c) => (
              <SelectItem key={c.code} value={c.code}>
                {c.name}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>

        <Select
          value={selectedProduct}
          onValueChange={setSelectedProduct}
        >
          <SelectTrigger className="select-trigger">
            <SelectValue placeholder="Wybierz produkt" />
          </SelectTrigger>
          <SelectContent>
            {productOptions.map((prod) => (
              <SelectItem key={prod} value={prod}>
                {prod}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {loading ? (
        <p>Wczytywanie danych...</p>
      ) : (
        <ResponsiveContainer width="100%" height={450}>
          <BarChart data={chartData}>
            <XAxis dataKey="year" />
            <YAxis />
            <Tooltip formatter={(val: any) => `${val.toFixed(2)}`} />
            <Legend />
            <Bar dataKey="amount" fill="#82ca9d" name={selectedProduct} />
          </BarChart>
        </ResponsiveContainer>
      )}
    </div>
  );
}
