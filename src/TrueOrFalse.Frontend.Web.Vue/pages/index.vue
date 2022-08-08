<script lang="ts" setup>
import { PageType } from '~~/components/shared/pageTypeEnum'

useHead({
  title: 'MemoWikis',
})

onMounted(() => {
  console.log('log')
  useState<PageType>('page', () => PageType.Welcome)
})
</script>

<template>
  <div>
    Erklärseite
  </div>
</template>

<style scoped>
</style>
