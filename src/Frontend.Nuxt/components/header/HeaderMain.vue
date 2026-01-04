<script lang="ts" setup>
import { VueElement } from 'vue'
import { useUserStore } from '../user/userStore'
import { QuestionItem, SearchType, PageItem, UserItem } from '~~/components/search/searchHelper'
import { SiteType } from '../shared/siteEnum'
import { BreadcrumbItem } from './breadcrumbItems'
import { useSideSheetStore } from '../sideSheet/sideSheetStore'

interface Props {
    site: SiteType
    questionPageData?: {
        primaryPageName: string
        primaryPageUrl: string
        title: string
    }
    breadcrumbItems?: BreadcrumbItem[]
}
const props = defineProps<Props>()

const { t } = useI18n()

const userStore = useUserStore()
const sideSheetStore = useSideSheetStore()

const showSearch = ref(false)

const openUrl = async (val: PageItem | QuestionItem | UserItem) => {
    if (isMobile || window?.innerWidth < 480)
        showSearch.value = false
    return await navigateTo(val.url)
}

const showRegisterButton = ref(false)
const handleScroll = () => {
    showSearch.value = false

    var scrollTop = document.documentElement.scrollTop
    if (scrollTop > 59)
        showRegisterButton.value = true
    else
        showRegisterButton.value = false
}

const handleResize = () => {
    if (showSearch.value)
        return
    if (window.innerWidth < 769) {
        showSearch.value = false
    }
}

const { isDesktopOrTablet, isMobile } = useDevice()
const distance = computed(() => {
    return userStore.isLoggedIn ? 24 : 6
})

onBeforeMount(() => {
    if (isMobile)
        showSearch.value = false
})
const headerContainer = ref<VueElement>()
const headerExtras = ref<VueElement>()

onMounted(async () => {
    if (!userStore.isLoggedIn || window?.innerWidth < 769 || isMobile) {
        showSearch.value = false
    }
    if (typeof window != "undefined") {
        window.addEventListener('resize', handleResize)
        window.addEventListener('scroll', handleScroll)
    }
})

const partialLeft = ref()
const navOptions = ref()

const { $vfm } = useNuxtApp()
const { openedModals } = $vfm
const modalIsOpen = ref(false)

watch(openedModals, (val) => {
    if (val.length > 0)
        modalIsOpen.value = true
    else
        modalIsOpen.value = false
}, { deep: true, immediate: true })

const hidePartial = computed(() => {
    if (typeof window != "undefined" && window.scrollY > 59)
        return false
    else if (userStore.isLoggedIn)
        return false
    else return true
})


onMounted(() => {
    if (import.meta.client) {
        handleScroll()
    }
})

const { sideSheetOpen } = useSideSheetState()

</script>

