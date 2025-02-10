<script lang="ts" setup>
import { useLoadingStore } from './loadingStore'

const wavePath = ref<SVGPathElement>()
const loadingStore = useLoadingStore()

watch(() => loadingStore.isLoading, async (active) => {
    if (active && wavePath.value) {
        const length = wavePath.value.getTotalLength()
        nextTick()

        wavePath.value.style.strokeDasharray = String(length)
        wavePath.value.style.strokeDashoffset = String(length)

        wavePath.value.classList.add('animate-wave')
    } else if (wavePath.value) {
        wavePath.value.classList.remove('animate-wave')

    }
})

</script>

<template>
    <Teleport to="body" v-if="loadingStore.isLoading">
        <LoadingLogoProgress v-if="loadingStore.longLoading && loadingStore.loadingDuration > 2000" class="loading-logo" />
        <LoadingLogo v-else class="loading-logo" />
    </Teleport>


</template>

<style lang="less" scoped>
.loading-logo {
    position: fixed;
    margin-top: 300px;
    left: calc(50% - 100px);
    text-align: center;
    z-index: 99999;
}
</style>
