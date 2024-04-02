<script lang="ts" setup>

const props = defineProps([
    'onDragOver',
    'onDragLeave',
    'onDrop'
])
function handleOnDragOver(event: any) {
    console.log(event)
    event.preventDefault()
    props.onDragOver && props.onDragOver(event)
}

</script>

<template>
    <span @dragover="handleOnDragOver" @dragleave="onDragLeave" @drop.stop="onDrop">
        <slot />
    </span>
</template>