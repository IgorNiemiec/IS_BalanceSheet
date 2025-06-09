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
};

const countries = [
  { code: "PL", color: "#dc3912", name: "Polska" },
  { code: "DE", color: "#3366cc", name: "Niemcy" },
  { code: "FR", color: "#3ac9c5", name: "Francja" },
  { code: "IT", color: "#109618", name: "Włochy" },
  { code: "UK", color: "#ff9999", name: "Wielka Brytania" },
  { code: "SE", color: "#f4ff26", name: "Szwecja" },
  { code: "ES", color: "#ff920d", name: "Hiszpania" },
  { code: "HR", color: "#7b00ff", name: "Chorwacja" },
];

const years = [2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022];

export default function YearComparisonChart() {
  const [dataByYear, setDataByYear] = useState<{ [year: number]: CountryProductData[] }>({});
  const [loading, setLoading] = useState(false);
  const [year1, setYear1] = useState(2019);
  const [year2, setYear2] = useState(2020);
  const [selectedCountry, setSelectedCountry] = useState<string>("ALL");

  useEffect(() => {
    const fetchYear = async (year: number) => {
      if (dataByYear[year]) return; // już pobrane

      setLoading(true);
      const allData: CountryProductData[] = [];

      for (const country of countries) {
        try {
          const res = await fetch(
            `http://localhost:5244/api/energy/report/by-renewableproduct-filtered?country=${country.code}&year=${year}`
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
          console.error(`Błąd pobierania danych ${year}:`, err);
        }
      }

      setDataByYear(prev => ({ ...prev, [year]: allData }));
      setLoading(false);
    };

    fetchYear(year1);
    fetchYear(year2);
  }, [year1, year2]);

  const getDisplayData = () => {
    const data1 = dataByYear[year1] || [];
    const data2 = dataByYear[year2] || [];

    const filtered1 = selectedCountry === "ALL" ? data1 : data1.filter(d => d.countryCode === selectedCountry);
    const filtered2 = selectedCountry === "ALL" ? data2 : data2.filter(d => d.countryCode === selectedCountry);

    const mapByKey = (arr: CountryProductData[], _key: string) => {
      const map: Record<string, number> = {};
      arr.forEach(d => {
        map[d.productDescription] = (map[d.productDescription] || 0) + d.totalAmount;
      });
      return map;
    };

    const map1 = mapByKey(filtered1, "productDescription");
    const map2 = mapByKey(filtered2, "productDescription");

    const products = Array.from(new Set([...Object.keys(map1), ...Object.keys(map2)]));
    return products.map(prod => ({
      product: prod,
      [year1]: map1[prod] || 0,
      [year2]: map2[prod] || 0,
    }));
  };

  const barColor1 = "#8884d8";
  const barColor2 = "#82ca9d";

  return (
    <div className="energy-comparison-card">
      <div className="chart-controls">
        <h2> Energia odnawialna - Produkcja  </h2>
        <div className="selectors">
          <Select value={year1.toString()} onValueChange={val => setYear1(parseInt(val))}>
            <SelectTrigger className="select-trigger"><SelectValue placeholder="Rok 1" /></SelectTrigger>
            <SelectContent>
              {years.map(y => (
                <SelectItem key={y} value={y.toString()}>{y}</SelectItem>
              ))}
            </SelectContent>
          </Select>

          <Select value={year2.toString()} onValueChange={val => setYear2(parseInt(val))}>
            <SelectTrigger className="select-trigger"><SelectValue placeholder="Rok 2" /></SelectTrigger>
            <SelectContent>
              {years.map(y => (
                <SelectItem key={y} value={y.toString()}>{y}</SelectItem>
              ))}
            </SelectContent>
          </Select>

          <Select value={selectedCountry} onValueChange={val => setSelectedCountry(val)}>
            <SelectTrigger className="select-trigger"><SelectValue placeholder="Kraj" /></SelectTrigger>
            <SelectContent>
              <SelectItem value="ALL">Wszystkie kraje</SelectItem>
              {countries.map(c => (
                <SelectItem key={c.code} value={c.code}>{c.name}</SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </div>

      {loading ? (
        <p>Wczytywanie danych...</p>
      ) : (
        <ResponsiveContainer width="100%" height={500}>
          <BarChart data={getDisplayData()} margin={{ top: 20, right: 30, left: 0, bottom: 5 }}>
            <XAxis dataKey="product" tick={{ fontSize: 12 }} />
            <YAxis tick={{ fontSize: 12 }} />
            <Tooltip formatter={(val: any, name: string) => [`${val.toFixed(2)}`, name]} />
            <Legend />
            <Bar dataKey={year1} fill={barColor1} name={`${year1}`} />
            <Bar dataKey={year2} fill={barColor2} name={`${year2}`} />
          </BarChart>
        </ResponsiveContainer>
      )}
    </div>
  );
}
