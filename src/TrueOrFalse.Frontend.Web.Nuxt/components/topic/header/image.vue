<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { useTopicStore } from '../topicStore'
import { ImageFormat } from '~~/components/image/imageFormatEnum'
const topicStore = useTopicStore()
const userStore = useUserStore()
const showModal = ref(false)

function openUploadModal() {
    if (userStore.isLoggedIn)
        showModal.value = true
}
</script>

<template>
    <Image :src="topicStore.imgUrl" class="topic-header-image" :format="ImageFormat.Topic" :show-license="true"
        :image-id="topicStore.imgId" @click="openUploadModal" :min-height="80" :min-width="80" />
    <ImageUploadModal v-if="userStore.isLoggedIn" :show="showModal" @close="showModal = false" />
</template>

<style scoped lang="less">
.topic-header-image {
    min-width: 80px;
    width: 80px;
    max-width: 80px;
    min-height: 80px;
    max-height: 80px;
}
</style>