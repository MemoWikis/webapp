<script lang="ts" setup>
import { ref } from 'vue';
import { useTopicStore } from '../topicStore'
const topicStore = useTopicStore()
const textArea = ref(null);

function resize() {
    let element = textArea.value;
    element.style.height = element.scrollHeight + "px";
}

onMounted(() => {
    window.addEventListener('resize', resize);
    topicStore.$subscribe((mutation, state) => {
        if (state.name) {
            if (topicStore.initialName != topicStore.name) {
                topicStore.contentHasChanged = true
            }
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
            <textarea placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea"
                v-model="topicStore.name"></textarea>
        </h1>
        <div id="TopicHeaderDetails">

            <div v-if="topicStore.parentTopicCount > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" />
                <div class="topic-detail-label">{{ topicStore.parentTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer" v-if="topicStore.parentTopicCount > 0 && topicStore.childTopicCount > 0">
            </div>

            <div v-if="topicStore.childTopicCount > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-sitemap" />
                <div class="topic-detail-label">{{ topicStore.childTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer"
                v-if="topicStore.views > 0 && (topicStore.childTopicCount > 0 || topicStore.parentTopicCount > 0)">
            </div>

            <div v-if="topicStore.views > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-eye" />
                <div class="topic-detail-label">{{ topicStore.views }}</div>
            </div>

            <div class="topic-detail-spacer"></div>

            <LazyAuthorIcon v-for="id in topicStore.authorIds" :id="id" />

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