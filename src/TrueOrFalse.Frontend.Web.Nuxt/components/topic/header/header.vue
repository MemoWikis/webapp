<script lang="ts" setup>

</script>

<template>
    <div class="row">
        <div id="TopicHeader" class="col-xs-12">
            <TopicHeaderImage />
            <TopicHeaderTitle />
            <TopicHeaderOptions />
        </div>

        <TopicTabs />
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#TopicHeader {
    margin-top: 25px;
    display: flex;
}
</style>