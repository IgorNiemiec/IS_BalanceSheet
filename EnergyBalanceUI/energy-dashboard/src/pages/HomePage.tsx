import { Button } from "../components/ui/button";
import { Card, CardContent } from "../components/ui/card";
import { Input } from "../components/ui/input";
import { Loader } from "../components/ui/loader";

export default function HomePage() {
return (
<Card className="w-full max-w-md mx-auto mt-10">
<CardContent className="space-y-4 p-6">
<h1 className="text-xl font-bold">Test komponentów UI</h1>
<Input placeholder="Wprowadź coś…" />
<Button>Przycisk</Button>
<Loader />
</CardContent>
</Card>
);
}