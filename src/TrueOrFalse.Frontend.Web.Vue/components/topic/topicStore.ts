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
  actions: {
    setTopic(topic) {
      this.id = topic.Id
      this.name = topic.Name
      this.imgUrl = topic.ImgUrl
    },
  },
  getters: {
    getTopicName(): string {
      return this.name
    },
  }
})