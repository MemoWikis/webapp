<script lang="ts" setup>
import { VueElement } from 'vue'
import { useTopicStore } from '../topic/topicStore'
import _ from 'underscore'
import { PageType } from '../shared/pageTypeEnum'

const props = defineProps(['headerContainer', 'headerExtras'])
const topicStore = useTopicStore()
interface BreadcrumbItem {
    Name: string
    Id: number
}
class Breadcrumb {
    newWikiId: number = 0
    personalWiki: BreadcrumbItem | null = null
    items: BreadcrumbItem[] = []
    rootTopic: BreadcrumbItem | null = null
    currentTopic: BreadcrumbItem | null = null
    breadcrumbHasGlobalWiki: boolean = false
    isInPersonalWiki: boolean = false
}
const breadcrumb = ref(null as Breadcrumb | null)

const breadcrumbItems = ref([] as BreadcrumbItem[])
const stackedBreadcrumbItems = ref([] as BreadcrumbItem[])

const breadcrumbEl = ref(null as VueElement | null)
const breadcrumbWidth = ref('')

const hide = ref(true)
function startUpdateBreadcrumb() {
    hide.value = true
    updateBreadcrumb()
}

const personalWiki = ref(null as BreadcrumbItem | null)

const updateBreadcrumb = _.throttle(async () => {
    if (breadcrumbEl.value != null && breadcrumbEl.value.clientHeight != null) {
        const width = props.headerContainer.clientWidth - props.headerExtras.clientWidth - 30

        if (width > 0)
            breadcrumbWidth.value = `width: ${width}px`

        await nextTick()

        if (breadcrumbEl.value.clientHeight > 21) {
            shiftToStackedBreadcrumbItems()
        } else if (breadcrumbEl.value.clientHeight < 22) {
            insertToBreadcrumbItems()
            setTimeout(() => {
                if (breadcrumbEl.value && breadcrumbEl.value.clientHeight > 21) {
                    shiftToStackedBreadcrumbItems()
                }
            }, 200)
        }
        await nextTick()

        setTimeout(() => {
            if (breadcrumbEl.value && breadcrumbEl.value.clientHeight < 22)
                hide.value = false
        }, 200)
    }
}, 10)

function shiftToStackedBreadcrumbItems() {
    if (breadcrumbItems.value.length > 0)
        stackedBreadcrumbItems.value.push(breadcrumbItems.value.shift()!)

}
function insertToBreadcrumbItems() {
    if (stackedBreadcrumbItems.value.length > 0)
        breadcrumbItems.value.unshift(stackedBreadcrumbItems.value.pop()!)
}
const documentTitle = ref('')

onBeforeMount(() => {
    getBreadcrumb()
    setPageType()
})

onMounted(async () => {
    if (typeof window !== 'undefined') {
        window.addEventListener('resize', startUpdateBreadcrumb)
        window.addEventListener('scroll', startUpdateBreadcrumb)
    }
    await nextTick()
    startUpdateBreadcrumb()
    documentTitle.value = document.title
})

onBeforeUnmount(() => {
    if (typeof window !== 'undefined') {
        window.removeEventListener('resize', startUpdateBreadcrumb)
        window.removeEventListener('scroll', startUpdateBreadcrumb)
    }
})
const pageType = ref(PageType.Topic as PageType)
const route = useRoute()
watch(() => route.path, (newRoute, oldRoute) => {
    setPageType()
})

function setPageType() {
    if (route.params.topic != null)
        pageType.value = PageType.Topic
    else if (route.params.question != null)
        pageType.value = PageType.Question

    getBreadcrumb()

}
// const config = useRuntimeConfig()
async function getBreadcrumb() {
    var sessionStorage = window.sessionStorage

    if (topicStore.isWiki)
        sessionStorage.setItem('currentWikiId', topicStore.id.toString())
    var sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId')!)

    var currentWikiId = 0;
    if (!isNaN(sessionWikiId))
        currentWikiId = sessionWikiId

    const data = {
        wikiId: currentWikiId,
        currentCategoryId: topicStore.id,
    }

    if (pageType.value == PageType.Topic) {
        const result = await $fetch<Breadcrumb>(`/apiVue/Breadcrumb/GetBreadcrumb/`,
            {
                method: 'POST',
                body: data,
                // baseURL: process.client ? config.public.clientBase : config.public.serverBase,
                credentials: 'include',
                mode: 'no-cors',
            })

        breadcrumb.value = result
        personalWiki.value = result.personalWiki
        breadcrumbItems.value = result.items
        sessionStorage.setItem('currentWikiId', result.newWikiId.toString())
        updateBreadcrumb()

    } else {
        const result = await $fetch<BreadcrumbItem>(`/apiVue/Breadcrumb/GetPersonalWiki/`,
            {
                method: 'POST',
                body: data,
                // baseURL: process.client ? config.public.clientBase : config.public.serverBase,
                credentials: 'include',
                mode: 'no-cors',
            })

        personalWiki.value = result
    }
}


