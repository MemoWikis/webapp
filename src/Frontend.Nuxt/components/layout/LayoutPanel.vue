<script setup lang="ts">
const props = defineProps({
    title: {
        type: String,
        default: '',
    },
    collapsable: {
        type: Boolean,
        default: true
    },
})

const showContent = ref(true)
const { isMobile } = useDevice()
const toggleContent = () => {
    if (props.collapsable === false) return
    showContent.value = !showContent.value
}
</script>

<template>
    <div class="layout-panel">
        <div class="panel-header" v-if="title" @click="toggleContent" :class="{ 'no-pointer': !collapsable }">
            <h2 class="panel-title">{{ title }}</h2>
            <div class="panel-actions">
                <div class="collapse-toggle" :class="{ 'is-mobile': isMobile }">
                    <font-awesome-icon :icon="['fas', 'chevron-up']" v-if="showContent" />
                    <font-awesome-icon :icon="['fas', 'chevron-down']" v-else />
                </div>
                <slot name="actions"></slot>
            </div>
        </div>

        <div class="panel-description" v-if="$slots.description">
            <slot name="description"></slot>
        </div>
        <Transition name="fade" mode="out-in">
            <div class="panel-content" v-if="showContent">
                <slot></slot>
            </div>
        </Transition>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-panel {
    background-color: @memo-grey-lightest;
    border-radius: 8px;
    margin-bottom: 24px;
    overflow: hidden;
    width: 100%;
    max-width: calc(100vw - 40px);

    .panel-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 16px 20px;
        user-select: none;
        cursor: pointer;



        &:hover {
            background-color: @memo-grey-lighter;
        }

        &.no-pointer {
            cursor: default;

            &:hover {
                background-color: @memo-grey-lightest;
            }
        }

        .panel-title {
            font-size: 1.8rem;
            font-weight: 600;
            color: @memo-grey-darker;
            margin: 0;
        }

        .panel-actions {
            display: flex;
            align-items: center;
            gap: 8px;

            .collapse-toggle {
                cursor: pointer;
                color: @memo-grey-dark;
                opacity: 0;
                font-size: 2rem;
                transition: opacity 0.2s ease-in-out;

                &.is-mobile {
                    opacity: 1;
                }
            }
        }

        &:hover {
            .panel-actions {
                .collapse-toggle {
                    opacity: 1;

                }
            }
        }
    }

    .panel-description {
        padding: 0 20px 16px;
        color: @memo-grey-dark;
        font-size: 1.4rem;
        line-height: 1.6;
    }

    .panel-content {
        padding: 16px 20px;
        padding-top: 0;
        width: 100%;
        display: flex;
        flex-wrap: wrap;
        gap: 1rem;
    }
}
</style>
