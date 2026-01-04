<script lang="ts" setup>
import { useTabsStore, Tab } from '~/components/page/tabs/tabsStore'
import type { Page } from '~/components/page/pageStore';
import { usePageStore } from '~/components/page/pageStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { SiteType } from '~~/components/shared/siteEnum'
import { useUserStore, FontSize } from '~~/components/user/userStore'
import { Visibility } from '~/components/shared/visibilityEnum'
import { useConvertStore } from '~/components/page/convert/convertStore'
import { useLearningSessionConfigurationStore } from '~/components/page/learning/learningSessionConfigurationStore'

const { $logger, $urlHelper } = useNuxtApp()
const userStore = useUserStore()
const tabsStore = useTabsStore()
const pageStore = usePageStore()
const loadingStore = useLoadingStore()
const convertStore = useConvertStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()

interface Props {
    tab?: Tab,
    site: SiteType,
}

const props = defineProps<Props>()

const { t } = useI18n()

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie', 'user-agent']) as HeadersInit

const pageUrl = computed(() => {
    if (route.params.token != null) {
        return `/apiVue/Page/GetPage/${route.params.id}?shareToken=${route.params.token}`
    }
    return `/apiVue/Page/GetPage/${route.params.id}`
})

const { data: page, refresh } = await useFetch<Page>(pageUrl.value,
    {
        credentials: 'include',
        mode: 'cors',
        onRequest({ options }) {
            if (import.meta.server) {
                options.headers = new Headers(headers)
                options.baseURL = config.public.serverBase
            }
        },
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
            throw createError({ statusMessage: context.error?.message })
        },
        server: true,
        retry: 3
    })

if (page.value?.errorCode && page.value?.messageKey) {
    $logger.warn(`Page: ${page.value.messageKey} route ${route.fullPath}`)
    throw createError({ statusCode: page.value.errorCode, statusMessage: t(page.value.messageKey) })
}

const tabSwitched = ref(false)

const router = useRouter()

const setPage = () => {
    if (page.value != null) {

        if (page.value?.errorCode && page.value?.messageKey) {
            $logger.warn(`Page: ${page.value.messageKey} route ${route.fullPath}`)
            throw createError({ statusCode: page.value.errorCode, statusMessage: t(page.value.messageKey) })
        } else {
            pageStore.setPage(page.value)
            if (route.params?.token != null) {
                pageStore.setToken(route.params.token.toString())
            }

            watch(() => pageStore.id, (val) => {
                if (val != 0)
                    loadingStore.startLoading()
            })

            useHead({
                title: page.value.name,
            })

            watch(() => tabsStore.activeTab, (t) => {
                tabSwitched.value = true
                if (page.value == null || parseInt(route.params.id.toString()) != page.value.id)
                    return

                if (t === Tab.Text)
                    router.push($urlHelper.getPageUrl(page.value.name, page.value.id))

                else if (t === Tab.Learning && route.params.questionId != null)
                    router.push($urlHelper.getPageUrlWithQuestionId(page.value.name, page.value.id, route.params.questionId.toString()))

                else if (t === Tab.Learning)
                    router.push($urlHelper.getPageUrl(page.value.name, page.value.id, Tab.Learning))

                else if (t === Tab.Analytics)
                    router.push($urlHelper.getPageUrl(page.value.name, page.value.id, Tab.Analytics))

                else if (t === Tab.Feed)
                    router.push($urlHelper.getPageUrl(page.value.name, page.value.id, Tab.Feed))
            })

            watch(() => pageStore.name, () => {
                useHead({
                    title: pageStore.name,
                })
            })
        }
    }
}

setPage()
const emit = defineEmits(['setPage'])

onBeforeMount(() => {
    emit('setPage', SiteType.Page)
})

const setTab = () => {
    if (tabsStore != null) {
        switch (props.tab) {
            case Tab.Learning:
                tabsStore.activeTab = Tab.Learning
                break
            case Tab.Feed:
                tabsStore.activeTab = Tab.Feed
                break
            case Tab.Analytics:
                tabsStore.activeTab = Tab.Analytics
                break
            default: tabsStore.activeTab = Tab.Text
        }
    }
}
onMounted(() => setTab())

