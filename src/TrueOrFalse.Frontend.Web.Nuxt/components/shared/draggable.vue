<script lang="ts" setup>
import { useDragStore } from './dragStore'
interface Props {
    transferData: any
    disabled?: boolean
}
const props = defineProps<Props>()
const dragStore = useDragStore()

function handleDragStart(event: any) {
    event.dataTransfer.setData('value', props.transferData)
    emit('selfDragStarted')
    dragStore.dragStart()
}

function handleDragEnd() {
    emit('dragEnded')
    dragStore.dragEnd()

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