<script lang="ts" setup>
import { useLoadingStore } from './loadingStore'
const loadingStore = useLoadingStore()
</script>

<template>
    <Teleport to="body" v-if="loadingStore.isLoading">
        <LoadingLogoProgress v-if="loadingStore.longLoading && loadingStore.loadingDuration > 2000" class="loading-logo" />
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
    background: white;
    border-radius: 42px;
    width: 320px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.16);
}
</style>
