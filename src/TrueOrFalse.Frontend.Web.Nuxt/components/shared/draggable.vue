<script lang="ts" setup>
interface Props {
    transferData: any
    disabled?: boolean
}
const props = defineProps<Props>()
function handleDragStart(event: any) {
    console.log(props.transferData)
    event.dataTransfer.setData('value', props.transferData)
    console.log(event.dataTransfer)
}
const emit = defineEmits(['dragEnded'])
</script>

<template>
    <span :draggable="!disabled" @dragstart="handleDragStart" @dragend="emit('dragEnded')">
        <slot />
    </span>
</template>