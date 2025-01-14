<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { Tab, useTabsStore } from '../tabs/tabsStore'
import { usePageStore } from '../pageStore'
import { useLearningSessionStore } from './learningSessionStore'
import { useDeleteQuestionStore } from '~/components/question/edit/delete/deleteQuestionStore'
import { AlertType, messages, useAlertStore } from '~/components/alert/alertStore'

const learningSessionStore = useLearningSessionStore()
const tabsStore = useTabsStore()
const spinnerStore = useSpinnerStore()
const pageStore = usePageStore()
const deleteQuestionStore = useDeleteQuestionStore()
const alertStore = useAlertStore()

interface Props {
    expandQuestion: boolean
}
const props = defineProps<Props>()

const questions = ref<QuestionListItem[]>([])

const itemCountPerPage = ref(25)
const { $logger } = useNuxtApp()
async function loadQuestions(page: number) {
    if (tabsStore.activeTab == Tab.Learning)
        spinnerStore.showSpinner()

    const result = await $api<any>('/apiVue/PageLearningQuestionList/LoadQuestions/', {
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
    if (result) {
        questions.value = result
        learningSessionStore.lastIndexInQuestionList = result.length > 0 ? questions.value[questions.value.length - 1].sessionIndex : 0
    }
    spinnerStore.hideSpinner()
}
const itemsPerPage = ref(25)
function loadPageWithSpecificQuestion() {
    const page = Math.ceil((learningSessionStore.currentIndex + 1) / itemsPerPage.value)
    currentPage.value = page

    if (page == 1 || learningSessionStore.currentIndex == 0)
        loadQuestions(1)
}

onBeforeMount(() => {
    learningSessionStore.$onAction(({ after, name }) => {
        if (name == 'startNewSession') {
            after(() => {
                loadQuestions(1)
            })
        } else if (name == 'startNewSessionWithJumpToQuestion') {
            after(() => {
                loadPageWithSpecificQuestion()
            })
        }
    })
})

const currentPage = ref(1)
watch(currentPage, (p) => loadQuestions(p))

learningSessionStore.$onAction(({ name, after }) => {
    if (name == 'addNewQuestionToList')
        after((result) => {
            loadNewQuestion(result)
        })

    if (name == 'addNewQuestionsToList')
        after((result) => {
            loadNewQuestions(result.startIndex, result.endIndex)
        })

    if (name == 'updateQuestionList')
        after((updatedQuestion) => {
            questions.value.forEach((q) => {
                if (q.id == updatedQuestion.id) {
                    q = updatedQuestion
                }
            })
        })
})

deleteQuestionStore.$onAction(({ name, after }) => {
    if (name == 'questionDeleted') {
        after((id) => {
            questions.value = questions.value.filter((q) => {
                return q.id != id
            })
        })
    }
})

async function loadNewQuestion(index: number) {
    spinnerStore.showSpinner()

    const result = await $api<FetchResult<QuestionListItem>>(`/apiVue/PageLearningQuestionList/LoadNewQuestion/${index}`, {
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
    spinnerStore.hideSpinner()

    if (result.success == true) {
        questions.value.push(result.data)
        learningSessionStore.lastIndexInQuestionList = index + 1
    } else {
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
    }
}

async function loadNewQuestions(startIndex: number, endIndex: number) {

    if (startIndex === endIndex) {
        return loadNewQuestion(startIndex)
    }
    spinnerStore.showSpinner()

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
    spinnerStore.hideSpinner()

    if (result.success == true) {
        questions.value.push(...result.data)
        learningSessionStore.lastIndexInQuestionList = endIndex + 1
    } else {
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
    }
}


</script>

<template>
    <div id="QuestionListComponentContainer" class="col-xs-12">
        <div class="col-xs-12" id="QuestionListComponent" v-show="!learningSessionStore.showResult">

            <PageLearningQuestion v-for="(q, index) in questions" :question="q"
                :is-last-item="index == (questions.length - 1)" :session-index="q.sessionIndex"
                :expand-question="props.expandQuestion" :key="`${index}-${q.id}`" />

            <PageLearningQuickCreateQuestion @new-question-created="loadNewQuestion" />

            <div id="QuestionListPagination" class="pagination" v-show="questions.length > 0">

                <vue-awesome-paginate v-if="currentPage > 0" :total-items="learningSessionStore?.activeQuestionCount"
                    :items-per-page="itemsPerPage" :max-pages-shown="5" v-model="currentPage" :show-ending-buttons="false"
                    :show-breakpoint-buttons="false" prev-button-content="Vorherige" next-button-content="Nächste"
                    first-page-content="Erste" last-page-content="Letzte" />
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
}

.drop-down-question-sort {
    display: flex;
    flex-wrap: wrap;
    font-size: 18px;
    justify-content: space-between;
    padding-right: 0;
}

//Variables
@colorPagination: @memo-grey-lighter ;

//Less
#QuestionListComponent {
    max-width: 840px;

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

    // @media(max-width: @screen-xxs-max) {
    //     padding-left: 0;
    //     padding-right: 0;
    //     margin-right: -10px;
    //     margin-left: -10px;
    // }

    #QuestionFooterIcons {
        .btn-link {
            color: @memo-grey-dark !important;
        }
    }

    .drop-down-question-sort {
        display: flex;
        flex-wrap: wrap;
        font-size: 18px;
        justify-content: space-between;
        padding-right: 0;

        #ButtonAndDropdown {
            display: flex;
            align-items: center;
            margin-top: -10px;

            @media(max-width: 768px) {
                padding-left: 10px;
            }
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