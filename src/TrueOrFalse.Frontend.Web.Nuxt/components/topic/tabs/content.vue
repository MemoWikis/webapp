<script setup lang="ts">
const { isMobile } = useDevice()
</script>

<template>
    <div id="TopicContent" class="row" :class="{ 'is-mobile': isMobile }">
        <TopicContentEditor />
        <TopicContentEditBar />
    </div>
</template>