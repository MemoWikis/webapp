<script setup lang="ts">
import { LayoutGridSize } from '~/composables/layoutGridSize'

interface Props {
    size?: LayoutGridSize
    gap?: string
    direction?: 'row' | 'column'
    title?: string
}

const props = withDefaults(defineProps<Props>(), {
    size: LayoutGridSize.Flex,
    gap: '0rem 1rem',
    direction: 'row',
    title: ''
})

const gridStyle = computed(() => ({
    '--grid-gap': props.gap,
    '--grid-direction': props.direction
}))

const gridClass = computed(() => ({
    'layout-grid': true,
    'layout-grid--divider': props.size === LayoutGridSize.Divider,
    'layout-grid--small': props.size === LayoutGridSize.Small,
    'layout-grid--medium': props.size === LayoutGridSize.Medium,
    'layout-grid--large': props.size === LayoutGridSize.Large,
    'layout-grid--flex': props.size === LayoutGridSize.Flex
}))
</script>

<template>
    <div :class="gridClass">
        <div class="grid-header" v-if="title">
            <h3 class="grid-title">{{ title }}</h3>
        </div>
        <div class="layout-grid-content" :style="gridStyle">
            <slot />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-grid {

    &--divider {
        width: 1rem;
    }

    &--small {
        width: 33%;
    }

    &--medium {
        width: 50%;
    }

    &--large {
        width: 100%;
    }

    &--flex {
        flex-grow: 2;
    }

    .grid-header {
        padding: 2rem 0rem 1rem;

        .grid-title {
            font-size: 1.6rem;
            font-weight: 600;
            color: @memo-grey-darker;
            margin: 0;
        }
    }
}

.layout-grid-content {
    display: flex;
    flex-wrap: wrap;
    gap: var(--grid-gap);
    flex-direction: var(--grid-direction);
}
</style>
