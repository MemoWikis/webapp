<script lang="ts" setup>
import { ref } from 'vue';
import {useTopicStore} from '../topicStore'
const tabsStore = useTopicStore()

const currentTitle = ref(tabsStore.getTopicName)
</script>

<template>
    <div>
        <h1>
            <resize-textarea
                placeholder="Type something here..."
                ref="topicTitle"
                :modelValue="currentTitle"
                :minHeight="30"
                >
            </resize-textarea>
        </h1>

    </div>
</template>

<style scoped></style>