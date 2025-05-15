import { Outlet } from "react-router-dom";

export default function RootLayout() {
return (
<div className="flex flex-col min-h-screen">
<header className="bg-blue-700 text-white p-4 text-xl font-bold">
⚡ Energy Dashboard
</header>
<main className="flex-1 p-4 bg-gray-50">
    <Outlet />
  </main>

  <footer className="bg-gray-200 text-center p-4 text-sm">
    &copy; {new Date().getFullYear()} Politechnika — Projekt Integracyjny
  </footer>
</div>
);
}