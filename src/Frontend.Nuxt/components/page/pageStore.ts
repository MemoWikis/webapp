import { defineStore } from 'pinia'
import { useUserStore } from '../user/userStore'
import { Visibility } from '../shared/visibilityEnum'
import { Author } from '../author/author'
import { PageItem } from '../search/searchHelper'
import { GridPageItem } from './content/grid/item/gridPageItem'
import { AlertType, useAlertStore } from '../alert/alertStore'
import { useSnackbarStore, SnackbarData } from '../snackBar/snackBarStore'
import { ErrorCode } from '../error/errorCodeEnum'
import { nanoid } from 'nanoid'
import { useLoadingStore } from '../loading/loadingStore'

export class Page {
    canAccess: boolean = false
    id: number = 0
    name: string = ''
    imageUrl: string = ''
    imageId: number = 0
    content: string = ''
    parentPageCount: number = 0
    parents: TinyPageModel[] = []
    childPageCount: number = 0
    directVisibleChildPageCount: number = 0
    views: number = 0
    commentCount: number = 0
    visibility: Visibility = Visibility.Private
    authorIds: number[] = []
    isWiki: boolean = false
    currentUserIsCreator: boolean = false
    canBeDeleted: boolean = false
    questionCount: number = 0
    directQuestionCount: number = 0
    authors: Author[] = []
    pageItem: PageItem | null = null
    metaDescription: string = ''
    knowledgeSummary: KnowledgeSummary = {
        solid: 0,
        needsConsolidation: 0,
        needsLearning: 0,
        notLearned: 0,

        total: 0,

        solidPercentage: 0,
        needsConsolidationPercentage: 0,
        needsLearningPercentage: 0,
        notLearnedPercentage: 0,
    }
    gridItems: GridPageItem[] = []
    isChildOfPersonalWiki: boolean = false
    textIsHidden: boolean = false
    messageKey: string | null = null
    errorCode: ErrorCode | null = null
    viewsLast30DaysAggregatedPage: ViewSummary[] | null = null
    viewsLast30DaysPage: ViewSummary[] | null = null
    viewsLast30DaysAggregatedQuestions: ViewSummary[] | null = null
    viewsLast30DaysQuestions: ViewSummary[] | null = null
    language: 'de' | 'en' | 'fr' | 'es' = 'en'
    canEdit: boolean = false
    isShared: boolean = false
    sharedWith: SharedWithUser[] | null = null
    canEditByToken: boolean | null = null
    totalQuestionViews: number = 0
}

export interface ViewSummary {
    count: number
    date: string
}

export interface SharedWithUser {
    id: number
    name: string
    imgUrl: string
}

export interface KnowledgeSummarySlim {
    solid: number
    needsConsolidation: number
    needsLearning: number
    notLearned: number
}

export interface FooterPages {
    rootWiki: Page
    mainPages: Page[]
    memoWiki: Page
    memoPages: Page[]
    helpPages: Page[]
    popularPages: Page[]
    documentation: Page
}

export interface TinyPageModel {
    id: number
    name: string
    imgUrl: string
}

export interface GeneratedFlashcard {
    front: string
    back: string
}

interface GetPageAnalyticsResponse {
    viewsPast90DaysAggregatedPages: ViewSummary[]
    viewsPast90DaysPage: ViewSummary[]
    viewsPast90DaysAggregatedQuestions: ViewSummary[]
    viewsPast90DaysDirectQuestions: ViewSummary[]
}

export interface GenerateFlashcardResponse {
    flashcards: GeneratedFlashcard[]
    messageKey: string
}

