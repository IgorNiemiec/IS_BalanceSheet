import { Loader2 } from "lucide-react";
import { cn } from "./utils";

export function Loader({ className }: { className?: string }) {
return <Loader2 className={cn("h-6 w-6 animate-spin", className)} />;
}