import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

export const useLearningSessionStore = defineStore('learningSessionStore', {
    state: () => {
        return{
            isLearningSession: true,
            isTestMode: false
    }}
})