import { useEffect, useState } from "react";
import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "./ui/select";
import { Card, CardContent } from "./ui/card";
import { Loader } from "./ui/loader";

const countries = ["PL", "DE", "FR", "IT"];

type ProductReport = {
  productCode: string;
  productDescription: string;
  totalAmount: number;
};

export default function ProductReportTable() {
  const [selectedCountry, setSelectedCountry] = useState("PL");
  const [data, setData] = useState<ProductReport[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchReport = async () => {
      setLoading(true);
      try {
        const res = await fetch(
          `http://localhost:5244/api/energy/report/by-product?country=${selectedCountry}`
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
  }, [selectedCountry]);

  return (
    <Card className="p-6 mt-6 w-full max-w-5xl mx-auto">
      <CardContent className="space-y-4">
        <div className="flex justify-between items-center">
          <h2 className="text-xl font-semibold">
            Statystyki zużycia energii wg produktu ({selectedCountry})
          </h2>
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
          <div className="overflow-x-auto">
            <table className="w-full table-auto border border-gray-300 text-sm">
              <thead className="bg-gray-100">
                <tr>
                  <th className="px-4 py-2 text-left border">Kod produktu</th>
                  <th className="px-4 py-2 text-left border">Opis</th>
                  <th className="px-4 py-2 text-right border">Zużycie (KTOE)</th>
                </tr>
              </thead>
              <tbody>
                {data.map((row) => (
                  <tr key={row.productCode} className="even:bg-gray-50">
                    <td className="px-4 py-2 border font-mono">{row.productCode}</td>
                    <td className="px-4 py-2 border">{row.productDescription}</td>
                    <td className="px-4 py-2 border text-right">
                      {row.totalAmount.toLocaleString(undefined, {
                        maximumFractionDigits: 2,
                      })}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </CardContent>
    </Card>
  );
}