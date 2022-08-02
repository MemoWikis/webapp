import { defineStore } from 'pinia'
import { Tab } from './tabsEnum'

export const useTabsStore = defineStore('tabsStore', {
  state: () => {
    return {
      activeTab: null as Tab,
    }
  },
  getters:{
    getActiveTab() { return this.activeTab },
  },
})