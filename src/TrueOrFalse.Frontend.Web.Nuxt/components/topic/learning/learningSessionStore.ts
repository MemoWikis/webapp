import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

export const useLearningSessionStore = defineStore('learningSessionStore', {
    state: () => {
        return {
            isLearningSession: true,
            isTestMode: false,
            lastIndex: 0,
            currentIndex: 0,
        }
    },
    actions: {
        loadQuestion(skipIndex: number, sessionIndex: number) {
        },
        async startNewSession() {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const json = learningSessionConfigurationStore.buildSessionConfigJson()
            const result = await $fetch<any>('/apiVue/LearningSessionStore/NewSession/', {
                method: 'POST',
                body: json,
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null) {
                return true
            } else return false
        }
    },
})