import { useEffect, useState } from "react";
import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "../ui/select";
import { Loader } from "../ui/loader";

import "../../styles/RenewableProductsReport.css";

const countries = ["PL", "DE", "FR", "IT", "UK", "SE", "ES", "HR"];
const years = [2010, 2011, 2012,2013,2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022];

type ProductData = {
  code: string;
  description: string;
  amount: number;
};

export default function FossilProductsReport() {
  const [selectedCountry, setSelectedCountry] = useState("PL");
  const [selectedYear, setSelectedYear] = useState(2020);
  const [data, setData] = useState<ProductData[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const res = await fetch(
          `http://localhost:5244/api/energy/report/by-renewableproduct-filtered?country=${selectedCountry}&year=${selectedYear}`
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

    fetchData();
  }, [selectedCountry, selectedYear]);

  return (
    <div className="energy-by-product-card">
      <div className="energy-by-product-header">
        <h2>
          Produkcja energii odnawialnej ({selectedCountry}, {selectedYear})
        </h2>

        <div className="select-group">
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
      </div>

      {loading ? (
        <div className="energy-by-product-loader">
          <Loader />
        </div>
      ) : (
        <div className="table-wrapper">
          <table className="energy-by-product-table">
            <thead>
              <tr>
                <th>Kod</th>
                <th>Produkt</th>
                <th>Ilość [KTOE]</th>
              </tr>
            </thead>
            <tbody>
              {data.map((row) => (
                <tr key={row.code + selectedYear}>
                  <td className="code-cell">{row.code}</td>
                  <td>{row.description}</td>
                  <td className="number-cell">
                    {typeof row.amount === "number"
                      ? row.amount.toLocaleString(undefined, {
                          maximumFractionDigits: 2,
                        })
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