// loginStateHasChanged and watch page is used to handle refreshNuxtData() on login/logouts
const loginStateHasChanged = ref<boolean>(false)
watch(() => userStore.isLoggedIn, () => loginStateHasChanged.value = true)
watch(page, async (oldPage, newPage) => {
    if (oldPage?.id === newPage?.id && loginStateHasChanged.value && import.meta.client) {
        await nextTick()
        setPage()
    }
    loginStateHasChanged.value = false
}, { deep: true, immediate: true })

useHead(() => ({
    htmlAttrs: {
        lang: page.value?.language ?? 'en'
    },
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}${$urlHelper.getPageUrl(page.value?.name ?? '', page.value?.id ?? 0)}`
        },
    ],
    meta: [
        {
            name: 'description',
            content: page.value?.metaDescription
        },
        {
            property: 'og:title',
            content: page.value?.name
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}${$urlHelper.getPageUrl(page.value?.name ?? '', page.value?.id ?? 0)}`
        },
        {
            property: 'og:type',
            content: 'article'
        },
        {
            property: 'og:image',
            content: page.value?.imageUrl
        }
    ]
}))

const { isMobile } = useDevice()
watch(() => props.tab, (t) => {
    if (t != null) {
        tabsStore.activeTab = t
    }

}, { immediate: true })

convertStore.$onAction(({ name, after }) => {
    if (name === 'confirmConversion') {
        after(async () => {
            await refresh()
            await setPage()
        })
    }
})

watch(() => pageStore.questionCount, (count) => {
    if (tabsStore.activeTab == Tab.Learning && count > 0)
        learningSessionConfigurationStore.showFilter = true
})

const learningTabLoaded = ref(false)
watch(() => tabsStore.activeTab, (tab) => {
    if (tab === Tab.Learning) {
        learningTabLoaded.value = true
    }
})

</script>

<template>
    <div v-if="page?.canAccess" class="page-container">
        <div class="page">
            <PageHeader />

            <template v-if="pageStore?.id != 0">

                <!-- <PageTabsContent /> -->
                <ClientOnly>
                    <PageTabsContent
                        v-show="tabsStore.activeTab === Tab.Text || (props.tab === Tab.Text && !tabSwitched)"
                        v-if="!pageStore.textIsHidden" />
                    <template #fallback>
                        <div v-if="!pageStore.textIsHidden"
                            v-show="tabsStore.activeTab === Tab.Text || (props.tab === Tab.Text && !tabSwitched)"
                            id="PageContent" class=""
                            :class="{ 'is-mobile': isMobile, 'no-grid-items': pageStore.gridItems.length === 0 }">
                            <div class=""
                                :class="{ 'private-page': pageStore.visibility === Visibility.Private, 'small-font': userStore.fontSize === FontSize.Small, 'large-font': userStore.fontSize === FontSize.Large }">
                                <div id="PageContentPlaceholder" class="ProseMirror content-placeholder"
                                    :class="{ 'is-mobile': isMobile }" v-html="pageStore.content" />
                            </div>
                        </div>
                    </template>
                </ClientOnly>
                <div id="EditBarAnchor" />

                <PageContentGrid v-show="tabsStore.activeTab === Tab.Text || (props.tab === Tab.Text && !tabSwitched)"
                    :children="pageStore.gridItems" />

                <ClientOnly>
                    <PageTabsQuestions
                        v-show="tabsStore.activeTab === Tab.Learning || (props.tab === Tab.Learning && !tabSwitched)" />
                    <template #fallback>
                        <div class="row" />
                    </template>
                    <LazyPageTabsFeed
                        v-show="tabsStore.activeTab === Tab.Feed || (props.tab === Tab.Feed && !tabSwitched)" />
                    <PageTabsAnalytics
                        v-show="tabsStore.activeTab === Tab.Analytics || (props.tab === Tab.Analytics && !tabSwitched)" />
                </ClientOnly>

                <ClientOnly>
                    <PageRelationEditModal />
                    <QuestionEditModal />
                    <QuestionEditDeleteModal />
                    <PagePublishModal />
                    <PageToPrivateModal />
                    <PageDeleteModal />
                    <PageLearningAiCreateFlashCard />
                    <PageContentAiCreatePage />
                    <PageSharingModal />
                    <LicenseLinkModal />
                </ClientOnly>
            </template>
        </div>

        <SidebarPage v-if="pageStore?.id != 0" :show-outline="true" />
    </div>
