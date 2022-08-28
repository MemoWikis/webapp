import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

export const useLearningSessionStore = defineStore('learningSessionStore', {
    state: () => {
        return{
            isTestMode: false
    }}
})