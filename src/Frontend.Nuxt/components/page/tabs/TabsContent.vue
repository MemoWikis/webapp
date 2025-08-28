<script setup lang="ts">
import { Visibility } from '~/components/shared/visibilityEnum'
import { usePageStore } from '../pageStore'

const { isMobile } = useDevice()
const pageStore = usePageStore()
</script>

<template>
    <div id="PageContent" class="" :class="{ 'is-mobile': isMobile, 'no-grid-items': pageStore.gridItems.length === 0, 'is-public': pageStore.visibility === Visibility.Public }">
        <PageContentEditor />
        <PageContentEditBar v-if="pageStore.visibility === Visibility.Public" />
    </div>
</template>

<style lang="less" scoped></style>