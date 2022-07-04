import { defineStore } from 'pinia'

export class Topic {
  Id: number
  Name: string
  ImgUrl: string
}

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
    setTopic(topic: Topic) {
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