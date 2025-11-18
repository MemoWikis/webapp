<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { Tab, useTabsStore } from '../tabs/tabsStore'
import { usePageStore } from '../pageStore'
import { useLearningSessionStore } from './learningSessionStore'
import { useDeleteQuestionStore } from '~/components/question/edit/delete/deleteQuestionStore'
import { AlertType, useAlertStore } from '~/components/alert/alertStore'

const learningSessionStore = useLearningSessionStore()
const tabsStore = useTabsStore()
const loadingStore = useLoadingStore()
const pageStore = usePageStore()
const deleteQuestionStore = useDeleteQuestionStore()
const alertStore = useAlertStore()
const { t } = useI18n()

interface Props {
    expandQuestion: boolean
    isWishknowledgeMode?: boolean
}
const props = defineProps<Props>()

const questions = ref<QuestionListItem[]>([])

const itemCountPerPage = ref(25)
const { $logger } = useNuxtApp()
async function loadQuestions(page: number) {
    if (tabsStore.activeTab === Tab.Learning)
        loadingStore.startLoading()

    let result
    if (props.isWishknowledgeMode) {
        // Use wishknowledge-specific API endpoint
        result = await $api<any>('/apiVue/WishknowledgeLearningQuestionList/LoadQuestions/', {
            method: 'POST',
            body: {
                itemCountPerPage: itemCountPerPage.value,
                pageNumber: page
            },
            mode: 'cors',
            credentials: 'include',
            onResponseError(context) {
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
            },
        })
    } else {
        // Use page-based API endpoint
        result = await $api<any>('/apiVue/PageLearningQuestionList/LoadQuestions/', {
            method: 'POST',
            body: {
                itemCountPerPage: itemCountPerPage.value,
                pageNumber: page,
                pageId: pageStore.id
            },
            mode: 'cors',
            credentials: 'include',
            onResponseError(context) {
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
            },
        })
    }

    if (result) {
        questions.value = result
        learningSessionStore.lastIndexInQuestionList = result.length > 0 ? questions.value[questions.value.length - 1].sessionIndex : 0
    }
    loadingStore.stopLoading()
}
const itemsPerPage = ref(25)
function loadPageWithSpecificQuestion() {
    const page = Math.ceil((learningSessionStore.currentIndex + 1) / itemsPerPage.value)
    currentPage.value = page

    if (page === 1 || learningSessionStore.currentIndex === 0)
        loadQuestions(1)
}

onBeforeMount(() => {
    learningSessionStore.$onAction(({ after, name }) => {
        if (name === 'startNewSession') {
            after(() => {
                loadQuestions(1)
            })
        } else if (name === 'startNewSessionWithJumpToQuestion') {
            after(() => {
                loadPageWithSpecificQuestion()
            })
        }
    })
})

const currentPage = ref(1)
watch(currentPage, (page) => loadQuestions(page))

learningSessionStore.$onAction(({ name, after }) => {
    if (name === 'addNewQuestionToList')
        after((result) => {
            loadNewQuestion(result)
        })

    if (name === 'addNewQuestionsToList')
        after((result) => {
            loadNewQuestions(result.startIndex, result.endIndex)
        })

    if (name === 'updateQuestionList')
        after((updatedQuestion) => {
            questions.value.forEach((question) => {
                if (question.id === updatedQuestion.id) {
                    question = updatedQuestion
                }
            })
        })
})

deleteQuestionStore.$onAction(({ name, after }) => {
    if (name === 'questionDeleted') {
        after((id) => {
            questions.value = questions.value.filter((q) => {
                return q.id != id
            })
        })
    }
})

