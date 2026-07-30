<script setup lang="ts">
import { computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useRiverStore } from '@/stores/river'
import { useDashboardData } from '@/composables/useDashboardData'
import RiverGauge from '@/components/river/RiverGauge.vue'

useDashboardData()

const river = useRiverStore()
const { reading, loading, error } = storeToRefs(river)

const distanceToFlood = computed(() => {
  if (!reading.value) return null
  return Math.max(0, reading.value.thresholds.flood - reading.value.levelMeters)
})

function fmt(n: number): string {
  return n.toFixed(2).replace('.', ',')
}
</script>

<template>
  <main class="min-h-screen bg-cota-mist p-8">
    <h1 class="text-2xl font-medium text-cota-deep mb-6">Cota</h1>

    <p v-if="loading && !reading">Carregando…</p>
    <p v-else-if="error" class="text-status-inundacao">{{ error }}</p>

    <div v-else-if="reading" class="flex flex-wrap gap-8 items-start">
      <RiverGauge />

      <div class="space-y-3">
        <div>
          <p class="text-sm text-gray-500">Nível atual do Guaíba</p>
          <p class="text-4xl font-medium text-cota-deep">{{ fmt(reading.levelMeters) }} m</p>
        </div>

        <p class="text-sm">
          Status:
          <span class="font-medium">{{ reading.status }}</span>
        </p>

        <p v-if="distanceToFlood !== null" class="text-sm">
          Faltam <span class="font-medium">{{ fmt(distanceToFlood) }} m</span> para a cota de inundação
        </p>

        <p class="text-xs text-gray-500">
          {{ reading.station.name }} · {{ reading.station.region }}
        </p>
      </div>
    </div>
  </main>
</template>