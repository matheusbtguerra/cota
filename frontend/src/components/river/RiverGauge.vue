<script setup lang="ts">
import { computed } from 'vue'
import { storeToRefs } from 'pinia'
import { useRiverStore } from '@/stores/river'

const river = useRiverStore()
const { reading } = storeToRefs(river)

const MAX_METERS = 6

const RECORD_2024 = 5.35

const waterHeightPct = computed(() => {
  const level = reading.value?.levelMeters ?? 0
  return Math.min(100, (level / MAX_METERS) * 100)
})

function markPosition(meters: number): number {
  return 100 - (meters / MAX_METERS) * 100
}

const marks = computed(() => {
  if (!reading.value) return []
  const t = reading.value.thresholds
  return [
    { meters: RECORD_2024, label: 'maio 2024', color: 'var(--color-status-inundacao)', dashed: true },
    { meters: t.flood, label: 'inundação', color: 'var(--color-status-inundacao)', dashed: false },
    { meters: t.alert, label: 'alerta', color: 'var(--color-status-alerta)', dashed: false },
  ]
})

function fmt(n: number): string {
  return n.toFixed(2).replace('.', ',')
}
</script>

<template>
  <div v-if="reading" class="flex gap-4">
    <div class="relative w-28 h-80 rounded-lg bg-cota-mist border border-gray-200 overflow-hidden">
      <div
        class="absolute bottom-0 left-0 right-0 bg-cota-light transition-all duration-700 ease-out"
        :style="{ height: waterHeightPct + '%' }"
      />
      <div
        v-for="mark in marks"
        :key="mark.label"
        class="absolute left-0 right-0"
        :style="{
          top: markPosition(mark.meters) + '%',
          borderTopWidth: '1.5px',
          borderColor: mark.color,
          borderStyle: mark.dashed ? 'dashed' : 'solid',
        }"
      />
    </div>

    <div class="relative h-80 text-xs">
      <span
        v-for="mark in marks"
        :key="mark.label"
        class="absolute whitespace-nowrap -translate-y-1/2"
        :style="{ top: markPosition(mark.meters) + '%', color: mark.color }"
      >
        {{ fmt(mark.meters) }} {{ mark.label }}
      </span>
    </div>
  </div>
</template>