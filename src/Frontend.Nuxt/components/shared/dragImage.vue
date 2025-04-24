<template>
    <div class="drag-image">
        {{ text }}
    </div>
</template>

<script setup lang="ts">
interface Props {
    text: string
}
const { text } = defineProps<Props>();

</script>

<style scoped>
.drag-image {
    background-color: green;
    color: white;
    width: 100px;
    height: 100px;
    line-height: 100px;
    text-align: center;
}
</style>