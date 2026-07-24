import { api } from "./api";

export interface RainForecast {
    totalNext7DaysMm: number;
    days: {
        date: string;
        precipitationMm: number;
    }[];
    fetchedAt: string;
}

export const getRainForecast = () =>
  api.get<RainForecast>('/api/weather/forecast').then(r => r.data)