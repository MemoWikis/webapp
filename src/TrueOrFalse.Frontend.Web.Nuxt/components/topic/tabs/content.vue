<script setup lang="ts">
const { isMobile } = useDevice()

const props = defineProps<{
    textIsHidden: boolean
}>()
</script>

<template>
    <div id="TopicContent" class="row" :class="{ 'is-mobile': isMobile, 'hideText': props.textIsHidden }">
        <TopicContentEditor v-if="!props.textIsHidden" />
        <TopicContentEditBar />
    </div>
</template>

<style lang="less" scoped>
#TopicContent {
    &.hideText {
        position: absolute;
        width: 100%;
        padding-top: 12px;
    }
}
</style>