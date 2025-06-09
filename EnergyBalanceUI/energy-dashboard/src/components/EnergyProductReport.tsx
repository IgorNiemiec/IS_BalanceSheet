import { useEffect, useState } from "react";
import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "./ui/select";
import { Loader } from "./ui/loader";

import "../styles/EnergyProductReportStyle.css";

const countries = ["PL", "DE", "FR", "IT", "UK", "SE", "ES", "HR"];
const years = ["2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022"];

type ProductReport = {
  productCode: string;
  productDescription: string;
  totalAmount: number;
};

export default function ProductReportTable() {
  const [selectedCountry, setSelectedCountry] = useState("PL");
  const [selectedYear, setSelectedYear] = useState("2020");
  const [data, setData] = useState<ProductReport[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchReport = async () => {
      setLoading(true);
      try {
        const res = await fetch(
          `http://localhost:5244/api/energy/report/by-product?country=${selectedCountry}&year=${selectedYear}`
        );
        const json = await res.json();
        setData(json);
      } catch (err) {
        console.error("Błąd pobierania danych:", err);
        setData([]);
      } finally {
        setLoading(false);
      }
    };

    fetchReport();
  }, [selectedCountry, selectedYear]);

  return (
    <div className="product-report-card">
      <div className="product-report-header">
        <h2>
          Statystyki zużycia energii wg produktu ({selectedCountry}, {selectedYear})
        </h2>

        <div className="select-container">
          <Select
            value={selectedCountry}
            onValueChange={(val) => setSelectedCountry(val)}
          >
            <SelectTrigger className="select-trigger">
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

          <Select
            value={selectedYear}
            onValueChange={(val) => setSelectedYear(val)}
          >
            <SelectTrigger className="select-trigger">
              <SelectValue placeholder="Wybierz rok" />
            </SelectTrigger>
            <SelectContent>
              {years.map((year) => (
                <SelectItem key={year} value={year}>
                  {year}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </div>

      {loading ? (
        <div className="product-report-loader">
          <Loader />
        </div>
      ) : (
        <div className="table-wrapper">
          <table className="product-report-table">
            <thead>
              <tr>
                <th>Kod produktu</th>
                <th>Opis</th>
                <th>Zużycie (KTOE)</th>
              </tr>
            </thead>
            <tbody>
              {data.map((row) => (
                <tr key={row.productCode}>
                  <td className="code-cell">{row.productCode}</td>
                  <td>{row.productDescription}</td>
                  <td className="number-cell">
                  {typeof row.totalAmount === "number"
                         ? row.totalAmount.toLocaleString(undefined, { maximumFractionDigits: 2 })
                         : "—"}
                 </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
