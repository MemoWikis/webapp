<script lang="ts" setup>
import { useTopicStore } from '../topic/topicStore'
import { ref } from 'vue'
import _ from 'underscore'
import { PageType } from '../shared/pageTypeEnum'
import { useUserStore } from '../user/userStore'

const props = defineProps(['headerContainer', 'headerExtras', 'route'])
const topicStore = useTopicStore()
class BreadcrumbItem {
    Name: string
    Id: number
}
class Breadcrumb {
    newWikiId: number
    personalWiki: BreadcrumbItem
    items: BreadcrumbItem[]
    rootTopic: BreadcrumbItem
    currentTopic: BreadcrumbItem
    breadcrumbHasGlobalWiki: boolean
    isInPersonalWiki: boolean
}
const breadcrumb = ref(null)

onBeforeMount(async () => {
    getBreadcrumb()
})

const breadcrumbItems = ref([])
const stackedBreadcrumbItems = ref([])

const breadcrumbEl = ref(null)
const breadcrumbWidth = ref('')

const hide = ref(true)
function startUpdateBreadcrumb() {
    hide.value = true
    updateBreadcrumb()
}

const personalWiki = ref(null as BreadcrumbItem)
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
                if (breadcrumbEl.value.clientHeight > 21) {
                    shiftToStackedBreadcrumbItems()
                }
            }, 200)
        }
        await nextTick()

        setTimeout(() => {
            if (breadcrumbEl.value.clientHeight < 22)
                hide.value = false
        }, 200)
    }
}, 10)

function shiftToStackedBreadcrumbItems() {
    if (breadcrumbItems.value.length > 0)
        stackedBreadcrumbItems.value.push(breadcrumbItems.value.shift())

}
function insertToBreadcrumbItems() {
    if (stackedBreadcrumbItems.value.length > 0)
        breadcrumbItems.value.unshift(stackedBreadcrumbItems.value.pop())
}
const documentTitle = ref(null)

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
const pageType = useState<PageType>('page')

async function getBreadcrumb() {
    var sessionStorage = window.sessionStorage;

    if (topicStore.isWiki)
        sessionStorage.setItem('currentWikiId', topicStore.id.toString())
    var sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId'))

    var currentWikiId = 0;
    if (!isNaN(sessionWikiId))
        currentWikiId = sessionWikiId

    var data = {
        wikiId: currentWikiId,
        currentCategoryId: topicStore.id,
    }
    if (pageType.value == PageType.Topic) {
        if (process.client) {
            breadcrumb.value = await $fetch<Breadcrumb>('/api/Breadcrumb/GetBreadcrumb/', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
        } else if (process.server) {
            const config = useRuntimeConfig()
            breadcrumb.value = await $fetch<Breadcrumb>('/Breadcrumb/GetBreadcrumb/', { method: 'POST', baseURL: config.apiBase, body: data, mode: 'cors', credentials: 'include' })
        }
        personalWiki.value = breadcrumb.personalWiki
        breadcrumbItems.value = breadcrumb.value.items
        sessionStorage.setItem('currentWikiId', breadcrumb.value.newWikiId)
        updateBreadcrumb()
    } else {
        if (process.client) {
            personalWiki.value = await $fetch<BreadcrumbItem>('/api/Breadcrumb/GetPersonalWiki/', { method: 'POST', mode: 'cors', credentials: 'include' })
        } else if (process.server) {
            const config = useRuntimeConfig()
            personalWiki.value = await $fetch<BreadcrumbItem>('/Breadcrumb/GetPersonalWiki/', { method: 'POST', baseURL: config.apiBase, mode: 'cors', credentials: 'include' })
        }
    }
}

const userStore = useUserStore()
watch([() => topicStore.id, () => userStore.id], () => {
    getBreadcrumb()
})


</script>

<template>
    <div v-if="breadcrumb != null && pageType == PageType.Topic" id="BreadCrumb" ref="breadcrumbEl"
        :style="breadcrumbWidth" :class="{ 'hide-breadcrumb': hide }">
        <NuxtLink :to="`/${encodeURI(breadcrumb.personalWiki.Name.replaceAll(' ', '-'))}/${breadcrumb.personalWiki.Id}`"
            class="breadcrumb-item" v-tooltip="breadcrumb.personalWiki.Name">
            <font-awesome-icon icon="fa-solid fa-house" />
        </NuxtLink>
        <div v-if="breadcrumb.rootTopic.Id != breadcrumb.currentTopic.Id && breadcrumb.isInPersonalWiki">
            <font-awesome-icon icon="fa-solid fa-chevron-right" />
        </div>
        <template v-else-if="breadcrumb.rootTopic.Id != breadcrumb.personalWiki.Id && !breadcrumb.isInPersonalWiki">
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