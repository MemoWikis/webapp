<script setup lang="ts">
const { isMobile } = useDevice()
const { $vfm } = useNuxtApp()
const { openedModals } = $vfm

const { sideSheetOpen } = useSideSheetState()

</script>

<template>
    <div
        class="layout-wrapper"
        :class="{
            'sidesheet-open': sideSheetOpen,
            'open-modal': openedModals.length > 0,
            'mobile-headings': isMobile,
        }">
        <div class="content-area">
            <slot />
        </div>
    </div>
</template>

<style lang="less" scoped>
.layout-wrapper {
    min-height: 86vh;
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.layout-wrapper {
    height: 100%;
    transition: all 0.3s ease-in-out;
    display: flex;
    justify-content: center;

    &.window-loading {
        padding-left: 0px;
    }

    &.modal-is-open {
        min-height: unset;
    }

    .content-area {
        height: 100%;
    }

    .content-area,
    .footer-area {
        display: flex;
        justify-content: center;
        flex-wrap: nowrap;

        gap: 0 1rem;
        width: 100%;
        padding: 0 10px;
        max-width: 1600px;

        @media (min-width: 901px) {
            padding-left: 90px;
        }
    }

    &.sidesheet-open {

        .content-area,
        .footer-area {
            padding-left: 420px;
            width: 100%;
        }

        @media (max-width: 1500px) {
            width: calc(100vw - 15px);

            .content-area,
            .footer-area {
                padding-left: 420px;
                width: 100%;
            }
        }

        @media (max-width: 1209px) {

            #Sidebar {
                display: none;
            }
        }

        @media (min-width: 1980px) {

            .content-area,
            .footer-area {
                padding-left: clamp(90px, calc(420px - (100vw - 1980px)), 420px);
            }
        }
    }
}

.main-content {
    padding-top: 30px;
    padding-bottom: 60px;
    width: 100%;
}
</style>