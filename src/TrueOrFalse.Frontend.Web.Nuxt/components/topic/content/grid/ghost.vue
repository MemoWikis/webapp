<script lang="ts" setup>
import { useDragStore } from '~/components/shared/dragStore'


const dragStore = useDragStore()

onMounted(() => {
    console.log('ghost mounted')
})
const style = computed(() => {
    const scrollX = document.documentElement.scrollLeft - 80;
    const scrollY = document.documentElement.scrollTop - 80;
    const str = `top:${dragStore.clientY + scrollY}px; left:${dragStore.clientX + scrollX}px; position: absolute; z-index: 2000 !important;`
    console.log(str)
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