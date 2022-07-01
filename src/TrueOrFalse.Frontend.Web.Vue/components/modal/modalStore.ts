import { defineStore } from 'pinia'

export const useModalStore = defineStore('modalStore', {
  state: () => {
    return {
      open: false,
    }
  },
  actions: {
    openModal() {
      this.open = true;
    },
    closeModal() {
      this.open = false;
    }
  }
})