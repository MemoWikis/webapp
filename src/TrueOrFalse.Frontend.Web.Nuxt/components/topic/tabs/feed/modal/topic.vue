<script setup lang="ts">
import { ContentChange, TopicChangeType, TopicFeedItem } from '../feedHelper'

interface Props {
    topicFeedItem: TopicFeedItem,
    contentChange?: ContentChange
}
const props = defineProps<Props>()

const showDiff = ref(true)

const { $urlHelper } = useNuxtApp()

</script>

<template>
    <div class="feed-modal-topic-container">
        <template v-if="topicFeedItem.type === TopicChangeType.Renamed">
            <div class="feed-modal-topic-body">
                <div class="">{{ topicFeedItem.nameChange?.oldName }}</div>
                <font-awesome-icon :icon="['fas', 'chevron-right']" />
                <div class="">{{ topicFeedItem.nameChange?.newName }}</div>
            </div>
        </template>

        <template v-if="topicFeedItem.type === TopicChangeType.Text && contentChange">

            <div class="show-diff-toggle">
                <div class="show-diff-toggle-button" :class="{ 'is-active': showDiff }" @click="showDiff = true">Änderungen anzeigen</div>
                <div class="show-diff-toggle-button" :class="{ 'is-active': !showDiff }" @click="showDiff = false">Vorschau</div>
            </div>

            <div class="feed-modal-topic-body" v-if="contentChange.diffContent && showDiff">
                <div class="feed-modal-diff-content" v-html="contentChange.diffContent"></div>
            </div>
            <div class="feed-modal-topic-body" v-else-if="contentChange.newContent && !showDiff">
                <div class="feed-modal-diff-content" v-html="contentChange.newContent"></div>
            </div>

        </template>

        <template v-if="topicFeedItem.type === TopicChangeType.Relations && topicFeedItem.relationChanges">

            <template v-if="topicFeedItem.relationChanges.addedParents && topicFeedItem.relationChanges.addedParents.length > 0">
                <h4>Hinzugefügte Oberthemen</h4>
                <div class="feed-modal-topic-body">
                    <ul>
                        <li v-for="parent in topicFeedItem.relationChanges.addedParents">
                            <NuxtLink :to="$urlHelper.getTopicUrl(parent.name, parent.id)">{{ parent.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>

            <template v-if="topicFeedItem.relationChanges.removedParents && topicFeedItem.relationChanges.removedParents.length > 0">
                <h4>Entfernte Oberthemen</h4>
                <div class="feed-modal-topic-body">
                    <ul>
                        <li v-for="parent in topicFeedItem.relationChanges.removedParents">
                            <NuxtLink :to="$urlHelper.getTopicUrl(parent.name, parent.id)">{{ parent.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>

            <template v-if="topicFeedItem.relationChanges.addedChildren && topicFeedItem.relationChanges.addedChildren.length > 0">
                <h4>Hinzugefügte Unterthemen</h4>
                <div class="feed-modal-topic-body">
                    <ul>
                        <li v-for="child in topicFeedItem.relationChanges.addedChildren">
                            <NuxtLink :to="$urlHelper.getTopicUrl(child.name, child.id)">{{ child.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>

            <template v-if="topicFeedItem.relationChanges.removedChildren && topicFeedItem.relationChanges.removedChildren.length > 0">
                <h4>Entfernte Unterthemen</h4>
                <div class="feed-modal-topic-body">
                    <ul>
                        <li v-for="child in topicFeedItem.relationChanges.removedChildren">
                            <NuxtLink :to="$urlHelper.getTopicUrl(child.name, child.id)">{{ child.name }}</NuxtLink>
                        </li>
                    </ul>
                </div>
            </template>
        </template>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-topic-body {
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

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-topic-body {
    ins {
        // background: fade(@memo-green, 10%);
        border-radius: 4px;
        color: @memo-green;

        img {
            border: solid 4px @memo-green;
        }
    }
    del {
        // background: fade(@memo-wuwi-red, 10%);
        text-decoration: line-through;
        border-radius: 4px;
        color: @memo-wuwi-red;

        img {
            border: solid 4px @memo-wuwi-red;
        }
    }
}
</style>