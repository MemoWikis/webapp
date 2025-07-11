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
                options.headers = new Headers(headers)
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

const { data: activeSessionsResult } = await useFetch<ActiveSessionsResponse>('/apiVue/VueMaintenance/GetActiveSessions', {
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})

const loggedInUserCount = ref(0)
const anonymousUserCount = ref(0)
watchEffect(() => {
    if (activeSessionsResult.value) {
        loggedInUserCount.value = activeSessionsResult.value.loggedInUserCount
        anonymousUserCount.value = activeSessionsResult.value.anonymousUserCount
    }

})

interface MethodData {
    url: string
    translationKey: string
}

interface ActiveSessionsResponse {
    loggedInUserCount: number,
    anonymousUserCount: number
}

interface RelationError {
    type: string
    childId: number
    description: string
}

interface RelationErrorItem {
    parentId: number
    errors: RelationError[]
}

interface RelationErrorsResponse {
    success: boolean
    data: RelationErrorItem[]
}

const questionMethods = ref<MethodData[]>([
    { url: 'RecalculateAllKnowledgeItems', translationKey: 'maintenance.questions.recalculateAllKnowledgeItems' },
    { url: 'CalcAggregatedValuesQuestions', translationKey: 'maintenance.questions.calcAggregatedValues' }
])
const cacheMethods = ref<MethodData[]>([
    { url: 'ClearCache', translationKey: 'maintenance.cache.clearCache' },
])
const pageMethods = ref<MethodData[]>([
    { url: 'UpdateCategoryAuthors', translationKey: 'maintenance.pages.updateCategoryAuthors' }
])
const relationMethods = ref<MethodData[]>([])

// Simple button for showing relation errors
const showRelationErrorsButton = async () => {
    await loadRelationErrors()
}
const meiliSearchMethods = ref<MethodData[]>([
    { url: 'MeiliReIndexAllQuestions', translationKey: 'maintenance.meiliSearch.questions' },
    { url: 'MeiliReIndexAllQuestionsCache', translationKey: 'maintenance.meiliSearch.questionsCache' },
    { url: 'MeiliReIndexAllPages', translationKey: 'maintenance.meiliSearch.pages' },
    { url: 'MeiliReIndexAllPagesCache', translationKey: 'maintenance.meiliSearch.pagesCache' },
    { url: 'MeiliReIndexAllUsers', translationKey: 'maintenance.meiliSearch.users' },
    { url: 'MeiliReIndexAllUsersCache', translationKey: 'maintenance.meiliSearch.usersCache' }
])
const userMethods = ref<MethodData[]>([
    { url: 'UpdateUserReputationAndRankings', translationKey: 'maintenance.users.updateReputationAndRankings' },
    { url: 'UpdateUserWishCount', translationKey: 'maintenance.users.updateWishCount' }
])
const miscMethods = ref<MethodData[]>([
    { url: 'CheckForDuplicateInteractionNumbers', translationKey: 'maintenance.misc.checkDuplicateInteractions' }
])
const toolsMethods = ref<MethodData[]>([
    { url: 'Throw500', translationKey: 'maintenance.tools.throwException' },
    { url: 'ReloadListFromIgnoreCrawlers', translationKey: 'maintenance.tools.reloadIgnoreCrawlers' },
    { url: 'CleanUpWorkInProgressQuestions', translationKey: 'maintenance.tools.cleanupWorkInProgress' },
    { url: 'Start100TestJobs', translationKey: 'maintenance.tools.start100TestJobs' },

])
const resultMsg = ref('')
const relationErrors = ref<RelationErrorItem[]>([])
const showRelationErrors = ref(false)

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

async function loadRelationErrors() {
    try {
        const result = await $api<RelationErrorsResponse>('/apiVue/VueMaintenance/ShowRelationErrors', {
            method: 'GET',
            mode: 'cors',
            credentials: 'include'
        })

        if (result.success) {
            relationErrors.value = result.data
            showRelationErrors.value = true
            resultMsg.value = `Found ${result.data.length} pages with relation errors.`
        } else {
            resultMsg.value = 'Error loading relation errors.'
        }
    } catch (error) {
        console.error('Error loading relation errors:', error)
        resultMsg.value = 'Error loading relation errors.'
    }
}

