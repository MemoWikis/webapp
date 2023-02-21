<script lang="ts" setup>
import { PropType, VueElement } from 'vue'
import { useUserStore } from '../user/userStore'
import { ImageStyle } from '../image/imageStyleEnum'
import { SearchType } from '~~/components/search/searchHelper'
import { Page } from '../shared/pageEnum'


interface Props {
    page: Page
    questionPageData?: {
        primaryTopicName: string
        primaryTopicUrl: string
        title: string
    }
}
const props = defineProps<Props>()

const showSearch = ref(false)

function openUrl(val: any) {
    navigateTo(val.Url)
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
    if (window.innerWidth < 769) {
        showSearch.value = false
    }
}

const { isDesktopOrTablet, isMobile } = useDevice()

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
    if (typeof window != undefined) {
        window.addEventListener('resize', handleResize)
        window.addEventListener('scroll', handleScroll)
    }

})

const partialSpacer = ref()
</script>

<template>
    <div id="Navigation">
        <div class="container">
            <div class="row">
                <div class="header-container col-xs-12" ref="headerContainer">

                    <div class="partial start" :class="{ 'search-open': showSearch }">
                        <HeaderBreadcrumb :header-container="headerContainer" :header-extras="headerExtras"
                            :page="props.page" :show-search="showSearch" :partial-spacer="partialSpacer"
                            :question-page-data="props.questionPageData" />
                    </div>
                    <div class="partial-spacer" ref="partialSpacer"></div>
                    <div class="partial end" ref="headerExtras">
                        <div class="StickySearchContainer" v-if="userStore.isLoggedIn"
                            :class="{ 'showSearch': showSearch }">
                            <div class="searchButton" :class="{ 'showSearch': showSearch }"
                                @click="showSearch = !showSearch">
                                <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                            </div>
                            <div class="StickySearch">
                                <Search :search-type="SearchType.All" :show-search="showSearch"
                                    v-on:select-item="openUrl" placement="bottom-end" />
                            </div>
                        </div>
                        <VDropdown :distance="6" v-if="userStore.isLoggedIn">
                            <div class="header-btn">
                                <Image :url="userStore.imgUrl" :style="ImageStyle.Author" class="header-author-icon" />
                                <div class="header-user-name">
                                    {{ userStore.name }}
                                </div>
                                <div class="user-dropdown-chevron">
                                    <font-awesome-icon icon="fa-solid fa-chevron-down" />
                                </div>
                            </div>
                            <template #popper>
                                <div class="user-dropdown">
                                    <div class="user-dropdown-info">
                                        <div class="user-dropdown-label">Deine Lernpunkte</div>
                                    </div>
                                    <div class="user-dropdown-social">
                                        <LazyNuxtLink to="/Nachrichten/">
                                            <div class="user-dropdown-label">Deine Nachrichten</div>
                                        </LazyNuxtLink>

                                        <div class="user-dropdown-label">Dein Netzwerk</div>
                                        <NuxtLink :to="`/Nutzer/${encodeURI(userStore.name)}/${userStore.id}`">
                                            <div class="user-dropdown-label">Deine Profilseite</div>
                                        </NuxtLink>
                                    </div>
                                    <div class="user-dropdown-managment">
                                        <div class="user-dropdown-label">Konto-Einstellungen</div>
                                        <div class="user-dropdown-label">Administrativ</div>
                                        <div class="user-dropdown-label">Adminrechte abgeben</div>
                                        <div class="user-dropdown-label" @click="userStore.logout()">Ausloggen</div>
                                    </div>
                                </div>
                            </template>
                        </VDropdown>

                        <div v-if="!userStore.isLoggedIn" class="nav-options-container"
                            :class="{ 'hide-nav': !showRegisterButton }">
                            <div class="StickySearchContainer"
                                :class="{ 'showSearch': showSearch, 'has-register-btn': isDesktopOrTablet }">
                                <div class="searchButton" :class="{ 'showSearch': showSearch }"
                                    @click="showSearch = !showSearch">
                                    <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                    <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                                </div>
                                <div class="StickySearch">
                                    <Search :search-type="SearchType.All" :show-search="showSearch"
                                        v-on:select-item="openUrl" placement="bottom-end" />
                                </div>
                            </div>
                            <div class="login-btn" @click="userStore.openLoginModal()">
                                <font-awesome-icon icon="fa-solid fa-right-to-bracket" />
                            </div>
                            <div class="register-btn-container hidden-xs hidden-sm" v-if="isDesktopOrTablet">
                                <NuxtLink to="/Registrieren">
                                    <div navigate class="btn memo-button register-btn">Kostenlos registrieren!</div>
                                </NuxtLink>
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
}

.StickySearchContainer {
    display: flex;
    flex-direction: row-reverse;
    flex-wrap: nowrap;
    height: 100%;
    align-items: center;
    z-index: 20;
    margin-right: 0;

    .searchButton {
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
            flex-grow: 1;

            &.start {
                align-items: baseline;
                padding-top: 11px;
                padding-bottom: 11px;
            }

            &.end {
                justify-content: flex-end;
                min-width: 45px;
            }
        }

        .partial-spacer {
            flex-shrink: 1;
            flex-grow: 2;
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
                margin-right: 31px;
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
            padding: 0 4px;
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

    .searchButton {
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