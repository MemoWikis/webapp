<script lang="ts" setup>
import {useTopicStore} from '../topicStore'
const topicStore = useTopicStore()
</script>

<template>
    <div>
        <img :src="topicStore.imgUrl"/>
    </div>
</template>

<style scoped></style>