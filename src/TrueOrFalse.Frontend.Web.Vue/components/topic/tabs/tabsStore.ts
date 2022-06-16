import { defineStore } from 'pinia'
import { Tab } from './tabsEnum'

export const useTabsStore = defineStore('tabsStore', {
  state: () => {
    return {
      activeTab: Tab.Topic,
    }
  },
  getters:{
    getActiveTab:(state)=> state.activeTab,
  },
})