</template>


<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

// .page-content {
//     min-height: 400px;
//     height: 100%;
//     margin-top: 0;
//     width: 100%;

//     @media(min-width: 992px) {
//         display: flex;
//     }
// }

:deep(.column) {
    width: 33%;
    float: left;
}

.MarkdownContent .row {
    @media (min-width: @extra-breakpoint-cards) {
        margin-top: 20px;
        margin-bottom: 25px;
    }
}

#PageContent {
    line-height: 1.5;
    .new-font-style();
    color: @global-text-color;

    h1,
    h2,
    h3,
    h4,
    h5 {
        //font-family: Open Sans;
        line-height: 1.2;
    }

    h1 {
        font-size: @font-size-h1-mem;
        font-style: normal;
        font-weight: 400;
        margin-top: 5px;

        .media-below-sm({
            font-size: max(6vw, 24px);
        });
}

h2 {
    font-size: @font-size-h2-mem;

    .media-below-sm({
        font-size: @font-size-h3-mem;
    });
}

h3 {
    font-size: @font-size-h3-mem;

    .media-below-sm({
        font-size: @font-size-h4-mem;
    });
}

h4 {
    font-size: @font-size-h3-mem;

    .media-below-sm({
        font-size: @font-size-h4-mem;
    });
}
}

#MasterMainWrapper {
    background-color: white;
    padding: 20px;
    width: 100%;
    padding-top: 0;
    margin-top: -10px;

    .Box {
        padding: 0;
        border: none;
        box-shadow: none;
    }
}

@media (min-width: @screen-sm) {
    #MasterMainWrapper {
        padding-left: 0;
        padding-right: 0;
    }
}

#AboveMainHeading {
    margin-top: -5px;

    @media (min-width: 991px) {
        margin-top: -21px;
    }
}

/*------------------------------------------------------
    PageTab
-------------------------------------------------------*/

#PageTabContent {
    margin-top: 40px;
    box-sizing: content-box;
}

[v-cloak] {
    display: none;
}


#PageContent {
    padding-top: 36px;
    max-width: calc(100vw - 15px);

    @media (min-width:1301px) {
        max-width: 100%;
        width: 100%;
    }

    &.is-mobile {
        max-width: 100vw;

        .small-font {
            p {
                font-size: 12px;
            }
        }
    }
}

.private-page {
    margin-bottom: -30px;
}

.page-container {
    display: flex;
    // justify-content: center;
    // align-items: center;
    flex-wrap: nowrap;
    gap: 0 1rem;
    width: 100%;

    .page {
        max-width: 1200px;
        width: calc(75% - 1rem);
        flex-grow: 2;
    }

    @media (max-width: 900px) {
        .page {
            // width: 100%;
        }
    }
}

.sidesheet-open {
    .page-container {
        @media (max-width: 1209px) {

            .page {
                width: 100%;
            }
        }
    }
}

.sidebar {
    flex: 0 0 25%;
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.caption-controller {
    position: absolute;
    top: -18px;
    left: calc(50% + 100px);
    transform: translateX(-50%);
    background: white;
    border: hidden;
    font-size: 14px;
    width: 36px;
    height: 36px;
    margin: 0px;
    color: @memo-grey-darker;
    text-align: center;
    padding: 0px 21px;
    display: flex;
    justify-content: center;
    align-items: center;
    transition: filter 0.1s;
    border-radius: 4px;
    z-index: 999;

    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.16);

    &:hover {
        filter: brightness(0.85);
    }

    &:active {
        filter: brightness(0.7);
    }
}

#PageContentPlaceholder {
    padding-bottom: 20px;
}
</style>
