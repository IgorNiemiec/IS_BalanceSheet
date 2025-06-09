// Plik: src/components/Export/JsonExportWidget.tsx
import { useState } from "react";
import {
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
} from "../ui/select"; 

import "../../styles/JsonExportWidget.css"; 

const countries = [
  { code: "all", name: "Wszystkie kraje" },
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
  { value: "all", label: "Wszystkie lata" }, 
  { value: "2010", label: "2010" },
  { value: "2011", label: "2011" },
  { value: "2012", label: "2012" },
  { value: "2013", label: "2013" },
  { value: "2014", label: "2014" },
  { value: "2015", label: "2015" },
  { value: "2016", label: "2016" },
  { value: "2017", label: "2017" },
  { value: "2018", label: "2018" },
  { value: "2019", label: "2019" },
  { value: "2020", label: "2020" },
  { value: "2021", label: "2021" },
  { value: "2022", label: "2022" },
];

const flowTypes = [
  { code: "all", name: "Wszystkie przepływy" },
  { code: "PPRD", name: "Produkcja pierwotna" },
  { code: "GIC_TOT", name: "Całkowite zużycie krajowe brutto" },
  { code: "FC_OTH_HH", name: "Zużycie w gospodarstwach domowych" },
  { code: "FC_IND", name: "Zużycie w przemyśle" },
];

const productTypes = [
  { code: "all", name: "Wszystkie produkty" }, 
  { code: "G3000", name: "Węgiel kamienny" },
  { code: "RA000", name: "Odnawialne źródła energii" },

];

export default function JsonExportWidget() {

  const [selectedCountry, setSelectedCountry] = useState<string>("all");
  const [selectedYear, setSelectedYear] = useState<string>("all");
  const [selectedFlow, setSelectedFlow] = useState<string>("all");
  const [selectedProduct, setSelectedProduct] = useState<string>("all");

  const handleExportToJson = () => {
    let url = `http://localhost:5244/api/energy/export-to-json?`;
    const params = [];

   
    if (selectedCountry !== "all") {
      params.push(`countryCode=${selectedCountry}`);
    }
    if (selectedYear !== "all") {
      params.push(`year=${selectedYear}`);
    }
    if (selectedFlow !== "all") {
      params.push(`flowCode=${selectedFlow}`);
    }
    if (selectedProduct !== "all") {
      params.push(`productCode=${selectedProduct}`);
    }

    url += params.join('&');

    const link = document.createElement('a');
    link.href = url;
    
  
    let fileName = 'energy_data';
    if (selectedCountry !== "all") fileName += `_${selectedCountry}`;
    if (selectedYear !== "all") fileName += `_${selectedYear}`;
    if (selectedFlow !== "all") fileName += `_${selectedFlow}`;
    if (selectedProduct !== "all") fileName += `_${selectedProduct}`;
    fileName += '.json';

    link.setAttribute('download', fileName);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

  return (
    <div className="json-export-widget-card">
      <h3>Eksport Danych do JSON</h3>
      <p>Wybierz kryteria, aby wyeksportować dane.</p>

      <div className="export-controls">
        <div className="select-group">
          <label htmlFor="exportCountry">Kraj:</label>
          <Select
            value={selectedCountry}
            onValueChange={setSelectedCountry}
          >
            <SelectTrigger id="exportCountry" className="select-trigger">
              <SelectValue placeholder="Wybierz kraj" />
            </SelectTrigger>
            <SelectContent>
              {countries.map((country) => (
               
                <SelectItem key={country.code} value={country.code}>
                  {country.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="select-group">
          <label htmlFor="exportYear">Rok:</label>
          <Select
            value={selectedYear}
            onValueChange={setSelectedYear}
          >
            <SelectTrigger id="exportYear" className="select-trigger">
              <SelectValue placeholder="Wybierz rok" />
            </SelectTrigger>
            <SelectContent>
              {years.map((year) => (
             
                <SelectItem key={year.value} value={year.value}>
                  {year.label}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="select-group">
          <label htmlFor="exportFlow">Typ Przepływu:</label>
          <Select
            value={selectedFlow}
            onValueChange={setSelectedFlow}
          >
            <SelectTrigger id="exportFlow" className="select-trigger">
              <SelectValue placeholder="Wybierz przepływ" />
            </SelectTrigger>
            <SelectContent>
              {flowTypes.map((flow) => (
               
                <SelectItem key={flow.code} value={flow.code}>
                  {flow.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="select-group">
          <label htmlFor="exportProduct">Typ Produktu:</label>
          <Select
            value={selectedProduct}
            onValueChange={setSelectedProduct}
          >
            <SelectTrigger id="exportProduct" className="select-trigger">
              <SelectValue placeholder="Wybierz produkt" />
            </SelectTrigger>
            <SelectContent>
              {productTypes.map((product) => (
              
                <SelectItem key={product.code} value={product.code}>
                  {product.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>
      </div>

      <button className="export-json-button" onClick={handleExportToJson}>
        Export JSON
      </button>
    </div>
  );
}