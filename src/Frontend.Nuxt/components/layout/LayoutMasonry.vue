<script setup lang="ts">
interface Props {
    columns?: number
    gap?: string
    breakpoints?: {
        mobile?: number
        tablet?: number
        desktop?: number
    }
}

const props = withDefaults(defineProps<Props>(), {
    columns: 3,
    gap: '1rem',
    breakpoints: () => ({
        mobile: 1,
        tablet: 2,
        desktop: 3
    })
})

const { isMobile, isTablet } = useDevice()

const currentColumns = computed(() => {
    if (isMobile) return props.breakpoints.mobile || 1
    if (isTablet) return props.breakpoints.tablet || 2
    return props.breakpoints.desktop || props.columns
})

const masonryStyle = computed(() => ({
    '--masonry-columns': currentColumns.value,
    '--masonry-gap': props.gap
}))
</script>

<template>
    <div class="layout-masonry" :style="masonryStyle">
        <slot />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-masonry {
    display: grid;
    grid-template-columns: repeat(var(--masonry-columns), 1fr);
    gap: var(--masonry-gap);
    align-items: start;
    width: 100%;

    // Responsive behavior
    @media (max-width: @screen-xs-max) {
        grid-template-columns: repeat(1, 1fr);
    }

    @media (min-width: @screen-sm-min) and (max-width: @screen-md-max) {
        grid-template-columns: repeat(2, 1fr);
    }

    @media (min-width: @screen-lg-min) {
        grid-template-columns: repeat(var(--masonry-columns), 1fr);
    }
}
</style>
