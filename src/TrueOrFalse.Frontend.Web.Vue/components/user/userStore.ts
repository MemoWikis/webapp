import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { Topic } from '../topic/topicStore'

export type UserLoginResult = {
  Success: boolean
  Message: string
  Id: number
  WikiId: number
  IsAdmin: boolean
  Name: string
  PersonalWikiId
}

export type LoginState = {
  IsLoggedIn: boolean
  UserId: number
}

export const useUserStore = defineStore('userStore', {
  state: () => {
    return {
      isLoggedIn: false,
      id: 0,
      type: UserType.Anonymous,
      showLoginModal: false,
      wikiId: 0,
      isAdmin: false,
      name: '',
      personalWiki: null as unknown as Topic
    }
  },
  actions: {
    initUserStore(loginState: LoginState) {
      this.isLoggedIn = loginState.IsLoggedIn
      this.id = loginState.UserId
    },
    async login(loginData) {
      var result = await $fetch<UserLoginResult>('/api/SessionUser/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' })

      if (!!result && result.Success){
        this.id = result.Id
        this.wikiId = result.WikiId
        this.isAdmin = result.IsAdmin
        this.name = result.Name
        this.showLoginModal = false
        this.isLoggedIn = true
        
        if (result.PersonalWikiId ? result.PersonalWikiId : 0)
          this.personalWiki = await $fetch<Topic>(`/api/Topic/GetTopic/${result.PersonalWikiId}`, { credentials: 'include' })
    
      }
    },

    async register(registerData) {
      var result = await $fetch<UserLoginResult>('/api/VueRegister/Register', { method: 'POST', body: registerData, mode: 'cors', credentials: 'include' })

      if (!!result && result.Success) {
        this.id = result.Id
        this.wikiId = result.WikiId
        this.isAdmin = result.IsAdmin
        this.name = result.Name
        this.isLoggedIn = true

        if (result.PersonalWikiId ? result.PersonalWikiId : 0)
          this.personalWiki = await $fetch<Topic>(`/api/Topic/GetTopic/${result.PersonalWikiId}`, { credentials: 'include' })

        return 'success'
      } else if (!!result && !result.Success)
        return result.Message
    },
    openLoginModal() {
      this.showLoginModal = true
    },
    async logout() {
      const spinnerStore = useSpinnerStore()
      spinnerStore.showSpinner()

      var result = await $fetch<UserLoginResult>('/api/SessionUser/Logout', { method: 'POST', mode: 'cors', credentials: 'include' 
        })
        
      if (!!result && result.Success) {
        spinnerStore.hideSpinner()
        this.isLoggedIn = false
        window.location.reload()
        }
      spinnerStore.hideSpinner()
      }
  }
})