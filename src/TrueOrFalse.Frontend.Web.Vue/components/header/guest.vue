<script lang="ts" setup>
import { SearchType } from '~~/components/search/searchTypeEnum';
import { ref, watch, onMounted } from 'vue'
import { useUserStore } from '../user/userStore'

const userStore = useUserStore()

const showSearch = ref(true)

function openUrl(val) {
    location.href = val.Url
}

</script>


<template>
    <div id="GuestNavigation" class="container">
        <div class="HeaderMainRow row">
            <div id="LogoContainer" class="col-sm-3 col-Logo col-xs-2">
                <a id="LogoLink" href="/">
                    <div id="MobileLogo">
                        <Image url="/Images/Logo/LogoMemoWiki.svg" />
                        <Image url="/Images/Logo/memoWikis.svg" class="hidden-xs" />

                    </div>
                </a>
            </div>
            <div id="HeaderBodyContainer" class="col-sm-9 col-LoginAndHelp col-xs-10 row">
                <div id="HeaderSearch" class="">
                    <div class="searchButton" :class="{ 'showSearch': showSearch }" @click="showSearch = !showSearch">
                        <i :class="[showSearch ? 'fas fa-times' : 'fa fa-search']" aria-hidden="true"></i>
                    </div>
                    <div class="SearchContainer" :class="{ 'showSearch': showSearch }">
                        <LazySearch :search-type="SearchType.All" :show-search="showSearch" v-on:select-item="openUrl"
                            id="SmallHeaderSearchComponent" />
                    </div>
                </div>
                <div id="loginAndHelp" class="">
                    <div class="login-register-container">
                        <div class="btn memo-button link-btn login-btn" @click="userStore.openLoginModal()">
                            <font-awesome-icon icon="fa-solid fa-right-to-bracket" />
                            Anmelden
                        </div>
                        <NuxtLink to="/user/register">
                            <div navigate class="btn memo-button register-btn">Kostenlos registrieren!</div>
                        </NuxtLink>
                    </div>

                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
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
        display: flex;
        align-items: center;
        width: 100%;

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
                        color: @memo-yellow  !important;
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
}
</style>