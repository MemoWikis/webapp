import { defineStore } from 'pinia'

export const useUtilsStore = defineStore('utilsStore', {
  state: () => {
    return {
      showSpinner: false,
    }
  },
  actions: {
    showSpinner() {
      this.showSpinner = true;
    },
    hideSpinner() {
      this.hideSpinner = false;
    },
  },
})