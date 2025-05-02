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
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
})
const { $urlHelper } = useNuxtApp()
const { t } = useI18n()
</script>

<template>
    <div class="container">
        <div class="row page-container main-page">
            <div class="col-xs-12">
                <div class="row">
                    <h1>{{ t('membership.cancel.title') }}</h1>
                    <p>
                        {{ t('membership.cancel.message') }}
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
    padding-top: 24px;
    font-size: 16px;
    white-space: pre-wrap;
}
</style>
