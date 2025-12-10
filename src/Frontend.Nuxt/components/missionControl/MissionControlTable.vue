<script setup lang="ts">
import { Tab } from '../page/tabs/tabsStore'
import { PageData } from '~/composables/missionControl/pageData'

interface Props {
    pages: PageData[]
    noPagesText?: string
}

const props = defineProps<Props>()
const { t } = useI18n()
const { $urlHelper } = useNuxtApp()
const { getFormattedNumber } = useFormatNumber()

const sortKey = ref<keyof PageData>('popularity')
const sortDirection = ref<'asc' | 'desc'>('desc')
const currentPage = ref(1)
const itemsPerPage = ref(20)

const sortedpages = computed(() => {
    return [...props.pages].sort((a, b) => {
        if (sortKey.value === 'knowledgebarData') {
            const aValue = a.knowledgebarData?.knowledgeStatusPoints || 0
            const bValue = b.knowledgebarData?.knowledgeStatusPoints || 0
            return sortDirection.value === 'asc' ? aValue - bValue : bValue - aValue
        } else {
            const aValue = a[sortKey.value as keyof PageData]
            const bValue = b[sortKey.value as keyof PageData]

            if (typeof aValue === 'string' && typeof bValue === 'string') {
                return sortDirection.value === 'asc'
                    ? aValue.localeCompare(bValue)
                    : bValue.localeCompare(aValue)
            }

            if (typeof aValue === 'number' && typeof bValue === 'number') {
                return sortDirection.value === 'asc' ? aValue - bValue : bValue - aValue
            }

            return 0
        }
    })
})

const paginatedPages = computed(() => {
    const startIndex = (currentPage.value - 1) * itemsPerPage.value
    const endIndex = startIndex + itemsPerPage.value
    return sortedpages.value.slice(startIndex, endIndex)
})

const toggleSort = (key: keyof PageData) => {
    if (sortKey.value === key) {
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
    } else {
        sortKey.value = key
        sortDirection.value = key === 'popularity' ? 'desc' : 'asc' // Default to desc for popularity, asc for others
    }
}

const getSortIconClass = (key: keyof PageData) => {
    if (sortKey.value !== key) return 'sort-icon'
    return sortDirection.value === 'asc' ? 'sort-icon sort-asc' : 'sort-icon sort-desc'
}

const { isMobile } = useDevice()
</script>

