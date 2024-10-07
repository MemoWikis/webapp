<script lang="ts" setup>
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { FeedItemGroupByAuthor } from './feedHelper'

interface Props {
    authorGroup: FeedItemGroupByAuthor
}
const props = defineProps<Props>()
const { $urlHelper } = useNuxtApp()
const emit = defineEmits(['open-feed-modal'])
</script>

<template>
    <div class="feed-card">
        <div class="feed-author">
            <Image :src="authorGroup.author.imageUrl" :alt="authorGroup.author.name" :width="40" :height="40" :format="ImageFormat.Author" />
        </div>
        <div class="feed-container">
            <div class="feed-header">
                {{ authorGroup.dateLabel }} von: <NuxtLink :to="$urlHelper.getUserUrl(authorGroup.author.name, authorGroup.author.id)">{{ authorGroup.author.name }}</NuxtLink>
            </div>
            <div class="feed-body">
                <TopicTabsFeedItem v-for=" feedItem in authorGroup.feedItems" :topic-feed-item="feedItem.topicFeedItem" :question-feed-item="feedItem.questionFeedItem" @open-feed-modal="emit('open-feed-modal', $event)" />
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
        // padding: 0 24px;
        min-width: 40px;
        margin: 0 24px;
        margin-left: 8px;
    }

    .feed-container {
        flex-grow: 2;
        overflow: hidden;
        .feed-header {
            display: flex;
            flex-wrap: flex;
            margin: 8px 0px;
            a {
                margin-left: 4px;
            }
        }

        .feed-body {
            overflow:hidden;
        }

    }
}
</style>