<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'

const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()
const userStore = useUserStore()

interface Result {
    isAdmin: boolean
    antiForgeryToken?: string
}

const { data: maintenanceData } = await useFetch<Result>('/apiVue/VueMaintenance/Get',
    {
        credentials: 'include',
        mode: 'cors',
        onRequest({ options }) {
            if (process.server) {
                options.headers = headers
                options.baseURL = config.public.serverBase
            }
        },
    })
if (!maintenanceData.value?.isAdmin && !userStore.isAdmin)
    throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })

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
const topicMethods = ref<MethodData[]>([
    { url: 'UpdateFieldQuestionCountForTopics', label: 'Feld: Anzahl Fragen pro Thema aktualisieren' },
    { url: 'UpdateCategoryAuthors', label: 'Themenautoren aktualisieren' }
])
const solrMethods = ref<MethodData[]>([
    { url: 'ReIndexAllQuestions', label: 'Fragen' },
    { url: 'ReIndexAllTopics', label: 'Themen' },
    { url: 'ReIndexAllUsers', label: 'Nutzer' }
])
const meiliSearchMethods = ref<MethodData[]>([
    { url: 'MeiliReIndexAllQuestions', label: 'Fragen' },
    { url: 'MeiliReIndexAllTopics', label: 'Themen' },
    { url: 'MeiliReIndexAllUsers', label: 'Nutzer' }
])
const userMethods = ref<MethodData[]>([
    { url: 'UpdateUserReputationAndRankings', label: 'Rankings und Reputation + Aggregates' },
    { url: 'UpdateUserWishCount', label: 'Wunschwissenzähler aktualisieren' }
])
const miscMethods = ref<MethodData[]>([
    { url: 'CheckForDuplicateInteractionNumbers', label: 'Auf Antworten mit selber Guid und InteractionNr checken' }
])

async function handleClick(url: string) {

    if (!maintenanceData.value?.isAdmin || !userStore.isAdmin || !maintenanceData.value?.antiForgeryToken)
        throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })

    const data = new FormData()
    data.append('__RequestVerificationToken', maintenanceData.value.antiForgeryToken)

    const result = await $fetch<string>(`/apiVue/VueMaintenance/${url}`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })
    console.log(result)
}

const emit = defineEmits(['setBreadcrumb'])

onBeforeMount(() => {
    emit('setBreadcrumb', [{ name: 'Maintenance', url: '/Maintenance' }])
})
</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="main-content">
                <div class="col-xs-12" v-if="maintenanceData">
                    <h1>Adminseite</h1>
                    <div class="row">
                        <MaintenanceSection title="Fragen" :methods="questionMethods" @method-clicked="handleClick" />
                        <MaintenanceSection title="Cache" :methods="cacheMethods" @method-clicked="handleClick" />
                        <MaintenanceSection title="Themen" :methods="topicMethods" @method-clicked="handleClick" />
                        <MaintenanceSection title="Suche Solr" :methods="solrMethods"
                            description="Alle für Suche neu indizieren:" @method-clicked="handleClick" />
                        <MaintenanceSection title="Suche MeiliSearch" :methods="meiliSearchMethods"
                            description="Alle für Suche neu indizieren:" @method-clicked="handleClick" />
                        <MaintenanceSection title="" :methods="userMethods" @method-clicked="handleClick" />
                        <MaintenanceSection title="" :methods="miscMethods" @method-clicked="handleClick" />
                    </div>

                </div>

            </div>
        </div>
    </div>
</template>
  
<style lang="less" scoped></style>
