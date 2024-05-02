<script lang="ts" setup>
import { useDragStore } from '~/components/shared/dragStore'

const dragStore = useDragStore()

const style = computed(() => {

    const x = dragStore.touchClientX
    const y = dragStore.touchClientY

    const str = `top:${y - 55}px; left:${x - 55}px;`
    return str
})


</script>

<template>
    <div class="drag-start-spinner-container" :style="style">
        <div class="loader"></div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.drag-start-spinner-container {
    position: fixed;
    pointer-events: none;
    touch-action: none;
    z-index: 100;

    .loader {
        min-width: 110px;
        min-height: 110px;
        max-width: 110px;
        max-height: 110px;
        border: 10px solid #9e9e9e70;
        border-radius: 50%;
        position: relative;
        transform: rotate(45deg);
        box-sizing: border-box;
    }

    .loader::before {
        content: "";
        position: absolute;
        box-sizing: border-box;
        inset: -10px;
        border-radius: 50%;
        border: 10px solid @memo-green;
        animation: prixClipFix 0.55s 1 linear;
    }

    @keyframes prixClipFix {
        0% {
            clip-path: polygon(50% 50%, 0 0, 0 0, 0 0, 0 0, 0 0)
        }

        25% {
            clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 0, 100% 0, 100% 0)
        }

        50% {
            clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 100%, 100% 100%, 100% 100%)
        }

        75% {
            clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 100%, 0 100%, 0 100%)
        }

        100% {
            clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 100%, 0 100%, 0 0)
        }
    }


}
</style>