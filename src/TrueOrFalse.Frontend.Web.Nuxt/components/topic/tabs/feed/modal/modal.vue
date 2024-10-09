<script lang="ts" setup>
import { FeedItem, FeedType, getTopicChangeTypeName } from '../feedHelper'

interface Props {
    show: boolean,
    feedItem: FeedItem,
}

const props = defineProps<Props>()
const emit = defineEmits(['close'])

const isTopic = ref(props.feedItem.type === FeedType.Topic)

watch(() => props.show, (val) => {
    if (val) {
        console.log(props.feedItem)
    }
})
</script>

<template>

    <Modal :show="props.show" @close="emit('close')" :show-close-button="true" :has-header="true">
        <template #header>
            <h2 v-if="isTopic && feedItem.topicFeedItem">{{ getTopicChangeTypeName(feedItem.topicFeedItem.type) }}</h2>
        </template>
        <template #body>
            <!-- <div class="feed-modal-content-change" v-if="oldContent && newContent">
                <div class="feed-modal-old-content feed-modal-content" v-html="props.oldContent"></div>
                <div class="feed-modal-new-content feed-modal-content" v-html="props.newContent"></div>
            </div> -->


            <template v-if="isTopic && feedItem.topicFeedItem">
                <TopicTabsFeedModalTopic :topicFeedItem="feedItem.topicFeedItem" />

            </template>
        </template>
    </Modal>
</template>


<style lang="less" scoped>

</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-content-change {
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