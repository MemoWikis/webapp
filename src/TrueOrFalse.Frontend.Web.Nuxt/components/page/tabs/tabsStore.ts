import { defineStore } from 'pinia'

export enum Tab {
  Text,
  Learning,
  Feed,
  Analytics
}

export const useTabsStore = defineStore('tabsStore', {
  state: () => {
    return {
      activeTab: null as Tab | null,
    }
  },
})