</script>

<template>
    <div v-if="breadcrumb != null && pageType == PageType.Topic" id="BreadCrumb" ref="breadcrumbEl"
        :style="breadcrumbWidth" :class="{ 'hide-breadcrumb': hide }">
        <NuxtLink :to="`/${encodeURI(breadcrumb.personalWiki.Name.replaceAll(' ', '-'))}/${breadcrumb.personalWiki.Id}`"
            class="breadcrumb-item" v-tooltip="breadcrumb.personalWiki.Name" v-if="breadcrumb.personalWiki">
            <font-awesome-icon icon="fa-solid fa-house" />
        </NuxtLink>
        <div
            v-if="breadcrumb.rootTopic && breadcrumb.currentTopic && breadcrumb.rootTopic.Id != breadcrumb.currentTopic.Id && breadcrumb.isInPersonalWiki">
            <font-awesome-icon icon="fa-solid fa-chevron-right" />
        </div>
        <template
            v-else-if="breadcrumb.rootTopic && breadcrumb.personalWiki && breadcrumb.rootTopic.Id != breadcrumb.personalWiki.Id && !breadcrumb.isInPersonalWiki">
            <div class="breadcrumb-divider"></div>
            <template v-if="topicStore.id != breadcrumb.rootTopic.Id">
                <NuxtLink
                    :to="`/${encodeURI(breadcrumb.rootTopic.Name.replaceAll(' ', '-'))}/${breadcrumb.rootTopic.Id}`"
                    class="breadcrumb-item" v-tooltip="breadcrumb.rootTopic.Name">
                    {{ breadcrumb.rootTopic.Name }}
                </NuxtLink>
                <font-awesome-icon icon="fa-solid fa-chevron-right" />
            </template>
        </template>
        <V-Dropdown v-if="stackedBreadcrumbItems.length > 0" :distance="0">
            <font-awesome-icon icon="fa-solid fa-ellipsis" class="breadcrumb-item" />
            <font-awesome-icon icon="fa-solid fa-chevron-right" />
            <template #popper>
                <ul>
                    <li v-for="s in stackedBreadcrumbItems">
                        <NuxtLink :to="`/${encodeURI(s.Name.replaceAll(' ', '-'))}/${s.Id}`" v-tooltip="s.Name">
                            {{ s.Name }}
                        </NuxtLink>
                    </li>
                </ul>
            </template>
        </V-Dropdown>
        <template v-for="b in breadcrumbItems">
            <NuxtLink :to="`/${encodeURI(b.Name.replaceAll(' ', '-'))}/${b.Id}`" class="breadcrumb-item"
                v-tooltip="b.Name">
                {{ b.Name }}
            </NuxtLink>
            <font-awesome-icon icon="fa-solid fa-chevron-right" />
        </template>

        <div class="breadcrumb-item last" v-tooltip="topicStore.name">
            {{ topicStore.name }}
        </div>

    </div>
    <div v-else-if="personalWiki != null" id="BreadCrumb" :style="breadcrumbWidth">
        <NuxtLink :to="`/${encodeURI(personalWiki.Name.replaceAll(' ', '-'))}/${personalWiki.Id}`"
            class="breadcrumb-item" v-tooltip="personalWiki.Name">
            <font-awesome-icon icon="fa-solid fa-house" />
        </NuxtLink>
        <div class="breadcrumb-divider"></div>
        <div class="breadcrumb-item last" v-tooltip="topicStore.name" v-if="pageType == PageType.Topic">
            {{ topicStore.name }}
        </div>
        <div class="breadcrumb-item last" v-tooltip="documentTitle" v-else>
            {{ documentTitle }}
        </div>
    </div>


</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#BreadCrumb {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    font-size: 14px;
    color: @memo-grey-dark;
    flex-wrap: wrap;
    opacity: 1;
    transition: opacity 0.5s;

    &.hide-breadcrumb {
        transition: opacity 0s;
        opacity: 0;
    }

    .breadcrumb-item {
        padding: 0 12px;
        max-width: 100px;
        text-overflow: ellipsis;
        overflow: hidden;
        cursor: pointer;

        &.last {
            max-width: 300px;
        }
    }

    .breadcrumb-divider {
        height: 60%;
        background: #ddd;
        max-width: 1px;
        border-radius: 4px;
        min-width: 1px;
    }
}
</style>