export const usePageStore = defineStore('pageStore', {
    state: () => {
        return {
            id: 0,
            name: '',
            initialName: '',
            imgUrl: '',
            imgId: 0,
            questionCount: 0,
            directQuestionCount: 0,
            content: '',
            initialContent: '',
            contentHasChanged: false,
            nameHasChanged: false,
            parentPageCount: 0,
            parents: [] as TinyPageModel[],
            childPageCount: 0,
            directVisibleChildPageCount: 0,
            views: 0,
            commentCount: 0,
            visibility: null as Visibility | null,
            authorIds: [] as number[],
            isWiki: false,
            currentUserIsCreator: false,
            canBeDeleted: false,
            authors: [] as Author[],
            searchPageItem: null as null | PageItem,
            knowledgeSummary: {} as KnowledgeSummary,
            knowledgeSummarySlim: {} as KnowledgeSummarySlim,
            gridItems: [] as GridPageItem[],
            isChildOfPersonalWiki: false,
            textIsHidden: false,
            uploadedImagesInContent: [] as string[],
            uploadedImagesMarkedForDeletion: [] as string[],
            uploadTrackingArray: [] as string[],
            viewsPast90DaysAggregatedPages: [] as ViewSummary[],
            viewsPast90DaysPage: [] as ViewSummary[],
            viewsPast90DaysAggregatedQuestions: [] as ViewSummary[],
            viewsPast90DaysDirectQuestions: [] as ViewSummary[],
            analyticsLoaded: false,
            saveTrackingArray: [] as string[],
            currentWiki: null as TinyPageModel | null,
            text: '',
            selectedText: '',
            contentLanguage: 'en' as 'en' | 'de' | 'fr' | 'es',
            canEdit: false,
            shareToken: null as string | null,
            isShared: false,
            sharedWith: [] as SharedWithUser[],
            canEditByToken: null as boolean | null,
            totalQuestionViews: 0,
        }
    },
    actions: {
        setPage(page: Page) {
            this.shareToken = null

            if (page != null) {
                this.id = page.id
                this.name = page.name
                this.initialName = page.name
                this.imgUrl = page.imageUrl
                this.imgId = page.imageId
                this.content = page.content
                this.initialContent = page.content

                this.parentPageCount = page.parentPageCount
                this.parents = page.parents
                this.childPageCount = page.childPageCount
                this.directVisibleChildPageCount =
                    page.directVisibleChildPageCount

                this.views = page.views
                this.commentCount = page.commentCount
                this.visibility = page.visibility

                this.authorIds = page.authorIds
                this.isWiki = page.isWiki
                this.currentUserIsCreator = page.currentUserIsCreator
                this.canBeDeleted = page.canBeDeleted

                this.questionCount = page.questionCount
                this.directQuestionCount = page.directQuestionCount

                this.authors = page.authors
                this.searchPageItem = page.pageItem

                this.knowledgeSummary = page.knowledgeSummary
                this.setKnowledgeSummarySlim(page.knowledgeSummary)
                this.gridItems = page.gridItems
                this.isChildOfPersonalWiki = page.isChildOfPersonalWiki
                this.textIsHidden = page.textIsHidden
                this.uploadedImagesInContent = []
                this.uploadedImagesMarkedForDeletion = []

                this.analyticsLoaded = false
                this.viewsPast90DaysAggregatedPages = []
                this.viewsPast90DaysPage = []
                this.viewsPast90DaysAggregatedQuestions = []
                this.viewsPast90DaysDirectQuestions = []
                this.text = ''
                this.selectedText = ''

                this.contentLanguage = page.language
                this.canEdit = page.canEdit
                this.isShared = page.isShared
                this.sharedWith = page.sharedWith || []
                this.canEditByToken = page.canEditByToken

                this.totalQuestionViews = page.totalQuestionViews

                this.handleLoginReminder()
            }
        },
        setKnowledgeSummarySlim(knowledgeSummary: KnowledgeSummary) {
            this.knowledgeSummarySlim = {
                solid: knowledgeSummary.solid,
                needsConsolidation: knowledgeSummary.needsConsolidation,
                needsLearning: knowledgeSummary.needsLearning,
                notLearned: knowledgeSummary.notLearned,
            }
        },
        async saveContent() {
            const userStore = useUserStore()
            const snackbarStore = useSnackbarStore()

            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            if (this.contentHasChanged == false) return
            await this.waitUntilAllUploadsComplete()
            await this.waitUntilAllSavingsComplete()

            const uploadId = nanoid(5)
            this.saveTrackingArray.push(uploadId)

            const data = {
                id: this.id,
                content: this.content,
                shareToken: this.shareToken,
            }

            const result = await $api<FetchResult<boolean>>(
                '/apiVue/PageStore/SaveContent',
                {
                    method: 'POST',
                    body: data,
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    host: context.request,
                                },
                            ]
                        )
                    },
                }
            )
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            if (
                result.success == true &&
                this.visibility != Visibility.Private
            ) {
                const data: SnackbarData = {
                    type: 'success',
                    text: { message: $i18n.t('success.page.saved') },
                }
                snackbarStore.showSnackbar(data)
                this.initialContent = this.content
                this.contentHasChanged = false
            } else if (result.success === false && result.messageKey != null) {
                if (
                    !(
                        result.messageKey === 'error.page.noChange' &&
                        this.visibility == Visibility.Private
                    )
                ) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, {
                        text: $i18n.t(result.messageKey),
                    })
                }
            }

            this.saveTrackingArray = this.saveTrackingArray.filter(
                (id) => id !== uploadId
            )
        },
        async saveName() {
            const userStore = useUserStore()

            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            if (this.name === this.initialName) return

            await this.waitUntilAllUploadsComplete()
            await this.waitUntilAllSavingsComplete()

            const uploadId = nanoid(5)
            this.saveTrackingArray.push(uploadId)

            const data = {
                id: this.id,
                name: this.name,
                shareToken: this.shareToken,
            }

            const result = await $api<FetchResult<boolean>>(
                '/apiVue/PageStore/SaveName',
                {
                    method: 'POST',
                    body: data,
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    host: context.request,
                                },
                            ]
                        )
                    },
                }
            )
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            if (result.success && this.visibility != Visibility.Private) {
                const data: SnackbarData = {
                    type: 'success',
                    text: { message: $i18n.t('success.page.saved') },
                }
                const snackbarStore = useSnackbarStore()

                snackbarStore.showSnackbar(data)
                this.initialName = this.name
                this.nameHasChanged = false
            } else if (result.success === false && result.messageKey != null) {
                if (
                    !(
                        result.messageKey === 'error.page.noChange' &&
                        this.visibility === Visibility.Private
                    )
                ) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, {
                        text: $i18n.t(result.messageKey),
                    })
                }
            }

            this.saveTrackingArray = this.saveTrackingArray.filter(
                (id) => id !== uploadId
            )
        },
        async waitUntilAllSavingsComplete() {
            while (this.saveTrackingArray.length > 0) {
                await new Promise((resolve) => setTimeout(resolve, 100))
            }
        },
        reset() {
            this.name = this.initialName
            this.nameHasChanged = false
            this.content = this.initialContent
            this.contentHasChanged = false
            this.uploadedImagesInContent = []
            this.uploadedImagesMarkedForDeletion = []
        },
        isOwnerOrAdmin() {
            const userStore = useUserStore()
            return userStore.isAdmin || this.currentUserIsCreator
        },

        async refreshPageImage() {
            this.imgUrl = await $api<string>(
                `/apiVue/PageStore/GetPageImageUrl/${this.id}`,
                {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    req: context.request,
                                },
                            ]
                        )
                    },
                }
            )
        },
        async reloadKnowledgeSummary() {
            this.knowledgeSummarySlim = await $api<KnowledgeSummarySlim>(
                `/apiVue/PageStore/GetUpdatedKnowledgeSummary/${this.id}`,
                {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    req: context.request,
                                },
                            ]
                        )
                    },
                }
            )
        },
        async reloadGridItems() {
            const result = await $api<GridPageItem[]>(
                `/apiVue/PageStore/GetGridPageItems/${this.id}`,
                {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    host: context.request,
                                },
                            ]
                        )
                    },
                }
            )

            if (result) this.gridItems = result
        },
        async hideOrShowText() {
            if (
                (!!this.content && this.content.length > 0) ||
                this.contentHasChanged
            )
                return

            const data = {
                hideText: !this.textIsHidden,
                pageId: this.id,
            }
            const result = await $api<boolean>(
                `/apiVue/PageStore/HideOrShowText/`,
                {
                    body: data,
                    method: 'POST',
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    req: context.request,
                                },
                            ]
                        )
                    },
                }
            )

            this.textIsHidden = result
        },
        async uploadContentImage(file: File): Promise<string> {
            const uploadId = nanoid(5)
            this.uploadTrackingArray.push(uploadId)

            const data = new FormData()
            data.append('file', file)
            data.append('pageId', this.id.toString())

            const result = await $api<string>(
                '/apiVue/PageStore/UploadContentImage',
                {
                    body: data,
                    method: 'POST',
                    mode: 'cors',
                    credentials: 'include',
                }
            )

            this.uploadTrackingArray = this.uploadTrackingArray.filter(
                (id) => id !== uploadId
            )

            return result
        },
        addImageUrlToDeleteList(url: string) {
            if (!this.uploadedImagesMarkedForDeletion.includes(url))
                this.uploadedImagesMarkedForDeletion.push(url)
        },
        refreshDeleteImageList() {
            const imagesToKeep = this.uploadedImagesInContent
            this.uploadedImagesMarkedForDeletion =
                this.uploadedImagesMarkedForDeletion.filter((url) =>
                    imagesToKeep.includes(url)
                )
        },
        async deletePageContentImages() {
            if (this.uploadedImagesMarkedForDeletion.length == 0) return

            const data = {
                pageId: this.id,
                imageUrls: this.uploadedImagesMarkedForDeletion,
            }
            await $api<void>('/apiVue/PageStore/DeleteContentImages', {
                body: data,
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            })
            this.uploadedImagesMarkedForDeletion = []
        },
        async waitUntilAllUploadsComplete() {
            while (this.uploadTrackingArray.length > 0) {
                await new Promise((resolve) => setTimeout(resolve, 100))
            }
        },
        async getAnalyticsData() {
            const data = await $api<GetPageAnalyticsResponse>(
                `/apiVue/PageStore/GetPageAnalytics/${this.id}`,
                {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    req: context.request,
                                },
                            ]
                        )
                    },
                }
            )

            if (data) {
                this.viewsPast90DaysAggregatedPages =
                    data.viewsPast90DaysAggregatedPages
                this.viewsPast90DaysPage = data.viewsPast90DaysPage
                this.viewsPast90DaysAggregatedQuestions =
                    data.viewsPast90DaysAggregatedQuestions
                this.viewsPast90DaysDirectQuestions =
                    data.viewsPast90DaysDirectQuestions

                this.analyticsLoaded = true
            }
        },
        async generateFlashcard(
            selectedText?: string
        ): Promise<GenerateFlashcardResponse> {
            const loadingStore = useLoadingStore()
            loadingStore.startLoading(9000, 'Karteikarten werden generiert')
            const data = {
                pageId: this.id,
                text:
                    (selectedText ?? '').length > 0 ? selectedText : this.text,
            }
            const result = await $api<GenerateFlashcardResponse>(
                `/apiVue/PageStore/GenerateFlashcard/`,
                {
                    body: data,
                    method: 'POST',
                    mode: 'cors',
                    credentials: 'include',
                }
            )

            if (selectedText != null && selectedText.length > 0)
                this.selectedText = selectedText

            await loadingStore.finishLoading()

            return result
        },
        async reGenerateFlashcard(): Promise<GenerateFlashcardResponse> {
            return await this.generateFlashcard(this.selectedText)
        },
        async updateQuestionCount() {
            const result = await $api<number>(
                `/apiVue/PageStore/GetQuestionCount/${this.id}`,
                {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include',
                    onResponseError(context) {
                        const { $logger } = useNuxtApp()
                        $logger.error(
                            `fetch Error: ${context.response?.statusText}`,
                            [
                                {
                                    response: context.response,
                                    req: context.request,
                                },
                            ]
                        )
                    },
                }
            )

            this.questionCount = result
        },
        setToken(token: string | null) {
            this.shareToken = token
            const userStore = useUserStore()
            if (token !== null) userStore.addShareToken(this.id, token)
        },
        async updateIsShared() {
            let url = `/apiVue/PageStore/GetIsShared/${this.id}`
            if (this.shareToken) {
                url += `?shareToken=${this.shareToken}`
            }

            const result = await $api<boolean>(url, {
                method: 'GET',
                mode: 'cors',
                credentials: 'include',
                onResponseError(context) {
                    const { $logger } = useNuxtApp()
                    $logger.error(
                        `fetch Error: ${context.response?.statusText}`,
                        [
                            {
                                response: context.response,
                                req: context.request,
                            },
                        ]
                    )
                },
            })
            this.isShared = result
        },

        handleLoginReminder() {
            const userStore = useUserStore()
            userStore.showLoginReminder = false

            if (this.isShared && this.canEditByToken) {
                if (!userStore.isLoggedIn) {
                    userStore.showLoginReminder = true
                }
            }
        },
    },
    getters: {
        getPageName(): string {
            return this.name
        },
    },
})
