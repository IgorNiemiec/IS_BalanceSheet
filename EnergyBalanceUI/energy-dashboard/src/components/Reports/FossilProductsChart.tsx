import { useEffect, useState } from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  ResponsiveContainer,
  LabelList,
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
};

const countries = [
  { code: "PL", color: "#dc3912", name: "Polska" },
  { code: "DE", color: "#3366cc", name: "Niemcy" },
  { code: "FR", color: "#ff9900", name: "Francja" },
  { code: "IT", color: "#109618", name: "Włochy" },
];

const years = [2010, 2015, 2020];

export default function FossilProductsChart() {
  const [data, setData] = useState<CountryProductData[]>([]);
  const [loading, setLoading] = useState(false);
  const [selectedYear, setSelectedYear] = useState(2020);

  useEffect(() => {
    const fetchAll = async () => {
      setLoading(true);
      const allData: CountryProductData[] = [];

      for (const country of countries) {
        try {
          const res = await fetch(
            `http://localhost:5244/api/energy/report/by-product-filtered?country=${country.code}&year=${selectedYear}`
          );
          const json = await res.json();
          json.forEach((item: any) => {
            allData.push({
              countryCode: country.code,
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
      setLoading(false);
    };

    fetchAll();
  }, [selectedYear]);

  const transformedData = Object.values(
    data.reduce((acc: any, item) => {
      if (!acc[item.productDescription]) {
        acc[item.productDescription] = { product: item.productDescription };
      }
      acc[item.productDescription][item.countryCode] = item.totalAmount;
      return acc;
    }, {})
  );

  const percentData = transformedData.map((entry: any) => {
    const total = countries.reduce((sum, c) => sum + (entry[c.code] || 0), 0);
    const obj: any = { product: entry.product };
    countries.forEach((c) => {
      const val = entry[c.code] || 0;
      obj[c.code] = val;
      obj[`${c.code}_percent`] = total > 0 ? (val / total) * 100 : 0;
    });
    return obj;
  });

  return (
    <div className="energy-comparison-card">
      <div className="chart-controls">
        <h2>Porównanie krajów wg produktu ({selectedYear})</h2>
        <Select
          value={selectedYear.toString()}
          onValueChange={(val) => setSelectedYear(parseInt(val))}
        >
          <SelectTrigger className="select-trigger">
            <SelectValue placeholder="Wybierz rok" />
          </SelectTrigger>
          <SelectContent>
            {years.map((year) => (
              <SelectItem key={year} value={year.toString()}>
                {year}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {loading ? (
        <p>Wczytywanie danych...</p>
      ) : (
        <ResponsiveContainer width="100%" height={500}>
          <BarChart data={percentData} margin={{ top: 20, right: 30, left: 0, bottom: 5 }}>
            <XAxis dataKey="product" tick={{ fontSize: 12 }} />
            <YAxis tick={{ fontSize: 12 }} />
            <Tooltip formatter={(val: any, name: string) => [`${val.toFixed(2)}`, name]} />
            <Legend />
            {countries.map((c) => (
              <Bar key={c.code} dataKey={c.code} fill={c.color} name={c.name}>
                <LabelList
                  dataKey={`${c.code}_percent`}
                  position="top"
                  formatter={(value: number) =>
                    value > 0 ? `${value.toFixed(1)}%` : ""
                  }
                />
              </Bar>
            ))}
          </BarChart>
        </ResponsiveContainer>
      )}
    </div>
  );
}
