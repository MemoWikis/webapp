<script lang="ts" setup>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { NuxtLink, LazySearch } from '~~/.nuxt/components';
import { QuestionItem, SearchType, TopicItem, UserItem } from '~~/components/search/searchHelper'
import { useUserStore } from '../user/userStore'

const userStore = useUserStore()

const showSearch = ref(true)

function openUrl(val: TopicItem | QuestionItem | UserItem | null) {
    if (val != null)
        navigateTo({ path: val.Url }, { replace: true })
}

</script>


<template>
    <div id="GuestNavigation">
        <div class="HeaderMainRow container">
            <div class="row">
                <div id="LogoContainer" class="col-sm-3 col-Logo col-xs-2">
                    <NuxtLink id="LogoLink" href="/">
                        <div id="Logo">
                            <Image url="/Images/Logo/LogoMemoWiki.svg" />
                            <Image url="/Images/Logo/memoWikis.svg" class="hidden-xs" />

                        </div>

                    </NuxtLink>
                </div>
                <div id="HeaderBodyContainer" class="col-sm-9 col-LoginAndHelp col-xs-10 row">
                    <div id="HeaderSearch" class="">
                        <div class="searchButton" :class="{ 'showSearch': showSearch }"
                            @click="showSearch = !showSearch">
                            <font-awesome-icon v-if="showSearch" :icon="['fa-solid', 'xmark']" />
                            <font-awesome-icon v-else :icon="['fa-solid', 'magnifying-glass']" />
                        </div>
                        <div class="SearchContainer" :class="{ 'showSearch': showSearch }">
                            <LazySearch :search-type="SearchType.All" :show-search="showSearch"
                                v-on:select-item="openUrl" id="SmallHeaderSearchComponent" />
                        </div>
                    </div>
                    <div id="loginAndHelp">
                        <div class="login-register-container">
                            <div class="btn memo-button link-btn login-btn" @click="userStore.openLoginModal()">
                                <font-awesome-icon :icon="['fa-solid', 'right-to-bracket']" />
                                <div class="login-btn-label">
                                    Anmelden
                                </div>
                            </div>
                            <NuxtLink to="/user/register">
                                <div navigate class="btn memo-button register-btn">Kostenlos registrieren!</div>
                            </NuxtLink>
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

        img {
            height: 24px;
            margin-right: 6px;
        }
    }

    .login-btn {
        .login-btn-label {
            padding-left: 6px;
        }
    }
}
</style>