<template>
    <div class="pages-table-container">
        <template v-if="pages.length">
            <table class="pages-table">
                <thead>
                    <tr>
                        <th class="sortable popularity-header" @click="toggleSort('popularity')">
                            {{ t('missionControl.pageTable.popularity') }}
                            <span :class="getSortIconClass('popularity')">
                                <font-awesome-icon v-if="sortKey === 'popularity'" :icon="['fas', sortDirection === 'asc' ? 'sort-up' : 'sort-down']" />
                                <font-awesome-icon v-else :icon="['fas', 'sort']" />
                            </span>
                        </th>
                        <th class="sortable name-header" @click="toggleSort('name')">
                            {{ t('missionControl.pageTable.name') }}
                            <span :class="getSortIconClass('name')">
                                <font-awesome-icon v-if="sortKey === 'name'" :icon="['fas', sortDirection === 'asc' ? 'sort-up' : 'sort-down']" />
                                <font-awesome-icon v-else :icon="['fas', 'sort']" />
                            </span>
                        </th>
                        <th class="sortable questioncount-header" @click="toggleSort('questionCount')">
                            {{ t('missionControl.pageTable.questions') }}
                            <span :class="getSortIconClass('questionCount')">
                                <font-awesome-icon v-if="sortKey === 'questionCount'" :icon="['fas', sortDirection === 'asc' ? 'sort-up' : 'sort-down']" />
                                <font-awesome-icon v-else :icon="['fas', 'sort']" />
                            </span>
                        </th>
                        <th class="sortable knowledgestatus-header" @click="toggleSort('knowledgebarData')">
                            {{ t('missionControl.pageTable.knowledgeStatus') }}
                            <span :class="getSortIconClass('knowledgebarData')">
                                <font-awesome-icon v-if="sortKey === 'knowledgebarData'" :icon="['fas', sortDirection === 'asc' ? 'sort-up' : 'sort-down']" />
                                <font-awesome-icon v-else :icon="['fas', 'sort']" />
                            </span>
                        </th>
                        <th class="actions-column"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(page, index) in paginatedPages" :key="page.id" class="page-row" :class="{ last: index === paginatedPages.length - 1 }">
                        <td class="popularity-cell">
                            <span class="popularity-count">{{ getFormattedNumber(page.popularity) }}</span>
                        </td>
                        <td class="page-name-cell">
                            <div class="page-name">
                                <div class="page-image" v-if="page.imgUrl">
                                    <Image
                                        :src="page.imgUrl"
                                        :alt="page.name"
                                        :width="40"
                                        :height="40"
                                        :customStyle="'object-fit: cover; border-radius: 6px;'" />
                                </div>
                                <div class="page-image" v-else>
                                    <Image
                                        src="/Images/Placeholders/placeholder-page-206.png"
                                        :alt="page.name"
                                        :width="40"
                                        :height="40"
                                        :customStyle="'object-fit: cover; border-radius: 6px;'" />
                                </div>
                                <NuxtLink :to="$urlHelper.getPageUrl(page.name, page.id)" class="page-link">
                                    {{ page.name }}
                                </NuxtLink>
                            </div>
                        </td>
                        <td class="question-count-cell">
                            {{ page.questionCount }}
                        </td>
                        <td class="knowledge-status-cell">
                            <PageContentGridKnowledgebar
                                v-if="page.knowledgebarData"
                                :knowledgebarData="page.knowledgebarData" />
                        </td>
                        <td class="actions-cell">
                            <NuxtLink
                                v-if="page.questionCount > 0"
                                :to="{ path: $urlHelper.getPageUrl(page.name, page.id, Tab.Learning), query: { inWishKnowledge: (page.knowledgebarData != null).toString() } }"
                                class="action-button"
                                :title="t('missionControl.pageTable.learnNow')"
                                v-tooltip="t('missionControl.pageTable.learnNow')">
                                <font-awesome-icon :icon="['fas', 'play']" />
                            </NuxtLink>
                        </td>
                    </tr>
                </tbody>
            </table>

            <div class="pagination-container" v-if="sortedpages.length > itemsPerPage">
                <vue-awesome-paginate
                    v-if="currentPage > 0"
                    :total-items="sortedpages.length"
                    :items-per-page="itemsPerPage"
                    :max-pages-shown="isMobile ? 3 : 10"
                    v-model="currentPage"
                    :show-ending-buttons="true"
                    :show-breakpoint-buttons="false">
                    <template #first-page-button>
                        <font-awesome-layers>
                            <font-awesome-icon :icon="['fas', 'chevron-left']" transform="left-3" />
                            <font-awesome-icon :icon="['fas', 'chevron-left']" transform="right-3" />
                        </font-awesome-layers>
                    </template>
                    <template #prev-button>
                        <font-awesome-icon :icon="['fas', 'chevron-left']" />
                    </template>
                    <template #next-button>
                        <font-awesome-icon :icon="['fas', 'chevron-right']" />
                    </template>
                    <template #last-page-button>
                        <font-awesome-layers>
                            <font-awesome-icon :icon="['fas', 'chevron-right']" transform="left-3" />
                            <font-awesome-icon :icon="['fas', 'chevron-right']" transform="right-3" />
                        </font-awesome-layers>
                    </template>
                </vue-awesome-paginate>
            </div>
        </template>

        <div v-else class="no-pages">
            <p>{{ noPagesText ? noPagesText : t('missionControl.pageTable.noPages') }}</p>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pages-table-container {
    .no-pages {
        text-align: center;
        color: @memo-grey-dark;
        font-style: italic;
        padding: 20px 0;

        p {
            margin-bottom: 0;
        }
    }

    .pagination-container {
        margin-top: 24px;
        display: flex;
        justify-content: center;

        :deep(.paginate-buttons) {
            background: white;
            color: @memo-grey-dark;
            font-size: 1.4rem;
            padding: 1rem 1.6rem;

            &.active-page {
                color: @memo-blue;
            }

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }
        }

        :deep(li:first-child) {
            border-top-left-radius: 2em;
            border-bottom-left-radius: 2em;
            overflow: hidden;
        }

        :deep(li:last-child) {
            border-top-right-radius: 2em;
            border-bottom-right-radius: 2em;
            overflow: hidden;
        }
    }
}

