<script setup lang="ts">
import { useUserStore } from '~/components/user/userStore'
import { usePageStore } from '../../pageStore'
import { Tab, useTabsStore } from '../tabsStore'
import { Author, FeedItem, FeedItemGroupByAuthor, FeedType } from './feedHelper'

const { t, localeProperties } = useI18n()
const pageStore = usePageStore()
const tabsStore = useTabsStore()
const userStore = useUserStore()

const feedItems = ref<FeedItem[]>()
const currentPage = ref(1)
const itemCount = ref(0)

watch(() => currentPage.value, async () => {
    getFeedItems()
})

const groupedFeedItemsByAuthor = computed(() => {
    if (!feedItems.value) return []

    const groupedFeedItems: FeedItemGroupByAuthor[] = []
    let currentGroup: FeedItemGroupByAuthor = { author: { id: -2 } as Author, feedItems: [], dateLabel: '' }

    feedItems.value.forEach((feedItem: FeedItem, index: number) => {
        if (currentGroup.author.id !== feedItem.author.id || currentGroup.dateLabel !== getDateLabel(feedItem.date)) {
            currentGroup = { author: feedItem.author, feedItems: [], dateLabel: getDateLabel(feedItem.date) }
            groupedFeedItems.push(currentGroup)
        }
        feedItem.index = index
        currentGroup.feedItems.push(feedItem)
    })

    return groupedFeedItems
})

function getDateLabel(dateString: string) {
    const date = new Date(dateString)
    if (date.toDateString() === new Date().toDateString()) {
        return t('page.feed.today')
    }

    if (date.toDateString() === new Date(new Date().setDate(new Date().getDate() - 1)).toDateString()) {
        return t('page.feed.yesterday')
    }

    const options = { year: 'numeric', month: 'long', day: 'numeric' } as Intl.DateTimeFormatOptions
    const iso = localeProperties.value.iso
    return date.toLocaleDateString(iso, options)
}

const getDescendants = ref(true)
const getQuestions = ref(true)
const getGroups = ref(true)

const getFeedItems = async () => {

    const data = {
        pageId: pageStore.id,
        page: currentPage.value,
        pageSize: 100,
        getDescendants: getDescendants.value,
        getQuestions: getQuestions.value,
        getItemsInGroups: !getGroups.value
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
    feedItems.value = []

    feedItems.value = result.feedItems
    itemCount.value = result.maxCount
}

watch(getDescendants, () => {
    getFeedItems()
})
watch(getGroups, () => {
    getFeedItems()
})
watch(getQuestions, () => {
    getFeedItems()
})

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

const selectedFeedItem = ref<FeedItem>()
const openModal = (e: { type: FeedType, id: number, index: number }) => {
    showModal.value = true
    if (feedItems.value) {
        const feedItem = feedItems.value[e.index]

        selectedFeedItem.value = feedItem
    }
}
const ariaId = useId()
</script>

<template>
    <div class="row">

        <div class="col-xs-12 feed">
            <div class="header">
                <VDropdown :aria-id="ariaId" :distance="0" :popperHideTriggers="(triggers: any) => []" :arrow-padding="300" placement="auto">
                    <div class="feed-settings-btn">
                        <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                    </div>
                    <template #popper>
                        <div class="checkbox-container dropdown-row" @click="getDescendants = !getDescendants">
                            <label>{{ t('page.feed.includeSubpages') }}</label>
                            <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="getDescendants" class="active" />

                            <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="not-active" />
                        </div>

                        <template v-if="userStore.isAdmin">
                            <div class="checkbox-container dropdown-row" @click="getGroups = !getGroups">
                                <label>{{ t('page.feed.groupItems') }}</label>
                                <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="getGroups" class="active" />

                                <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="not-active" />
                            </div>

                            <div class="checkbox-container dropdown-row" @click="getQuestions = !getQuestions">
                                <label>{{ t('page.feed.includeQuestions') }}</label>
                                <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="getQuestions" class="active" />

                                <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="not-active" />
                            </div>
                        </template>

                    </template>
                </VDropdown>

            </div>

            <PageTabsFeedUserCard v-for="feedItemsByAuthor in groupedFeedItemsByAuthor" :authorGroup="feedItemsByAuthor" @open-feed-modal="openModal" class="feed-item" />


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

        <PageTabsFeedModal :show="showModal" @close="showModal = false" v-if="selectedFeedItem" :feed-item="selectedFeedItem" @get-feed-items="getFeedItems" />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.header {
    display: flex;
    justify-content: flex-end;
    align-items: center;
    z-index: 2;
    margin-bottom: -24px;
    margin-top: 8px;
    position: relative;
}

.feed-settings-btn {
    cursor: pointer;
    font-size: 18px;
    color: @memo-grey-dark;
    background: white;
    border-radius: 44px;
    width: 32px;
    height: 32px;
    display: flex;
    justify-content: center;
    align-items: center;
    user-select: none;
    z-index: 5;

    &:hover {
        filter: brightness(0.95);
    }

    &:active {
        filter: brightness(0.9);
    }
}

.checkbox-container {
    padding: 2px 8px;
    user-select: none;
    display: flex;
    align-items: center;
    justify-content: space-between;
    cursor: pointer;
    margin-top: 4px;
    background: white;
    z-index: 2;

    label {
        margin-bottom: 0;
        color: @memo-grey-dark;
        cursor: pointer;
        z-index: 2;

    }

    .active,
    .not-active {
        margin-left: 8px;
        font-size: 1.8em;
    }

    .active {
        color: @memo-blue-link;
    }

    .not-active {
        color: @memo-grey-light;
    }
}

.feed-item {
    margin: 8px;
    padding: 8px;
    width: 100%;
    max-width: 880px;
}

.feed {
    max-width: calc(100vw - 20px);
}
</style>