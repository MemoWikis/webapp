<script lang="ts" setup>
import { useDragStore } from '~/components/shared/dragStore'


const dragStore = useDragStore()

onMounted(() => {
    console.log('ghost mounted')
})

const { isDesktop } = useDevice()
const style = computed(() => {

    const x = isDesktop ? dragStore.x : dragStore.screenX
    const y = isDesktop ? dragStore.y : dragStore.screenY

    const str = `top:${y}px; left:${x}px; position: absolute; z-index: 2000 !important;`
    return str
})
onUnmounted(() => {
    console.log('ghost unmounted')
})
</script>

<template>
    <div class="ghost-container" :style="style">
        <div class="">
            <div class="name">
                {{ 'TEST' }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ghost-container {
    background: @memo-green;
    padding: 20px;
}
</style>