<script lang="ts" setup>
import { useTabsStore, Tab } from '~/components/page/tabs/tabsStore'
import { Page, usePageStore } from '~/components/page/pageStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { SiteType } from '~~/components/shared/siteEnum'
import { useUserStore, FontSize } from '~~/components/user/userStore'
import { Visibility } from '~/components/shared/visibilityEnum'
import { useConvertStore } from '~/components/page/convert/convertStore'

const { $logger, $urlHelper } = useNuxtApp()
const userStore = useUserStore()
const tabsStore = useTabsStore()
const pageStore = usePageStore()
const loadingStore = useLoadingStore()
const convertStore = useConvertStore()

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
                options.headers = headers
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

function setPage() {
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

function setTab() {
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
            href: `${config.public.officialBase}${$urlHelper.getPageUrl(page.value?.name!, page.value?.id!)}`
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
            content: `${config.public.officialBase}${$urlHelper.getPageUrl(page.value?.name!, page.value?.id!)}`
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

</script>

<template>
    <div class="container page-container">
        <div class="row page-content main-page">
            <template v-if="page?.canAccess">
                <div class="col-lg-9 col-md-12 container page">
                    <PageHeader />

                    <template v-if="pageStore?.id != 0">
                        <ClientOnly>
                            <PageTabsContent
                                v-show="tabsStore.activeTab === Tab.Text || (props.tab === Tab.Text && !tabSwitched)"
                                :text-is-hidden="pageStore.textIsHidden" />
                            <template #fallback>
                                <div id="PageContent" class="row" :class="{ 'is-mobile': isMobile, 'no-grid-items': pageStore.gridItems.length === 0 }"
                                    v-if="!pageStore.textIsHidden"
                                    v-show="tabsStore.activeTab === Tab.Text || (props.tab === Tab.Text && !tabSwitched)">
                                    <div class="col-xs-12" :class="{ 'private-page': pageStore.visibility === Visibility.Private, 'small-font': userStore.fontSize === FontSize.Small, 'large-font': userStore.fontSize === FontSize.Large }">
                                        <div class="ProseMirror content-placeholder" v-html="pageStore.content"
                                            id="PageContentPlaceholder" :class="{ 'is-mobile': isMobile }">
                                        </div>
                                    </div>
                                </div>
                            </template>
                        </ClientOnly>
                        <div id="EditBarAnchor"></div>

                        <PageContentGrid v-show="tabsStore.activeTab === Tab.Text || (props.tab === Tab.Text && !tabSwitched)" :children="pageStore.gridItems" />

                        <ClientOnly>
                            <PageTabsQuestions v-show="tabsStore.activeTab === Tab.Learning || (props.tab === Tab.Learning && !tabSwitched)" />
                            <template #fallback>
                                <div class="row">
                                </div>
                            </template>
                            <LazyPageTabsFeed v-show="tabsStore.activeTab === Tab.Feed || (props.tab === Tab.Feed && !tabSwitched)" />
                            <PageTabsAnalytics v-show="tabsStore.activeTab === Tab.Analytics || (props.tab === Tab.Analytics && !tabSwitched)" />
                        </ClientOnly>

                        <ClientOnly>
                            <PageRelationEdit />
                            <QuestionEditModal />
                            <QuestionEditDelete />
                            <PagePublishModal />
                            <PageToPrivateModal />
                            <PageDeleteModal />
                            <PageLearningAiCreateFlashCard />
                            <PageSharingShareModal />
                        </ClientOnly>
                    </template>
                </div>

                <ClientOnly>
                    <Sidebar class="is-page" :show-outline="true" :site="SiteType.Page" v-if="pageStore?.id != 0" />

                    <template #fallback>
                        <SidebarFallback class="is-page" />
                    </template>
                </ClientOnly>
            </template>
        </div>
    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

#InlineEdit {
    padding: 0px;
    border: none;
}

#PageContentPlaceholder {
    padding: 0px;
    margin-bottom: 70px;

    p {
        min-height: calc(5em / 3);

        .media-below-sm({
            min-height: 1.5em;
        });

    img {

        // Apply styles to p if it contains img
        & {
            margin-bottom: 40px !important;
        }
    }

    .tiptapImgMixin(false)
}


&.is-mobile {
    p {
        min-height: 21px;
    }
}

ul {
    margin-bottom: 10px;

    ul {
        margin-bottom: 0px;
    }
}

pre {
    margin-bottom: 20px;
}
}

.small-font {
    p {
        font-size: 16px;
    }

    .media-below-sm({
        font-size: 12px;
    });
}

.large-font {
    h2 {
        font-size: 2.6rem;
    }

    h3 {
        font-size: 2.3rem;
    }

    h4 {
        font-size: 2.1rem;
    }

    p {
        font-size: 20px;
    }

    .media-below-sm({
        font-size: 16px;
    });
}

#PageContent {
    &.no-grid-items {
        min-height: 50vh;
    }
}
</style>


<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.page-content {
    min-height: 400px;
    height: 100%;
    margin-top: 0;

    @media(min-width: 992px) {
        display: flex;
    }
}

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

@media (min-width: 768px) {
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
    max-width: calc(100vw - 20px);

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
</style>
