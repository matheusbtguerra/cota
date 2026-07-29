import { api } from "./api";

export interface RiverReading {
  levelMeters: number
  status: 'Normal' | 'Atencao' | 'Alerta' | 'Inundacao'
  measuredAt: string
  station: {
    code: string
    name: string
    region: string
  }
  thresholds: {
    attention: number
    alert: number
    flood: number
  }
}

export const getRiverCurrent = () =>
    api.get<RiverReading>('/api/river/current').then(r => r.data);