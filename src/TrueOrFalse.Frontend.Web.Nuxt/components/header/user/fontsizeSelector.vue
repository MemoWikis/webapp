<script lang="ts" setup></script>

<template>
    <div class="user-dropdown-font-size-selector">
        <div class="user-dropdown-label">Schriftgröße</div>
        <div class="user-dropdown-container">
            <div class="font-size-selector">
                <div>smoll</div>
                <div>medium</div>
                <div>large</div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.user-dropdown-font-size-selector {
    .user-dropdown-label {
        &:hover {
            background-color: unset;
            cursor: unset;
        }
    }

    .font-size-selector {
        display: flex;
        justify-content: space-around;
        padding: 10px 0;

        div {
            cursor: pointer;
            padding: 4px 8px;
            border-radius: 8px;

            &:hover {
                background-color: @memo-grey-lighter;
            }
        }
    }
}
</style>