<script lang="ts" setup>
import { useDragStore } from '~/components/shared/dragStore'


const dragStore = useDragStore()


onMounted(() => {
    console.log('ghost mounted')
})

onUnmounted(() => {
    console.log('ghost unmounted')
})
</script>

<template>
    <div class="grid-item-placeholder" :style="`top:${dragStore.clientY}px; left:${dragStore.clientX}px`">
        <div class="image-container">
            <div class="image-placeholder"></div>
        </div>

        <div class="body-placeholder">
            <div class="topic-name-placeholder">
                {{ dragStore.transferData.name }}
            </div>

            <!-- <div class="label-placeholder"></div>

            <div class="knowledgebar-container">
                <div class="knowledgebar-placeholder"></div>
            </div> -->
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-item-placeholder {
    z-index: 200;
    user-select: none;
    display: flex;
    flex-wrap: nowrap;
    padding: 10px 0px;
    background: green;
    // border-top: solid 1px #DDDDDD;
    position: absolute;
    width: 400px;
    height: 80px;

    .left-spacer {
        width: 32px;
        max-width: 32px;
    }

    .image-container {
        max-width: 40px;

        .image-placeholder {
            background: @memo-grey-lighter;
            height: 40px;
            width: 40px;
            min-width: 40px;
            min-height: 40px;
        }
    }

    .body-placeholder {
        display: flex;
        flex-direction: column;
        padding-left: 8px;

        .topic-name-placeholder {
            font-size: 18px;
            color: @memo-green;
            height: 26px;
            width: 100%;
        }
    }
}
</style>