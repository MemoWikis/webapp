<script lang="ts" setup>
import { ContentChange, FeedItem, FeedType, getTopicChangeTypeName, TopicChangeType } from '../feedHelper'

interface Props {
    show: boolean,
    feedItem: FeedItem,
}

const props = defineProps<Props>()
const emit = defineEmits(['close'])

const isTopic = ref(props.feedItem.type === FeedType.Topic)

onBeforeMount(() => {
    if (props.show)
        getContentChange()
})

watch(() => props.show, (val) => {
    console.log('show', val)
    if (val)
        getContentChange()
})

const contentChange = ref<ContentChange>()

const getContentChange = async () => {
    if (props.feedItem.topicFeedItem?.type === TopicChangeType.Text) {
        const data = {
            topicId: props.feedItem.topicFeedItem.topicId,
            changeId: props.feedItem.topicFeedItem.categoryChangeId
        }
        const response = await $api<ContentChange>(`/apiVue/FeedModalTopic/GetContentChange/`, {
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
            body: data,
            onResponseError(context) {
                const { $logger } = useNuxtApp()
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
            }
        })

        contentChange.value = response
    }
}
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
                <TopicTabsFeedModalTopic :topicFeedItem="feedItem.topicFeedItem" :content-change="contentChange" />

            </template>
        </template>
    </Modal>
</template>


<style lang="less" scoped>

h2 {
    margin-bottom: 36px;
}

</style>
