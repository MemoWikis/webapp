import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useSpinnerStore } from '../spinner/spinnerStore'
const isLoggedIn = (e) => useState<boolean>('isLoggedIn', e)

type UserLoginResult = {
  Success: boolean;
  Message: string;
}

export type LoginState = {
  IsLoggedIn: boolean;
  UserId: number;
}

export const useUserStore = defineStore('userStore', {
  state: () => {
    return {
      isLoggedIn: false,
      id: 0,
      type: UserType.Anonymous,
      showLoginModal: false
    }
  },
  actions: {
    async login(loginData) {
      const spinnerStore = useSpinnerStore()
      spinnerStore.showSpinner()
      var result = await $fetch<UserLoginResult>('/api/Login/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' 
      })

      if (!!result && result.Success){
        this.isLoggedIn = true
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

      var result = await $fetch<UserLoginResult>('/api/Login/Logout', { method: 'POST', mode: 'cors', credentials: 'include' 
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