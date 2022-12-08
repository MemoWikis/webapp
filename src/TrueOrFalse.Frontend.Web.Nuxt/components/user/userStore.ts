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
  PersonalWikiId: number
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
      personalWiki: null as Topic | null
    }
  },
  actions: {
    initUserStore(loginState: LoginState) {
      this.isLoggedIn = loginState.IsLoggedIn
      this.id = loginState.UserId
    },
    async login(loginData: {
      EmailAddress: string,
      Password: string,
      PersistentLogin: boolean
    }) {
      var result = await $fetch<UserLoginResult>('/apiVue/SessionUser/Login', { baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local', method: 'POST', body: loginData, mode: 'cors', credentials: 'include' })

      if (!!result && result.Success) {
        this.id = result.Id
        this.wikiId = result.WikiId
        this.isAdmin = result.IsAdmin
        this.name = result.Name
        this.showLoginModal = false
        this.isLoggedIn = true

        if (result.PersonalWikiId ? result.PersonalWikiId : 0)
          this.personalWiki = await $fetch<Topic>(`/apiVue/Topic/GetTopic/${result.PersonalWikiId}`, { baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local', credentials: 'include' })

      }
    },

    async register(registerData: {
      Name: string,
      Email: string,
      Password: string
    }) {
      var result = await $fetch<UserLoginResult>('/apiVue/VueRegister/Register', { baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local', method: 'POST', body: registerData, mode: 'cors', credentials: 'include' })

      if (!!result && result.Success) {
        this.id = result.Id
        this.wikiId = result.WikiId
        this.isAdmin = result.IsAdmin
        this.name = result.Name
        this.isLoggedIn = true

        if (result.PersonalWikiId ? result.PersonalWikiId : 0)
          this.personalWiki = await $fetch<Topic>(`/apiVue/Topic/GetTopic/${result.PersonalWikiId}`, { baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local', credentials: 'include' })

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

      var result = await $fetch<UserLoginResult>('/apiVue/SessionUser/Logout', {
        baseURL: process.client ? 'http://memucho.local:3000' : 'http://memucho.local', method: 'POST', mode: 'cors', credentials: 'include'
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