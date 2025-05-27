<script setup lang="ts">

interface Props {
    title?: string
    noPadding?: boolean
    size?: LayoutCardSize
}

withDefaults(defineProps<Props>(), {
    title: '',
    noPadding: false,
    size: LayoutCardSize.Large
})


</script>

<template>
    <div class="layout-card" :class="`size-${size}`">
        <div class="card-header" v-if="title">
            <h2 class="card-title">{{ title }}</h2>
            <div class="card-actions" v-if="$slots.actions">
                <slot name="actions"></slot>
            </div>
        </div>
        <div class="card-content" :class="{ 'no-padding': noPadding }">
            <slot></slot>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-card {
    width: 100%;
    margin-top: 1rem;
    border-radius: 8px;
    overflow: hidden;
    width: 100%;

    &.size-medium {
        @media (min-width: 768px) {
            width: calc(50% - 0.5rem);
        }
    }

    &.size-small {
        @media (min-width: 768px) {
            width: calc(33.3333% - 0.666rem);
        }

        @media (min-width: 360px) and (max-width: 767px) {
            width: calc(50% - 0.5rem);
        }

        @media (min-width: 500px) and (max-width: 767px) {
            width: calc(50% - 0.5rem);
        }

        @media (max-width: 499px) {
            width: calc(100%);
        }
    }

    &.size-tiny {
        @media (min-width: 768px) {
            width: calc(25% - 0.75rem);
        }

        @media (min-width: 500px) and (max-width: 767px) {
            width: calc(50% - 0.5rem);
        }

        @media (max-width: 499px) {
            width: calc(100%);
        }
    }

    .card-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 16px 0;

        .card-title {
            font-size: 1.8rem;
            font-weight: 600;
            color: @memo-grey-darker;
            margin: 0;
        }

        .card-actions {
            display: flex;
            align-items: center;
            gap: 8px;
        }
    }

    .card-content {
        padding: 16px 20px;
        border-radius: 8px;
        overflow: hidden;
        background: white;

        &.no-padding {
            padding: 0;
        }
    }
}
</style>

<style lang="less">
.sidesheet-open {
    .layout-card {
        &.size-medium {
            @media (max-width:1300px) {
                width: 100%;
            }
        }

        &.size-small {
            @media (max-width:1300px) {
                width: calc(50% - 0.5rem);
            }

            @media (max-width: 900px) {
                width: 100%;
            }
        }

        &.size-tiny {
            @media (max-width:1300px) {
                width: calc(50% - 0.5rem);
            }

            @media (max-width: 900px) {
                width: 100%;
            }
        }
    }
}
</style>