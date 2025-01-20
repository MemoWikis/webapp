<script setup lang="ts">
import { Visibility } from '~/components/shared/visibilityEnum'
import { usePageStore } from '../pageStore'

const { isMobile } = useDevice()
const pageStore = usePageStore()

const props = defineProps<{
    textIsHidden: boolean
}>()
</script>

<template>
    <div id="PageContent" class="row" :class="{ 'is-mobile': isMobile, 'hideText': props.textIsHidden, 'no-grid-items': pageStore.gridItems.length === 0 }">
        <template v-if="!props.textIsHidden">
            <PageContentEditor />
            <PageContentEditBar v-if="pageStore.visibility === Visibility.All" />
        </template>
    </div>
</template>

<style lang="less" scoped>
#PageContent {
    &.hideText {
        position: absolute;
        width: 100%;
        padding-top: 12px;
    }
}
</style>