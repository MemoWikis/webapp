<script lang="ts" setup>
import { useLoadingStore } from './loadingStore'
const loadingStore = useLoadingStore()
</script>

<template>
    <Teleport to="body" v-if="loadingStore.isLoading">
        <LoadingLogoProgress v-if="loadingStore.longLoading && loadingStore.loadingDuration > loadingStore.loadingBarThreshold" class="loading-logo bg-w" />
        <!-- <LoadingLogo v-else class="loading-logo" /> -->
        <LoadingSpinner v-else class="loading-logo" />
    </Teleport>


</template>

<style lang="less" scoped>
.loading-logo {
    position: fixed;
    margin-top: 300px;
    left: calc(50% - 100px);
    text-align: center;
    z-index: 99999;
    padding: 4rem 2rem;
    border-radius: 42px;
    width: 260px;

    &.bg-w {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.16);
        background: rgba(255, 255, 255, 0.95);
    }
}
</style>
