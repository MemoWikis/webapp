import { defineStore } from 'pinia'

export class Topic {
  Id: number
  Name: string
  ImgUrl: string
  Content: string
}

export const useTopicStore = defineStore('topicStore', {
  state: () => {
    return {
      id: 0,
      name: '',
      imgUrl:'',
      questionCount: 0,
      authorIds: [],
      content: ''
      }
  },
  actions: {
    setTopic(topic: Topic) {
      this.id = topic.Id
      this.name = topic.Name
      this.imgUrl = topic.ImgUrl
      this.content = topic.Content
    },
  },
  getters: {
    getTopicName(): string {
      return this.name
    },
  }
})