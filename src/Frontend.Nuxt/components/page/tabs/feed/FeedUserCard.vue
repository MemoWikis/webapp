<script lang="ts" setup>
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { FeedItemGroupByAuthor } from './feedHelper'

interface Props {
    authorGroup: FeedItemGroupByAuthor
}
const props = defineProps<Props>()
const { $urlHelper } = useNuxtApp()
const emit = defineEmits(['open-feed-modal'])
const { isDesktop, isMobile } = useDevice()
const showCard = ref(true)
const { t } = useI18n()

watch(() => props.authorGroup, () => {
    showCard.value = true
}, { deep: true })
</script>

<template>
    <div class="feed-card">
        <div class="feed-author" v-if="isDesktop">
            <Image :src="authorGroup.author.imageUrl" :alt="authorGroup.author.name" :width="40" :height="40" :format="ImageFormat.Author" />
        </div>
        <div class="feed-container">
            <div class="feed-header">
                {{ authorGroup.dateLabel }} {{ t('page.feed.userCard.by') }}:
                <NuxtLink :to="$urlHelper.getUserUrl(authorGroup.author.name, authorGroup.author.id)">
                    <Image v-if="isMobile" :src="authorGroup.author.imageUrl" :alt="authorGroup.author.name" :width="20" :height="20" :format="ImageFormat.Author" class="header-icon" />
                    {{ authorGroup.author.name }}
                </NuxtLink>

                <div @click="showCard = !showCard" class="collapse-button" :aria-label="t('page.feed.userCard.collapseExpand')">
                    <font-awesome-icon v-if="showCard" icon="fa-solid fa-chevron-up"
                        class="filter-button-icon" />
                    <font-awesome-icon v-else icon="fa-solid fa-chevron-down" class="filter-button-icon" />
                </div>

            </div>
            <Transition name="collapse">
                <div v-if="showCard" class="feed-item-list">
                    <PageTabsFeedItem v-for="feedItem in authorGroup.feedItems" :page-feed-item="feedItem.pageFeedItem" :question-feed-item="feedItem.questionFeedItem" :index="feedItem.index"
                        @open-feed-modal="emit('open-feed-modal', $event)" />
                </div>
            </Transition>

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
        min-width: 40px;
        margin: 0 24px;
        margin-left: 8px;
    }

    .feed-container {
        flex-grow: 2;
        overflow: hidden;

        .feed-header {
            display: flex;
            flex-wrap: nowrap;
            color: @memo-grey-dark;
            margin: 8px 0px;
            align-items: center;

            a {
                margin-left: 4px;
                display: flex;
                flex-wrap: nowrap;
                align-items: center;
            }

            .header-icon {
                margin-right: 4px;
                margin-left: 4px;
            }

            .collapse-button {
                height: 24px;
                width: 24px;
                color: @memo-grey-dark;
                background: white;
                border-radius: 24px;
                display: flex;
                justify-content: center;
                align-items: center;
                cursor: pointer;
                margin-left: 8px;
                user-select: none;
                ;

                &:hover {
                    filter: brightness(0.95);
                }

                &:active {
                    filter: brightness(0.9);
                }
            }
        }

        .feed-body {
            overflow: hidden;
        }

    }
}

.feed-item-list {
    user-select: none;
}
</style>