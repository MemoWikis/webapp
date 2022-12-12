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

export interface CurrentUser {
  IsLoggedIn: boolean
  UserId: number
  Name: string
  IsAdmin: boolean
  WikiId: number
  Type: UserType
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
    async initCurrentUser() {
      const config = useRuntimeConfig()
      var currentUser = await $fetch<CurrentUser>(`/apiVue/VueSessionUser/GetCurrentUser/`, {
        baseURL: config.public.clientBasez,
        method: 'Get',
        credentials: 'include',
        mode: 'no-cors',
      })
      this.initUserStore(currentUser)
    },
    initUserStore(currentUser: CurrentUser) {
      this.isLoggedIn = currentUser.IsLoggedIn
      this.id = currentUser.UserId
      this.name = currentUser.Name
      this.isAdmin = currentUser.IsAdmin
      this.wikiId = currentUser.WikiId
      this.type = currentUser.Type

      this.getPersonalWiki(this.wikiId)
    },
    async login(loginData: {
      EmailAddress: string,
      Password: string,
      PersistentLogin: boolean
    }) {
      var result = await $fetch<UserLoginResult>('/apiVue/VueSessionUser/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' })

      if (!!result && result.Success) {
        this.id = result.Id
        this.wikiId = result.WikiId
        this.isAdmin = result.IsAdmin
        this.name = result.Name
        this.showLoginModal = false
        this.isLoggedIn = true

        if (result.PersonalWikiId ? result.PersonalWikiId : 0)
          this.getPersonalWiki(result.PersonalWikiId)
      }
    },
    async getPersonalWiki(id: number) {
      this.personalWiki = await $fetch<Topic>(`/apiVue/Topic/GetTopic/${id}`, { credentials: 'include' })
    },
    async register(registerData: {
      Name: string,
      Email: string,
      Password: string
    }) {
      var result = await $fetch<UserLoginResult>('/apiVue/VueRegister/Register', { method: 'POST', body: registerData, mode: 'cors', credentials: 'include' })

      if (!!result && result.Success) {
        this.id = result.Id
        this.wikiId = result.WikiId
        this.isAdmin = result.IsAdmin
        this.name = result.Name
        this.isLoggedIn = true

        if (result.PersonalWikiId ? result.PersonalWikiId : 0)
          this.personalWiki = await $fetch<Topic>(`/apiVue/Topic/GetTopic/${result.PersonalWikiId}`, { baseURL: 'http://memucho.local:3000', credentials: 'include' })

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

      var result = await $fetch<UserLoginResult>('/apiVue/VueSessionUser/Logout', {
        method: 'POST', mode: 'cors', credentials: 'include'
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