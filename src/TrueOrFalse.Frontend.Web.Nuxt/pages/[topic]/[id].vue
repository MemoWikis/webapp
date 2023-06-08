<script lang="ts" setup>
import { useTabsStore, Tab } from '~~/components/topic/tabs/tabsStore'
import { Topic, useTopicStore } from '~~/components/topic/topicStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { Page } from '~~/components/shared/pageEnum'
import { useUserStore } from '~~/components/user/userStore'

const { $logger } = useNuxtApp()

const tabsStore = useTabsStore()
const userStore = useUserStore()
const topicStore = useTopicStore()

interface Props {
    tab?: Tab,
    documentation: Topic
}
const props = defineProps<Props>()
const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

// GetTopic w/o Segments - GetTopicWithSegments with segments
const { data: topic, refresh } = await useFetch<Topic>(`/apiVue/Topic/GetTopicWithSegments/${route.params.id}`,
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

//preset segmentation
const segmentation = ref()
if (topic.value != null) {
    if (topic.value?.CanAccess) {

        topicStore.setTopic(topic.value)

        const spinnerStore = useSpinnerStore()
        //preset segmentation
        segmentation.value = topic.value.Segmentation
        watch(() => topicStore.id, (val) => {
            if (val != 0)
                spinnerStore.showSpinner()
        })

        useHead({
            title: topic.value.Name,
        })

        watch(() => tabsStore.activeTab, (t) => {
            preloadTopicTab.value = false
            if (topic.value == null)
                return
            if (t == Tab.Topic) {
                history.pushState(null, topic.value.Name, `/${topic.value.EncodedName}/${topic.value.Id}`)
            }
            else if (t == Tab.Learning && route.params.questionId != null)
                history.pushState(null, topic.value.Name, `/${topic.value.EncodedName}/${topic.value.Id}/Lernen/${route.params.questionId}`)
            else if (t == Tab.Learning)
                history.pushState(null, topic.value.Name, `/${topic.value.EncodedName}/${topic.value.Id}/Lernen`)
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

const preloadTopicTab = ref(true)

onBeforeMount(() => {
    if (props.tab != Tab.Topic)
        preloadTopicTab.value
})
onMounted(() => setTab())
watch(() => userStore.isLoggedIn, () => {
    refresh()
})

useHead(() => ({
    link: [
        {
            rel: 'canonical',
            href: `${config.public.serverBase}/${topic.value?.EncodedName}/${topic.value?.Id}`,
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
            content: `${config.public.serverBase}/${topic.value?.EncodedName}/${topic.value?.Id}`
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
                    <TopicHeader v-if="topicStore.id != 0" />
                    <TopicTabsContent v-if="topicStore.id != 0"
                        v-show="tabsStore.activeTab == Tab.Topic || (preloadTopicTab && props.tab == Tab.Topic)" />
                    <TopicContentSegmentation v-if="topicStore.id != 0" v-show="tabsStore.activeTab == Tab.Topic"
                        :segmentation="segmentation" />
                    <TopicTabsQuestions v-if="topicStore.id != 0" v-show="tabsStore.activeTab == Tab.Learning" />
                    <LazyTopicTabsAnalytics v-if="topicStore.id != 0" v-show="tabsStore.activeTab == Tab.Analytics" />
                    <TopicRelationEdit v-if="userStore.isLoggedIn" />
                    <QuestionEditModal v-if="userStore.isLoggedIn" />
                    <QuestionEditDelete v-if="userStore.isLoggedIn" />
                    <TopicPublishModal v-if="userStore.isLoggedIn" />
                    <TopicToPrivateModal v-if="userStore.isLoggedIn" />
                    <TopicDeleteModal
                        v-if="topic?.CanBeDeleted && (topic.CurrentUserIsCreator || userStore.isAdmin) && userStore.isLoggedIn" />
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
