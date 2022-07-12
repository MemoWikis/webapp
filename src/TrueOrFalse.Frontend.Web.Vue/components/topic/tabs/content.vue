<script setup>
const props = defineProps({
  categoryId: Number
})
</script>

<template>
  <div>
    {{ props.categoryId }}
    <TopicContentEditor />
  </div>
</template>