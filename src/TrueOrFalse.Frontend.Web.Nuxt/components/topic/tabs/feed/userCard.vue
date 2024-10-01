<script lang="ts" setup>
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { FeedItemGroupByAuthor } from './feedHelper'

interface Props {
    authorGroup: FeedItemGroupByAuthor
}
const props = defineProps<Props>()

</script>

<template>
    <div class="feed-card">
        <div class="feed-author">
            <Image :src="authorGroup.author.imageUrl" :alt="authorGroup.author.name" :width="50" :height="50" :format="ImageFormat.Author" />
        </div>
        <div class="feed-container">
            <div class="feed-header">
                {{ authorGroup.dateLabel }} von: <div>{{ authorGroup.author.name }}</div>
            </div>
            <div class="feed-body">
                <div v-for="feedItem in authorGroup.feedItems">
                    <TopicTabsFeedTopicItem :feedItem="feedItem.topicFeedItem!" v-if="feedItem.type == 0" />
                    <TopicTabsFeedQuestionItem :feedItem="feedItem.questionFeedItem!" v-else-if="feedItem.type == 1" />
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-card {
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;

    .feed-author {
        display: flex;
        justify-content: space-between;
        flex-direction: row;
        flex-wrap: nowrap;
        padding: 0 24px;
    }

    .feed-container {
        flex-grow: 2;

    }
}
</style>