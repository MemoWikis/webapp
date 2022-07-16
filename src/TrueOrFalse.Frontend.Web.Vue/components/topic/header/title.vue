<script lang="ts" setup>
import {useTopicStore} from '../topicStore'
const topicStore = useTopicStore()
</script>

<template>
    <div>
        <h1>
            <resize-textarea
                placeholder="Type something here..."
                ref="topicTitle"
                :modelValue="topicStore.name"
                :minHeight="30"
                >
            </resize-textarea>
        </h1>

    </div>
</template>

<style scoped></style>