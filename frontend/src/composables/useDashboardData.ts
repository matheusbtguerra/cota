import { onMounted, onUnmounted } from 'vue'
import { useRiverStore } from '@/stores/river'
import { useWeatherStore } from '@/stores/weather'

const RIVER_REFRESH_MS = 30_000

export function useDashboardData() {
  const river = useRiverStore()
  const weather = useWeatherStore()

  let timer: number | undefined

  onMounted(() => {
    river.fetch()
    weather.fetch()
    timer = window.setInterval(() => river.fetch(), RIVER_REFRESH_MS)
  })

  onUnmounted(() => {
    if (timer) window.clearInterval(timer)
  })

  return { river, weather }
}