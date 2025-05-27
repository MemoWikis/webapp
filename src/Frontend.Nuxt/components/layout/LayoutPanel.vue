<script setup lang="ts">
defineProps({
    title: {
        type: String,
        default: '',
    }
})

const showContent = ref(true)
</script>

<template>
    <div class="layout-panel">
        <div class="panel-header" v-if="title" @click="showContent = !showContent">
            <h2 class="panel-title">{{ title }}</h2>
            <div class="panel-actions" v-if="$slots.actions">
                <slot name="actions"></slot>
            </div>
        </div>

        <Transition name="fade" mode="out-in">
            <div class="panel-content" v-if="showContent">
                <slot></slot>
            </div>
        </Transition>
        <!-- <div class="panel-content">
            <slot></slot>
        </div> -->
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

        cursor: pointer;

        &:hover {
            background-color: @memo-grey-lighter;
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
        }
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
