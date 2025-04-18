<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'

const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()
const userStore = useUserStore()
const { $logger } = useNuxtApp()

const isAdmin = ref(false)
const antiForgeryToken = ref<string>()
const { data: maintenanceDataResult } = await useFetch<FetchResult<string>>('/apiVue/VueMaintenance/Get',
    {
        credentials: 'include',
        mode: 'cors',
        onRequest({ options }) {
            if (import.meta.server) {
                options.headers = headers
                options.baseURL = config.public.serverBase
            }
        },
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

if (maintenanceDataResult.value?.success) {
    isAdmin.value = true
    antiForgeryToken.value = maintenanceDataResult.value.data
}

interface MethodData {
    url: string
    label: string
}
const questionMethods = ref<MethodData[]>([
    { url: 'RecalculateAllKnowledgeItems', label: 'Alle Antwortwahrscheinlichkeiten neu berechnen' },
    { url: 'CalcAggregatedValuesQuestions', label: 'Aggregierte Zahlen aktualisieren' }
])
const cacheMethods = ref<MethodData[]>([
    { url: 'ClearCache', label: 'Cache leeren' },
])
const pageMethods = ref<MethodData[]>([
    { url: 'UpdateCategoryAuthors', label: 'Seitenautoren aktualisieren' }
])
const meiliSearchMethods = ref<MethodData[]>([
    { url: 'MeiliReIndexAllQuestions', label: 'Fragen' },
    { url: 'MeiliReIndexAllQuestionsCache', label: 'Fragen (Cache)' },
    { url: 'MeiliReIndexAllPages', label: 'Seiten' },
    { url: 'MeiliReIndexAllPagesCache', label: 'Seiten (Cache)' },
    { url: 'MeiliReIndexAllUsers', label: 'Nutzer' },
    { url: 'MeiliReIndexAllUsersCache', label: 'Nutzer (Cache)' }
])
const userMethods = ref<MethodData[]>([
    { url: 'UpdateUserReputationAndRankings', label: 'Rankings und Reputation + Aggregates' },
    { url: 'UpdateUserWishCount', label: 'Wunschwissenzähler aktualisieren' }
])
const miscMethods = ref<MethodData[]>([
    { url: 'CheckForDuplicateInteractionNumbers', label: 'Auf Antworten mit selber Guid und InteractionNr checken' }
])
const toolsMethods = ref<MethodData[]>([
    { url: 'Throw500', label: 'Exception werfen' },
    { url: 'ReloadListFromIgnoreCrawlers', label: 'List von den igniorierten Crawlers neu lade' },
    { url: 'CleanUpWorkInProgressQuestions', label: 'Clean up work in progress questions' },
    { url: 'Start100TestJobs', label: 'Start 100 test jobs' },

])
const resultMsg = ref('')

async function handleClick(url: string) {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/${url}`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success)
        resultMsg.value = result.data
}

const emit = defineEmits(['setBreadcrumb'])

onBeforeMount(() => {
    if (!isAdmin.value && !userStore.isAdmin)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    emit('setBreadcrumb', [{ name: 'Maintenance', url: '/Maintenance' }])
})

const userIdToDelete = ref(0)
async function deleteUser() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('userId', userIdToDelete.value.toString())

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/DeleteUser`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success)
        resultMsg.value = result.data
}

async function removeAdminRights() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/RemoveAdminRights`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success) {
        userStore.isAdmin = false
        antiForgeryToken.value = undefined
        await navigateTo('/')
    }
}
</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="main-content">
                <div class="col-xs-12"
                    v-if="isAdmin && userStore.isAdmin && antiForgeryToken != null && antiForgeryToken?.length > 0">
                    <h1>Adminseite</h1>
                    <div class="row">
                        <div class="alert alert-warning alert-dismissible" role="alert" v-if="resultMsg.length > 0">
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close"
                                @click.prevent="resultMsg = ''"><span aria-hidden="true">&times;</span></button>
                            {{ resultMsg }}
                        </div>
                        <MaintenanceSection title="Metriken" :methods="[]">
                            <div class="custom-container">
                                <NuxtLink to="/Metriken" class="memo-button btn btn-primary">
                                    Übersicht aufrufen
                                </NuxtLink>
                            </div>
                        </MaintenanceSection>
                        <MaintenanceSection title="Fragen" :methods="questionMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Cache" :methods="cacheMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Seiten" :methods="pageMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Suche MeiliSearch" :methods="meiliSearchMethods"
                            description="Alle für Suche neu indizieren:" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Nutzer" :methods="userMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']">
                            <div class="delete-user-container">
                                <h4>Nutzer löschen (ID)</h4>
                                <div class="delete-user-input">
                                    <input v-model="userIdToDelete" />
                                    <button @click="deleteUser" class="memo-button btn btn-primary">
                                        Nutzer Löschen
                                    </button>
                                </div>
                            </div>
                        </MaintenanceSection>
                        <MaintenanceSection title="Sonstige" :methods="miscMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Tools" :methods="toolsMethods" @method-clicked="handleClick"
                            :icon="['fas', 'hammer']" />
                        <div class="remove-admin-rights-section col-xs-12 col-lg-6">
                            <h3>Adminrechte abgeben</h3>
                            <div>
                                <button @click="removeAdminRights" class="memo-button btn btn-primary">
                                    Adminrechte abgeben
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.alert {
    position: relative;

    .close-button {
        position: absolute;
        right: 15px;
        top: 15px;
        cursor: pointer;
    }
}

.custom-container {
    padding: 15px;
}

.delete-user-container {
    padding: 15px;

    .delete-user-input {

        display: flex;
        align-items: center;
        flex-direction: row;

        input {
            border: solid 1px @memo-grey-light;
            padding: 7px;
            margin-right: 8px;
        }
    }
}

.remove-admin-rights-section {
    border: solid 1px @memo-grey-lighter;
    padding: 8px;
    margin: 8px;

    h3,
    .description {
        padding: 6px 12px;
        margin-top: 0;
    }
}
</style>
