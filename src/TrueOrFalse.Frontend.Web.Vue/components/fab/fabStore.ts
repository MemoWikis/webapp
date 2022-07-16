import { defineStore } from 'pinia'
import { useTabsStore } from '../topic/tabs/tabsStore'

export const useFabStore = defineStore('fabStore', {
    state: () => {
      return {
        open: false,
      }
    },
    getters:{
    },
  })