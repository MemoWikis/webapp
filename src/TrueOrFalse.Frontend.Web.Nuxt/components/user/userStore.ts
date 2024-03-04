import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { Topic } from '../topic/topicStore'
import { useActivityPointsStore } from '../activityPoints/activityPointsStore'
import * as Subscription from '~~/components/user/membership/subscription'
import { AlertType, messages, useAlertStore } from '../alert/alertStore'

export interface CurrentUser {
    isLoggedIn: boolean
    id: number
    name: string
    email?: string
    isAdmin: boolean
    wikiId: number
    type: UserType
    imgUrl: string
    reputation: number
    reputationPos: number
    personalWiki: Topic
    activityPoints: {
        points: number
        level: number
        levelUp: boolean
        activityPointsTillNextLevel: number
        activityPointsPercentageOfNextLevel?: number
    }
    unreadMessagesCount?: number
    subscriptionType?: Subscription.Type
    hasStripeCustomerId: boolean
    endDate?: Date
    subscriptionStartDate?: Date
    isSubscriptionCanceled: boolean
    isEmailConfirmed: boolean
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
            hasStripeCustomerId: false,
            EndDate: null as Date | null,
            isSubscriptionCanceled: false,
            subscriptionStartDate: null as Date | null,
            isEmailConfirmed: false
        }
    },
    actions: {
        initUser(currentUser: CurrentUser) {
            this.isLoggedIn = currentUser.isLoggedIn
            this.id = currentUser.id
            this.name = currentUser.name
            this.isAdmin = currentUser.isAdmin
            this.type = currentUser.type
            this.imgUrl = currentUser.imgUrl
            this.reputation = currentUser.reputation
            this.reputationPos = currentUser.reputationPos
            this.personalWiki = currentUser.personalWiki
            this.email = currentUser.email ? currentUser.email : ''
            this.unreadMessagesCount = currentUser.unreadMessagesCount ? currentUser.unreadMessagesCount : 0
            this.subscriptionType = currentUser.subscriptionType != null ? currentUser.subscriptionType : null
            this.hasStripeCustomerId = currentUser.hasStripeCustomerId
            this.EndDate = currentUser.endDate != null ? new Date(currentUser.endDate) : null
            this.isSubscriptionCanceled = currentUser.isSubscriptionCanceled
            this.subscriptionStartDate = currentUser.subscriptionStartDate != null ? new Date(currentUser.subscriptionStartDate) : null
            const activityPointsStore = useActivityPointsStore()
            activityPointsStore.setData(currentUser.activityPoints)
            this.isEmailConfirmed = currentUser.isEmailConfirmed
            console.log(currentUser);
            return
        },
        async login(loginData: {
            EmailAddress: string,
            Password: string,
            PersistentLogin: boolean
        }) {
            const result = await $fetch<FetchResult<CurrentUser>>('/apiVue/UserStore/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' })
            console.log(result)
            if (!!result && result.success) {
                this.showLoginModal = false
                this.initUser(result.data)
                return { success: true }
            } else return { success: false, msg: messages.getByCompositeKey(result.messageKey) }
        },
        async register(registerData: {
            Name: string,
            Email: string,
            Password: string
        }) {
            const result = await $fetch<FetchResult<CurrentUser>>('/apiVue/UserStore/Register', { method: 'POST', body: registerData, mode: 'cors', credentials: 'include' })

            if (!!result && result.success) {
                this.isLoggedIn = true
                this.initUser(result.data)
                await refreshNuxtData()
                return 'success'
            } else if (!!result && !result.success)
                return messages.getByCompositeKey(result.messageKey)
        },
        openLoginModal() {
            this.showLoginModal = true
        },
        async logout() {
            const spinnerStore = useSpinnerStore()

            spinnerStore.showSpinner()

            const result = await $fetch<FetchResult<any>>('/apiVue/UserStore/Logout', {
                method: 'POST', mode: 'cors', credentials: 'include'
            })
            spinnerStore.hideSpinner()

            if (result?.success) {
                return true
            } else if (result?.success == false) {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey() })
                return false
            }
            return false
        },
        async resetPassword(email: string): Promise<FetchResult<void>> {
            const { $logger } = useNuxtApp()
            const alertStore = useAlertStore()
            const result = await $fetch<FetchResult<void>>('/apiVue/UserStore/ResetPassword', {
                mode: 'cors',
                method: 'POST',
                body: { email: email },
                credentials: 'include',
                onResponseError(context) {
                    $logger.error(`resetpassword: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
                    alertStore.openAlert(AlertType.Error, messages.errror.default)
                }
            })
            return result
        },
        async getUnreadMessagesCount() {
            this.unreadMessagesCount = await $fetch<number>('/apiVue/UserStore/GetUnreadMessagesCount', {
                method: 'GET',
                mode: 'cors',
                credentials: 'include'
            })
        },
        async requestVerificationMail() {
            const { $logger } = useNuxtApp()
            const alertStore = useAlertStore()
            const result = await $fetch<FetchResult<void>>('/apiVue/UserStore/RequestVerificationMail', {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                onResponseError(context) {
                    $logger.error(`requestVerificationMail: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
                    alertStore.openAlert(AlertType.Error, messages.errror.default)
                }
            })
            return result
        },
        reset() {
            this.$reset()
        },
        apiLogin(result: boolean) {
            return result
        }
    }
})