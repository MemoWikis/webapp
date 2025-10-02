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
    subpageViews: number = 0
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

        knowledgeStatusPoints: 0,
        knowledgeStatusPointsTotal: 0,
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
    directQuestionViews: number = 0
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

export const usePageStore = defineStore('pageStore', () => {
    // State
    const id = ref(0)
    const name = ref('')
    const initialName = ref('')
    const imgUrl = ref('')
    const imgId = ref(0)
    const questionCount = ref(0)
    const directQuestionCount = ref(0)
    const content = ref('')
    const initialContent = ref('')
    const contentHasChanged = ref(false)
    const nameHasChanged = ref(false)
    const parentPageCount = ref(0)
    const parents = ref<TinyPageModel[]>([])
    const childPageCount = ref(0)
    const directVisibleChildPageCount = ref(0)
    const views = ref(0)
    const subpageViews = ref(0)
    const commentCount = ref(0)
    const visibility = ref<Visibility | null>(null)
    const authorIds = ref<number[]>([])
    const isWiki = ref(false)
    const currentUserIsCreator = ref(false)
    const canBeDeleted = ref(false)
    const authors = ref<Author[]>([])
    const searchPageItem = ref<PageItem | null>(null)
    const knowledgeSummary = ref<KnowledgeSummary>({} as KnowledgeSummary)
    const knowledgeSummarySlim = ref<KnowledgeSummarySlim>({} as KnowledgeSummarySlim)
    const gridItems = ref<GridPageItem[]>([])
    const isChildOfPersonalWiki = ref(false)
    const textIsHidden = ref(false)
    const uploadTrackingArray = ref<string[]>([])
    const viewsPast90DaysAggregatedPages = ref<ViewSummary[]>([])
    const viewsPast90DaysPage = ref<ViewSummary[]>([])
    const viewsPast90DaysAggregatedQuestions = ref<ViewSummary[]>([])
    const viewsPast90DaysDirectQuestions = ref<ViewSummary[]>([])
    const analyticsLoaded = ref(false)
    const saveTrackingArray = ref<string[]>([])
    const currentWiki = ref<TinyPageModel | null>(null)
    const text = ref('')
    const selectedText = ref('')
    const contentLanguage = ref<'en' | 'de' | 'fr' | 'es'>('en')
    const canEdit = ref(false)
    const shareToken = ref<string | null>(null)
    const isShared = ref(false)
    const sharedWith = ref<SharedWithUser[]>([])
    const canEditByToken = ref<boolean | null>(null)
    const totalQuestionViews = ref(0)
    const directQuestionViews = ref(0)

    // Actions
    const setPage = (page: Page) => {
        shareToken.value = null

        if (page != null) {
            id.value = page.id
            name.value = page.name
            initialName.value = page.name
            imgUrl.value = page.imageUrl
            imgId.value = page.imageId
            content.value = page.content
            initialContent.value = page.content

            parentPageCount.value = page.parentPageCount
            parents.value = page.parents
            childPageCount.value = page.childPageCount
            directVisibleChildPageCount.value = page.directVisibleChildPageCount

            views.value = page.views
            subpageViews.value = page.subpageViews
            commentCount.value = page.commentCount
            visibility.value = page.visibility

            authorIds.value = page.authorIds
            isWiki.value = page.isWiki
            currentUserIsCreator.value = page.currentUserIsCreator
            canBeDeleted.value = page.canBeDeleted

            questionCount.value = page.questionCount
            directQuestionCount.value = page.directQuestionCount

            authors.value = page.authors
            searchPageItem.value = page.pageItem

            knowledgeSummary.value = page.knowledgeSummary
            setKnowledgeSummarySlim(page.knowledgeSummary)
            gridItems.value = page.gridItems
            isChildOfPersonalWiki.value = page.isChildOfPersonalWiki
            textIsHidden.value = page.textIsHidden

            analyticsLoaded.value = false
            viewsPast90DaysAggregatedPages.value = []
            viewsPast90DaysPage.value = []
            viewsPast90DaysAggregatedQuestions.value = []
            viewsPast90DaysDirectQuestions.value = []
            text.value = ''
            selectedText.value = ''

            contentLanguage.value = page.language
            canEdit.value = page.canEdit
            isShared.value = page.isShared
            sharedWith.value = page.sharedWith || []
            canEditByToken.value = page.canEditByToken

            totalQuestionViews.value = page.totalQuestionViews
            directQuestionViews.value = page.directQuestionViews

            handleLoginReminder()
        }
    }

    const setKnowledgeSummarySlim = (knowledgeSummaryData: KnowledgeSummary) => {
        knowledgeSummarySlim.value = {
            solid: knowledgeSummaryData.solid,
            needsConsolidation: knowledgeSummaryData.needsConsolidation,
            needsLearning: knowledgeSummaryData.needsLearning,
            notLearned: knowledgeSummaryData.notLearned,
        }
    }

    const saveContent = async () => {
        const userStore = useUserStore()
        const snackbarStore = useSnackbarStore()

        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return
        }

        if (contentHasChanged.value == false) return
        await waitUntilAllUploadsComplete()
        await waitUntilAllSavingsComplete()

        const uploadId = nanoid(5)
        saveTrackingArray.value.push(uploadId)

        const data = {
            id: id.value,
            content: content.value,
            shareToken: shareToken.value,
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
            visibility.value != Visibility.Private
        ) {
            const data: SnackbarData = {
                type: 'success',
                text: { message: $i18n.t('success.page.saved') },
            }
            snackbarStore.showSnackbar(data)
            initialContent.value = content.value
            contentHasChanged.value = false
        } else if (result.success === false && result.messageKey != null) {
            if (
                !(
                    result.messageKey === 'error.page.noChange' &&
                    visibility.value == Visibility.Private
                )
            ) {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, {
                    text: $i18n.t(result.messageKey),
                })
            }
        }

        saveTrackingArray.value = saveTrackingArray.value.filter(
            (filterId) => filterId !== uploadId
        )
    }

    const saveName = async () => {
        const userStore = useUserStore()

        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return
        }
        if (name.value === initialName.value) return

        await waitUntilAllUploadsComplete()
        await waitUntilAllSavingsComplete()

        const uploadId = nanoid(5)
        saveTrackingArray.value.push(uploadId)

        const data = {
            id: id.value,
            name: name.value,
            shareToken: shareToken.value,
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

        if (result.success && visibility.value != Visibility.Private) {
            const data: SnackbarData = {
                type: 'success',
                text: { message: $i18n.t('success.page.saved') },
            }
            const snackbarStore = useSnackbarStore()

            snackbarStore.showSnackbar(data)
            initialName.value = name.value
            nameHasChanged.value = false
        } else if (result.success === false && result.messageKey != null) {
            if (
                !(
                    result.messageKey === 'error.page.noChange' &&
                    visibility.value === Visibility.Private
                )
            ) {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, {
                    text: $i18n.t(result.messageKey),
                })
            }
        }

        saveTrackingArray.value = saveTrackingArray.value.filter(
            (filterId) => filterId !== uploadId
        )
    }

    const waitUntilAllSavingsComplete = async () => {
        while (saveTrackingArray.value.length > 0) {
            await new Promise((resolve) => setTimeout(resolve, 100))
        }
    }

    const reset = () => {
        name.value = initialName.value
        nameHasChanged.value = false
        content.value = initialContent.value
        contentHasChanged.value = false
    }

    const clearPage = () => {
        setPage(new Page())
    }

    const isOwnerOrAdmin = () => {
        const userStore = useUserStore()
        return userStore.isAdmin || currentUserIsCreator.value
    }

    const refreshPageImage = async () => {
        imgUrl.value = await $api<string>(
            `/apiVue/PageStore/GetPageImageUrl/${id.value}`,
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
    }

    const reloadKnowledgeSummary = async () => {
        knowledgeSummarySlim.value = await $api<KnowledgeSummarySlim>(
            `/apiVue/PageStore/GetUpdatedKnowledgeSummary/${id.value}`,
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
    }

    const reloadGridItems = async () => {
        const result = await $api<GridPageItem[]>(
            `/apiVue/PageStore/GetGridPageItems/${id.value}`,
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

        if (result) gridItems.value = result
    }

    const hideOrShowText = async () => {
        if (
            (!!content.value && content.value.length > 0) ||
            contentHasChanged.value
        )
            return

        const data = {
            hideText: !textIsHidden.value,
            pageId: id.value,
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

        textIsHidden.value = result
    }

    const uploadContentImage = async (file: File): Promise<string> => {
        const uploadId = nanoid(5)
        uploadTrackingArray.value.push(uploadId)

        const data = new FormData()
        data.append('file', file)
        data.append('pageId', id.value.toString())

        const result = await $api<string>(
            '/apiVue/PageStore/UploadContentImage',
            {
                body: data,
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            }
        )

        uploadTrackingArray.value = uploadTrackingArray.value.filter(
            (filterId) => filterId !== uploadId
        )

        return result
    }

    const waitUntilAllUploadsComplete = async () => {
        while (uploadTrackingArray.value.length > 0) {
            await new Promise((resolve) => setTimeout(resolve, 100))
        }
    }

    const getAnalyticsData = async () => {
        const data = await $api<GetPageAnalyticsResponse>(
            `/apiVue/PageStore/GetPageAnalytics/${id.value}`,
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
            viewsPast90DaysAggregatedPages.value =
                data.viewsPast90DaysAggregatedPages
            viewsPast90DaysPage.value = data.viewsPast90DaysPage
            viewsPast90DaysAggregatedQuestions.value =
                data.viewsPast90DaysAggregatedQuestions
            viewsPast90DaysDirectQuestions.value =
                data.viewsPast90DaysDirectQuestions

            analyticsLoaded.value = true
        }
    }

    const generateFlashcard = async (
        selectedTextParam?: string
    ): Promise<GenerateFlashcardResponse> => {
        const loadingStore = useLoadingStore()
        loadingStore.startLoading(9000, 'Karteikarten werden generiert')
        const data = {
            pageId: id.value,
            text:
                (selectedTextParam ?? '').length > 0 ? selectedTextParam : text.value,
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

        if (selectedTextParam != null && selectedTextParam.length > 0)
            selectedText.value = selectedTextParam

        await loadingStore.finishLoading()

        return result
    }

    const reGenerateFlashcard = async (): Promise<GenerateFlashcardResponse> => {
        return await generateFlashcard(selectedText.value)
    }

    const updateQuestionCount = async () => {
        const result = await $api<number>(
            `/apiVue/PageStore/GetQuestionCount/${id.value}`,
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

        questionCount.value = result
    }

    const setToken = (token: string | null) => {
        shareToken.value = token
        const userStore = useUserStore()
        if (token !== null) userStore.addShareToken(id.value, token)
    }

    const updateIsShared = async () => {
        let url = `/apiVue/PageStore/GetIsShared/${id.value}`
        if (shareToken.value) {
            url += `?shareToken=${shareToken.value}`
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
        isShared.value = result
    }

    const handleLoginReminder = () => {
        const userStore = useUserStore()
        userStore.showLoginReminder = false

        if (isShared.value && canEditByToken.value) {
            if (!userStore.isLoggedIn)
                userStore.showLoginReminder = true
        }
    }

    // Getters
    const getPageName = computed(() => name.value)
    const hasVisibleDirectChildren = computed(() => directVisibleChildPageCount.value > 0)

    return {
        // State
        id,
        name,
        initialName,
        imgUrl,
        imgId,
        questionCount,
        directQuestionCount,
        content,
        initialContent,
        contentHasChanged,
        nameHasChanged,
        parentPageCount,
        parents,
        childPageCount,
        directVisibleChildPageCount,
        views,
        subpageViews,
        commentCount,
        visibility,
        authorIds,
        isWiki,
        currentUserIsCreator,
        canBeDeleted,
        authors,
        searchPageItem,
        knowledgeSummary,
        knowledgeSummarySlim,
        gridItems,
        isChildOfPersonalWiki,
        textIsHidden,
        uploadTrackingArray,
        viewsPast90DaysAggregatedPages,
        viewsPast90DaysPage,
        viewsPast90DaysAggregatedQuestions,
        viewsPast90DaysDirectQuestions,
        analyticsLoaded,
        saveTrackingArray,
        currentWiki,
        text,
        selectedText,
        contentLanguage,
        canEdit,
        shareToken,
        isShared,
        sharedWith,
        canEditByToken,
        totalQuestionViews,
        directQuestionViews,
        
        // Actions
        setPage,
        setKnowledgeSummarySlim,
        saveContent,
        saveName,
        waitUntilAllSavingsComplete,
        reset,
        clearPage,
        isOwnerOrAdmin,
        refreshPageImage,
        reloadKnowledgeSummary,
        reloadGridItems,
        hideOrShowText,
        uploadContentImage,
        waitUntilAllUploadsComplete,
        getAnalyticsData,
        generateFlashcard,
        reGenerateFlashcard,
        updateQuestionCount,
        setToken,
        updateIsShared,
        handleLoginReminder,
        
        // Getters
        getPageName,
        hasVisibleDirectChildren
    }
})
