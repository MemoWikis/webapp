<script lang="ts" setup>
import { SearchType } from '~~/components/search/searchTypeEnum';
import {ref, watch, onMounted} from 'vue'

const showSearch = ref(false)

function openUrl(val) {
    location.href = val.Url
} 

</script>


<template>
<div id="MasterHeaderContainer" class="container">
            <div class="HeaderMainRow row">
                <div id="LogoContainer" class="col-sm-3 col-Logo col-xs-2">
                    <a id="LogoLink" href="/">
                        <div id="FullLogo" class="hidden-xs">
                            <Image url="/Images/Logo/Logo.svg"/>
                        </div>
                        <div id="MobileLogo" class="hidden-sm hidden-md hidden-lg">
                            <Image url="/Images/Logo/LogoSmall.png"/>
                        </div>
                    </a>
                </div>
                <div id="HeaderBodyContainer" class="col-sm-9 col-LoginAndHelp col-xs-10 row">
                    <div id="HeaderSearch" class="">
                        <div class="searchButton" :class="{ 'showSearch' : showSearch }" @click="showSearch = !showSearch">
                            <i :class="[ showSearch ? 'fas fa-times' : 'fa fa-search']" aria-hidden="true"></i>
                        </div>
                        <div class="SearchContainer" :class="{ 'showSearch' : showSearch }">
                            <!-- <search-component v-on:select-item="openUrl" :search-type="SearchType.All" id="SmallHeaderSearchComponent" :show-search="showSearch"/> -->
                        </div>
                    </div>
                    <div id="loginAndHelp" class="">
                    </div>
                </div>
            </div>
        </div>
</template>

<style lang="css" scoped>

#MasterHeaderContainer {
    margin: 0;
    width: 100%;
    z-index: 100;
    background-color: #003264;
    min-height: 60px;
}

</style>