<script lang="ts" setup>
const { isDesktop } = useDevice()
</script>

<template>
    <div id="Sidebar" class="col-lg-3 hidden-md hidden-sm hidden-xs container" v-if="isDesktop">
        <div id="SidebarDivider"></div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#Sidebar {
    display: flex;
    align-items: stretch;
    flex-grow: 1;

    #SidebarDivider {
        margin-top: 20px;
        margin-bottom: 20px;
        border-left: 1px solid @memo-grey-light;
        top: 0;
        flex-grow: 1;
    }
}
</style>