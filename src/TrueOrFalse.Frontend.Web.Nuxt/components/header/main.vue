<script lang="ts" setup>
import { VueElement } from 'vue'
import { useUserStore } from '../user/userStore'
import { QuestionItem, SearchType, TopicItem, UserItem } from '~~/components/search/searchHelper'
import { Page } from '../shared/pageEnum'
import { useActivityPointsStore } from '../activityPoints/activityPointsStore'
import { BreadcrumbItem } from './breadcrumbItems'

interface Props {
    page: Page
    questionPageData?: {
        primaryTopicName: string
        primaryTopicUrl: string
        title: string
    }
    breadcrumbItems?: BreadcrumbItem[]
}
const props = defineProps<Props>()

const showSearch = ref(false)

async function openUrl(val: TopicItem | QuestionItem | UserItem) {
    if (isMobile || window?.innerWidth < 480)
        showSearch.value = false
    return await navigateTo(val.url)
}
const userStore = useUserStore()

const showRegisterButton = ref(false)
function handleScroll() {
    showSearch.value = false

    var scrollTop = document.documentElement.scrollTop
    if (scrollTop > 59)
        showRegisterButton.value = true
    else
        showRegisterButton.value = false
}

function handleResize() {
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
watch(() => openedModals, (val) => {
    if (val.length > 0)
        modalIsOpen.value = true
    else modalIsOpen.value = false
}, { deep: true, immediate: true })

const hidePartial = computed(() => {
    if (typeof window != "undefined" && window.scrollY > 59)
        return false
    else if (userStore.isLoggedIn)
        return false
    else return true
})

</script>

<template>
    <div id="Navigation">
        <div class="container">
            <div class="row">
                <div class="header-container col-xs-12" ref="headerContainer">
                    <div class="partial start" :class="{ 'search-open': showSearch, 'modal-is-open': modalIsOpen }"
                        ref="partialLeft">
                        <HeaderBreadcrumb :page="props.page" :show-search="showSearch"
                            :question-page-data="props.questionPageData"
                            :custom-breadcrumb-items="props.breadcrumbItems" :partial-left="partialLeft" />
                    </div>
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

                        <HeaderUserDropdown v-if="userStore.isLoggedIn" />

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
                            <div class="register-btn-container hidden-xs hidden-sm" v-if="isDesktopOrTablet">
                                <div navigate class="btn memo-button register-btn">
                                    <NuxtLink to="/Registrieren">
                                        Kostenlos registrieren!
                                    </NuxtLink>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
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
    height: 45px;
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

    &.showSearch {
        :deep(input) {
            border: 1px solid #ccc;
            width: 100%;
            padding: 6px 40px 6px 12px;
            background: white;
            box-shadow: -10px 0px 10px 0px rgba(255, 255, 255, 1);
        }

        padding-left: 8px;
    }
}


#Navigation {
    width: 100%;
    height: 45px;
    font-size: 14px;
    background-color: white;
    line-height: 21px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.16);
    position: sticky;
    z-index: 99;
    white-space: nowrap;
    top: 0;
    min-height: 45px;

    .container {
        height: 100%;
    }

    .row {
        height: 100%;
    }

    .header-container {
        display: flex;
        justify-content: space-between;
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
                    width: 0p;
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

        .register-btn-container {
            background: @memo-green;
            height: 100%;
            display: flex;
            align-items: center;

            &:hover {
                filter: brightness(0.85)
            }

            &:active {
                filter: brightness(0.7)
            }

            .register-btn {
                color: white;
                height: 100% !important;
                align-items: center;
                line-height: unset !important;
            }

            @media (min-width: 1200px) {
                margin-right: 33px;
            }
        }
    }


}

:global(#StickySearch,
    #HeaderSearch) {
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

        :deep(&.showSearch) {
            input {
                border: 1px solid #ccc;
                width: 100%;
                padding: 6px 40px 6px 12px;
                background: white;
            }
        }

        &.showSearch {
            input {
                border: 1px solid #ccc;
                width: 100%;
                padding: 6px 40px 6px 12px;
                background: white;
            }
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

            svg.fa-xmark,
            svg.fa-magnifying-glass {
                color: @memo-green;
            }
        }
    }
}
</style>