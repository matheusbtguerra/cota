import { api } from "./api";

export interface RiverReading {
    levelMeters: number;
    status: 'Normal' | 'Attention' | 'Alert' | 'Flood';
    measuredAt: string;
    station: string;
}

export const getRiverCurrent = () =>
    api.get<RiverReading>('/api/river/current').then(r => r.data);