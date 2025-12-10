<script setup lang="ts">
import { ContentChange, PageChangeType, PageFeedItem } from './feedHelper'

interface Props {
    pageFeedItem: PageFeedItem,
    contentChange?: ContentChange
}
const props = defineProps<Props>()

const showDiff = ref(true)

const { $urlHelper } = useNuxtApp()
const { t } = useI18n()

</script>

<template>
    <div class="feed-modal-page-container">
        <template v-if="pageFeedItem.type === PageChangeType.Renamed">
            <div class="feed-modal-page-body">
                <div class="">{{ pageFeedItem.nameChange?.oldName }}</div>
                <font-awesome-icon :icon="['fas', 'chevron-right']" />
                <div class="">{{ pageFeedItem.nameChange?.newName }}</div>
            </div>
        </template>

        <template v-if="pageFeedItem.type === PageChangeType.Text && contentChange">

            <div class="show-diff-toggle">
                <div class="show-diff-toggle-button" :class="{ 'is-active': showDiff }" @click="showDiff = true">
                    {{ t('page.feed.modal.showChanges') }}
                </div>
                <div class="show-diff-toggle-button" :class="{ 'is-active': !showDiff }" @click="showDiff = false">
                    {{ t('page.feed.modal.showWithoutChanges') }}
                </div>
            </div>

            <div class="feed-modal-page-body" v-if="contentChange.diffContent && showDiff">
                <div class="feed-modal-diff-content" v-html="contentChange.diffContent"></div>
            </div>
            <div class="feed-modal-page-body" v-else-if="contentChange.currentContent && !showDiff">
                <div class="feed-modal-diff-content" v-html="contentChange.currentContent"></div>
            </div>

        </template>

        <template v-if="pageFeedItem.type === PageChangeType.Relations && pageFeedItem.relationChanges">

            <template v-if="pageFeedItem.relationChanges.addedParents && pageFeedItem.relationChanges.addedParents.length > 0">
                <h4>{{ t('page.feed.modal.addedParentPages') }}</h4>
                <div class="feed-modal-page-body">
                    <ul>
                        <li v-for="parent in pageFeedItem.relationChanges.addedParents">
                            <NuxtLink :to="$urlHelper.getPageUrl(parent.name, parent.id)">{{ parent.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>

            <template v-if="pageFeedItem.relationChanges.removedParents && pageFeedItem.relationChanges.removedParents.length > 0">
                <h4>{{ t('page.feed.modal.removedParentPages') }}</h4>
                <div class="feed-modal-page-body">
                    <ul>
                        <li v-for="parent in pageFeedItem.relationChanges.removedParents">
                            <NuxtLink :to="$urlHelper.getPageUrl(parent.name, parent.id)">{{ parent.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>

            <template v-if="pageFeedItem.relationChanges.addedChildren && pageFeedItem.relationChanges.addedChildren.length > 0">
                <h4>{{ t('page.feed.modal.addedChildPages') }}</h4>
                <div class="feed-modal-page-body">
                    <ul>
                        <li v-for="child in pageFeedItem.relationChanges.addedChildren">
                            <NuxtLink :to="$urlHelper.getPageUrl(child.name, child.id)">{{ child.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>

            <template v-if="pageFeedItem.relationChanges.removedChildren && pageFeedItem.relationChanges.removedChildren.length > 0">
                <h4>{{ t('page.feed.modal.removedChildPages') }}</h4>
                <div class="feed-modal-page-body">
                    <ul>
                        <li v-for="child in pageFeedItem.relationChanges.removedChildren">
                            <NuxtLink :to="$urlHelper.getPageUrl(child.name, child.id)">{{ child.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>
        </template>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-page-body {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;

    .feed-modal-content {
        width: 50%;
        padding: 16px;
    }

    .feed-modal-diff-content {
        padding: 16px;
    }
}

.show-diff-toggle {
    display: flex;
    align-items: center;
    margin-top: 20px;
    height: 56px;
    padding-bottom: 20px;
    border-bottom: solid 1px @memo-grey-lighter;

    .show-diff-toggle-button {
        padding: 8px 16px;
        cursor: pointer;
        border-radius: 24px;
        background: white;
        color: @memo-grey-darker;

        &:hover {
            filter: brightness(0.95);
        }

        &:active {
            filter: brightness(0.9);
        }

        &.is-active {
            background: @memo-info;

        }
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-page-body {
    ins {
        // background: fade(@memo-green, 10%);
        border-radius: 4px;
        color: @memo-green;

        img {
            border: solid 4px @memo-green;
        }
    }

    del {
        // background: fade(@memo-wish-knowledge-red, 10%);
        text-decoration: line-through;
        border-radius: 4px;
        color: @memo-wish-knowledge-red;

        img {
            border: solid 4px @memo-wish-knowledge-red;
        }
    }
}
</style>