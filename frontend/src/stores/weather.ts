import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { getRainForecast, type RainForecast } from '@/services/weatherServices'

const HEAVY_RAIN_THRESHOLD_MM = 50

export const useWeatherStore = defineStore('weather', () => {
  const rainForecast = ref<RainForecast | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const totalMm = computed(() => rainForecast.value?.totalNext7DaysMm ?? 0)
  const heavyRainAhead = computed(() => totalMm.value >= HEAVY_RAIN_THRESHOLD_MM)
  const nextDayMm = computed(() => rainForecast.value?.days[0]?.precipitationMm ?? null)

  async function fetch() {
    if (loading.value) return

    loading.value = true
    error.value = null
 
    try {
      rainForecast.value = await getRainForecast()
    } catch (e) {
      console.error('[weather] fetch failed', e)
      error.value = 'Não foi possível carregar a previsão de chuva'
    } finally {
      loading.value = false
    }
  }

  return { rainForecast, loading, error, totalMm, heavyRainAhead, nextDayMm, fetch }
})