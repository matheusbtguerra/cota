<script setup lang="ts">
import { useDashboardData } from '@/composables/useDashboardData'

const { river, weather } = useDashboardData()
</script>

<template>
  <main class="min-h-screen bg-cota-mist p-8">
    <p v-if="river.loading">Carregando…</p>
    <p v-else-if="river.error" class="text-status-inundacao">{{ river.error }}</p>
    <div v-else-if="river.reading">
      <p class="text-4xl font-medium">{{ river.reading.levelMeters.toFixed(2).replace('.', ',') }} m</p>
      <p>Status: {{ river.reading.status }}</p>
      <p>Faltam {{ river.distanceToFlood?.toFixed(2).replace('.', ',') }} m para a cota de inundação</p>
      <p class="text-sm text-gray-500">{{ river.reading.station }}</p>
      
      <p v-if="weather.rainForecast">
        Chuva prevista: <span class="font-medium">{{ weather.totalMm }} mm</span> em 7 dias
      </p>
      <p v-if="weather.heavyRainAhead" class="text-status-atencao">
        Volume alto de chuva previsto para a próxima semana
      </p>
    </div>
  </main>
</template>