import { defineStore } from 'pinia'

export enum Tab {
  Topic,
  Learning,
  Feed,
  Analytics
}

export const useTabsStore = defineStore('tabsStore', {
  state: () => {
    return {
      activeTab: null as unknown as Tab,
    }
  },
})