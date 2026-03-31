<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import type { BreadcrumbItem } from '~~/components/header/breadcrumbItems'

const { t } = useI18n()
const { $urlHelper, $logger } = useNuxtApp()
const config = useRuntimeConfig()

interface WikiListItem {
    id: number
    name: string
    imgUrl: string
    questionCount: number
    childPageCount: number
    popularity: number
    creatorName: string
    creatorId: number
}

interface WikisResult {
    wikis: WikiListItem[]
    totalCount: number
}

const currentPage = ref(1)
const pageSize = ref(20)

const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: wikisData, status } = await useFetch<WikisResult>('/apiVue/Wikis/Get', {
    query: {
        page: currentPage,
        pageSize: pageSize
    },
    credentials: 'include',
    mode: 'cors',
    watch: [currentPage],
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

const totalPages = computed(() => {
    if (!wikisData.value) {
        return 0
    }
    return Math.ceil(wikisData.value.totalCount / pageSize.value)
})

const emit = defineEmits(['setBreadcrumb', 'setPage'])

onBeforeMount(() => {
    emit('setPage', SiteType.Default)
})

onMounted(() => {
    const breadcrumbItem: BreadcrumbItem = {
        name: t('wikisPage.title'),
        url: '/wikis'
    }
    emit('setBreadcrumb', [breadcrumbItem])
})

useHead(() => ({
    title: t('wikisPage.meta.title'),
    meta: [
        {
            name: 'description',
            content: t('wikisPage.meta.description')
        }
    ]
}))
</script>

<template>
    <div class="wikis-page">
        <div class="wikis-container">
            <div class="wikis-header">
                <h1>{{ t('wikisPage.title') }}</h1>
                <p class="wikis-subtitle">{{ t('wikisPage.subtitle') }}</p>
            </div>

            <div v-if="wikisData && wikisData.wikis.length > 0" class="wikis-grid">
                <NuxtLink v-for="wiki in wikisData.wikis" :key="wiki.id" :to="$urlHelper.getPageUrl(wiki.name, wiki.id)"
                    class="wiki-card">
                    <div class="wiki-image-container">
                        <Image :src="wiki.imgUrl" class="wiki-image" :alt="wiki.name" />
                    </div>
                    <div class="wiki-info">
                        <div class="wiki-name">{{ wiki.name }}</div>
                        <div class="wiki-meta">
                            <span v-if="wiki.childPageCount > 0" class="wiki-stat">
                                {{ wiki.childPageCount }} {{ t('wikisPage.pages') }}
                            </span>
                            <span v-if="wiki.questionCount > 0" class="wiki-stat">
                                {{ wiki.questionCount }} {{ t('wikisPage.questions') }}
                            </span>
                        </div>
                        <div v-if="wiki.creatorName" class="wiki-creator">
                            {{ t('wikisPage.by') }} {{ wiki.creatorName }}
                        </div>
                    </div>
                </NuxtLink>
            </div>

            <div v-else-if="status !== 'pending'" class="wikis-empty">
                <p>{{ t('wikisPage.noWikis') }}</p>
            </div>

            <div v-if="totalPages > 1" class="wikis-pagination">
                <button class="pagination-btn" :disabled="currentPage <= 1" @click="currentPage = currentPage - 1">
                    <font-awesome-icon icon="fa-solid fa-chevron-left" />
                </button>
                <span class="pagination-info">{{ currentPage }} / {{ totalPages }}</span>
                <button class="pagination-btn" :disabled="currentPage >= totalPages"
                    @click="currentPage = currentPage + 1">
                    <font-awesome-icon icon="fa-solid fa-chevron-right" />
                </button>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.wikis-page {
    display: flex;
    justify-content: center;
    padding: 40px 20px;
}

.wikis-container {
    max-width: 1200px;
    width: 100%;
}

.wikis-header {
    margin-bottom: 32px;

    h1 {
        font-size: 28px;
        font-weight: 600;
        color: @memo-grey-darkest;
        margin-bottom: 4px;
    }

    .wikis-subtitle {
        font-size: 15px;
        color: @memo-grey-dark;
    }
}

.wikis-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
    gap: 16px;
}

.wiki-card {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 16px;
    border: 1px solid @memo-grey-light;
    border-radius: 8px;
    text-decoration: none;
    color: inherit;
    transition: border-color 0.15s, box-shadow 0.15s;

    &:hover {
        border-color: @memo-blue;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    }

    .wiki-image-container {
        flex-shrink: 0;

        .wiki-image {
            width: 48px;
            height: 48px;
            border-radius: 6px;
            object-fit: cover;
        }
    }

    .wiki-info {
        min-width: 0;

        .wiki-name {
            font-size: 15px;
            font-weight: 600;
            color: @memo-grey-darkest;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .wiki-meta {
            display: flex;
            gap: 12px;
            margin-top: 4px;

            .wiki-stat {
                font-size: 13px;
                color: @memo-grey-dark;
            }
        }

        .wiki-creator {
            font-size: 13px;
            color: @memo-grey-dark;
            margin-top: 2px;
        }
    }
}

.wikis-empty {
    text-align: center;
    padding: 60px 20px;
    color: @memo-grey-dark;
    font-size: 15px;
}

.wikis-pagination {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 16px;
    margin-top: 32px;

    .pagination-btn {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 36px;
        height: 36px;
        border: 1px solid @memo-grey-light;
        border-radius: 6px;
        background: white;
        cursor: pointer;
        color: @memo-grey-darkest;
        transition: border-color 0.15s;

        &:hover:not(:disabled) {
            border-color: @memo-blue;
        }

        &:disabled {
            opacity: 0.4;
            cursor: not-allowed;
        }
    }

    .pagination-info {
        font-size: 14px;
        color: @memo-grey-dark;
    }
}
</style>
