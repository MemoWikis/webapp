<script setup lang="ts">
import { FeedItem, useTopicStore, FeedItemType } from '../topicStore'

const topicStore = useTopicStore()
const getFeedItems = async () => {
    const result = await topicStore.getFeed()
    feedItems.value = result
}
const feedItems = ref()

const getFeedItemsWithDescendants = async () => {
    const result = await topicStore.getFeedWithDescendants()
    feedItemsWithDescendants.value = result
}
const feedItemsWithDescendants = ref()

const last30FeedItems = computed(() => {
    return feedItems.value?.slice(0, 30) as FeedItem[]
})

const last30FeedItemsWithDescendants = computed(() => {
    return feedItemsWithDescendants.value?.slice(0, 150) as FeedItem[]
})
</script>

<template>
    <div class="row">
        <div class="col-xs-12">
            <div class="memo-button btn-default" @click="getFeedItems()">GetFeed</div>
            <div class="memo-button btn-default" @click="getFeedItemsWithDescendants()">getFeedWithDescendants</div>
        </div>

        <div class="col-xs-12">
            <h3>FeedItems</h3>
            <div class="feed-item" v-for="feedItem in last30FeedItems">
                {{ feedItem.date }} - {{ FeedItemType[feedItem.type] }} - {{ feedItem.categoryChangeId }} - {{ feedItem.topicId }} - {{ feedItem.visibility }}
            </div>

            <h3>FeedItemsWithDescendants</h3>
            <div class="feed-item" v-for="feedItem in last30FeedItemsWithDescendants">
                {{ feedItem.date }} - {{ FeedItemType[feedItem.type] }} - {{ feedItem.categoryChangeId }} - {{ feedItem.topicId }} - {{ feedItem.visibility }}
            </div>
        </div>

        <div class="col-xs-12">
            <div class="pager">
                <div></div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.col-xs-12 {
    width: 100%;
}

.feed-item {

    margin: 8px;
    border: solid 1px @memo-grey-lighter;
    padding: 8px;

}
</style>