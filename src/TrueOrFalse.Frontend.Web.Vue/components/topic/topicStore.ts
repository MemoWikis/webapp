import { defineStore } from 'pinia'

export const useTopicStore = defineStore('topicStore', {
  state: () => {
    return {
      id: 0,
      name: '',
      imgUrl:'',
      questionCount: 0,
      authorIds: []
      }
  },
  getters: {
    getTopicName(): string {
      return this.name
    },
  }
})