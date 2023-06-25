<script lang="ts" setup>
import { Page } from '~/components/shared/pageEnum'
import { useSpinnerStore } from '~/components/spinner/spinnerStore'
import { TopicChangeType } from '~/components/topic/history/topicChangeTypeEnum'
import { Change } from '~~/components/topic/history/all/change.vue'

const spinnerStore = useSpinnerStore()
interface Day {
    date: string
    changes: Change[]
    groupedChanges?: GroupedChanges[]
}

interface GroupedChanges {
    collapsed: boolean
    changes: Change[]
}
const emit = defineEmits(['setBreadcrumb', 'setPage'])

const page = ref(1)
watch(page, (page) => {
    history.pushState(null, `Bearbeitungshistorie aller Themen - Seite ${page}`, `/Historie/Themen/${page}`)
    emit('setBreadcrumb', [{ name: `Bearbeitungshistorie aller Themen - Seite ${page}`, url: `/Historie/Themen/${page}` }])
})

const url = computed(() => {
    return `/apiVue/HistoryTopicAllTopicsOverview/Get?page=${page.value}`
})
const route = useRoute()
onBeforeMount(() => {
    if (route.params.page != null)
        page.value = parseInt(route.params.page.toString())
})
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { $logger } = useNuxtApp()
const { pending, data: days } = await useLazyFetch<Day[]>(url, {
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
watch(pending, (val) => {
    if (val)
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()
})
onMounted(() => {
    emit('setPage', Page.Default)

    if (days.value != null) {
        if (days.value.length > 0)
            buildGroupedChanges(days.value)
    }
    emit('setBreadcrumb', [{ name: `Bearbeitungshistorie aller Themen - Seite ${page.value}`, url: `/Historie/Themen/${page.value}` }])
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


watch(days, (val) => {
    if (val != null && val.length > 0)
        buildGroupedChanges(val)
}, { deep: true })
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
                <div class="category-change-day row" v-if="days" v-for="day, dIndex in days">
                    <div class="col-xs-12">
                        <h3>{{ day.date }}</h3>
                    </div>
                    <div class="colx-xs-12">

                        <template v-if="day.groupedChanges != null" v-for="g, gcIndex in day.groupedChanges">

                            <TopicHistoryAllChange :change="g.changes[0]" :group-index="gcIndex"
                                :class="{ 'is-group': g.changes.length > 1 }"
                                :is-last="gcIndex == day.groupedChanges.length - 1 && g.collapsed" @click="handleClick(g)"
                                :first-edit-id="g.changes[g.changes.length - 1].revisionId">
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
                    <button :disabled="page == 1" class="pager-btn" @click="page--">Neuere Revisionen</button>
                    <button class="pager-btn" @click="page++">Ältere Revisionen</button>

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

    .pager-btn {
        border: solid 1px @memo-grey-light;
    }
}
</style>