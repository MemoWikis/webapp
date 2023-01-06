<script lang="ts" setup>
import { useTopicStore } from '../topicStore'
import { ImageStyle } from '~~/components/image/imageStyleEnum';
const topicStore = useTopicStore()
</script>

<template>
    <Image :url="topicStore.imgUrl" class="topic-header-image" :style="ImageStyle.Topic" :show-license="true" />
</template>

<style scoped lang="less">
.topic-header-image {
    width: 80px;
    height: 80px;
}
</style>