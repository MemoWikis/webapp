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
            <perfect-scrollbar>

                <div class="welcome-nav">

                    <NuxtLink to="/">
                        <div class="nav-item km" :class="{ active: currentPage === PagePath.Main }">
                            Wissensmanagement
                        </div>
                    </NuxtLink>
                    <NuxtLink to="/wikis">
                        <div class="nav-item w" :class="{ active: currentPage === PagePath.Wikis }">
                            Wikis
                        </div>
                    </NuxtLink>
                    <NuxtLink to="/learning">

                        <div class="nav-item l" :class="{ active: currentPage === PagePath.Learning }">
                            Lernen
                        </div>
                    </NuxtLink>

                </div>
            </perfect-scrollbar>

            <NuxtLink to="https://github.com/MemoWikis/webapp" external>

                <div class="nav-item">

                    <div v-if="!isMobile" class="opensource-label">OpenSource</div>
                    <font-awesome-icon :icon="['fab', 'github']" class="icon" />

                </div>
            </NuxtLink>

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

            max-width: calc(100% - 50px);
        }

        a {
            color: @memo-grey-darker;
            text-decoration: none;
            display: flex;
            justify-content: center;
            align-items: center;

            .nav-item {
                display: flex;
                align-items: center;
                justify-content: center;
                padding: 0.6rem 1.2rem;
                font-size: 1.6rem;
                font-weight: 500;
                color: @memo-grey-darker;
                cursor: pointer;
                background: white;
                border-radius: 4rem;
                text-decoration: none;
                display: flex;
                justify-content: center;
                align-items: center;

                &.km {
                    width: 18rem;
                    max-width: 18rem;
                }

                &.w {
                    width: 6.4rem;
                    max-width: 6.4rem;
                }

                &.l {
                    width: 6.8rem;
                    max-width: 6.8rem;
                }

                &.active {
                    color: @memo-blue;
                    font-weight: 600;
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

                .opensource-label {
                    @media screen and (max-width: 500) {
                        display: none;
                    }
                }
            }
        }

        @media screen and (max-width: 1091px) {
            width: 100%;
            margin: 0;
        }
    }


}
</style>
