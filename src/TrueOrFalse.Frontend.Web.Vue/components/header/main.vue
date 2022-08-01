<script lang="ts" setup>
import { ref } from 'vue'
import { useUserStore } from '../user/userStore';
import { Author } from '../author/author';
import { ImageStyle } from '../image/imageStyleEnum';
const props = defineProps(['route'])
const userStore = useUserStore()
const currentUser = ref(null as Author)

if (userStore.isLoggedIn) {

    const data = {
        id: userStore.id
    }

    if (process.client) {
        currentUser.value = await $fetch<Author>('/api/Author/GetAuthor/', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
    } else if (process.server) {
        const config = useRuntimeConfig()
        currentUser.value = await $fetch<Author>('/Author/GetAuthor/', { method: 'POST', baseURL: config.apiBase, body: data, mode: 'cors', credentials: 'include' })
    }
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
                        <HeaderBreadcrumb :headerContainer="headerContainer" :headerExtras="headerExtras"
                            :route="route" />
                    </div>

                    <div class="partial" ref="headerExtras">
                        <template v-if="userStore.isLoggedIn">
                            <div>
                                <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                            </div>
                            <VDropdown :distance="6">
                                <div class="header-btn">
                                    <Image :src="currentUser.ImageUrl" :style="ImageStyle.Author"
                                        class="header-author-icon" />
                                    <div class="header-user-name">
                                        {{ currentUser.Name }}
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
                                            <NuxtLink href="#">
                                                <div class="user-dropdown-label">Deine Nachrichten</div>
                                            </NuxtLink>

                                            <div class="user-dropdown-label">Dein Netzwerk</div>
                                            <NuxtLink :href="`${encodeURI(userStore.name)}/${userStore.id}`"></NuxtLink>
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
                        </template>

                        <template v-else-if="showRegisterButton">
                            <div>
                                <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
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
    }
}
</style>