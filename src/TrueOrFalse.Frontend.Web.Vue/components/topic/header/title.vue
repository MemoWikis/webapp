<script lang="ts" setup>
import { ref } from 'vue';
import { useTopicStore } from '../topicStore'
const topicStore = useTopicStore()
const textArea = ref(null);

function resize() {
    let element = textArea.value;

    element.style.height = "1px";
    element.style.height = element.scrollHeight + "px";
}

onMounted(() => {
    resize()
    window.addEventListener('resize', resize);
})
const loadAuthor = ref(false)
</script>

<template>
    <div id="TopicHeaderContainer">
        <h1 id="TopicTitle">
            <textarea placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea"
                v-model="topicStore.name"></textarea>
        </h1>
        <div id="TopicHeaderDetails">
            <div v-if="topicStore.parentTopicCount > 0">
                <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" />{{ topicStore.parentTopicCount }}
            </div>
            <div v-if="topicStore.childTopicCount > 0">
                <font-awesome-icon icon="fa-solid fa-sitemap" />{{ topicStore.childTopicCount }}
            </div>
            <div v-if="topicStore.views > 0">
                <font-awesome-icon icon="fa-solid fa-eye" />{{ topicStore.views }}
            </div>

            <LazyAuthorIcon v-for="id in topicStore.authorIds" :id="id" />
        </div>
    </div>
</template>
                           
<style scoped lang="less">
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
        }
    }

    #TopicHeaderDetails {
        display: flex;
    }
}
</style>