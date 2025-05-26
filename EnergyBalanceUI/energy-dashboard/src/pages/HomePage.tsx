import { useNavigate } from "react-router-dom";
import "../styles/Home.css";
import { FetchDataButton } from "../components/FetchDataButton";

export default function HomePage() {
    const navigate = useNavigate();
return (
 <div className="home-container">
      <div className="home-content">
        <h1 className="home-title">Raport Węglowy Polskiego Imperium</h1>
        <h2 className="home-subtitle">Autorzy: Igor Niemiec oraz Michał Mazurek</h2>
        <div className="home-buttons">
          <button className="home-button" onClick={() => navigate("/login")}>
            Zaloguj się
          </button>
          <button className="home-button home-button-secondary" onClick={() => navigate("/register")}>
            Zarejestruj się
          </button>
        </div>
        <div className="p-4">
      <h1 className="text-xl font-bold mb-4">Pobieranie danych</h1>
         <FetchDataButton />
        </div>
      </div>
    </div>
);
}