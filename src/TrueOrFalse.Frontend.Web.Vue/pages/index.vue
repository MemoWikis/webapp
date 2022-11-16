<script lang="ts" setup>
import { PageType } from '~~/components/shared/pageTypeEnum'

useHead({
  title: 'MemoWikis',
})

onMounted(() => {
  useState<PageType>('page', () => PageType.Welcome)
})
</script>

<template>
  <div>

  </div>
</template>

<style scoped>

</style>
