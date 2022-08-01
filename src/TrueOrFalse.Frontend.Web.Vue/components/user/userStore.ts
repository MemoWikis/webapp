import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useSpinnerStore } from '../spinner/spinnerStore'
const isLoggedIn = (e) => useState<boolean>('isLoggedIn', e)

type UserLoginResult = {
  Success: boolean
  Message: string
  Id: number
  WikiId: number
  IsAdmin: boolean
  Name: string
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
      name: ''
    }
  },
  actions: {
    initUserStore(loginState: LoginState) {
      this.isLoggedIn = loginState.IsLoggedIn
      this.id = loginState.UserId
    },
    async login(loginData) {
      const spinnerStore = useSpinnerStore()
      spinnerStore.showSpinner()
      var result = await $fetch<UserLoginResult>('/api/SessionUser/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' 
      })

      if (!!result && result.Success){
        this.isLoggedIn = true
        this.id = result.Id
        this.wikiId = result.WikiId
        this.isAdmin = result.IsAdmin
        this.name = result.Name
        window.location.reload()
      }
      spinnerStore.hideSpinner()
      this.showLoginModal = false
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
        this.isLoggedIn = false
        isLoggedIn(false)
        window.location.reload()
        }
      spinnerStore.hideSpinner()

      }
  }
})