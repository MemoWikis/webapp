import { defineStore } from 'pinia'
import { UserType } from './userTypeEnum'
import { useLoadingStore } from '../loading/loadingStore'
import { Page } from '../page/pageStore'
import { useActivityPointsStore } from '../activityPoints/activityPointsStore'
import * as Subscription from '~~/components/user/membership/subscription'
import { AlertType, useAlertStore } from '../alert/alertStore'

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
    personalWiki: Page
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
    collaborationToken?: string
    uiLanguage: 'de' | 'en' | 'fr' | 'es'
}

export enum FontSize {
    Small = 0,
    Medium = 1,
    Large = 2,
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
            personalWiki: null as Page | null,
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
            isEmailConfirmed: false,
            showBanner: false,
            gridInfoShown: false,
            collaborationToken: undefined as string | undefined,
            fontSize: FontSize.Medium,
            uiLanguage: 'en' as 'de' | 'en' | 'fr' | 'es',
            showLoginReminder: false,
            showAsVisitor: false,
            tokenBalance: null as number | null,
            isLoadingTokenBalance: false
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
            this.unreadMessagesCount = currentUser.unreadMessagesCount
                ? currentUser.unreadMessagesCount
                : 0
            this.subscriptionType =
                currentUser.subscriptionType != null
                    ? currentUser.subscriptionType
                    : null
            this.hasStripeCustomerId = currentUser.hasStripeCustomerId
            this.EndDate =
                currentUser.endDate != null
                    ? new Date(currentUser.endDate)
                    : null
            this.isSubscriptionCanceled = currentUser.isSubscriptionCanceled
            this.subscriptionStartDate =
                currentUser.subscriptionStartDate != null
                    ? new Date(currentUser.subscriptionStartDate)
                    : null
            const activityPointsStore = useActivityPointsStore()
            activityPointsStore.setData(currentUser.activityPoints)
            this.isEmailConfirmed = currentUser.isEmailConfirmed
            this.collaborationToken = currentUser.collaborationToken
            this.uiLanguage = currentUser.uiLanguage

            if (currentUser.isLoggedIn) this.showLoginReminder = false

            return
        },
        async login(loginData: {
            EmailAddress: string
            Password: string
            PersistentLogin: boolean
        }) {
            const result = await $api<FetchResult<CurrentUser>>(
                '/apiVue/UserStore/Login',
                {
                    method: 'POST',
                    body: loginData,
                    mode: 'cors',
                    credentials: 'include',
                }
            )
            if (!!result && result.success) {
                this.showLoginModal = false
                this.initUser(result.data)
                return { success: true }
            } else {
                const nuxtApp = useNuxtApp()
                const { $i18n } = nuxtApp

                return { success: false, msg: $i18n.t(result.messageKey) }
            }
        },
        async register(registerData: {
            Name: string
            Email: string
            Password: string
            Language: string
        }) {
            const result = await $api<FetchResult<CurrentUser>>(
                '/apiVue/UserStore/Register',
                {
                    method: 'POST',
                    body: registerData,
                    mode: 'cors',
                    credentials: 'include',
                }
            )

            if (!!result && result.success) {
                this.isLoggedIn = true
                this.initUser(result.data)
                return 'success'
            } else if (!!result && !result.success) {
                return result.messageKey
            }
        },
        openLoginModal() {
            this.showLoginModal = true
        },
        async logout() {
            const loadingStore = useLoadingStore()

            loadingStore.startLoading()

            const result = await $api<FetchResult<any>>(
                '/apiVue/UserStore/Logout',
                {
                    method: 'POST',
                    mode: 'cors',
                    credentials: 'include',
                }
            )
            loadingStore.stopLoading()

            if (result?.success) {
                return true
            } else if (result?.success == false) {
                const alertStore = useAlertStore()
                const nuxtApp = useNuxtApp()
                const { $i18n } = nuxtApp

                alertStore.openAlert(AlertType.Error, {
                    text: $i18n.t(result.messageKey),
                })
                return false
            }
            return false
        },
        async resetPassword(email: string): Promise<FetchResult<void>> {
            const { $logger } = useNuxtApp()
            const alertStore = useAlertStore()
            const result = await $api<FetchResult<void>>(
                '/apiVue/UserStore/ResetPassword',
                {
                    mode: 'cors',
                    method: 'POST',
                    body: { email: email },
                    credentials: 'include',
                    onResponseError(context) {
                        $logger.error(
                            `resetpassword: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    host: context.request,
                                },
                            ]
                        )
                        const nuxtApp = useNuxtApp()
                        const { $i18n } = nuxtApp

                        alertStore.openAlert(AlertType.Error, {
                            text: $i18n.t('error.default'),
                        })
                    },
                }
            )
            return result
        },
        async getUnreadMessagesCount() {
            this.unreadMessagesCount = await $api<number>(
                '/apiVue/UserStore/GetUnreadMessagesCount',
                {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include',
                }
            )
        },
        async requestVerificationMail() {
            const { $logger } = useNuxtApp()
            const alertStore = useAlertStore()
            const result = await $api<FetchResult<void>>(
                '/apiVue/UserStore/RequestVerificationMail',
                {
                    method: 'POST',
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        $logger.error(
                            `requestVerificationMail: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    host: context.request,
                                },
                            ]
                        )
                        const nuxtApp = useNuxtApp()
                        const { $i18n } = nuxtApp

                        alertStore.openAlert(AlertType.Error, {
                            text: $i18n.t('error.default'),
                        })
                    },
                }
            )
            return result
        },
        reset() {
            this.$reset()
        },
        apiLogin(result: boolean) {
            return result
        },
        setFontSize(fontSize: FontSize) {
            this.fontSize = fontSize
            if (import.meta.client)
                document.cookie = `fontSize=${fontSize}-${this.id}; expires=Fri, 31 Dec 9999 23:59:59 GMT`
        },
        deleteUser() {
            this.$reset()
        },
        async updateLanguageSetting(language: 'de' | 'en' | 'fr' | 'es') {
            if (this.uiLanguage === language) return

            this.uiLanguage = language

            if (!this.isLoggedIn) return

            const result = await $api<FetchResult<void>>(
                '/apiVue/UserStore/UpdateLanguageSetting',
                {
                    method: 'POST',
                    body: { language },
                    mode: 'cors',
                    credentials: 'include',
                }
            )
        },
        async addShareToken(pageId: number, shareToken: string) {
            await $api<void>('/apiVue/UserStore/AddShareToken', {
                method: 'POST',
                body: { pageId, shareToken },
                mode: 'cors',
                credentials: 'include',
            })
        },
        toggleShowAsVisitor() {
            this.showAsVisitor = !this.showAsVisitor
        },
        async fetchTokenBalance() {
            if (!this.isLoggedIn) {
                this.tokenBalance = null
                return
            }

            this.isLoadingTokenBalance = true
            try {
                interface GetTokenBalanceResponse {
                    success: boolean
                    totalBalance: number
                }

                const result = await $api<GetTokenBalanceResponse>('/apiVue/UserStore/GetTokenBalance', {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include'
                })

                if (result.success) {
                    this.tokenBalance = result.totalBalance
                }
            } catch (error) {
                console.error('Failed to fetch token balance:', error)
            } finally {
                this.isLoadingTokenBalance = false
            }
        }
    },
    getters: {
        showLoginToEditReminderBanner(): boolean {
            return !this.isLoggedIn && this.showLoginReminder
        },
    },
})
