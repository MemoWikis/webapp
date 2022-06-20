<script lang="ts" setup>
const props = defineProps({
  url: String,
  square: Boolean,
})
const config = useRuntimeConfig();
</script>

<template>
    <div>
        <img :src="config.base + props.url"/>
    </div>
</template>

<style scoped></style>