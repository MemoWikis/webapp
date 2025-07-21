<script lang="ts" setup>
interface Question {
    id: number
    title: string
    knowledgeStatus: 'not-learned' | 'needs-learning' | 'needs-consolidation' | 'solid'
    popularity: number
    creationDate: string
    wikiId?: number
}

interface Props {
    questions: Question[]
    noQuestionsText?: string
}

const props = withDefaults(defineProps<Props>(), {
    noQuestionsText: 'No questions available'
})

const { t } = useI18n()
const { getFormattedNumber } = useFormatNumber()

const sortKey = ref<keyof Question>('popularity')
const sortDirection = ref<'asc' | 'desc'>('desc')

const sortedQuestions = computed(() => {
    return [...props.questions].sort((a, b) => {
        const aValue = a[sortKey.value]
        const bValue = b[sortKey.value]

        if (sortKey.value === 'creationDate') {
            const aDate = new Date(a.creationDate).getTime()
            const bDate = new Date(b.creationDate).getTime()
            return sortDirection.value === 'asc' ? aDate - bDate : bDate - aDate
        }

        if (typeof aValue === 'string' && typeof bValue === 'string') {
            return sortDirection.value === 'asc'
                ? aValue.localeCompare(bValue)
                : bValue.localeCompare(aValue)
        }

        if (typeof aValue === 'number' && typeof bValue === 'number') {
            return sortDirection.value === 'asc' ? aValue - bValue : bValue - aValue
        }

        return 0
    })
})

function toggleSort(key: keyof Question) {
    if (sortKey.value === key) {
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
    } else {
        sortKey.value = key
        sortDirection.value = key === 'popularity' ? 'desc' : 'asc' // Default to desc for popularity, asc for others
    }
}

function getSortIconClass(key: keyof Question) {
    if (sortKey.value !== key) return 'sort-icon'
    return sortDirection.value === 'asc' ? 'sort-icon sort-asc' : 'sort-icon sort-desc'
}

function formatDate(dateString: string): string {
    const date = new Date(dateString)
    const { locale } = useI18n()

    return date.toLocaleDateString(locale.value, {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    })
}

function learnQuestion(question: Question) {
    // TODO: Implement navigation to question learning
    console.log('Learning question:', question.id)
}
</script>

<template>
    <div class="question-list">
        <div v-if="questions.length === 0" class="no-questions">
            {{ noQuestionsText }}
        </div>

        <table v-else class="question-table">
            <thead>
                <tr>
                    <th class="sortable popularity-header" @click="toggleSort('popularity')">
                        {{ t('layout.questionList.popularity') }}
                        <span :class="getSortIconClass('popularity')">
                            <font-awesome-icon v-if="sortKey === 'popularity'" :icon="['fas', sortDirection === 'asc' ? 'sort-up' : 'sort-down']" />
                            <font-awesome-icon v-else :icon="['fas', 'sort']" />
                        </span>
                    </th>
                    <th class="sortable question-header" @click="toggleSort('title')">
                        {{ t('layout.questionList.question') }}
                        <span :class="getSortIconClass('title')">
                            <font-awesome-icon v-if="sortKey === 'title'" :icon="['fas', sortDirection === 'asc' ? 'sort-up' : 'sort-down']" />
                            <font-awesome-icon v-else :icon="['fas', 'sort']" />
                        </span>
                    </th>
                    <th class="sortable creation-date-header" @click="toggleSort('creationDate')">
                        {{ t('layout.questionList.creationDate') }}
                        <span :class="getSortIconClass('creationDate')">
                            <font-awesome-icon v-if="sortKey === 'creationDate'" :icon="['fas', sortDirection === 'asc' ? 'sort-up' : 'sort-down']" />
                            <font-awesome-icon v-else :icon="['fas', 'sort']" />
                        </span>
                    </th>
                    <th class="actions-column"></th>
                </tr>
            </thead>
            <tbody>
                <tr
                    v-for="(question, index) in sortedQuestions"
                    :key="question.id"
                    class="question-row"
                    :class="{ last: index === sortedQuestions.length - 1 }">
                    <td class="popularity-cell">
                        <span class="popularity-count">{{ getFormattedNumber(question.popularity) }}</span>
                    </td>

                    <td class="question-cell">
                        <span class="question-title">{{ question.title }}</span>
                    </td>

                    <td class="creation-date-cell">
                        <span class="date">{{ formatDate(question.creationDate) }}</span>
                    </td>

                    <td class="actions-cell">
                        <button
                            class="learn-button"
                            @click="learnQuestion(question)"
                            :title="t('layout.questionList.learnQuestion')">
                            <font-awesome-icon :icon="['fas', 'play']" />
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.question-list {
    width: 100%;
}

.no-questions {
    text-align: center;
    padding: 2rem;
    color: @memo-grey-dark;
    font-style: italic;
}

.question-table {
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

        &.question-header {
            flex: 1;
            text-align: left;
        }

        &.creation-date-header {
            width: 200px;
            max-width: 200px;
            text-align: center;
        }

        &.actions-column {
            width: 68px;
            text-align: center;
            max-width: 68px;
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

    .question-row {
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

    .question-cell {
        flex: 1;
    }

    .creation-date-cell {
        width: 200px;
        max-width: 200px;
        text-align: center;
    }

    .actions-cell {
        width: 68px;
        max-width: 68px;
        text-align: center;
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

.question-title {
    font-weight: 500;
    line-height: 1.4;
    color: @memo-grey-darker;
    text-decoration: none;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    line-clamp: 2;
    -webkit-box-orient: vertical;
}

.date {
    font-size: 0.9em;
    color: @memo-grey-dark;
}

.learn-button {
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

@media (max-width: 768px) {
    .question-table {

        th,
        td {

            &.creation-date-header,
            &.creation-date-cell {
                display: none;
            }
        }
    }
}
</style>
