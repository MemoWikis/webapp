<script setup lang="ts">
import { LayoutGridSize } from '~/composables/layoutGridSize'

interface Props {
    size?: LayoutGridSize
    gap?: string
}

const props = withDefaults(defineProps<Props>(), {
    size: LayoutGridSize.Flex,
    gap: '0rem 1rem'
})

const gridStyle = computed(() => ({
    '--grid-gap': props.gap
}))

const gridClass = computed(() => ({
    'layout-grid': true,
    'layout-grid--half': props.size === LayoutGridSize.Half,
    'layout-grid--full': props.size === LayoutGridSize.Full,
    'layout-grid--flex': props.size === LayoutGridSize.Flex
}))
</script>

<template>
    <div :class="gridClass" :style="gridStyle">
        <slot />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-grid {
    display: flex;
    flex-wrap: wrap;
    gap: var(--grid-gap);

    &--half {
        width: 50%;
    }

    &--full {
        width: 100%;
    }

    &--flex {
        flex-grow: 2;
    }
}
</style>
