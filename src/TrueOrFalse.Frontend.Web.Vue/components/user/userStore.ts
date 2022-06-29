import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'

export const useUserStore = defineStore('userStore', {
  state: () => {
    return {
      isLoggedIn: false,
      id: 0,
      type: null as UserType
    }
  },
})