.pages-table {
    width: 100%;
    border-collapse: separate;
    border-spacing: 0;
    font-size: 14px;

    th,
    td {
        padding: 12px 16px;
        text-align: left;
        vertical-align: middle;
        border-bottom: 1px solid @memo-grey-lighter;
    }

    th {
        font-weight: 600;
        color: @memo-grey-darker;
        position: sticky;
        top: 0;
        z-index: 1;
        text-overflow: none;
        white-space: nowrap;
        background: white;
        text-align: center;

        &.sortable {
            cursor: pointer;
            user-select: none;

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }
        }

        &.popularity-header {
            width: 120px;
            max-width: 120px;
            text-align: center;
        }

        &.name-header {
            flex: 1;
            text-align: left;
        }

        &.questioncount-header {
            width: 150px;
            max-width: 150px;
            text-align: center;
        }

        &.knowledgestatus-header {
            width: 180px;
            min-width: 180px;
        }

        &.actions-column {
            width: 60px;
            text-align: center;
            max-width: 60px;
        }
    }

    .sort-icon {
        display: inline-block;
        margin-left: 4px;
        position: relative;
        opacity: 0.3;
    }

    .sort-asc,
    .sort-desc {
        opacity: 1;
    }

    .page-row {
        transition: background-color 0.15s ease;

        &:hover {
            background-color: @memo-grey-lightest;
        }

        &.last {

            td {
                border-bottom: none;
            }
        }
    }

    .popularity-cell {
        width: 120px;
        max-width: 120px;
        text-align: center;
    }

    .page-name-cell {
        flex: 1;
    }

    .page-name {
        display: flex;
        align-items: center;
        gap: 12px;

        .page-image {
            flex-shrink: 0;
        }

        .page-link {
            color: @memo-blue;
            font-weight: 500;
            text-decoration: none;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 1;
            line-clamp: 1;
            -webkit-box-orient: vertical;

            &:hover {
                text-decoration: underline;
            }
        }
    }

    .question-count-cell {
        width: 150px;
        max-width: 150px;
        text-align: center;
        font-weight: 600;
    }

    .knowledge-status-cell {
        width: 180px;
        min-width: 180px;
        padding-right: 24px;
    }

    .actions-cell {
        width: 60px;
        text-align: center;
    }

    .action-button {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        width: 36px;
        height: 36px;
        border-radius: 50%;
        background-color: @memo-green;
        color: white;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
            background-color: darken(@memo-green, 10%);
        }

        &:active {
            background-color: darken(@memo-green, 20%);
        }

        .fa-play {
            font-size: 14px;
            padding-left: 2px;
        }
    }
}

.popularity-count {
    background-color: @light-blue;
    color: @dark-blue;
    padding: 4px 8px;
    border-radius: 12px;
    font-size: 0.9em;
    font-weight: 600;
}

@media (max-width: 767px) {
    .pages-table {

        th,
        td {
            padding: 10px 8px;
        }

        .page-name {
            gap: 8px;
        }
    }
}
</style>
