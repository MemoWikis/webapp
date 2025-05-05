import { defineStore } from "pinia"

export enum Tab {
    Text,
    Learning,
    Feed,
    Analytics,
}

export const useTabsStore = defineStore("tabsStore", () => {
    const activeTab = ref<Tab | null>(null)

    const isText = computed(() => activeTab.value === Tab.Text)
    const isLearning = computed(() => activeTab.value === Tab.Learning)
    const isFeed = computed(() => activeTab.value === Tab.Feed)
    const isAnalytics = computed(() => activeTab.value === Tab.Analytics)

    return {
        activeTab,

        isText,
        isLearning,
        isFeed,
        isAnalytics,
    }
})
