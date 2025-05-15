import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import EnergyChart from "../components/EnergyCharts";
import ProductReportTable from "../components/EnergyProductReport";

export default function DashboardPage() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
  const confirmed = window.confirm("Czy na pewno chcesz się wylogować?");
  if (confirmed) {
    logout();
    navigate("/login");
  }
};


  return (
    <div className="flex flex-col items-center justify-center min-h-screen">
      <h1 className="text-2xl font-bold mb-4">Panel użytkownika</h1>
       <div>
      <ProductReportTable />
     </div>
      <EnergyChart />
      <button
        onClick={handleLogout}
        className="bg-red-500 text-white px-4 py-2 rounded"
      >
        Wyloguj
      </button>
    </div>
  );
}
