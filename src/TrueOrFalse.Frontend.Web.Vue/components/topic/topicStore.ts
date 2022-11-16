import { defineStore } from 'pinia'
import { useUserStore } from '../user/userStore'
import { Visibility } from '../shared/visibilityEnum'

export class Topic {
  CanAccess: boolean
  Id: number
  Name: string
  ImgUrl: string
  Content: string
  ParentTopicCount: number
  ChildTopicCount: number
  Views: number
  CommentCount: number
  Visibility: Visibility
  AuthorIds: number[]
  IsWiki: boolean
  CurrentUserIsCreator: boolean
  CanBeDeleted: boolean
  QuestionCount: number
}

export const useTopicStore = defineStore('topicStore', {
  state: () => {
    return {
      id: 0,
      name: '',
      initialName: '',
      imgUrl:'',
      questionCount: 0,
      content: '',
      initialContent: '',
      contentHasChanged: false,
      parentTopicCount: 0,
      childTopicCount: 0,
      views: 0,
      commentCount: 0,
      visibility: null as unknown as Visibility,
      authorIds: [],
      isWiki: false,
      currentUserIsCreator: false,
      canBeDeleted: false,
    }
  },
  actions: {
    setTopic(topic: Topic) {
      if (topic != null) {
        this.id = topic.Id
        this.name = topic.Name
        this.initialName = topic.Name
        this.imgUrl = topic.ImgUrl
        this.content = topic.Content
        this.initialContent = topic.Content

        this.parentTopicCount = topic.ParentTopicCount
        this.childTopicCount = topic.ChildTopicCount

        this.views = topic.Views
        this.commentCount = topic.CommentCount
        this.visibility = topic.Visibility

        this.authorIds = topic.AuthorIds
        this.isWiki = topic.IsWiki
        this.currentUserIsCreator = topic.CurrentUserIsCreator
        this.canBeDeleted = topic.CanBeDeleted

        this.questionCount = topic.QuestionCount
      }
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
      var result = await $fetch('/api/Topic/SaveTopic', { method: 'POST', body: json, mode: 'cors', credentials: 'include' })
      if (result == true)
        this.contentHasChanged = false
    },
    reset() {
      this.name = this.initialName
      this.content = this.initialContent
      this.contentHasChanged = false
    },
    isOwnerOrAdmin(){
      const userStore = useUserStore()
      return userStore.isAdmin || this.currentUserIsCreator
    },
  },
  getters: {
    getTopicName(): string {
      return this.name
    },
  },
})