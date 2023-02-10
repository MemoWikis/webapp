<script lang="ts" setup>
const props = defineProps(['transferData', 'disabled'])
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