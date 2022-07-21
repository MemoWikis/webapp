import { defineStore } from 'pinia'
import { useUserStore } from '~~/components/user/userStore'

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
      initialName: '',
      imgUrl:'',
      questionCount: 0,
      authorIds: [],
      content: '',
      initialContent: '',
      contentHasChanged: false,
      }
  },
  actions: {
    setTopic(topic: Topic) {
      this.id = topic.Id
      this.name = topic.Name
      this.initialName = topic.Name

      this.imgUrl = topic.ImgUrl
      this.content = topic.Content
      this.initialContent = topic.Content
    },
    async saveTopic() {
      const userStore = useUserStore()
      if (!userStore.isLoggedIn)
        return

      const json = {
        id: this.id,
        name: this.name,
        saveName: this.name != this.initialName,
        content: this.content,
        saveContent: this.content != this.initialContent
      }
      var result = await $fetch('/api/Topic/SaveTopic', { method: 'POST', body: json, mode: 'cors', credentials: 'include' 
      })
      if (result == true)
        this.contentHasChanged = false
    },
    reset() {
      this.name = this.initialName
      this.content = this.initialContent
      this.contentHasChanged = false
    }
  },
  getters: {
    getTopicName(): string {
      return this.name
    },
  },
})