<script lang="ts" setup>
import { Page } from '~~/components/shared/pageEnum'
const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', Page.Welcome)
})
</script>

<template>
    <div> hello </div>

    <NuxtLink to="/random/1">To global wiki</NuxtLink>
</template>