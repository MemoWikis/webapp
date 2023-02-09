<script setup>
const props = defineProps(['topic', 'answerBodyModel'])
</script>

<template>
  <div class="row">
    <TopicLearning keep-alive />
  </div>
</template>