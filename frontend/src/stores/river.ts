import { defineStore } from "pinia";
import { ref, computed } from "vue";
import { getRiverCurrent, type RiverReading } from "@/services/riverServices";

export const useRiverStore = defineStore('river', () => {
  const reading = ref<RiverReading | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const distanceToFlood = computed(() =>
    reading.value ? Math.max(0, 3.0 - reading.value.levelMeters) : null)

  async function fetch() {
    loading.value = true
    error.value = null
    try {
      reading.value = await getRiverCurrent()
    } catch {
      error.value = 'Não foi possível carregar o nível do rio'
    } finally {
      loading.value = false
    }
  }

  return { reading, loading, error, distanceToFlood, fetch }
})