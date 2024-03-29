import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { Topic } from '../topic/topicStore'
import { useActivityPointsStore } from '../activityPoints/activityPointsStore'
import * as Subscription from '~~/components/user/membership/subscription'
import { AlertType, messages, useAlertStore } from '../alert/alertStore'

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
    HasStripeCustomerId: boolean
    EndDate?: Date
    SubscriptionStartDate?: Date
    IsSubscriptionCanceled: boolean
    IsEmailConfirmed: boolean
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
            this.hasStripeCustomerId = currentUser.HasStripeCustomerId
            this.EndDate = currentUser.EndDate != null ? new Date(currentUser.EndDate) : null
            this.isSubscriptionCanceled = currentUser.IsSubscriptionCanceled
            this.subscriptionStartDate = currentUser.SubscriptionStartDate != null ? new Date(currentUser.SubscriptionStartDate) : null
            const activityPointsStore = useActivityPointsStore()
            activityPointsStore.setData(currentUser.ActivityPoints)
            this.isEmailConfirmed = currentUser.IsEmailConfirmed
            return
        },
        async login(loginData: {
            EmailAddress: string,
            Password: string,
            PersistentLogin: boolean
        }) {
            const result = await $fetch<FetchResult<CurrentUser>>('/apiVue/UserStore/Login', { method: 'POST', body: loginData, mode: 'cors', credentials: 'include' })

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