export default function ErrorPage() {
return (
<div className="flex flex-col items-center justify-center min-h-screen text-center bg-red-100 p-8">
<h1 className="text-3xl font-bold text-red-600">Ups! 😢</h1>
<p className="text-lg text-gray-700 mt-4">
Nie znaleziono strony lub wystąpił inny błąd.
</p>
</div>
);
}