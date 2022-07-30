<script lang="ts" setup>
import { useUserStore } from '../user/userStore';
import { Author } from '../author/author';
import { ImageStyle } from '../image/imageStyleEnum';
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
        window.addEventListener('scroll', handleScroll);
    }

})

</script>


<template>
    <div id="Navigation">
        <div class="container">
            <div class="row">
                <div class="header-container col-xs-12">

                    <div class="partial">
                        <HeaderBreadcrumb />
                    </div>

                    <div class="partial" v-if="userStore.isLoggedIn">
                        <div>
                            <Image :src="currentUser.ImageUrl" :style="ImageStyle.Author" class="header-author-icon" />
                        </div>
                    </div>
                    <div class="partial register-btn-container" v-else-if="showRegisterButton">
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

        .partial {
            height: 100%;
            display: flex;
            align-items: center;
        }

        .register-btn-container {
            background: @memo-green;

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
}
</style>