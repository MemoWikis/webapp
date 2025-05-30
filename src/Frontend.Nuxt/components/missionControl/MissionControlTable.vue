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

const sortKey = ref<keyof PageData>('name')
const sortDirection = ref<'asc' | 'desc'>('asc')

const sortedpages = computed(() => {
    return [...props.pages].sort((a, b) => {
        if (sortKey.value === 'knowledgebarData') {
            const aValue = a.knowledgebarData?.solidPercentage || 0
            const bValue = b.knowledgebarData?.solidPercentage || 0
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

function toggleSort(key: keyof PageData) {
    if (sortKey.value === key) {
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
    } else {
        sortKey.value = key
        sortDirection.value = 'asc'
    }
}

function getSortIconClass(key: keyof PageData) {
    if (sortKey.value !== key) return 'sort-icon'
    return sortDirection.value === 'asc' ? 'sort-icon sort-asc' : 'sort-icon sort-desc'
}
</script>

<template>
    <div class="pages-table-container">
        <table v-if="pages.length" class="pages-table">
            <thead>
                <tr>
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
                <tr v-for="(page, index) in sortedpages" :key="page.id" class="page-row" :class="{ last: index === sortedpages.length - 1 }">
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
                            :to="{ path: $urlHelper.getPageUrl(page.name, page.id, Tab.Learning), query: { inWuWi: (page.knowledgebarData != null).toString() } }"
                            class="action-button"
                            :title="t('missionControl.pageTable.learnNow')"
                            v-tooltip="t('missionControl.pageTable.learnNow')">
                            <font-awesome-icon :icon="['fas', 'play']" />
                        </NuxtLink>
                    </td>
                </tr>
            </tbody>
        </table>
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

        &.name-header {
            min-width: 50%;
            max-width: 50%;
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

    .page-name-cell {
        min-width: 50%;
        max-width: 50%;
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
