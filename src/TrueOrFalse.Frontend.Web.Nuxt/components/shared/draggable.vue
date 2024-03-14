<script lang="ts" setup>
interface Props {
    transferData: any
    disabled?: boolean
}
const props = defineProps<Props>()
function handleDragStart(event: any) {
    event.dataTransfer.setData('value', props.transferData)
    emit('dragStarted')

    if (document != null)
        document.body.classList.add('dnd-grabbing')
}

function handleDragEnd(event: any) {
    emit('dragEnded')

    if (document != null)
        document.body.classList.remove('dnd-grabbing')
}
const emit = defineEmits(['dragEnded', 'dragStarted'])
</script>

<template>
    <span :draggable="!disabled" @dragstart="handleDragStart" @dragend="handleDragEnd">
        <slot />
    </span>
</template>

<style lang="less">
.dnd-grabbing {
    cursor: grabbing !important;
}
</style>