<template>
    <div id="Navigation">
        <div class="sidesheet-button" @click="sideSheetStore.showSideSheet = !sideSheetStore.showSideSheet">
            <font-awesome-layers>
                <font-awesome-icon :icon="['fas', 'bars']" />
                <ClientOnly>
                    <font-awesome-icon v-if="sideSheetStore.showSideSheet" :icon="['fas', 'caret-left']"
                        transform="right-2" class="angle-bg" />
                    <font-awesome-icon v-if="sideSheetStore.showSideSheet" :icon="['fas', 'angle-left']"
                        transform="right-5" class="animate-grow" />
                </ClientOnly>
            </font-awesome-layers>
        </div>

        <div class="nav-container" :class="{ 'sidesheet-open': sideSheetOpen }">
            <div class="header-container" ref="headerContainer">

                <div class="main-container" :class="{ 'logged-in': userStore.isLoggedIn, 's-open': showSearch }">
                    <div class="partial start" :class="{ 'search-open': showSearch, 'modal-is-open': modalIsOpen }"
                        ref="partialLeft">
                        <HeaderBreadcrumb :site="props.site" :show-search="showSearch"
                            :question-page-data="props.questionPageData"
                            :custom-breadcrumb-items="props.breadcrumbItems" :partial-left="partialLeft" />
                    </div>
                    <ClientOnly>
                        <div class="partial end" ref="headerExtras" :class="{ 'hide-partial': hidePartial }">
                            <div class="StickySearchContainer" v-if="userStore.isLoggedIn"
                                :class="{ 'showSearch': showSearch }">
                                <div class="search-button" :class="{ 'showSearch': showSearch }"
                                    @click="showSearch = !showSearch">
                                    <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                    <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                                </div>
                                <div class="StickySearch">
                                    <Search :search-type="SearchType.all" :show-search="showSearch"
                                        v-on:select-item="openUrl" placement="bottom-end" :main-search="true"
                                        :distance="distance" />
                                </div>
                            </div>

                            <HeaderUserDropdown
                                v-if="userStore.isLoggedIn && (isDesktopOrTablet || isMobile && !showSearch)" />

                            <div v-if="!userStore.isLoggedIn" class="nav-options-container" ref="navOptions"
                                :class="{ 'hide-nav': !showRegisterButton, 'login-modal-is-open': modalIsOpen }">
                                <div class="StickySearchContainer"
                                    :class="{ 'showSearch': showSearch, 'has-register-btn': isDesktopOrTablet }">
                                    <div class="search-button" :class="{ 'showSearch': showSearch }"
                                        @click="showSearch = !showSearch">
                                        <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                        <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                                    </div>
                                    <div class="StickySearch">
                                        <Search :search-type="SearchType.all" :show-search="showSearch"
                                            v-on:select-item="openUrl" v-on:navigate-to-url="openUrl"
                                            placement="bottom-end" />
                                    </div>
                                </div>
                                <div class="login-btn" @click="userStore.openLoginModal()">
                                    <font-awesome-icon icon="fa-solid fa-right-to-bracket" />
                                </div>
                            </div>
                        </div>
                        <template #fallback>
                            <div class="partial end" ref="headerExtras">
                                <div class="StickySearchContainer" v-if="userStore.isLoggedIn"
                                    :class="{ 'showSearch': showSearch }">
                                    <div class="search-button" :class="{ 'showSearch': showSearch }"
                                        @click="showSearch = !showSearch">
                                        <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                        <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                                    </div>
                                    <div class="StickySearch">
                                        <Search :search-type="SearchType.all" :show-search="showSearch"
                                            v-on:select-item="openUrl" placement="bottom-end" :main-search="true"
                                            :distance="distance" />
                                    </div>
                                </div>

                                <HeaderUserDropdown
                                    v-if="userStore.isLoggedIn && (isDesktopOrTablet || isMobile && !showSearch)" />

                                <div v-if="!userStore.isLoggedIn" class="nav-options-container" ref="navOptions"
                                    :class="{ 'hide-nav': !showRegisterButton, 'login-modal-is-open': modalIsOpen }">
                                    <div class="StickySearchContainer"
                                        :class="{ 'showSearch': showSearch, 'has-register-btn': isDesktopOrTablet }">
                                        <div class="search-button" :class="{ 'showSearch': showSearch }"
                                            @click="showSearch = !showSearch">
                                            <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                            <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                                        </div>
                                        <div class="StickySearch">
                                            <Search :search-type="SearchType.all" :show-search="showSearch"
                                                v-on:select-item="openUrl" v-on:navigate-to-url="openUrl"
                                                placement="bottom-end" />
                                        </div>
                                    </div>
                                    <div class="login-btn" @click="userStore.openLoginModal()">
                                        <font-awesome-icon icon="fa-solid fa-right-to-bracket" />
                                    </div>
                                </div>
                            </div>
                        </template>
                    </ClientOnly>
                </div>

                <ClientOnly>
                    <div v-if="isDesktopOrTablet && !userStore.isLoggedIn" class="register-btn-container"
                        :class="{ 'hide-partial': hidePartial, 'hide-nav': !showRegisterButton, 'login-modal-is-open': modalIsOpen, 's-open': showSearch }">
                        <div navigate class="btn memo-button register-btn">
                            <NuxtLink :to="`/${t('url.register')}`">
                                {{ t('label.register') }}
                            </NuxtLink>
                        </div>
                    </div>
                    <template #fallback>
                        <div v-if="isDesktopOrTablet && !userStore.isLoggedIn" class="register-btn-container"
                            :class="{ 'hide-partial': hidePartial, 'hide-nav': !showRegisterButton }">
                            <div navigate class="btn memo-button register-btn">
                                <NuxtLink :to="`/${t('url.register')}`">
                                    {{ t('label.register') }}
                                </NuxtLink>
                            </div>
                        </div>
                    </template>
                </ClientOnly>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.modal-is-open {
    width: 100%;
}

.nav-options-container {
    position: fixed;
    top: 0;
    height: 47px;
    display: flex;
    justify-content: flex-end;
    align-items: center;
    opacity: 1;
    transition: opacity 0.2s ease-in-out;

    &.hide-nav {
        opacity: 0;
    }

    &.login-modal-is-open {
        position: unset;
    }
}

