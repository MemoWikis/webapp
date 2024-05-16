<script lang="ts" setup>
import { useTabsStore, Tab } from '~~/components/topic/tabs/tabsStore'
import { Topic, useTopicStore } from '~~/components/topic/topicStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { Page } from '~~/components/shared/pageEnum'
import { useUserStore } from '~~/components/user/userStore'

const { $logger, $urlHelper } = useNuxtApp()
const userStore = useUserStore()
const tabsStore = useTabsStore()
const topicStore = useTopicStore()
const spinnerStore = useSpinnerStore()

interface Props {
    tab?: Tab,
    documentation: Topic
}
const props = defineProps<Props>()
const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie', 'user-agent']) as HeadersInit

const { data: topic } = await useFetch<Topic>(`/apiVue/Topic/GetTopic/${route.params.id}`,
    {
        credentials: 'include',
        mode: 'cors',
        onRequest({ options }) {
            if (process.server) {
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

const tabSwitched = ref(false)

const router = useRouter()

function setTopic() {
    if (topic.value != null) {
        if (topic.value?.canAccess) {

            topicStore.setTopic(topic.value)

            watch(() => topicStore.id, (val) => {
                if (val != 0)
                    spinnerStore.showSpinner()
            })

            useHead({
                title: topic.value.name,
            })
            watch(() => tabsStore.activeTab, (t) => {
                tabSwitched.value = true
                if (topic.value == null)
                    return
                if (t == Tab.Topic)
                    router.push($urlHelper.getTopicUrl(topic.value.name, topic.value.id))

                else if (t == Tab.Learning && route.params.questionId != null)
                    router.push($urlHelper.getTopicUrlWithQuestionId(topic.value.name, topic.value.id, route.params.questionId.toString()))

                else if (t == Tab.Learning)
                    router.push($urlHelper.getTopicUrl(topic.value.name, topic.value.id, Tab.Learning))

                else if (t == Tab.Analytics)
                    router.push($urlHelper.getTopicUrl(topic.value.name, topic.value.id, Tab.Analytics))
            })

            watch(() => topicStore.name, () => {
                useHead({
                    title: topicStore.name,
                })
            })
        } else {
            $logger.error(`Topic: NoAccess - routeId: ${route.params.id}`)
            throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
        }
    }
}

onMounted(() => {
    watch(() => route, (val) => {
    }, { deep: true, immediate: true })
})
setTopic()

const emit = defineEmits(['setPage'])
emit('setPage', Page.Topic)

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
            default: tabsStore.activeTab = Tab.Topic
        }
    }
}
onMounted(() => setTab())

// loginStateHasChanged and watch topic is used to handle refreshNuxtData() on login/logouts
const loginStateHasChanged = ref<boolean>(false)
watch(() => userStore.isLoggedIn, () => loginStateHasChanged.value = true)
watch(topic, async (oldTopic, newTopic) => {
    if (oldTopic?.id == newTopic?.id && loginStateHasChanged.value && process.client) {
        await nextTick()
        setTopic()
    }
    loginStateHasChanged.value = false
}, { deep: true, immediate: true })

useHead(() => ({
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}${$urlHelper.getTopicUrl(topic.value?.name!, topic.value?.id!)}`
        },
    ],
    meta: [
        {
            name: 'description',
            content: topic.value?.metaDescription
        },
        {
            property: 'og:title',
            content: topic.value?.name
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}${$urlHelper.getTopicUrl(topic.value?.name!, topic.value?.id!)}`
        },
        {
            property: 'og:type',
            content: 'article'
        },
        {
            property: 'og:image',
            content: topic.value?.imageUrl
        }
    ]
}))

const { isMobile } = useDevice()
watch(() => props.tab, (t) => {
    if (t != null) {
        tabsStore.activeTab = t
    }

}, { immediate: true })
</script>

<template>
    <div class="container">
        <div class="row topic-container main-page">
            <template v-if="topic?.canAccess">
                <div class="col-lg-9 col-md-12 container">
                    <TopicHeader />

                    <template v-if="topicStore?.id != 0">
                        <ClientOnly>
                            <TopicTabsContent
                                v-show="tabsStore.activeTab == Tab.Topic || (props.tab == Tab.Topic && !tabSwitched)" />
                            <template #fallback>
                                <div id="TopicContent" class="row" :class="{ 'is-mobile': isMobile }"
                                    v-show="tabsStore.activeTab == Tab.Topic || (props.tab == Tab.Topic && !tabSwitched)">
                                    <div class="col-xs-12">
                                        <div class="ProseMirror content-placeholder" v-html="topicStore.content"
                                            id="TopicContentPlaceholder" :class="{ 'is-mobile': isMobile }">
                                        </div>
                                    </div>
                                </div>
                            </template>
                        </ClientOnly>
                        <div id="EditBarAnchor"></div>

                        <TopicContentGrid
                            v-show="tabsStore.activeTab == Tab.Topic || (props.tab == Tab.Topic && !tabSwitched)"
                            :children="topicStore.gridItems" />

                        <ClientOnly>
                            <TopicTabsQuestions
                                v-show="tabsStore.activeTab == Tab.Learning || (props.tab == Tab.Learning && !tabSwitched)" />
                            <template #fallback>
                                <div class="row">
                                </div>
                            </template>
                        </ClientOnly>
                        <TopicTabsAnalytics
                            v-show="tabsStore.activeTab == Tab.Analytics || (props.tab == Tab.Analytics && !tabSwitched)" />

                        <ClientOnly>
                            <TopicRelationEdit />
                            <QuestionEditModal />
                            <QuestionEditDelete />
                            <TopicPublishModal />
                            <TopicToPrivateModal />
                            <TopicDeleteModal />
                        </ClientOnly>
                    </template>
                </div>
                <Sidebar :documentation="props.documentation" class="is-topic" />
            </template>
        </div>
    </div>
</template>

<style lang="less">
#InlineEdit {
    padding: 0px;
    border: none;
}

#TopicContentPlaceholder {
    padding: 0px;
    margin-bottom: 70px;

    p {
        min-height: 30px;
    }

    &.is-mobile {
        p {
            min-height: 21px;
        }
    }

    ul,
    pre {
        margin-bottom: 20px;
    }
}
</style>


<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.topic-container {
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

#TopicContent {
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
    TopicTab
-------------------------------------------------------*/

#TopicTabContent {
    margin-top: 40px;
    box-sizing: content-box;
}

[v-cloak] {
    display: none;
}


#TopicContent {
    padding-top: 36px;
    max-width: calc(100vw - 20px);

    &.is-mobile {
        max-width: 100vw;
    }
}
</style>
