<script lang="ts" setup>
const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()

interface HelperPages {
    name: string
    id: number
}
const { data: helperPages } = await useFetch<FetchResult<HelperPages[]>>('/apiVue/Cancel/GetHelperPages', {
    method: 'GET',
    credentials: 'include',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
})
const { $urlHelper } = useNuxtApp()

</script>

<template>
    <div class="container">
        <div class="row page-container main-page">
            <h1>Abbruch</h1>
            <p>
                Es tut uns leid zu sehen, dass Sie Ihre Zahlung abgebrochen haben.
                Wenn Sie auf Probleme gestoßen sind oder Fragen haben,
                zögern Sie bitte nicht, uns zu kontaktieren.
                Unser freundliches Support-Team steht Ihnen gerne zur Verfügung.
            </p>
            <div class="helper-links">

                <NuxtLink to="https://discord.com/invite/nXKwGrN" external>
                    <font-awesome-icon :icon="['fa-brands', 'discord']" />&nbsp;Discord
                </NuxtLink>

                <template v-if="helperPages?.success === true">
                    <NuxtLink v-for="helperPage in helperPages.data" :to="$urlHelper.getPageUrl(helperPage.name, helperPage.id)" :key="helperPage.id">
                        {{ helperPage.name }}
                    </NuxtLink>
                </template>
            </div>
        </div>
    </div>
</template>
<style lang="less" scoped>
.helper-links {
    display: flex;
    flex-direction: column;
    font-size: 16px;
    font-weight: 600;
}

p {
    font-size: 16px;
    font-weight: 600;
}
</style>
