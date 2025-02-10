import { defineStore } from 'pinia'

export const useLoadingStore = defineStore('loadingStore', () => {
  const isLoading = ref(false)
  const loadingDuration = ref(0)
  const longLoading = ref(false)
  const isDone = ref(false)

  const startLoading = (durationMs?: number) =>  {
    isDone.value = false
    if (typeof durationMs === 'number' && durationMs > 2000) {
      loadingDuration.value = durationMs
      longLoading.value = true
    } 
    else {
      loadingDuration.value = 0
      longLoading.value = false
    }

    isLoading.value = true
  }

  const stopLoading = () => {
    isDone.value = true
    isLoading.value = false
    loadingDuration.value = 0
    longLoading.value = false
  }

  const finishLoading = () => {
    isDone.value = true
  }

  return { isLoading, startLoading, stopLoading, loadingDuration, longLoading, isDone, finishLoading  }
})