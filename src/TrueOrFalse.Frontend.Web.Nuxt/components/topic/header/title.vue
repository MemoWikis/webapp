<script lang="ts" setup>
import { VueElement } from 'vue'
import { useTopicStore, Topic } from '../topicStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'

const topicStore = useTopicStore()
const tabsStore = useTabsStore()
const textArea = ref(null)
const topic = useState<Topic>('topic')
function resize() {
    let element = textArea.value as unknown as VueElement
    if (element) {
        element.style.height = "54px"
        element.style.height = element.scrollHeight + "px"
    }

}

const readonly = ref(false)
watch(() => tabsStore.activeTab, (val: Tab) => {
    if (val == Tab.Topic)
        readonly.value = false
    else {
        readonly.value = true
    }
})

onBeforeMount(() => {
    window.addEventListener('resize', resize);

    watch(() => topicStore.name, (newName) => {
        if (topicStore.initialName != newName) {
            topicStore.contentHasChanged = true
        }
    })

})

onUnmounted(() => {
    window.removeEventListener('resize', resize);
})

</script>

<template>
    <div id="TopicHeaderContainer">
        <h1 id="TopicTitle">
            <textarea v-if="topicStore" placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea"
                v-model="topicStore.name" :readonly="readonly"></textarea>
            <textarea v-else ref="textArea" v-model="topic.Name"></textarea>
        </h1>
        <div id="TopicHeaderDetails">

            <div v-if="topicStore.childTopicCount > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-sitemap" />
                <div class="topic-detail-label">{{ topicStore.childTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer" v-if="topicStore.parentTopicCount > 0 && topicStore.childTopicCount > 0">
            </div>

            <div v-if="topicStore.parentTopicCount > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" />
                <div class="topic-detail-label">{{ topicStore.parentTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer"
                v-if="topicStore.views > 0 && (topicStore.childTopicCount > 0 || topicStore.parentTopicCount > 0)">
            </div>

            <div v-if="topicStore.views > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-eye" />
                <div class="topic-detail-label">{{ topicStore.views }}</div>
            </div>

            <div class="topic-detail-spacer"></div>

            <AuthorIcon v-for="author in topicStore.authors" :author="author" />
        </div>
    </div>
</template>
                           
<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#TopicHeaderContainer {
    width: 100%;

    #TopicTitle {
        margin: 0;

        textarea {
            width: 100%;
            border: none;
            outline: none;
            min-height: 18px;
            resize: none;
            margin-top: -8px;
            padding: 0;
            padding-left: 12px;
            height: 54px;
            overflow: hidden;
        }
    }

    #TopicHeaderDetails {
        display: flex;
        padding-left: 12px;
        font-size: 14px;
        color: @memo-grey-dark;
        height: 20px;

        .topic-detail {
            margin-right: 8px;
            display: flex;
            flex-wrap: nowrap;
            align-items: center;

            .topic-detail-label {
                padding-left: 6px;
            }
        }

        .topic-detail-spacer {
            height: 100%;
            width: 1px;
            background: @memo-grey-light;
            margin-right: 8px;
        }
    }
}
</style>