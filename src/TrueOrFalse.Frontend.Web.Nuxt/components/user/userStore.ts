import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { Topic } from '../topic/topicStore'
import { useActivityPointsStore } from '../activityPoints/activityPointsStore'
import * as Subscription from '~~/components/user/membership/subscription'

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
    SubscriptionType?: Subscription.Type
    SubscriptionDuration?: Date
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
            unreadMessagesCount: 0,
            subscriptionType: null as Subscription.Type | null,
            subscriptionDuration: null as Date | null
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
            this.subscriptionType = currentUser.SubscriptionType != null ? currentUser.SubscriptionType : null
            this.subscriptionDuration = currentUser.SubscriptionDuration != null ? currentUser.SubscriptionDuration : null

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
                return { success: true }
            } else return { success: false, msg: result.Message }
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
            this.isLoggedIn = false

            const spinnerStore = useSpinnerStore()

            spinnerStore.showSpinner()

            var result = await $fetch<UserLoginResult>('/apiVue/UserStore/Logout', {
                method: 'POST', mode: 'cors', credentials: 'include'
            })

            if (!!result && result.Success) {
                spinnerStore.hideSpinner()
                refreshNuxtData()
                this.$reset()
            }
            spinnerStore.hideSpinner()
        },
        async resetPassword(email: string) {
            const result = await $fetch<boolean>('/apiVue/VueUserSettings/ResetPassword', {
                mode: 'cors',
                method: 'POST',
                credentials: 'include'
            })
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