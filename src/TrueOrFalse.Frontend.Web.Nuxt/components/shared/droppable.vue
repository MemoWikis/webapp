<script lang="ts" setup>
const props = defineProps([
    'onDragOver',
    'onDragLeave',
    'onDrop'
])
function handleOnDragOver(event: any) {
    event.preventDefault()
    props.onDragOver && props.onDragOver(event)
}

</script>

<template>
    <div @dragover="handleOnDragOver" @dragleave="onDragLeave" @drop="onDrop">
        <slot />
    </div>
</template>