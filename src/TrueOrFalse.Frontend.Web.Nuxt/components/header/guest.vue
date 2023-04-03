<script lang="ts" setup>
import { QuestionItem, SearchType, TopicItem, UserItem } from '~~/components/search/searchHelper'
import { useUserStore } from '../user/userStore'

interface Props {
    isError?: boolean
}

const props = defineProps<Props>()

const userStore = useUserStore()

const showSearch = ref(true)

function openUrl(val: TopicItem | QuestionItem | UserItem | null) {
    if (val != null)
        navigateTo({ path: val.Url }, { replace: true })
}
const { isDesktopOrTablet, isMobile } = useDevice()

function handleResize() {
    if (window.innerWidth < 480) {
        showSearch.value = false
    } else
        showSearch.value = true

}

onMounted(() => {
    if (isMobile || window?.innerWidth < 480)
        showSearch.value = false

    if (typeof window != undefined) {
        window.addEventListener('resize', handleResize)
    }
})

function handleError() {
    if (props.isError)
        clearError()
}

</script>

<template>
    <div id="GuestNavigation">
        <div class="HeaderMainRow container" :class="{ 'search-is-open': showSearch }">
            <div class="row">
                <div id="LogoContainer" class="col-Logo col-sm-4 col-md-4 col-xs-4">
                    <NuxtLink id="LogoLink" @click="handleError"
                        :to="userStore.isLoggedIn ? `/${userStore.personalWiki?.EncodedName}/${userStore.personalWiki?.Id}` : '/Globales-Wiki/1'">
                        <div id="Logo">
                            <Image url="/Images/Logo/Logo.svg" class="hidden-xs" />
                            <Image url="/Images/Logo/LogoSmall.png" class="hidden-sm hidden-md hidden-lg hidden-xl small" />
                        </div>
                    </NuxtLink>
                </div>
                <div id="HeaderBodyContainer" class="col-LoginAndHelp col-sm-8 col-md-8 col-xs-8 row">
                    <div id="HeaderSearch" class="" v-if="!props.isError">
                        <div class="searchButton" :class="{ 'showSearch': showSearch }" @click="showSearch = !showSearch">
                            <font-awesome-icon v-if="showSearch" :icon="['fa-solid', 'xmark']" />
                            <font-awesome-icon v-else :icon="['fa-solid', 'magnifying-glass']" />
                        </div>
                        <div class="SearchContainer" :class="{ 'showSearch': showSearch }">
                            <Search :search-type="SearchType.All" :show-search="showSearch" v-on:select-item="openUrl"
                                id="SmallHeaderSearchComponent" />
                        </div>
                    </div>
                    <div id="loginAndHelp">
                        <div class="login-register-container">
                            <button class="btn memo-button link-btn login-btn" @click="userStore.openLoginModal()">
                                <font-awesome-icon :icon="['fa-solid', 'right-to-bracket']" />
                                <div class="login-btn-label hidden-xxs">
                                    Anmelden
                                </div>
                            </button>
                            <div class="register-btn-container hidden-xs hidden-sm" v-if="isDesktopOrTablet">
                                <button navigate class="btn memo-button register-btn">
                                    <NuxtLink to="/Registrieren" class="" @click="handleError">
                                        Kostenlos registrieren!
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
    background-color: @memo-blue;
    min-height: 60px;

    .HeaderMainRow {
        min-height: 60px;
        height: 100%;

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


        .row {
            height: 60px;
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
                    @media(min-width: 1200px) {
                        margin-right: 33px;
                    }

                    .register-btn {
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
                        background-color: @memo-wuwi-red;
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
                height: 40px;
            }
        }
    }

    .login-btn {
        font-size: 20px;

        .login-btn-label {
            padding-left: 6px;
            font-size: 14px;
        }
    }
}

#HeaderSearch {
    padding-left: 8px;
}
</style>