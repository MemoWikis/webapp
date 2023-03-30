import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { Topic } from '../topic/topicStore'
import { useActivityPointsStore } from '../activityPoints/activityPointsStore'

export interface UserLoginResult {
    Success: boolean
    Message: string
    CurrentUser: CurrentUser
}

export interface CurrentUser {
    IsLoggedIn: boolean
    Id: number
    Name: string
    Email?: string
    IsAdmin: boolean
    WikiId: number
    Type: UserType
    ImgUrl: string
    Reputation: number
    ReputationPos: number
    PersonalWiki: Topic
    ActivityPoints: {
        points: number
        level: number
        levelUp: boolean
        activityPointsTillNextLevel: number
        activityPointsPercentageOfNextLevel?: number
    }
    UnreadMessagesCount?: number
}

export const useUserStore = defineStore('userStore', {
    state: () => {
        return {
            isLoggedIn: false,
            id: 0,
            type: UserType.Anonymous,
            showLoginModal: false,
            isAdmin: false,
            name: '',
            personalWiki: null as Topic | null,
            imgUrl: '',
            reputation: 0,
            reputationPos: 0,
            email: '',
            unreadMessagesCount: 0
        }
    },
    actions: {
        initUser(currentUser: CurrentUser) {
            this.isLoggedIn = currentUser.IsLoggedIn
            this.id = currentUser.Id
            this.name = currentUser.Name
            this.isAdmin = currentUser.IsAdmin
            this.type = currentUser.Type
            this.imgUrl = currentUser.ImgUrl
            this.reputation = currentUser.Reputation
            this.reputationPos = currentUser.ReputationPos
            this.personalWiki = currentUser.PersonalWiki
            this.email = currentUser.Email ? currentUser.Email : ''
            this.unreadMessagesCount = currentUser.UnreadMessagesCount ? currentUser.UnreadMessagesCount : 0

            const activityPointsStore = useActivityPointsStore()
            activityPointsStore.setData(currentUser.ActivityPoints)
        },
        async login(loginData: {
            EmailAddress: string,
            Password: string,
            PersistentLogin: boolean
        }) {
            const result = await $fetch<UserLoginResult>('/apiVue/UserStore/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' })

            if (!!result && result.Success) {
                this.showLoginModal = false
                this.initUser(result.CurrentUser)

            }
        },
        async register(registerData: {
            Name: string,
            Email: string,
            Password: string
        }) {
            const result = await $fetch<UserLoginResult>('/apiVue/UserStore/Register', { method: 'POST', body: registerData, mode: 'cors', credentials: 'include' })

            if (!!result && result.Success) {
                this.isLoggedIn = true
                this.initUser(result.CurrentUser)
                refreshNuxtData()
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

            var result = await $fetch<UserLoginResult>('/apiVue/UserStore/Logout', {
                method: 'POST', mode: 'cors', credentials: 'include'
            })

            if (!!result && result.Success) {
                spinnerStore.hideSpinner()
                this.isLoggedIn = false
                refreshNuxtData()
            }
            spinnerStore.hideSpinner()
        },
        async follow(id: number) {
            const result = await $fetch<boolean>('/apiVue/UserStore/Follow', { method: 'POST', body: { userId: id }, mode: 'cors', credentials: 'include' })

            if (result) return true
            else return false
        },
        async unfollow(id: number) {
            const result = await $fetch<boolean>('/apiVue/UserStore/UnFollow', { method: 'POST', body: { userId: id }, mode: 'cors', credentials: 'include' })

            if (result) return true
            else return false
        },
        async getUnreadMessagesCount() {
            this.unreadMessagesCount = await $fetch<number>('/apiVue/UserStore/GetUnreadMessagesCount', {
                method: 'GET',
                mode: 'cors',
                credentials: 'include'
            })
        }
    }
})