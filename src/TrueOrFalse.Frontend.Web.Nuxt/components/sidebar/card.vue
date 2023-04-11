<script lang="ts" setup>

const slots = useSlots()
</script>

<template>
    <div class="sidebar-card">
        <div class="sidebar-title" v-if="slots['header']">

            <div class="overline-m no-line">
                <slot name="header"></slot>
            </div>
        </div>

        <div class="sidebar-body" v-if="slots['body']">
            <slot name="body"></slot>
        </div>

        <div class="sidebar-footer" v-if="slots['footer']">
            <slot name="footer"></slot>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.sidebar-card {
    padding-left: 30px;
    font-family: 'Open Sans';
    padding-bottom: 10px;

    a {
        color: @memo-grey-dark;

        &:hover {
            color: @memo-blue-link;
        }
    }

    .sidebar-title {
        font-size: 12px;
        text-transform: uppercase;
        font-weight: 600;
        letter-spacing: 0.25px;
    }

    .sidebar-body {
        color: @memo-grey-dark;
        padding-bottom: 25px;

        .sidebar-card-preheader {
            font-size: 12px;
            padding-bottom: 10px;
        }

        .sidebar-card-header {
            font-size: 18px;
            padding-bottom: 15px;
            font-weight: 600;
        }

        .sidebar-card-content {
            font-size: 12px;
        }
    }

    .sidebar-footer {
        padding-top: 5px;
        font-size: 12px;
        border-top: solid 1px @memo-grey-light;
        text-align: right;

        a {
            color: @memo-grey-dark;
        }
    }
}
</style>