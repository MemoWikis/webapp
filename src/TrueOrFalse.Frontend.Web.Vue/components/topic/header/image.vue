<script lang="ts" setup>
import {useTopicStore} from '../topicStore'
const topicStore = useTopicStore()
</script>

<template>
    <div>
        <Image :url="topicStore.imgUrl"/>
    </div>
</template>

<style scoped></style>