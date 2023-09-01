<script lang="ts" setup>
import { useTabsStore, Tab } from '~~/components/topic/tabs/tabsStore'
import { Topic, useTopicStore } from '~~/components/topic/topicStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { Page } from '~~/components/shared/pageEnum'
import { useUserStore } from '~~/components/user/userStore'
import { useRootTopicChipStore } from '~/components/header/rootTopicChipStore'

const { $logger, $urlHelper } = useNuxtApp()

const tabsStore = useTabsStore()
const userStore = useUserStore()
const topicStore = useTopicStore()
const rootTopicChipStore = useRootTopicChipStore()

interface Props {
    tab?: Tab,
    documentation: Topic
}
const props = defineProps<Props>()
const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: topic, refresh } = await useFetch<Topic>(`/apiVue/Topic/GetTopic/${route.params.id}`,
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
        },
        server: true,
        retry: 3
    })

const tabSwitched = ref(false)

const router = useRouter()
function setTopic() {
    if (topic.value != null) {
        if (topic.value?.CanAccess) {

            topicStore.setTopic(topic.value)

            const spinnerStore = useSpinnerStore()
            watch(() => topicStore.id, (val) => {
                if (val != 0)
                    spinnerStore.showSpinner()
            })

            useHead({
                title: topic.value.Name,
            })
            watch(() => tabsStore.activeTab, (t) => {
                tabSwitched.value = true
                if (topic.value == null)
                    return
                if (t == Tab.Topic)
                    router.push($urlHelper.getTopicUrl(topic.value.Name, topic.value.Id))

                else if (t == Tab.Learning && route.params.questionId != null)
                    router.push($urlHelper.getTopicUrlWithQuestionId(topic.value.Name, topic.value.Id, route.params.questionId.toString()))

                else if (t == Tab.Learning)
                    router.push($urlHelper.getTopicUrl(topic.value.Name, topic.value.Id, Tab.Learning))

                else if (t == Tab.Analytics)
                    router.push($urlHelper.getTopicUrl(topic.value.Name, topic.value.Id, Tab.Analytics))

            })

            watch(() => topicStore.name, () => {
                useHead({
                    title: topicStore.name,
                })
            })
        } else {
            throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
        }
    }
}
setTopic()
const emit = defineEmits(['setPage'])
emit('setPage', Page.Topic)

function setTab() {
    if (tabsStore != null) {
        switch (props.tab) {
            case Tab.Learning:
                tabsStore.activeTab = Tab.Learning
                break;
            case Tab.Feed:
                tabsStore.activeTab = Tab.Feed
                break;
            case Tab.Analytics:
                tabsStore.activeTab = Tab.Analytics
                break;
            default: tabsStore.activeTab = Tab.Topic
        }
    }
}

onMounted(() => setTab())
watch(() => userStore.isLoggedIn, async (isLoggedIn) => {
    if (isLoggedIn && topic.value?.Id == rootTopicChipStore.id && userStore.personalWiki && userStore.personalWiki.Id != rootTopicChipStore.id)
        await navigateTo($urlHelper.getTopicUrl(userStore.personalWiki.Name, userStore.personalWiki.Id))
})

useHead(() => ({
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}${$urlHelper.getTopicUrl(topic.value?.Name!, topic.value?.Id!)}`
        },
    ],
    meta: [
        {
            name: 'description',
            content: topic.value?.MetaDescription
        },
        {
            property: 'og:title',
            content: topic.value?.Name
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}${$urlHelper.getTopicUrl(topic.value?.Name!, topic.value?.Id!)}`
        },
        {
            property: 'og:type',
            content: 'article'
        },
        {
            property: 'og:image',
            content: topic.value?.ImageUrl
        }
    ]
}))

</script>

<template>
    <div class="container">
        <div class="row topic-container main-page">
            <template v-if="topic?.CanAccess">
                <div class="col-lg-9 col-md-12 container">
                    <template v-if="topicStore.id != 0">
                        <TopicHeader />

                        <TopicTabsContent
                            v-show="tabsStore.activeTab == Tab.Topic || (props.tab == Tab.Topic && !tabSwitched)" />
                        <ClientOnly>
                            <TopicContentGrid
                                v-show="tabsStore.activeTab == Tab.Topic || (props.tab == Tab.Topic && !tabSwitched)"
                                :children="topicStore.gridItems" />
                            <template #fallback>
                                <TopicContentGrid
                                    v-show="tabsStore.activeTab == Tab.Topic || (props.tab == Tab.Topic && !tabSwitched)"
                                    :children="topic.gridItems" />
                            </template>
                        </ClientOnly>

                        <TopicTabsQuestions
                            v-show="tabsStore.activeTab == Tab.Learning || (props.tab == Tab.Learning && !tabSwitched)" />
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

    // ul,
    // pre {
    //     margin-bottom: 20px;
    // }
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
