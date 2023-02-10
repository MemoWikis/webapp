<script lang="ts" setup>
import { Page } from '~~/components/shared/pageEnum'
const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', Page.Register)
})
</script>

<template>

    <div class="container main-page">
        <div class="row register-container">
            <div class="col-lg-9 col-md-12 container">
                <LazyUserRegister />
            </div>
            <div id="Sidebar" class="col-lg-3 hidden-md hidden-sm hidden-xs container">
                <div id="SidebarDivider"></div>
            </div>
        </div>

    </div>
</template>

<style lang="less" scoped>
@import '~~/assets/shared/register.less';

.register-container {
    display: flex;
}

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