async function loadNewQuestion(index: number) {
    loadingStore.startLoading()

    const url = props.isWishknowledgeMode ? `/apiVue/WishknowledgeLearningQuestionList/LoadNewQuestion/${index}` : `/apiVue/PageLearningQuestionList/LoadNewQuestion/${index}`

    const result = await $api<FetchResult<QuestionListItem>>(url, {
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
    loadingStore.stopLoading()

    if (result.success === true) {
        questions.value.push(result.data)
        learningSessionStore.lastIndexInQuestionList = index + 1
    } else {
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
    }
}

async function loadNewQuestions(startIndex: number, endIndex: number) {

    if (startIndex === endIndex) {
        return loadNewQuestion(startIndex)
    }

    if (props.isWishknowledgeMode) {
        // For wishknowledge mode, load questions one by one
        for (let i = startIndex; i <= endIndex; i++) {
            await loadNewQuestion(i)
        }
        return
    }

    loadingStore.startLoading()

    const result = await $api<FetchResult<QuestionListItem[]>>(`/apiVue/PageLearningQuestionList/LoadNewQuestions/`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        body: {
            startIndex: startIndex,
            endIndex: endIndex
        },
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
    loadingStore.stopLoading()

    if (result.success === true) {
        questions.value.push(...result.data)
        learningSessionStore.lastIndexInQuestionList = endIndex + 1
    } else {
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
    }
}
</script>

<template>
    <div id="QuestionListComponentContainer">
        <div class="" id="QuestionListComponent">

            <PageLearningQuestion v-for="(q, index) in questions" :question="q"
                :is-last-item="index === (questions.length - 1)" :session-index="q.sessionIndex"
                :expand-question="props.expandQuestion" :key="`${index}-${q.id}`" />

            <!-- Only show QuickCreateQuestion in page mode, not in wishknowledge mode -->
            <PageLearningQuickCreateQuestion v-if="!props.isWishknowledgeMode" @new-question-created="loadNewQuestion" />

            <div id="QuestionListPagination" class="pagination" v-show="questions.length > 0">

                <vue-awesome-paginate v-if="currentPage > 0" :total-items="learningSessionStore?.activeQuestionCount"
                    :items-per-page="itemsPerPage" :max-pages-shown="5" v-model="currentPage" :show-ending-buttons="false"
                    :show-breakpoint-buttons="false"
                    :prev-button-content="t('page.questionsSection.list.pagination.previous')"
                    :next-button-content="t('page.questionsSection.list.pagination.next')"
                    :first-page-content="t('page.questionsSection.list.pagination.first')"
                    :last-page-content="t('page.questionsSection.list.pagination.last')" />
            </div>

            <CommentModal />
        </div>
    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

#QuestionListPagination {
    .paginate-buttons {
        background: @memo-grey-lighter;
    }
}
</style>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#QuestionListComponentContainer {
    display: flex;
    justify-content: center;
    width: 100%;
}

//Variables
@colorPagination: @memo-grey-lighter ;

//Less
#QuestionListComponent {
    width: 100%;

    h4 {
        margin-bottom: 0;

        @media (max-width: 767px) {
            &.header {
                text-transform: capitalize;
            }
        }
    }

    #QuestionListHeaderDropDown {
        .btn {
            font-size: 18px;
            color: @memo-grey-dark;
            transition: all .1s ease-in-out;

            &:hover {
                color: @memo-blue;
                transition: all .1s ease-in-out;
            }
        }
    }

    #QuestionFooterIcons {
        .btn-link {
            color: @memo-grey-dark !important;
        }
    }

    .notShow {
        display: none !important;
    }

    .questionListHeader {
        .questionListTitle {
            font-size: 24px;
            padding: 8px;
        }

        .questionListFilter {
            font-size: 18px;
            padding: 8px 24px;
            text-align: right;
        }
    }

    #QuestionListPagination {
        padding-top: 32px;
        cursor: pointer;
        font-size: 12px;
        color: #949494;

        .pagination {
            display: flex;
            justify-content: center;
            flex-wrap: wrap;

            .page-item {
                width: 34px;


                &.page-btn {
                    width: unset;
                }

                &.disabled {
                    .page-link {
                        color: @memo-grey-light;
                    }
                }

                .page-link {
                    width: 100%;
                    text-align: center;
                    padding: 6px 4px !important;
                    background-color: @colorPagination;
                }
            }

            .selected {
                border-bottom: 3px solid @memo-blue;

                .page-link {
                    color: @memo-blue !important;
                }
            }

            .page-link {
                color: #949494;
            }
        }

        span {
            border: none;
        }
    }


    .ProseMirror,
    input,
    textarea,
    select {
        border: solid 1px @memo-grey-light;
        border-radius: 0;

        &.is-empty {
            border: solid 1px @memo-salmon;
        }
    }

    .is-empty {

        .ProseMirror {
            border: solid 1px @memo-salmon;
        }
    }

    input,
    .ProseMirror-focused,
    textarea {

        &:focus,
        &:focus-visible {
            outline: none !important;
            border: solid 1px @memo-green;
        }
    }
}
</style>