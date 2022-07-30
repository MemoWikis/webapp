<script lang="ts" setup>
import { useTopicStore } from '../topicStore'
const topicStore = useTopicStore()
</script>

<template>
    <Image :url="topicStore.imgUrl" class="topic-header-image" />
</template>

<style scoped lang="less">
.topic-header-image {
    width: 80px;
    height: 80px;
}
</style>