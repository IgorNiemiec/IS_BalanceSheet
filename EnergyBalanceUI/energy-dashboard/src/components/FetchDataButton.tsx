import { useState } from "react";
import { Button } from "../components/ui/button";
import { Progress } from "../components/ui/progress";
import { CheckCircle, Loader2, XCircle } from "lucide-react";

export function FetchDataButton() {
  const [status, setStatus] = useState<"idle" | "loading" | "success" | "error">("idle");
  const [progress, setProgress] = useState(0);

  const simulateProgress = () => {
    let value = 0;
    const interval = setInterval(() => {
      value += Math.random() * 20;
      if (value >= 95) {
        value = 95;
      }
      setProgress(value);
    }, 200);

    return interval;
  };

  const fetchData = async () => {
    setStatus("loading");
    setProgress(0);
    const interval = simulateProgress();

    try {
      const response = await fetch("http://localhost:5244/api/energy/primary-production");
      clearInterval(interval);
      if (!response.ok) throw new Error("Network error");

      setProgress(100);
      setStatus("success");
    } catch (error) {
      clearInterval(interval);
      setStatus("error");
    }
  };

  return (
    <div className="space-y-2 w-full max-w-md">
      <div className="flex items-center space-x-4">
        <Button onClick={fetchData} disabled={status === "loading"}>
          {status === "loading" ? (
            <span className="flex items-center gap-2">
              <Loader2 className="animate-spin w-4 h-4" /> Pobieranie...
            </span>
          ) : (
            "Pobierz dane"
          )}
        </Button>

        {status === "success" && (
          <div className="text-green-600 flex items-center gap-1">
            <CheckCircle className="w-4 h-4" /> Sukces!
          </div>
        )}

        {status === "error" && (
          <div className="text-red-600 flex items-center gap-1">
            <XCircle className="w-4 h-4" /> Błąd pobierania!
          </div>
        )}
      </div>

      {status === "loading" && (
        <Progress value={progress} className="h-2 transition-all" />
      )}
    </div>
  );
}