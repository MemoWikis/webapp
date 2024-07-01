<script lang="ts" setup>
import { TopicChangeType } from '~~/components/topic/history/topicChangeTypeEnum'
import { Change } from '~~/components/topic/history/change.vue'
import { Page } from '~/components/shared/pageEnum'
import { useSpinnerStore } from '~/components/spinner/spinnerStore'

const route = useRoute()

interface Day {
    date: string
    groupedChanges: GroupedChanges[]
}

interface GroupedChanges {
    collapsed: boolean
    changes: Change[]
}

interface HistoryResult {
    topicName: string
    days: Day[]
}
const { $logger } = useNuxtApp()

const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { status, data: historyResult } = await useLazyApi<HistoryResult>(`/apiVue/HistoryTopicOverview/Get/${route.params.id}`, {
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
const emit = defineEmits(['setBreadcrumb', 'setPage'])
watch(historyResult, (val) => {
    if (val)
        emit('setBreadcrumb', [{ name: `Bearbeitungshistorie von ${val.topicName}`, url: `/Historie/Thema/${route.params.id}` }])

})
const spinnerStore = useSpinnerStore()
watch(status, (val) => {
    if (val == 'pending')
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()
})
onMounted(async () => {
    if (status.value == 'pending')
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()
    emit('setPage', Page.Default)

    if (await historyResult.value != null)
        emit('setBreadcrumb', [{ name: `Bearbeitungshistorie von ${historyResult.value?.topicName}`, url: `/Historie/Thema/${route.params.id}` }])
})

function handleClick(g: GroupedChanges) {
    if (g.changes.length > 1)
        g.collapsed = !g.collapsed
}
</script>

<template>
    <div class="container">
        <div class="row main-page">
            <template v-if="historyResult">
                <div class="col-xs-12">
                    <h1>Bearbeitungshistorie '{{ historyResult.topicName }}'</h1>
                    <div>
                        <button class="memo-button btn btn-default link-to-all">
                            <NuxtLink to="/Historie/Themen">
                                Bearbeitungshistorie aller Themen
                            </NuxtLink>
                        </button>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="category-change-day row" v-for="day in historyResult.days" :key="day.date">
                        <div class="col-xs-12">
                            <h3>{{ day.date }}</h3>
                        </div>
                        <div class="col-xs-12">
                            <template v-for="g, gcIndex in day.groupedChanges">

                                <TopicHistoryChange :change="g.changes[0]" :group-index="gcIndex"
                                    :class="{ 'is-group': g.changes.length > 1 }"
                                    :is-last="gcIndex == day.groupedChanges.length - 1 && g.collapsed"
                                    @click="handleClick(g)" :first-edit-id="g.changes[g.changes.length - 1].revisionId">
                                    <template v-slot:extras v-if="g.changes.length > 1">
                                        <font-awesome-icon v-if="g.collapsed" :icon="['fas', 'chevron-down']" />
                                        <font-awesome-icon v-else :icon="['fas', 'chevron-up']" />
                                    </template>
                                </TopicHistoryChange>
                                <div v-if="g.changes.length > 1 && !g.collapsed">
                                    <TopicHistoryChange v-for="c, i in g.changes" :change="c" :group-index="i"
                                        :is-last="i == g.changes.length - 1" />
                                </div>
                            </template>

                        </div>
                    </div>
                </div>
            </template>

        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';


.placeholder {
    height: 60px;
    width: 100%;
    border: solid 1px @memo-grey-light;
}

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
</style>