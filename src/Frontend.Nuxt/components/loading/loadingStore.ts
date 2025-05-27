import { defineStore } from 'pinia'
import { delay } from '../../utils/utils'

export const useLoadingStore = defineStore('loadingStore', () => {
  const isLoading = ref(false)
  const loadingDuration = ref(0)
  const longLoading = ref(false)
  const isDone = ref(false)
  const loadingBarThreshold = ref(3000)
  const loadingLabel = ref('')
  const finalFillDuration = ref(500)

  const startLoading = (durationMs?: number, label: string = '') =>  {
    isDone.value = false
    loadingLabel.value = ''
    if (typeof durationMs === 'number' && durationMs > loadingBarThreshold.value) {
      loadingDuration.value = durationMs
      longLoading.value = true
      loadingLabel.value = label
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
    isDone.value = false
  }

  const finishLoading = async () => {
    isDone.value = true
    await delay(finalFillDuration.value)
    return
  }

  return { isLoading, startLoading, stopLoading, loadingDuration, longLoading, isDone, finishLoading, loadingBarThreshold, loadingLabel, finalFillDuration } 
})