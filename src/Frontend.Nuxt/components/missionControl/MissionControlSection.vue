<script setup lang="ts">
defineProps({
    title: {
        type: String,
        default: '',
    }
})
</script>

<template>
    <div class="mission-control-section">
        <div class="section-header" v-if="title">
            <h2 class="section-title">{{ title }}</h2>
            <div class="section-actions" v-if="$slots.actions">
                <slot name="actions"></slot>
            </div>
        </div>
        <div class="section-content">
            <slot></slot>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.mission-control-section {
    background-color: @memo-grey-lightest;
    border-radius: 8px;
    margin-bottom: 24px;
    overflow: hidden;
    // border: solid 1px @memo-grey-lightest;

    .section-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 16px 20px;

        .section-title {
            font-size: 18px;
            font-weight: 600;
            color: #333;
            margin: 0;
        }

        .section-actions {
            display: flex;
            align-items: center;
            gap: 8px;
        }
    }

    .section-content {
        padding: 16px 20px;
        padding-top: 0;
    }
}
</style>