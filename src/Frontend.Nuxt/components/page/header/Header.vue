<script lang="ts" setup>

</script>

<template>
    <div class="row">
        <div id="PageHeader">
            <PageHeaderImage />
            <PageHeaderTitle />
            <PageHeaderOptions />
        </div>

        <PageTabs />
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#PageHeader {
    margin-top: 25px;
    display: flex;
    padding: 0 10px;
}
</style>