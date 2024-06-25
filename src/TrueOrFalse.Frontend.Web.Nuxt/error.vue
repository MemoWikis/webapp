<script lang="ts" setup>
import type { NuxtError } from '#app'

const props = defineProps({
    error: Object as () => NuxtError
})

const emit = defineEmits(['setPage'])
</script>

<template>
    <ErrorContent :error="props.error" :in-error-boundary="false" />
</template>