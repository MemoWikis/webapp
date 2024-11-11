<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { usePageStore } from '../pageStore'
import { ImageFormat } from '~~/components/image/imageFormatEnum'
const pageStore = usePageStore()
const userStore = useUserStore()
const showModal = ref(false)

function openUploadModal() {
    if (userStore.isLoggedIn)
        showModal.value = true
}
const hover = ref(false)
</script>

<template>
    <div @mouseenter="hover = true" @mouseleave="hover = false" :class="{ 'editable': userStore.isLoggedIn }">
        <Image :src="pageStore.imgUrl" class="page-header-image" :format="ImageFormat.Page" :show-license="true"
            :image-id="pageStore.imgId" :min-height="80" :min-width="80" :alt="`${pageStore.name}'s image'`" />
        <div v-if="userStore.isLoggedIn" class="edit-overlay" :class="{ 'show-overlay': hover }" @click="openUploadModal">
            <font-awesome-icon :icon="['fas', 'pen']" class="edit-overlay-icon" />
            <div class="edit-overlay-label">
                Bild ändern
            </div>
        </div>
    </div>

    <ImageUploadModal v-if="userStore.isLoggedIn" :show="showModal" @close="showModal = false" />
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.page-header-image {
    min-width: 80px;
    width: 80px;
    max-width: 80px;
    min-height: 80px;
    max-height: 80px;
}

.editable {
    cursor: pointer;
}

.edit-overlay {
    width: 80px;
    height: 80px;
    background-color: rgba(255, 255, 255, 0.9);
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
    color: @memo-blue;
    flex-direction: column;
    position: absolute;
    z-index: 2;
    font-size: 16px;
    opacity: 0;
    top: 0;
    transition: opacity ease-in 0.2s;

    &:active {
        filter: brightness(0.85)
    }

    &.show-overlay {
        opacity: 1;
    }

    .edit-overlay-icon {
        font-size: 22px;
    }

    .edit-overlay-label {
        line-height: 18px;
        padding-top: 4px;
        font-weight: 400;
        color: @memo-grey-dark;
    }
}
</style>