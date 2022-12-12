<script lang="ts" setup>
import { PropType } from 'vue'
import { useUserStore } from '../user/userStore'
import { Author } from '../author/author'
import { ImageStyle } from '../image/imageStyleEnum'
import { SearchType } from '~~/components/search/searchHelper'
import { Page } from '../shared/pageEnum'

const props = defineProps({
    page: { type: Number as PropType<Page>, required: true }
})

const showSearch = ref(true)

function openUrl(val: any) {
    navigateTo(val.Url)
}
const userStore = useUserStore()
const currentUser = ref(null as Author | null)

if (userStore.isLoggedIn) {

    const data = {
        id: userStore.id
    }
    currentUser.value = await $fetch<Author>('/apiVue/Author/GetAuthor/', {
        method: 'POST', body: data, mode: 'cors', credentials: 'include'
    })
}
const showRegisterButton = ref(false)
function handleScroll() {
    var scrollTop = document.documentElement.scrollTop
    if (scrollTop > 59)
        showRegisterButton.value = true
    else
        showRegisterButton.value = false
}
onBeforeMount(() => {
    if (!userStore.isLoggedIn) {
        handleScroll()
    }

})
const headerContainer = ref(null)
const headerExtras = ref(null)

onMounted(() => {
    if (!userStore.isLoggedIn) {
        window.addEventListener('scroll', handleScroll);
    }
})

</script>

<template>
    <div id="Navigation">
        <div class="container">
            <div class="row">
                <div class="header-container col-xs-12" ref="headerContainer">

                    <div class="partial">
                        <HeaderBreadcrumb :header-container="headerContainer" :header-extras="headerExtras"
                            :page="props.page" />
                    </div>

                    <div class="partial" ref="headerExtras">
                        <div class="stickySearchContainer" v-if="userStore.isLoggedIn">
                            <div class="searchButton" :class="{ 'showSearch': showSearch }"
                                @click="showSearch = !showSearch">
                                <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                            </div>
                            <div class="StickySearch" :class="{
                                'showSearch': showSearch
                            }">
                                <LazySearch :search-type="SearchType.All" :show-search="showSearch"
                                    v-on:select-item="openUrl" id="SmallHeaderSearchComponent" />
                            </div>
                        </div>
                        <VDropdown :distance="6" v-show="userStore.isLoggedIn">
                            <div class="header-btn">
                                <Image v-if="currentUser" :src="currentUser.ImgUrl" :style="ImageStyle.Author"
                                    class="header-author-icon" />
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
                                        <LazyNuxtLink to="/user/messages/">
                                            <div class="user-dropdown-label">Deine Nachrichten</div>
                                        </LazyNuxtLink>

                                        <div class="user-dropdown-label">Dein Netzwerk</div>
                                        <NuxtLink :to="`${encodeURI(userStore.name)}/${userStore.id}`"></NuxtLink>
                                        <div class="user-dropdown-label">Deine Profilseite</div>
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

                        <template v-if="showRegisterButton && !userStore.isLoggedIn">
                            <div class="stickySearchContainer">
                                <div class="searchButton" :class="{ 'showSearch': showSearch }"
                                    @click="showSearch = !showSearch">
                                    <font-awesome-icon v-if="showSearch" icon="fa-solid fa-xmark" />
                                    <font-awesome-icon v-else icon="fa-solid fa-magnifying-glass" />
                                </div>
                                <div class="StickySearch" :class="{ 'showSearch': showSearch }">
                                    <LazySearch :search-type="SearchType.All" :show-search="showSearch"
                                        v-on:select-item="openUrl" id="SmallHeaderSearchComponent" />
                                </div>
                            </div>
                            <div>
                                <font-awesome-icon icon="fa-solid fa-right-to-bracket" />
                            </div>
                            <div class="register-btn-container">
                                <NuxtLink to="/user/register">
                                    <div navigate class="btn memo-button register-btn">Kostenlos registrieren!</div>
                                </NuxtLink>
                            </div>

                        </template>
                    </div>

                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.stickySearchContainer {
    display: flex;
    flex-direction: row-reverse;
    flex-wrap: nowrap;
    height: 100%;
    align-items: center;

    .searchButton {
        align-items: center;
        display: flex;
        font-size: 20px;
        height: 100%;
        justify-content: center;
        position: absolute;
        transform: translateZ(0);
        transition: all .3s;
        width: 34px;
        z-index: 1050;
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

#StickySearch,
#HeaderSearch {
    width: 100%;
    display: flex;
    flex-direction: row-reverse;
    height: 100%;
    align-items: center;

    .StickySearchContainer,
    .SearchContainer {
        width: 100%;

        input {
            min-width: 0px;
            width: 0px;
            border: none;
            padding: 0;
            transition: all 0.3s;
            background: transparent;
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