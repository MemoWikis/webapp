<script setup lang="ts">

interface Props {
    title?: string
    size?: LayoutContentSize
    defaultOpen?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    title: '',
    size: LayoutContentSize.Large,
    defaultOpen: false
})

const isOpen = ref(props.defaultOpen)

const toggleCollapse = () => {
    isOpen.value = !isOpen.value
}

</script>

<template>
    <div class="layout-collapse" :class="`size-${size}`">
        <div class="collapse-header" @click="toggleCollapse" :class="{ 'is-open': isOpen }">
            <slot name="header">
                <h3 class="collapse-title">{{ title }}</h3>
            </slot>
            <div class="chevron-container">
                <font-awesome-icon icon="fa-solid fa-chevron-up" v-if="isOpen" class="pointer" />
                <font-awesome-icon icon="fa-solid fa-chevron-down" v-else class="pointer" />
            </div>
        </div>

        <Transition name="fade">
            <div class="collapse-body" v-show="isOpen">
                <slot name="body"></slot>
            </div>
        </Transition>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-collapse {
    width: 100%;
    margin-top: 1rem;
    border-radius: 8px;
    overflow: hidden;
    background: white;

    &.size-flex {
        width: unset;
    }

    &.size-large {
        width: 100%;
    }

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

    .collapse-header {
        cursor: pointer;
        user-select: none;
        padding: 16px 20px;
        background: white;
        display: flex;
        justify-content: space-between;
        align-items: center;

        &.is-open {
            border-bottom: 1px solid #f0f0f0;
        }

        &:hover {
            filter: brightness(0.95);
        }

        &:active {
            filter: brightness(0.90);
        }

        .collapse-title {
            font-size: 1.6rem;
            font-weight: 600;
            color: @memo-grey-darker;
            margin: 0;
        }

        .chevron-container {
            display: flex;
            align-items: center;
            justify-content: center;
            color: @memo-grey-dark;
            font-size: 14px;
        }
    }

    .collapse-body {
        padding: 16px 20px;
        background: white;
    }
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>

<style lang="less">
.sidesheet-open {
    .layout-collapse {
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
