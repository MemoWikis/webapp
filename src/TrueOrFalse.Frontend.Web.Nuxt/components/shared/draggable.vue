<script lang="ts" setup>
interface Props {
    transferData: any
    disabled?: boolean
}
const props = defineProps<Props>()
function handleDragStart(event: any) {
    event.dataTransfer.setData('value', JSON.stringify(props.transferData))
}
const emit = defineEmits(['dragEnded'])
</script>

<template>
    <span :draggable="!disabled" @dragstart="handleDragStart" @dragend="emit('dragEnded')">
        <slot />
    </span>
</template>