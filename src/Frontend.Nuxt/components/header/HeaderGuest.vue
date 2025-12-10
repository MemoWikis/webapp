<script lang="ts" setup>
import { QuestionItem, SearchType, PageItem, UserItem } from '~~/components/search/searchHelper'
import { useUserStore } from '../user/userStore'
import { useRootPageChipStore } from './rootPageChipStore'

interface Props {
    isError?: boolean
}

const props = defineProps<Props>()

const { t } = useI18n()

const userStore = useUserStore()
const showSearch = ref(true)
const { $urlHelper } = useNuxtApp()
const openUrl = async (val: PageItem | QuestionItem | UserItem) => {
    if (isMobile || window?.innerWidth < 480)
        showSearch.value = false
    return navigateTo(val.url)
}
const { isDesktopOrTablet, isMobile } = useDevice()

const handleResize = () => {
    if (showSearch.value)
        return

    if (window.innerWidth < 480) {
        showSearch.value = false
    } else
        showSearch.value = true
}

const windowIsAvailable = ref(false)

onMounted(() => {
    if (isMobile || window?.innerWidth < 480)
        showSearch.value = false

    if (typeof window != "undefined") {
        window.addEventListener('resize', handleResize)
        windowIsAvailable.value = true
    }
})

const handleError = () => {
    if (props.isError)
        clearError()
}

const rootPageChipStore = useRootPageChipStore()
const { sideSheetOpen } = useSideSheetState()
</script>

<template>
    <div id="GuestNavigation">
        <div class="HeaderMainRow" :class="{ 'search-is-open': showSearch }">
            <div class="guest-header-container" :class="{ 'sidesheet-open': sideSheetOpen }">
                <div id="LogoContainer" class="col-Logo col-sm-4 col-md-4 col-xs-4">
                    <NuxtLink id="LogoLink" @click="handleError"
                        :to="userStore.isLoggedIn ? $urlHelper.getPageUrl(userStore.personalWiki?.name!, userStore.personalWiki?.id!) : $urlHelper.getPageUrl(rootPageChipStore.name, rootPageChipStore.id)"
                        alt="homepage">
                        <div id="Logo">
                            <Image src="/Images/Logo/Logo.svg" class="hidden-xs" alt="memoWikis logo" />
                            <Image src="/Images/Logo/LogoSmall.svg"
                                class="hidden-sm hidden-md hidden-lg hidden-xl small" :height="40"
                                alt="small memoWikis logo" />
                        </div>
                    </NuxtLink>
                </div>
                <div id="HeaderBodyContainer" class="col-LoginAndHelp col-sm-8 col-md-8 col-xs-8">
                    <div id="HeaderSearch" class="" v-if="!props.isError">
                        <div class="search-button" :class="{ 'showSearch': showSearch }" v-if="windowIsAvailable"
                            @click="showSearch = !showSearch">
                            <font-awesome-icon v-if="showSearch" :icon="['fa-solid', 'xmark']" />
                            <font-awesome-icon v-else :icon="['fa-solid', 'magnifying-glass']" />
                        </div>
                        <div class="SearchContainer" :class="{ 'showSearch': showSearch }">
                            <Search :search-type="SearchType.all" :show-search="showSearch" v-on:select-item="openUrl"
                                id="SmallHeaderSearchComponent" />
                        </div>
                    </div>
                    <div id="loginAndHelp">
                        <div class="login-register-container" v-if="!props.isError">
                            <button class="btn memo-button link-btn login-btn" @click="userStore.openLoginModal()">
                                <font-awesome-icon :icon="['fa-solid', 'right-to-bracket']" class="login-icon" />
                                <div class="login-btn-label hidden-xxs">
                                    {{ t('label.login') }}
                                </div>
                            </button>
                            <div class="register-btn-container hidden-xs hidden-sm" v-if="isDesktopOrTablet">
                                <button navigate class="btn memo-button register-btn">
                                    <NuxtLink :to="`/${t('url.register')}`" class="" @click="handleError">
                                        {{ t('label.register') }}
                                    </NuxtLink>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

