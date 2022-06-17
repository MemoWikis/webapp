<script lang="ts" setup>
import { ref } from 'vue';
import {useTopicStore} from '../topicStore'
const tabsStore = useTopicStore()

const currentTitle = ref(tabsStore.getTopicName)
</script>

<template>
    <div>
        <div>{{currentTitle}}</div>
        <div></div>
        <!-- <TextareaAutosize
            placeholder="Type something here..."
            ref="topicTitle"
            :v-model="tabsStore.getTopicName"
            :min-height="30"
            /> -->
    </div>
</template>

<style scoped></style>