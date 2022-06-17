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
        <!-- <textarea-autosize
            placeholder="Type something here..."
            ref="myTextarea"
            :v-model="currentTitle"
            :min-height="30"
            :max-height="350"
            /> -->
    </div>
</template>

<style scoped></style>