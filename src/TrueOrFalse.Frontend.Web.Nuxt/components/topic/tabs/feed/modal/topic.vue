<script setup lang="ts">
import { TopicChangeType, TopicFeedItem } from '../feedHelper'

interface Props {
    topicFeedItem: TopicFeedItem
}
const props = defineProps<Props>()

const showDiff = ref(true)

</script>

<template>
    <div class="feed-modal-topic-container">
        <template v-if="topicFeedItem.type === TopicChangeType.Renamed">
            <div class="feed-modal-content-change">
                <div class="">{{ topicFeedItem.nameChange?.oldName }}</div>
                <font-awesome-icon :icon="['fas', 'chevron-right']" />
                <div class="">{{ topicFeedItem.nameChange?.newName }}</div>
            </div>
        </template>

        <template v-if="topicFeedItem.type === TopicChangeType.Text">

            <div class="show-diff-toggle">
                <div class="show-diff-toggle-button" :class="{ 'is-active': showDiff }" @click="showDiff = true">Änderungen anzeigen</div>
                <div class="show-diff-toggle-button" :class="{ 'is-active': !showDiff }" @click="showDiff = false">Themenvorschau</div>
            </div>
            <div class="feed-modal-content-change" v-if="props.topicFeedItem.contentChange?.diffContent && showDiff">
                <div class="feed-modal-diff-content" v-html="props.topicFeedItem.contentChange?.diffContent"></div>
            </div>
            <div class="feed-modal-content-change" v-else-if="props.topicFeedItem.contentChange?.newContent && !showDiff">
                <div class="feed-modal-diff-content" v-html="props.topicFeedItem.contentChange?.newContent"></div>
            </div>
        </template>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';


.feed-modal-content-change {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;
    .feed-modal-old-content {
        border-right: 1px solid @memo-grey-lighter;
    }
    .feed-modal-new-content {
    }

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

        &:hover{
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