#GuestNavigation {
    position: sticky;
    margin: 0;
    width: 100%;
    z-index: 100;
    background-color: white;
    min-height: 60px;
    border-bottom: 1px solid @memo-grey-light;

    .HeaderMainRow {
        min-height: 60px;
        height: 100%;

        display: flex;
        justify-content: center;
        align-items: center;

        &.search-is-open {
            @media(max-width: 479px) {

                #LogoContainer {
                    visibility: hidden;
                    width: 0;
                }

                #HeaderBodyContainer {
                    width: 100%;
                    padding-right: 20px;
                }
            }
        }

        .guest-header-container {
            height: 60px;
            display: flex;
            justify-content: center;
            align-items: center;
            max-width: 1600px;
            width: 100%;
            padding: 0 10px;
            padding-left: 80px;

            &.sidesheet-open {
                padding-left: 410px;

                @media (max-width: 900px) {
                    padding-left: 0px;
                }

                @media (min-width: 1980px) {
                    padding-left: clamp(80px, calc(410px - (100vw - 1980px)), 410px);
                }
            }
        }

        .col-LoginAndHelp {
            display: flex;
            justify-content: flex-end;
            align-items: center;

            #loginAndHelp {
                color: @light-blue;
                float: right;
                height: 100%;

                .header-item {
                    display: flex;
                    flex-direction: column;
                    height: 100%;
                    text-align: center;
                    margin-right: 15px;

                    .TextSpan,
                    span:not(.level-display):not(#NextLevelProgressSpanPercentageDone) {
                        display: inline-block;
                        font-size: 14px;

                        @media(max-width: (@screen-md - 0.5)) {
                            display: none;
                        }
                    }

                    .KnowledgeLink {
                        color: @memo-yellow !important;
                    }

                    #SmallHeaderSearchBoxDiv {
                        width: 43px;
                        transition: width 0.3s;
                        -webkit-transition: width 0.3s;
                        display: none;
                        margin-top: 2px;

                        @media(max-width:(@screen-md-min - 0.5)) {
                            display: table;
                        }

                        #SmallHeaderSearchBox {
                            border: none;
                            padding: 0px;
                            font-weight: 400;
                            text-overflow: ellipsis;
                        }
                    }


                    .btn-default {
                        display: none;
                        background-color: transparent;
                        border: none;

                        @media (max-width:(@screen-md-min - 0.5)) {
                            display: block;
                        }
                    }

                    @media(max-width: 992px) {
                        margin-right: 0px;
                    }
                }

                .header-item:not(#MenuButtonContainer) {
                    @media(max-width: 580px) {
                        margin-top: -5px;
                    }
                }

                #MenuButtonContainer {
                    @media(max-width: 992px) {
                        padding-left: 15px;
                    }
                }

                .register-btn-container {

                    .register-btn {
                        border-radius: 24px;

                        a {
                            color: @memo-blue;
                        }
                    }
                }

                #Login {
                    .dropdown {
                        height: 100%;
                        line-height: 34px;

                        @media(max-width:580px) {
                            padding-top: 5px;
                        }

                        .userName {
                            max-width: 145px;
                            overflow: hidden;
                            white-space: nowrap;
                            text-overflow: ellipsis;
                        }
                    }

                    .badge-header {
                        background-color: @memo-wish-knowledge-red;
                        width: 19px;
                        margin-top: 3px;
                        left: 11px;
                        top: -11px;
                        border: 1.5px solid @memo-blue;
                        position: relative;
                        margin-right: 0px;
                    }
                }
            }
        }

        .fa-dot-circle-o {
            @media(max-width: 580px) {
                margin-top: 4px;
            }
        }
    }

    .col-Logo {
        height: 100%;
    }

    #Logo {
        display: flex;
        align-items: center;
        height: 100%;

        .img-container {
            height: 24px;
            margin-right: 6px;

            &.small {
                width: 59px;
                height: 24px;
            }
        }
    }

    .login-btn {
        font-size: 20px;
        color: @memo-grey-dark;

        .login-btn-label {
            padding-left: 6px;
            font-size: 14px;
        }

        .login-icon {
            font-size: 20px;
            color: @memo-grey-dark;
        }
    }
}

#HeaderSearch {
    padding-left: 8px;
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
                border: none;
                width: 100%;
                padding: 6px 40px 6px 12px;
                background: @memo-grey-lighter;
            }
        }
    }

    :deep(.search-button) {
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

        svg.fa-magnifying-glass {
            color: @memo-grey-dark;
        }

        svg.fa-xmark,
        svg.fa-magnifying-glass {
            transition: all 0.1s;
            color: @memo-grey-dark;
        }

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
}
</style>