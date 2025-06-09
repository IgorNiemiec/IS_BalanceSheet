import { useEffect, useState } from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "../ui/select";

import "../../styles/FossilProductsChart.css";

type CountryProductData = {
  countryCode: string;
  productCode: string;
  productDescription: string;
  totalAmount: number;
  year: number;
};

const countries = [
  { code: "PL", color: "#dc3912", name: "Polska" },
  { code: "DE", color: "#3366cc", name: "Niemcy" },
  { code: "FR", color: "#ff9900", name: "Francja" },
  { code: "IT", color: "#109618", name: "Włochy" },
  { code: "UK", color: "#ff9999", name: "Wielka Brytania" },
  { code: "SE", color: "#f4ff26", name: "Szwecja" },
  { code: "ES", color: "#ff920d", name: "Hiszpania" },
  { code: "HR", color: "#7b00ff", name: "Chorwacja" },
];

const years = [
  2010, 2011, 2012, 2013, 2014, 2015, 2016,
  2017, 2018, 2019, 2020, 2021, 2022,
];

export default function ProductYearTrendChart() {
  const [data, setData] = useState<CountryProductData[]>([]);
  const [loading, setLoading] = useState(false);
  const [selectedCountry, setSelectedCountry] = useState("PL");
  const [selectedProduct, setSelectedProduct] = useState<string>("");
  const [productOptions, setProductOptions] = useState<string[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      const allData: CountryProductData[] = [];

      for (const year of years) {
        try {
          const res = await fetch(
            `http://localhost:5244/api/energy/report/by-product-filtered?country=${selectedCountry}&year=${year}`
          );
          const json = await res.json();
          json.forEach((item: any) => {
            allData.push({
              countryCode: selectedCountry,
              productCode: item.code,
              productDescription: item.description,
              totalAmount: item.amount,
              year: year,
            });
          });
        } catch (err) {
          console.error("Błąd pobierania danych:", err);
        }
      }

      setData(allData);

      const productSet = new Set(allData.map((d) => d.productDescription));
      const unique = Array.from(productSet) as string[];
      setProductOptions(unique);

      if (!selectedProduct && unique.length > 0) {
        setSelectedProduct(unique[0]);
      }

      setLoading(false);
    };

    fetchData();
  }, [selectedCountry]);

  const chartData = years.map((year) => {
    const match = data.find(
      (d) => d.year === year && d.productDescription === selectedProduct
    );
    return {
      year,
      amount: match ? match.totalAmount : 0,
    };
  });

  return (
    <div className="energy-comparison-card">
      <div className="chart-controls">
        <h2> Paliwa kopalniane - rok po roku</h2>

        <div className="controls-row">
          <Select
            value={selectedCountry}
            onValueChange={(val) => setSelectedCountry(val)}
          >
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
            onValueChange={(val) => setSelectedProduct(val)}
          >
            <SelectTrigger className="select-trigger">
              <SelectValue placeholder="Wybierz produkt" />
            </SelectTrigger>
            <SelectContent>
              {productOptions.map((p) => (
                <SelectItem key={p} value={p}>
                  {p}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </div>

      {loading ? (
        <p>Wczytywanie danych...</p>
      ) : (
        <ResponsiveContainer width="100%" height={500}>
          <BarChart data={chartData} margin={{ top: 20, right: 30, left: 0, bottom: 5 }}>
            <XAxis dataKey="year" tick={{ fontSize: 12 }} />
            <YAxis tick={{ fontSize: 12 }} />
            <Tooltip formatter={(val: any) => [`${val.toFixed(2)}`, selectedProduct]} />
            <Legend />
            <Bar dataKey="amount" fill="#8884d8" name={selectedProduct} />
          </BarChart>
        </ResponsiveContainer>
      )}
    </div>
  );
}