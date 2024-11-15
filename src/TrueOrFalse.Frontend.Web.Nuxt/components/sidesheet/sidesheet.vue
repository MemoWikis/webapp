<script lang="ts" setup>

import { debounce } from 'underscore'
const windowWidth = ref(0)

const resize = () => {
    if (window) {
        windowWidth.value = window.innerWidth
    }
}


const handleWidth = (newWidth: number) => {
    if (newWidth < 901) {
        hidden.value = true
        collapse.value = true
    }
    else if (newWidth < 1701 && newWidth > 900) {
        hidden.value = false
        collapse.value = true
    } else {
        hidden.value = false

        collapse.value = false
    }
}

onMounted(() => {
    if (window) {
        windowWidth.value = window.innerWidth
        window.addEventListener('resize', debounce(resize, 20))
        handleWidth(windowWidth.value)
    }

})

const collapse = ref(false)
const hidden = ref(false)


watch(windowWidth, (oldWidth, newWidth) => {

    if (newWidth) {
        handleWidth(newWidth)
    }


}, { immediate: true })

</script>
<template>
    <div id="SideSheet" :class="{ 'collapsed': collapse, 'hide': hidden }">
        <div class="sidesheet">
            <slot></slot>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#SideSheet {
    // height: calc(100vh - 60px);
    height: 100vh;
    background: @memo-grey-lighter;
    width: 400px;
    position: fixed;
    z-index: 2000;
    transition: width 0.3s ease-in-out;

    &.collapsed {
        width: 100px;
    }

    &.hide {
        width: 0px;
    }
}
</style>