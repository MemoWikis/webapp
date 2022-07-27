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

</script>

<template>
    <div id="TopicHeaderLabelContainer">
        <h1 id="TopicHeaderLabel">
            <textarea placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea"
                v-model="topicStore.name"></textarea>
        </h1>

    </div>
</template>

<style scoped lang="less">
#TopicHeaderLabelContainer {
    width: 100%;

    #TopicHeaderLabel {
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
        }
    }
}
</style>