async function healRelations(pageId: number) {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    if (pageId <= 0) {
        resultMsg.value = 'Please enter a valid page ID.'
        return
    }

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('pageId', pageId.toString())

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/HealRelations`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success) {
        resultMsg.value = result.data
        // Refresh relation errors if they are currently displayed
        if (showRelationErrors.value) {
            await loadRelationErrors()
        }
    }
}
</script>

<template>
    <div class="main-content"
        v-if="isAdmin && userStore.isAdmin && antiForgeryToken != null && antiForgeryToken?.length > 0">
        <h1>{{ $t('maintenance.title') }}</h1>
        <div class="">
            <div class="alert alert-warning alert-dismissible" role="alert" v-if="resultMsg.length > 0">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close"
                    @click.prevent="resultMsg = ''"><span aria-hidden="true">&times;</span></button>
                {{ resultMsg }}
            </div>
            <LayoutPanel :title="$t('maintenance.metrics.title')">
                <NuxtLink to="/Metriken" class="memo-button btn btn-primary">
                    {{ $t('maintenance.metrics.viewOverview') }}
                </NuxtLink>
            </LayoutPanel>
            <MaintenanceSection :title="$t('maintenance.questions.title')" :methods="questionMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.cache.title')" :methods="cacheMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.pages.title')" :methods="pageMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <LayoutPanel :title="$t('maintenance.relations.title')">
                <template #description>
                    <button @click="showRelationErrorsButton" class="memo-button btn btn-primary">
                        {{ $t('maintenance.relations.showErrors') }}
                    </button>
                </template>

                <div class="relation-errors" v-if="showRelationErrors && relationErrors.length > 0">
                    <h4>{{ $t('maintenance.relations.foundErrors') }}</h4>
                    <div class="error-cards">
                        <LayoutCard v-for="errorItem in relationErrors" :key="errorItem.parentId" :size="LayoutCardSize.Small">
                            <div class="error-card-content">
                                <h5>Parent Page ID: {{ errorItem.parentId }}</h5>
                                <ul class="error-list">
                                    <li v-for="error in errorItem.errors" :key="`${error.childId}-${error.type}`" :class="'error-type-' + error.type.toLowerCase()">
                                        <strong>{{ error.type }}:</strong> Child ID: {{ error.childId }} - {{ error.description }}
                                    </li>
                                </ul>
                                <div class="heal-button-container">
                                    <button @click="healRelations(errorItem.parentId)" class="memo-button btn btn-sm btn-primary heal-card-button">
                                        {{ $t('maintenance.relations.healButton') }}
                                    </button>
                                </div>
                            </div>
                        </LayoutCard>
                    </div>
                </div>
            </LayoutPanel>
            <MaintenanceSection :title="$t('maintenance.meiliSearch.title')" :methods="meiliSearchMethods" :description="$t('maintenance.meiliSearch.description')" @method-clicked="handleClick" :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.users.title')" :methods="userMethods" @method-clicked="handleClick" :icon="['fas', 'retweet']">
                <LayoutCard :size="LayoutCardSize.Tiny">
                    <div class="active-users-info">
                        <h4>{{ $t('maintenance.users.activeSessions') }}</h4>
                        <ul>
                            <li>{{ $t('maintenance.users.loggedIn') }}: {{ loggedInUserCount }} ({{ $t('maintenance.users.last5Minutes') }})</li>
                            <li>{{ $t('maintenance.users.anonymous') }}: {{ anonymousUserCount }} ({{ $t('maintenance.users.lastMinute') }})</li>
                        </ul>
                    </div>
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small">
                    <div class="delete-user-container">
                        <h4>{{ $t('maintenance.users.deleteUser') }}</h4>
                        <div class="delete-user-input">
                            <input v-model="userIdToDelete" />
                            <button @click="deleteUser" class="memo-button btn btn-primary">
                                {{ $t('maintenance.users.deleteUserButton') }}
                            </button>
                        </div>
                    </div>
                </LayoutCard>

            </MaintenanceSection>
            <MaintenanceSection :title="$t('maintenance.misc.title')" :methods="miscMethods" @method-clicked="handleClick" :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.tools.title')" :methods="toolsMethods" @method-clicked="handleClick" :icon="['fas', 'hammer']" />
            <LayoutPanel :title="$t('maintenance.removeAdminRights.title')">
                <button @click="removeAdminRights" class="memo-button btn btn-primary">
                    {{ $t('maintenance.removeAdminRights.button') }}
                </button>
            </LayoutPanel>
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

.active-users-info {
    padding: 15px;
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

.relation-errors {
    margin-bottom: 20px;

    h4 {
        margin-bottom: 15px;
        color: @memo-grey-darker;
    }

    .error-cards {
        display: flex;
        flex-wrap: wrap;
        gap: 16px;
    }

    .error-card-content {
        padding: 15px;

        h5 {
            margin: 0 0 15px 0;
            color: @memo-grey-darker;
            font-weight: 600;
        }

        .error-list {
            margin: 0 0 15px 0;
            padding-left: 20px;

            li {
                margin-bottom: 8px;
                padding: 4px 0;

                &.error-type-duplicate {
                    color: #d9534f;
                }

                &.error-type-brokenlink {
                    color: #f0ad4e;
                }

                &.error-type-brokenorder {
                    color: #5bc0de;
                }

                strong {
                    font-weight: 600;
                }
            }
        }

        .heal-button-container {
            display: flex;
            justify-content: flex-end;
            padding-top: 10px;
            border-top: 1px solid @memo-grey-lighter;

            .heal-card-button {
                padding: 6px 16px;
                font-size: 12px;
            }
        }
    }
}
</style>
