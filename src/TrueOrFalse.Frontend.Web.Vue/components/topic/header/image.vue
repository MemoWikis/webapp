<script lang="ts" setup>
import {useTopicStore} from '../topicStore'
const config = useRuntimeConfig();
const topicStore = useTopicStore()
</script>

<template>
    <div>
        <img :src="config.base + topicStore.imgUrl"/>
    </div>
</template>

<style scoped></style>