<script lang="ts" setup>

import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { FooterTopics } from '~/components/topic/topicStore'
import { Page } from './components/shared/pageEnum'

const userStore = useUserStore()
const config = useRuntimeConfig()

const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/App/GetCurrentUser', {
    method: 'GET',
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    }
})
if (currentUser.value)
    userStore.initUser(currentUser.value)

const { data: footerTopics } = await useFetch<FooterTopics>(`/apiVue/App/GetFooterTopics`, {
    method: 'GET',
    mode: 'no-cors',
    onRequest({ options }) {
        if (process.server) {
            options.baseURL = config.public.serverBase
        }
    }
})
const router = useRouter()

onBeforeMount(() => {
    if (window.location.pathname != '/Fehler') {
        router.push({ path: "/Fehler" })
    }

})

function handleError() {
    clearError({ redirect: '/' })
}
watch(() => userStore.isLoggedIn, () => {
    handleError()
})

</script>

<template>
    <HeaderGuest v-if="!userStore.isLoggedIn" :is-error="true" />

    <HeaderMain :page="Page.Error" :breadcrumb-items="[{ name: 'Fehler', url: '' }]" />
    <div class="container">
        <div class="row topic-container main-page">
            <div class="col-xs-12 container">
                <div class="error-page">
                    <Image url="/Images/Error/memo-500_german_600.png" class="error-image" />

                    <div class="button-container">
                        <button class="memo-button btn-primary" @click="handleError">Zurück zur Startseite</button>
                    </div>
                    <div class="error-message">
                        <p>Oder schicke eine E-Mail an team@memucho.de.</p>
                        <ul>
                            <li>Wir wurden per E-Mail informiert.</li>
                            <li>Bei dringenden Fragen kannst du Robert unter 0178-1866848 erreichen.</li>
                            <li>Oder schicke eine E-Mail an team@memucho.de.</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <ClientOnly>
        <LazyUserLogin v-if="!userStore.isLoggedIn" />
        <LazySpinner />
        <LazyAlert />
    </ClientOnly>
    <Footer :footer-topics="footerTopics" v-if="footerTopics" :is-error="true" />
</template>

<style lang="less" scoped>
.error-page {
    display: flex;
    justify-content: center;
    align-items: center;
    padding-top: 60px;
    padding-bottom: 60px;
    flex-direction: column;

    .error-image {
        max-width: 600px;
        margin-bottom: 60px;
    }

    .error-message {
        padding-top: 45px;
        width: 100%;
    }
}
</style>