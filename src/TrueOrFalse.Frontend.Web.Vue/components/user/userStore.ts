import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'

type UserLoginResult = {
  Success: boolean;
  Message: string;
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
      var result = await $fetch<UserLoginResult>('/api/Login/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' 
      })
      if (!!result && result.Success)
        this.isLoggedIn = true
    },
    openLoginModal() {
      this.showLoginModal = true
    }
  }
})