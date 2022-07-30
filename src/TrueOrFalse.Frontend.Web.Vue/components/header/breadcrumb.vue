<script lang="ts" setup>
import { useTopicStore } from '../topic/topicStore';
import { ref } from 'vue'
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

async function getBreadcrumb() {
    var sessionStorage = window.sessionStorage;

    if (topicStore.isWiki)
        sessionStorage.setItem('currentWikiId', topicStore.id.toString());
    var sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId'));

    var currentWikiId = 0;
    if (!isNaN(sessionWikiId))
        currentWikiId = sessionWikiId;

    var data = {
        wikiId: currentWikiId,
        currentCategoryId: topicStore.id,
    }

    if (process.client) {
        breadcrumb.value = await $fetch<Breadcrumb>('/api/Breadcrumb/GetBreadcrumb/', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
    } else if (process.server) {
        const config = useRuntimeConfig()
        breadcrumb.value = await $fetch<Breadcrumb>('/Breadcrumb/GetBreadcrumb/', { method: 'POST', baseURL: config.apiBase, body: data, mode: 'cors', credentials: 'include' })
    }
    sessionStorage.setItem('currentWikiId', breadcrumb.value.newWikiId);
}

</script>

<template>
    <div id="BreadCrumb" v-if="breadcrumb != null">
        <NuxtLink :to="`/${encodeURIComponent(breadcrumb.personalWiki.Name)}/${breadcrumb.personalWiki.Id}`"
            class="breadcrumb-item" v-tooltip="breadcrumb.personalWiki.Name">
            <font-awesome-icon icon="fa-solid fa-house" />
        </NuxtLink>
        <div v-if="breadcrumb.rootTopic.Id != breadcrumb.currentTopic.Id && breadcrumb.isInPersonalWiki">
            <font-awesome-icon icon="fa-solid fa-chevron-right" />
        </div>
        <template v-else-if="breadcrumb.rootTopic.Id != breadcrumb.personalWiki.Id && !breadcrumb.isInPersonalWiki">
            <div>
                |
            </div>
            <template v-if="topicStore.id != breadcrumb.rootTopic.Id">
                <NuxtLink :to="`/${encodeURIComponent(breadcrumb.rootTopic.Name)}/${breadcrumb.rootTopic.Id}`"
                    class="breadcrumb-item" v-tooltip="breadcrumb.rootTopic.Name">
                    {{ breadcrumb.rootTopic.Name }}
                </NuxtLink>
                <font-awesome-icon icon="fa-solid fa-chevron-right" />
            </template>
        </template>

        <template v-for="b in breadcrumb.items">
            <NuxtLink :to="`/${encodeURIComponent(b.Name)}/${b.Id}`" class="breadcrumb-item" v-tooltip="b.Name">
                {{ b.Name }}
            </NuxtLink>
            <font-awesome-icon icon="fa-solid fa-chevron-right" />
        </template>

        <div class="breadcrumb-item" v-tooltip="topicStore.name">
            {{ topicStore.name }}
        </div>

    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#BreadCrumb {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    height: 100%;
    font-size: 14px;
    color: @memo-grey-dark;

    .breadcrumb-item {
        padding: 0 12px;
    }
}
</style>