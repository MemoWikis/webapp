<script lang="ts" setup>
interface Props {
    marginTop?: number
}

const props = withDefaults(defineProps<Props>(), {
    marginTop: 25
})
</script>

<template>
    <!-- Empty sidebar - learning mode doesn't need sidebar content -->
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

/* No styles needed for empty sidebar */
</style>