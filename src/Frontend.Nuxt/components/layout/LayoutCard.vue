<script setup lang="ts">
defineProps({
    title: {
        type: String,
        default: '',
    },
    fullWidth: {
        type: Boolean,
        default: true,
    },
    noPadding: {
        type: Boolean,
        default: false,
    },
})

</script>

<template>
    <div class="layout-card" :class="{ 'half-width': !fullWidth }">
        <div class="card-header" v-if="title">
            <h2 class="card-title">{{ title }}</h2>
            <div class="card-actions" v-if="$slots.actions">
                <slot name="actions"></slot>
            </div>
        </div>
        <div class="card-content" :class="{ 'has-title': title, 'no-padding': noPadding }">
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

    &.half-width {
        @media (min-width: 768px) {
            width: calc(50% - 0.5rem);
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

        &.has-title {
            border-radius: 0 0 8px 8px;
        }

        &.no-padding {
            padding: 0;
        }
    }
}
</style>

<style lang="less">
.sidesheet-open {
    .layout-card {
        &.half-width {
            @media (max-width:1300px) {
                width: 100%;
            }
        }
    }
}
</style>