<script lang="ts" setup>

const route = useRoute()
enum PagePath {
    Main = '/',
    Wikis = '/wikis',
    Learning = '/learning'
}

const currentPage = ref<PagePath>(PagePath.Main)

watch(() => route.path, () => {
    switch (route.path) {
        case PagePath.Wikis:
            currentPage.value = PagePath.Wikis
            break
        case PagePath.Learning:
            currentPage.value = PagePath.Learning
            break
        default:
            currentPage.value = PagePath.Main
            break
    }
})
const { isMobile } = useDevice()
</script>

<template>
    <div id="WelcomeHeader">
        <div class="container">
            <div class="welcome-nav">

                <div class="nav-item" :class="{ active: currentPage === PagePath.Main }">
                    <NuxtLink to="/">
                        Wissensmanagement
                    </NuxtLink>
                </div>

                <div class="nav-item" :class="{ active: currentPage === PagePath.Wikis }">
                    <NuxtLink to="/wikis">
                        Wikis
                    </NuxtLink>
                </div>

                <div class="nav-item" :class="{ active: currentPage === PagePath.Learning }">
                    <NuxtLink to="/learning">
                        Lernen
                    </NuxtLink>
                </div>

            </div>
            <div class="nav-item">

                <NuxtLink to="https://github.com/MemoWikis/webapp" external>
                    <template v-if="!isMobile">OpenSource</template>
                    <font-awesome-icon :icon="['fab', 'github']" class="icon" />
                </NuxtLink>

            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#WelcomeHeader {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 48px;
    background-color: white;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.16);
    position: sticky;
    top: 0;
    z-index: 1000;

    .container {
        display: flex;
        justify-content: space-between;
        align-items: center;

        .welcome-nav {
            flex: 2;
            width: 100%;

            display: flex;
            justify-content: start;
        }
    }

    .nav-item {
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 0 1rem;
        font-size: 1.5rem;
        font-weight: 500;
        color: @memo-grey-darker;
        cursor: pointer;

        &.active {
            color: @memo-blue;
            font-weight: 600;
        }

        a {
            color: inherit;
            text-decoration: none;
        }



        .icon {
            margin-left: 0.5rem;
        }

        &:hover {
            // color: @memo-grey-darker;
            filter: brightness(0.95);
        }

        &:active {
            color: @memo-grey-darker;
            // border: solid 2px @memo-grey-dark;
            filter: brightness(0.85);
        }
    }
}

div {
    display: flex;
    justify-content: space-around;
}
</style>
