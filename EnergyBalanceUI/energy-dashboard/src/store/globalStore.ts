import { create } from "zustand";

type Language = "en" | "pl";

interface GlobalStore {
  language: Language;
  setLanguage: (lang: Language) => void;
}

export const useGlobalStore = create<GlobalStore>((set) => ({
  language: "pl",
  setLanguage: (lang) => set({ language: lang }),
}));