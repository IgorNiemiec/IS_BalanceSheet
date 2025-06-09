import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import ProductReportTable from "../components/EnergyProductReport";

import "../styles/DashboardPage.css"
import FossilProductsReport from "../components/Reports/FossilProductsReport";
import FossilProductsChart from "../components/Reports/FossilProductsChart";
import FossilProductsChartYearByYear from "../components/Reports/FossilProductsChartYearByYear"

import FossilProductsExtraChart from "../components/Reports/FossilProductsExtraChart"

import RenewableProductsReport from "../components/Reports/RenewableProductsReport"
import RenewableProductsChart from "../components/Reports/RenewableProductsChart"
import RenewableProductsChartYearByYear from "../components/Reports/RenewableProductsChartYearByYear"
import RenewableProductsExtraChart from "../components/Reports/RenewableProductsExtraChart"
import ExportJsonWidget from "../components/Reports/JsonExportWidget"

export default function DashboardPage() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
  const confirmed = window.confirm("Czy na pewno chcesz się wylogować?");
  if (confirmed) 
  {
    logout();
    navigate("/login");
  }
};

  return (
    <div className="dashboard-container">
      <h1 className="dashboard-title">Panel użytkownika</h1>
        <ProductReportTable/>
        <FossilProductsReport/>
        <FossilProductsChart/>
        <FossilProductsChartYearByYear/>
        <FossilProductsExtraChart/>
        <RenewableProductsReport/>
        <RenewableProductsChart/>
        <RenewableProductsChartYearByYear/>
        <RenewableProductsExtraChart/>
        <ExportJsonWidget/>
      <button className="logout-button" onClick={handleLogout}>
        Wyloguj
      </button>
    </div>
  );
}
