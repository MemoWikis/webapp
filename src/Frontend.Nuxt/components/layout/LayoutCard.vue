<script setup lang="ts">

interface Props {
    title?: string
    noPadding?: boolean
    size?: LayoutCardSize
    url?: string
    backgroundColor?: string
}

const props = withDefaults(defineProps<Props>(), {
    title: '',
    noPadding: false,
    size: LayoutCardSize.Large,
    backgroundColor: 'white'
})

const backgroundColorStyle = computed(() => {
    if (props.backgroundColor)
        return `background: ${props.backgroundColor};`
})


</script>

<template>
    <div class="layout-card" :class="[`size-${size}`, { 'has-link': url }]">

        <NuxtLink v-if="url" :to="url" class="card-link">
            <div class="card-header" v-if="title || $slots.actions || $slots.header">

                <slot name="header"></slot>

                <template v-if="!$slots.header">
                    <h2 class="card-title">{{ title }}</h2>
                    <div class="card-actions" v-if="$slots.actions">
                        <slot name="actions"></slot>
                    </div>

                </template>

            </div>
            <div class="card-content" :class="{ 'no-padding': noPadding }" :style="backgroundColorStyle">
                <slot></slot>
            </div>
        </NuxtLink>

        <template v-else>
            <div class="card-header" v-if="title || $slots.actions || $slots.header">

                <slot name="header"></slot>

                <template v-if="!$slots.header">
                    <h2 class="card-title">{{ title }}</h2>
                    <div class="card-actions" v-if="$slots.actions">
                        <slot name="actions"></slot>
                    </div>

                </template>

            </div>
            <div class="card-content" :class="{ 'no-padding': noPadding }" :style="backgroundColorStyle">
                <slot></slot>
            </div>
        </template>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-card {
    width: 100%;
    margin-top: 1rem;
    border-radius: 8px;
    overflow: hidden;

    &.size-flex {
        width: unset;
    }

    &.size-large {
        width: 100%;
    }

    &.size-medium {
        @media (min-width: @screen-sm) {
            width: calc(50% - 0.5rem);
        }
    }

    &.size-small {
        @media (min-width: @screen-sm) {
            width: calc(33.3333% - 0.6667rem);
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
        @media (min-width: @screen-sm) {
            width: calc(25% - 0.75rem);
        }

        @media (min-width: 500px) and (max-width: 767px) {
            width: calc(50% - 0.5rem);
        }

        @media (max-width: 499px) {
            width: calc(100%);
        }
    }

    &.size-micro {
        @media (min-width: 768px) {
            width: calc(20% - 0.8rem);
        }

        @media (max-width: 767px) {
            width: calc(50% - 0.5rem);
        }
    }

    &.size-nano {
        @media (min-width: 768px) {
            width: calc(16.6667% - 0.8334rem);
        }

        @media (max-width: 767px) {
            width: calc(33.3333% - 0.6667rem);
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
        height: 100%;

        &.no-padding {
            padding: 0;
        }
    }

    &.has-link {
        cursor: pointer;

        &:hover {
            .card-content {
                background: @memo-grey-lightest;

                &[style*="background"] {
                    filter: brightness(0.95);
                }
            }

            :deep(*) {
                text-decoration: none;
            }
        }

        &:active {
            .card-content {
                background: @memo-grey-lighter;

                &[style*="background"] {
                    filter: brightness(0.9);
                }
            }
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

        &.size-micro {
            @media (max-width:1300px) {
                width: calc(33.3333% - 0.6667rem);
            }

            @media (max-width: 900px) {
                width: calc(50% - 0.5rem);
            }
        }

        &.size-nano {
            @media (max-width:1300px) {
                width: calc(33.3333% - 0.6667rem);
            }

            @media (max-width: 900px) {
                width: calc(50% - 0.5rem);
            }
        }
    }
}
</style>