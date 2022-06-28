import { defineStore } from 'pinia'

export const useModalStore = defineStore('modalStore', {
  state: () => {
    return {
      isOpen: false,
    }
  },
})