<script setup lang="ts">
import { useTopicStore } from '../../topicStore'
import { Tab, useTabsStore } from '../tabsStore'
import { Author, FeedItem, FeedItemGroupByAuthor } from './feedHelper'

const topicStore = useTopicStore()
const tabsStore = useTabsStore()

const feedItems = ref<FeedItem[]>()
const currentPage = ref(1)
const itemCount = ref(0)

watch(() => currentPage.value, async () => {
    getFeedItems()
})

interface FeedItemGroupByDay {
    dateLabel: string
    feedItemsByAuthor: FeedItemGroupByAuthor[]
}

const groupedFeedItemsByDay = computed(() => {
    if (!groupedFeedItemsByAuthor.value) return []

    const groupedFeedItems: FeedItemGroupByDay[] = []

    groupedFeedItemsByAuthor.value.forEach((group: FeedItemGroupByAuthor) => {
        let currentGroup = groupedFeedItems.find(g => g.dateLabel === group.dateLabel)

        if (!currentGroup) {
            currentGroup = { dateLabel: group.dateLabel, feedItemsByAuthor: [] }
            groupedFeedItems.push(currentGroup)
        }

        currentGroup.feedItemsByAuthor.push(group)
    })

    return groupedFeedItems
})

const groupedFeedItemsByAuthor = computed(() => {
    if (!feedItems.value) return []

    const groupedFeedItems: FeedItemGroupByAuthor[] = []
    let currentGroup: FeedItemGroupByAuthor = { author: { id: -2 } as Author, feedItems: [], dateLabel: '' }

    feedItems.value.forEach((feedItem: FeedItem) => {
        if (currentGroup.author.id !== feedItem.author.id || currentGroup.dateLabel !== getDateLabel(feedItem.date)) {
            currentGroup = { author: feedItem.author, feedItems: [], dateLabel: getDateLabel(feedItem.date) }
            groupedFeedItems.push(currentGroup)
        }

        currentGroup.feedItems.push(feedItem)
    })

    return groupedFeedItems
})

function getDateLabel(dateString: string) {
    const date = new Date(dateString)
    if (date.toDateString() === new Date().toDateString()) {
        return 'Heute'
    }

    if (date.toDateString() === new Date(new Date().setDate(new Date().getDate() - 1)).toDateString()) {
        return 'Gestern'
    }

    const options = { year: 'numeric', month: 'long', day: 'numeric' } as Intl.DateTimeFormatOptions
    return date.toLocaleDateString('de-DE', options)
}

const getFeedItems = async () => {
    const data = {
        topicId: topicStore.id,
        page: currentPage.value,
        pageSize: 100,
        getDescendants: true,
        getQuestions: true
    }

    interface GetFeedResponse {
        feedItems: FeedItem[]
        maxCount: number
    }

    const result = await $api<GetFeedResponse>(`/apiVue/Feed/Get/`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        body: data,
        onResponseError(context) {
            const { $logger } = useNuxtApp()
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
        }
    })

    feedItems.value = result.feedItems
    itemCount.value = result.maxCount
}

const getDescendants = ref(true)
const getQuestions = ref(true)

watch(() => tabsStore.activeTab, (tab) => {
    if (tab === Tab.Feed && import.meta.client) {
        getFeedItems()
    }
})

onMounted(() => {
    if (tabsStore.activeTab === Tab.Feed) {
        getFeedItems()
    }
})

const showModal = ref(false)

</script>

<template>
    <div class="row">

        <div class="col-xs-12">
            <TopicTabsFeedUserCard v-for="feedItemsByAuthor in groupedFeedItemsByAuthor" :authorGroup="feedItemsByAuthor" @open-feed-modal="showModal = true" class="feed-item" />
        </div>

        <div class="col-xs-12" v-if="itemCount > 0">
            <div class="pager pagination">
                <vue-awesome-paginate :total-items="itemCount" :items-per-page="100" :max-pages-shown="3" v-model="currentPage" :show-ending-buttons="true" :show-breakpoint-buttons="false">
                    <template #first-page-button>
                        <font-awesome-layers>
                            <font-awesome-icon :icon="['fas', 'chevron-left']" transform="left-3" />
                            <font-awesome-icon :icon="['fas', 'chevron-left']" transform="right-3" />
                        </font-awesome-layers>
                    </template>
                    <template #prev-button>
                        <font-awesome-icon :icon="['fas', 'chevron-left']" />
                    </template>
                    <template #next-button>
                        <font-awesome-icon :icon="['fas', 'chevron-right']" />
                    </template>
                    <template #last-page-button>
                        <font-awesome-layers>
                            <font-awesome-icon :icon="['fas', 'chevron-right']" transform="left-3" />
                            <font-awesome-icon :icon="['fas', 'chevron-right']" transform="right-3" />
                        </font-awesome-layers>
                    </template>
                </vue-awesome-paginate>
            </div>
        </div>

        <TopicTabsFeedModal :show="showModal" @close="showModal = false" />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-item {

    margin: 8px;
    padding: 8px;
    width: 100%;
}
</style>