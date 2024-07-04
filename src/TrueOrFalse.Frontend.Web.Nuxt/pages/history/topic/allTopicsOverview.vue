<script lang="ts" setup>
import { Page } from '~/components/shared/pageEnum'
import { useSpinnerStore } from '~/components/spinner/spinnerStore'
import { Change } from '~~/components/topic/history/all/change.vue'

const spinnerStore = useSpinnerStore()

interface Day {
    date: string
    groupedChanges: GroupedChanges[]
}

interface GroupedChanges {
    collapsed: boolean
    changes: Change[]
}

const currentState = ref()

onMounted(() => {
    if (window != null)
        currentState.value = window.history.state
})

const emit = defineEmits(['setBreadcrumb', 'setPage'])
const router = useRouter()
const pageNumber = ref(1)
watch(pageNumber, (page) => {
    router.push({ path: `/Historie/Themen/${page}` })
    emit('setBreadcrumb', [{ name: `Bearbeitungshistorie aller Themen - Seite ${page}`, url: `/Historie/Themen/${page}` }])
})

const route = useRoute()

onBeforeMount(() => {
    if (route.params.page != null) {
        pageNumber.value = parseInt(route.params.page.toString())
    }
})

if (route.params.page != null)
    pageNumber.value = parseInt(route.params.page.toString())

const url = computed(() => {
    return `/apiVue/HistoryTopicAllTopicsOverview/Get?page=${pageNumber.value}`
})
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { $logger } = useNuxtApp()
const { data: days, status } = await useLazyFetch<Day[]>(url.value, {
    mode: 'cors',
    credentials: 'include',
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

watch(status, (val) => {
    if (val == 'pending')
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()
})

onMounted(() => {
    emit('setPage', Page.Default)
    if (status.value == 'pending')
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()

    emit('setBreadcrumb', [{ name: `Bearbeitungshistorie aller Themen - Seite ${pageNumber.value}`, url: `/Historie/Themen/${pageNumber.value}` }])
})


function handleClick(g: GroupedChanges) {
    if (g.changes.length > 1)
        g.collapsed = !g.collapsed
}

</script>

<template>
    <div class="container">
        <div class="main-page row">
            <div class="col-xs-12">
                <h1>Bearbeitungshistorie aller Themen</h1>
            </div>
            <div class="col-xs-12">
                <div class="category-change-day row" v-if="days" v-for="day in days">
                    <div class="col-xs-12">
                        <h3>{{ day.date }}</h3>
                    </div>
                    <div class="colx-xs-12">

                        <template v-if="day.groupedChanges != null" v-for="g, gcIndex in day.groupedChanges">

                            <TopicHistoryAllChange :change="g.changes[0]" :group-index="gcIndex"
                                :class="{ 'is-group': g.changes.length > 1 }"
                                :is-last="gcIndex == day.groupedChanges.length - 1 && g.collapsed"
                                @click="handleClick(g)" :first-edit-id="g.changes[g.changes.length - 1].revisionId">
                                <template v-slot:extras v-if="g.changes.length > 1">
                                    <font-awesome-icon v-if="g.collapsed" :icon="['fas', 'chevron-down']" />
                                    <font-awesome-icon v-else :icon="['fas', 'chevron-up']" />
                                </template>
                            </TopicHistoryAllChange>
                            <div v-if="g.changes.length > 1 && !g.collapsed">
                                <TopicHistoryAllChange v-for="c, i in g.changes" :change="c" :group-index="i"
                                    :is-last="i == g.changes.length - 1" />
                            </div>

                        </template>
                    </div>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="pager">
                    <button :disabled="pageNumber == 1" class="memo-button btn btn-default" @click="pageNumber--">Neuere
                        Revisionen</button>
                    <button class="memo-button btn btn-default" @click="pageNumber++">Ältere Revisionen</button>

                </div>
            </div>
        </div>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.is-group {
    cursor: pointer;
    background: white;

    &:hover {
        filter: brightness(0.95)
    }

    &:active {
        filter: brightness(0.85)
    }
}

.pager {
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>