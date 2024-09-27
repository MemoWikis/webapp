<script setup lang="ts">
import { FeedItem, useTopicStore, FeedItemType } from '../topicStore'

const topicStore = useTopicStore()

const feedItems = ref()
const getFeedItems = async () => {
    const result = await topicStore.getFeed()
    feedItems.value = result.feedItems
    itemCount.value = result.maxCount
}

const feedItemsWithDescendants = ref()
const getFeedItemsWithDescendants = async () => {
    const result = await topicStore.getFeedWithDescendants()
    feedItemsWithDescendants.value = result.feedItems
    itemCount.value = result.maxCount
}

const currentPage = ref(1)
const itemCount = ref(0)
</script>

<template>
    <div class="row">
        <div class="col-xs-12">
            <div class="memo-button btn-default" @click="getFeedItems()">GetFeed</div>
            <div class="memo-button btn-default" @click="getFeedItemsWithDescendants()">getFeedWithDescendants</div>
        </div>

        <div class="col-xs-12">
            <h3>FeedItems</h3>
            <div class="feed-item" v-for="feedItem in feedItems">
                {{ feedItem.date }} - {{ FeedItemType[feedItem.type] }} - {{ feedItem.categoryChangeId }} - {{ feedItem.topicId }} - {{ feedItem.visibility }}
            </div>

            <h3>FeedItemsWithDescendants</h3>
            <div class="feed-item" v-for="feedItem in feedItemsWithDescendants">
                {{ feedItem.date }} - {{ FeedItemType[feedItem.type] }} - {{ feedItem.categoryChangeId }} - {{ feedItem.topicId }} - {{ feedItem.visibility }}
            </div>
        </div>

        <div class="col-xs-12">
            <div class="pager">
                <vue-awesome-paginate :total-items="itemCount" :items-per-page="100" :max-pages-shown="3" v-model="currentPage" :show-ending-buttons="false" :show-breakpoint-buttons="false" />
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