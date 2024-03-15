<script lang="ts" setup>

interface Props {
    transferData: any
    disabled?: boolean
}
const props = defineProps<Props>()
const nuxtApp = useNuxtApp()
function handleDragStart(event: any) {
    event.dataTransfer.setData('value', props.transferData)
    emit('selfDragStarted')
    nuxtApp.provide('dragstarted', true)
}

function handleDragEnd(event: any) {
    emit('dragEnded')
    nuxtApp.provide('dragstarted', false)
}
const emit = defineEmits(['dragEnded', 'selfDragStarted'])
</script>

<template>
    <span :draggable="!disabled" @dragstart="handleDragStart" @dragend="handleDragEnd" class="draggable">
        <slot />
    </span>
</template>

<style lang="less">
.draggable {
    cursor: grab;
}
</style>