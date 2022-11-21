import { defineStore } from 'pinia'

export const useSpinnerStore = defineStore('spinnerStore', {
  state: () => {
    return {
      active: false,
    }
  },
  actions: {
    showSpinner() {
      this.active = true;
    },
    hideSpinner() {
      this.active = false;
    },
  },
})