.StickySearchContainer {
    display: flex;
    flex-direction: row-reverse;
    flex-wrap: nowrap;
    height: 100%;
    align-items: center;
    z-index: 20;
    margin-right: 0;
    min-height: 47px;

    .search-button {
        align-items: center;
        display: flex;
        font-size: 20px;
        height: 100%;
        justify-content: center;
        position: absolute;
        transform: translateZ(0);
        transition: all .3s;
        width: 30px;
        z-index: 1050;
        cursor: pointer;
        background: white;
        border-radius: 15px;
        height: 30px;
        margin-right: 2px;
        color: @memo-grey-dark;

        &.showSearch {
            background: @memo-grey-lighter;
        }

        &:hover {
            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }

    :deep(input) {
        min-width: 0px;
        width: 0px;
        border: none;
        padding: 0;
        transition: all 0.3s;
        background: transparent;
    }

    :deep(&.showSearch) {
        padding-left: 8px;

        input {
            border: none;
            width: 100%;
            padding: 6px 40px 6px 12px;
            background: @memo-grey-lighter;
        }
    }
}

#Navigation {
    height: 47px;
    font-size: 14px;
    overflow: hidden;
    line-height: 21px;
    background-color: white;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.16);
    position: sticky;
    z-index: 99;
    white-space: nowrap;
    top: 0;
    min-height: 47px;
    width: 100%;

    display: flex;
    justify-content: center;
    align-items: center;

    .nav-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 47px;
        width: 100%;
        max-width: 1600px;

        @media (min-width: 900px) {
            padding-left: 80px;
        }

        &.sidesheet-open {
            padding-left: 410px;

            @media (max-width: 900px) {
                padding-left: 0;
            }

            @media (min-width: 1980px) {
                padding-left: clamp(80px, calc(410px - (100vw - 1980px)), 410px);
            }
        }
    }

    .container {
        height: 100%;
    }

    @media (max-width: @screen-sm) {
        .container {
            margin-left: 0;
            margin-right: 0;
            width: 100%;
        }
    }

    .row {
        height: 100%;
    }


    .main-container {
        padding: 0 10px;
        width: calc(75% - 1rem);
        justify-content: space-between;

        &.logged-in {
            width: 100%;
        }

        &.s-open {
            @media (max-width: @screen-sm) {
                width: calc(100% - 1rem);
            }
        }
    }

    .header-container {
        width: 100%;
        gap: 0 5px;

        @media (max-width: 899px) {
            justify-content: space-between;

        }
    }

    .sidesheet-open {
        .header-container {
            gap: 0 2px;

            @media (min-width: 900px) and (max-width: 1500px) {
                gap: 0;
            }

            @media (min-width: 1980px) {
                gap: 0 5px;
            }
        }
    }

    .header-container,
    .main-container {
        display: flex;
        align-items: center;
        height: 100%;
        overflow: hidden;

        .partial {
            height: 100%;
            display: flex;
            align-items: center;
            flex-shrink: 2;
            flex-grow: 2;

            &.start {
                align-items: center;
            }

            &.end {
                justify-content: flex-end;
                min-width: 45px;
                flex-grow: 0;

                &.hide-partial {
                    min-width: 0px;
                    width: 0px;
                    max-width: 0px;
                }
            }
        }

        .login-btn {
            margin: 0 8px;
            height: 30px;
            width: 30px;
            min-width: 30px;
            display: flex;
            justify-content: center;
            align-items: center;
            cursor: pointer;
            background: white;
            border-radius: 20px;
            font-size: 20px;
            color: @memo-grey-dark;

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }
        }
    }

    .register-btn-container {
        height: 100%;
        display: flex;
        align-items: center;
        flex: 0 0 25%;
        min-width: unset;
        width: unset;
        max-width: unset;

        &:hover {
            filter: brightness(0.85)
        }

        &:active {
            filter: brightness(0.7)
        }

        .register-btn {
            color: white;
            align-items: center;
            line-height: unset !important;
            background: @memo-green;
            height: 47px !important;
            display: flex;
            justify-content: center;
            align-items: center;
            width: 100%;
            max-width: 300px;

            @media (min-width: 900px) {
                display: flex;
                justify-content: center;
                align-items: center;
            }
        }

        &.hide-partial {
            display: none;
            min-width: 0px;
            width: 0px;
            max-width: 0px;
        }

        &.hide-nav {
            opacity: 0;
        }

        &.login-modal-is-open {
            position: unset;
        }

        &.s-open {
            @media (max-width: @screen-sm) {
                display: none;
                min-width: 0px;
                width: 0px;
                max-width: 0px;
            }
        }
    }

    .sidesheet-button {
        border-right: 1px solid @memo-grey-light;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;
        padding-left: 16px;
        padding-right: 16px;
        width: 48px;
        user-select: none;
        background: white;
        height: 47px;
        margin-right: 2px;

        @media (min-width: 900px) {
            position: absolute;
            left: 0px;
            z-index: 2000;
            width: 80px;
            border-right: none;
        }

        .angle-bg {
            color: white;
            font-size: 24px;
        }

        &:hover {
            filter: brightness(0.95);
        }

        &:active {
            filter: brightness(0.9);
        }
    }
}

#StickySearch {
    width: 100%;
    display: flex;
    flex-direction: row-reverse;
    height: 100%;
    align-items: center;

    .StickySearchContainer,
    .SearchContainer {
        width: 100%;

        :deep(&input) {
            min-width: 0px;
            width: 0px;
            border: none;
            padding: 0;
            transition: all 0.3s;
            background: transparent;
        }

        input {
            min-width: 0px;
            width: 0px;
            border: none;
            padding: 0;
            transition: all 0.3s;
            background: transparent;
        }
    }

    .search-button {
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        transition: all 0.3s;
        font-size: 20px;
        position: absolute;
        z-index: 1050;
        width: 34px;
        transform: translateZ(0);

        svg.fa-magnifying-glass {
            color: white;
        }

        svg.fa-xmark,
        svg.fa-magnifying-glass {
            transition: all 0.1s;
        }

        &:hover {
            cursor: pointer;
        }
    }
}

.animate-grow {
    animation: grow 0.15s ease-in-out;
}

@keyframes grow {
    0% {
        transform: scale(0);
        opacity: 0;
    }

    100% {
        transform: scale(1);
        opacity: 1;
    }
}
</style>