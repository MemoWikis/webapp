<script lang="ts" setup>
import type { NuxtError } from '#app'
import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { FooterPages } from '~/components/page/pageStore'
import { SiteType } from './components/shared/siteEnum'

const props = defineProps({
    error: Object as () => NuxtError
})

const userStore = useUserStore()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/App/GetCurrentUser', {
    method: 'GET',
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
})

if (currentUser.value != null) {
    userStore.initUser(currentUser.value)
    useState('currentuser', () => currentUser.value)
}

const { data: footerPages } = await useFetch<FooterPages>('/apiVue/App/GetFooterPages', {
    method: 'GET',
    mode: 'no-cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.baseURL = config.public.serverBase
        }
    },
})

const handleError = () => {
    clearError({ redirect: '/' })
}
</script>

<template>
    <HeaderNavigation />
    <SideSheet :footer-pages="footerPages" />

    <div class="nuxt-page">
        <NuxtLayout>
            <ErrorContent :error="props.error" :in-error-boundary="false" />
        </NuxtLayout>
    </div>

    <Footer :footer-pages="footerPages" v-if="footerPages" :site="SiteType.Error" :is-error="true" />
</template>