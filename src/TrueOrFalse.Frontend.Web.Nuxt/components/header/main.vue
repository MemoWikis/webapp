<script lang="ts" setup>
import { VueElement } from 'vue'
import { useUserStore } from '../user/userStore'
import { ImageFormat } from '../image/imageFormatEnum'
import { SearchType } from '~~/components/search/searchHelper'
import { Page } from '../shared/pageEnum'
import { useActivityPointsStore } from '../activityPoints/activityPointsStore'
import { BreadcrumbItem } from './breadcrumbItems'

const activityPointsStore = useActivityPointsStore()

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

async function openUrl(val: any) {
    if (isMobile || window?.innerWidth < 480)
        showSearch.value = false
    return navigateTo(val.Url)
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
    if (!userStore.isLoggedIn || window.innerWidth < 769 || isMobile) {
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

</script>

<template>
    <div id="Navigation">
        <div class="container">
            <div class="row">
                <div class="header-container col-xs-12" ref="headerContainer">
                    <div class="partial start" :class="{ 'search-open': showSearch, 'modal-is-open': modalIsOpen }"
                        ref="partialLeft">
                        <HeaderBreadcrumb :page="props.page" :show-search="showSearch"
                            :question-page-data="props.questionPageData" :custom-breadcrumb-items="props.breadcrumbItems"
                            :partial-left="partialLeft" />
                    </div>
                    <div class="partial end" ref="headerExtras">
                        <div class="StickySearchContainer" v-if="userStore.isLoggedIn"
                            :class="{ 'showSearch': showSearch }">
                            <div class="search-button" :class="{ 'showSearch': showSearch }"
                                @click="showSearch = !showSearch">
                                <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                            </div>
                            <div class="StickySearch">
                                <Search :search-type="SearchType.All" :show-search="showSearch" v-on:select-item="openUrl"
                                    placement="bottom-end" :main-search="true" :distance="distance" />
                            </div>
                        </div>
                        <VDropdown :distance="6" v-if="userStore.isLoggedIn">
                            <div class="header-btn">
                                <Image :src="userStore.imgUrl" :format="ImageFormat.Author" class="header-author-icon"
                                    :alt="`${userStore.name}'s profile picture'`" />
                                <div class="header-user-name">
                                    {{ userStore.name }}
                                </div>
                                <div class="user-dropdown-chevron">
                                    <font-awesome-icon icon="fa-solid fa-chevron-down" />
                                </div>
                            </div>
                            <template #popper="{ hide }">
                                <div class="user-dropdown">
                                    <div class="user-dropdown-info">
                                        <div class="user-dropdown-label">Deine Lernpunkte</div>
                                        <div class="user-dropdown-container level-info">
                                            <div class="primary-info">
                                                Mit {{ activityPointsStore.points }} <b>Lernpunkten</b> <br />
                                                bist du in <b>Level {{ activityPointsStore.level }}</b>.
                                            </div>
                                            <div class="progress-bar-container">
                                                <div class="p-bar">
                                                    <div class="p-bar-a"
                                                        :style="`left: -${100 - activityPointsStore.activityPointsPercentageOfNextLevel}%`">
                                                    </div>
                                                    <div class="p-bar-label-grey"
                                                        v-if="activityPointsStore.activityPointsPercentageOfNextLevel < 30">
                                                        {{ activityPointsStore.activityPointsPercentageOfNextLevel }}%
                                                    </div>
                                                    <div class="p-bar-label" v-else
                                                        :style="`width: ${activityPointsStore.activityPointsPercentageOfNextLevel}%`">
                                                        {{ activityPointsStore.activityPointsPercentageOfNextLevel }}%
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="secondary-info">
                                                Noch {{ activityPointsStore.activityPointsTillNextLevel }} Punkte <br />
                                                bis Level {{ activityPointsStore.level + 1 }}
                                            </div>
                                        </div>

                                    </div>
                                    <div class="divider"></div>
                                    <div class="user-dropdown-social">
                                        <NuxtLink :to="`/Nutzer/${encodeURI(userStore.name)}/${userStore.id}`"
                                            @click="hide()">
                                            <div class="user-dropdown-label">Deine Profilseite</div>
                                        </NuxtLink>
                                    </div>
                                    <div class="divider"></div>

                                    <div class="user-dropdown-managment">
                                        <NuxtLink @click="hide()" :to="`/Nutzer/Einstellungen`">
                                            <div class="user-dropdown-label">Konto-Einstellungen</div>
                                        </NuxtLink>

                                        <LazyNuxtLink to="/Maintenance" @click="hide()">
                                            <div class="user-dropdown-label" @click="hide()" v-if="userStore.isAdmin">
                                                Administrativ
                                            </div>
                                        </LazyNuxtLink>
                                        <div class="user-dropdown-label" @click="userStore.logout(), hide()">
                                            Ausloggen
                                        </div>
                                    </div>
                                </div>
                            </template>
                        </VDropdown>

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
                                    <Search :search-type="SearchType.All" :show-search="showSearch"
                                        v-on:select-item="openUrl" v-on:navigate-to-url="openUrl" placement="bottom-end" />
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

.header-author-icon {
    height: 32px;
    width: 32px;
    margin-left: -8px;
    margin-right: 4px;
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

    .v-popper {
        height: 100%;
        padding: 4px 0;
    }

    .header-btn {
        cursor: pointer;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        background: white;
        padding: 4px 12px;
        border-radius: 24px;
        transition: filter 0.1s;

        &:hover {
            filter: brightness(0.95)
        }


        &:active {
            filter: brightness(0.85)
        }

        .header-user-name {
            font-weight: 600;
            padding: 0 4px;
        }

        .unread-msg-badge-container {
            position: relative;
            width: 100%;

            .unread-msg-badge {
                background: @memo-wuwi-red;
                height: 12px;
                width: 12px;
                border-radius: 12px;
                position: absolute;
                border: 2px solid white;
                top: -2px;
                right: -2px;
            }
        }
    }

    .v-popper--shown {

        .user-dropdown-chevron {
            transform: rotate3d(1, 0, 0, 180deg);
        }
    }


}

.user-dropdown {
    .user-dropdown-label {
        padding: 10px 25px;

        &:hover {
            background-color: @memo-grey-lighter;
            cursor: pointer;
        }

        &.has-badge {
            display: flex;
            align-items: center;

            .counter-badge {
                height: 16px;
                border-radius: 16px;
                padding: 0 5px;
                display: flex;
                justify-content: center;
                align-items: center;
                background: @memo-grey-light;
                color: @memo-grey-dark;
                font-size: 10px;
                font-weight: 700;
                margin-left: 8px;

                &.red-badge {
                    background: @memo-wuwi-red;
                    color: white;
                }
            }
        }


    }

    .user-dropdown-container {
        padding: 10px 25px;
    }

    a {
        text-decoration: none;
    }

    color: @memo-grey-darker;
}

.user-dropdown-info {

    .user-dropdown-label {
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: 600;
        padding-bottom: 0px;

        &:hover {
            background-color: unset;
            cursor: default;
        }
    }

    .progress-bar-container {
        background: @memo-grey-light;
        border-radius: 25px;
        height: 25px;
        width: 100%;
        margin: 6px 0;

        .p-bar {
            position: relative;
            width: 100%;
            height: 25px;
            overflow: hidden;
            border-radius: 25px;

            .p-bar-a {
                position: absolute;
                display: flex;
                justify-content: center;
                align-items: center;
                border-radius: 25px;
                height: 25px;
                background: @memo-green;
                width: 100%;
            }

            .p-bar-label,
            .p-bar-label-grey {
                position: absolute;
                display: flex;
                justify-content: center;
                align-items: center;
                height: 25px;
                font-weight: 600;
            }

            .p-bar-label {
                color: white;
            }

            .p-bar-label-grey {
                color: @memo-grey-darker;
                width: 100%;
            }
        }


    }

    .primary-info,
    .secondary-info {
        font-size: 12px;
        text-align: center;
        padding: 6px 0;

        b {
            font-weight: 600;
        }
    }

    .secondary-info {
        color: @memo-grey-dark;
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