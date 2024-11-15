<script lang="ts" setup>

</script>

<template>
    <div class="sidesheet-section">
        <div class="header">
            <slot name="header"></slot>
        </div>
        <div class="content">
            <slot name="content"></slot>
        </div>
        <div class="slot">
            <slot name="footer"></slot>
        </div>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
@memo-grey-lightest: #f9f9f9;

.sidesheet-section {
    padding-bottom: 20px;

    .header,
    .content,
    .slot {
        padding: 0 10px;
    }

    .header {
        :slotted(h4) {
            color: @memo-grey-darker;
        }

        :slotted(.header-title) {
            padding: 8px 0p;
        }
    }

    .content {
        :slotted(a) {
            display: block;
            text-decoration: none;
            color: @memo-grey-dark;

            .link {
                border-radius: 24px;
                padding: 8px 16px;
                background: @memo-grey-lightest;
                color: @memo-grey-dark;
                text-overflow: ellipsis;
                overflow: hidden;
                white-space: nowrap;

                &:hover {
                    filter: brightness(0.95);
                }

                &:active {
                    filter: brightness(0.9);
                }
            }
        }




    }
}
</style>