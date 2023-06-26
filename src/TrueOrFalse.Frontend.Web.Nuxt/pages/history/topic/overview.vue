<script lang="ts" setup>
import { TopicChangeType } from '~~/components/topic/history/topicChangeTypeEnum'
import { Change } from '~~/components/topic/history/change.vue'
import { Page } from '~/components/shared/pageEnum'
import { useSpinnerStore } from '~/components/spinner/spinnerStore'

const route = useRoute()

interface Day {
    date: string
    changes: Change[]
    groupedChanges?: GroupedChanges[]
}

interface GroupedChanges {
    collapsed: boolean
    changes: Change[]
}

interface HistoryResult {
    topicName: string
    topicNameEncoded: string
    days: Day[]
}
const { $logger } = useNuxtApp()

const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { pending, data: historyResult } = await useLazyFetch<HistoryResult>(`/apiVue/HistoryTopicOverview/Get/${route.params.id}`, {
    mode: 'cors',
    credentials: 'include',
    onRequest({ options }) {
        if (process.server) {
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
watch(pending, (val) => {
    if (val)
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()
})
onMounted(async () => {
    emit('setPage', Page.Default)

    if (historyResult.value != null) {
        if (historyResult.value.days.length > 0)
            buildGroupedChanges(historyResult.value.days)
    }
    if (await historyResult.value != null)
        emit('setBreadcrumb', [{ name: `Bearbeitungshistorie von ${historyResult.value?.topicName}`, url: `/Historie/Thema/${route.params.id}` }])
})

function buildGroupedChanges(days: Day[]) {
    days.forEach((d) => {
        let currentGroupIndex = 0
        const newGroupedChange = {
            collapsed: true,
            changes: []
        }
        d.groupedChanges = [newGroupedChange]
        d.changes.forEach((c) => {
            if (d.groupedChanges != null && d.groupedChanges.length) {
                let currentGroupChanges = d.groupedChanges[currentGroupIndex].changes

                if (currentGroupChanges.length == 0) {
                    currentGroupChanges.push(c)

                } else {
                    if (currentGroupChanges[0].topicId == c.topicId &&
                        currentGroupChanges[0].topicChangeType == TopicChangeType.Text &&
                        currentGroupChanges[0].author.id == c.author.id) {
                        currentGroupChanges.push(c)
                    } else {
                        currentGroupIndex++
                        d.groupedChanges.push(newGroupedChange)
                    }
                }
            }
        })
    })
}


watch(historyResult, (val) => {
    if (val != null && val.days.length > 0)
        buildGroupedChanges(val.days)
}, { deep: true })
function handleClick(g: GroupedChanges) {
    if (g.changes.length > 1)
        g.collapsed = !g.collapsed
}
</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12" v-if="pending">
                Seite lädt
            </div>
            <template v-else-if="historyResult">
                <div class="col-xs-12">
                    <h1>{{ historyResult.topicName }}</h1>
                    <div>
                        <button class="memo-button btn btn-link link-to-all">
                            <NuxtLink to="/Historie/Themen">
                                Bearbeitungshistorie aller Themen
                            </NuxtLink>
                        </button>
                    </div>
                </div>
                <div class="col-xs-12">
                    <div class="category-change-day row" v-for="day, dIndex in historyResult.days">
                        <div class="col-xs-12">
                            <h3>{{ day.date }}</h3>
                        </div>
                        <div class="col-xs-12">
                            <template v-if="day.groupedChanges != null" v-for="g, gcIndex in day.groupedChanges">

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

.link-to-all {
    border: 1px solid @memo-grey-light;
    text-decoration: none;
}
</style>