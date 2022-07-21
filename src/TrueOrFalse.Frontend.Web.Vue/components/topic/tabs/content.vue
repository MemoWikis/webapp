<script setup>
const props = defineProps({
  categoryId: Number
})
</script>

<template>
  <div id="TopicContent">
    {{ props.categoryId }}
    <TopicContentEditor />
  </div>
</template>