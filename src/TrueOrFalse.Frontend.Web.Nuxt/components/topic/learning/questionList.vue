<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import _ from 'underscore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { Tab, useTabsStore } from '../tabs/tabsStore'
import { useTopicStore } from '../topicStore'
import { useLearningSessionStore } from './learningSessionStore'

const learningSessionStore = useLearningSessionStore()
const tabsStore = useTabsStore()
const spinnerStore = useSpinnerStore()
const topicStore = useTopicStore()

const props = defineProps([
    'categoryId',
    'isAdmin',
    'expandQuestion',
    'activeQuestionId',
    'selectedPageFromActiveQuestion',
    'questionCount'])

const questions = ref([] as QuestionListItem[])

const itemCountPerPage = ref(25)

async function loadQuestions(page: number) {
    if (tabsStore.activeTab == Tab.Learning)
        spinnerStore.showSpinner()
    var result = await $fetch<any>('/apiVue/TopicLearningQuestionList/LoadQuestions/', {
        method: 'POST', body: {
            itemCountPerPage: itemCountPerPage.value,
            pageNumber: page,
            topicId: topicStore.id
        }, mode: 'cors', credentials: 'include'
    })
    if (result != null) {
        questions.value = result
        learningSessionStore.lastIndexInQuestionList = questions.value[questions.value.length - 1].SessionIndex
    }
    spinnerStore.hideSpinner()
}
const itemsPerPage = ref(24)
function loadPageWithSpecificQuestion() {
    if (learningSessionStore.currentIndex == 0)
        loadQuestions(1)
    const page = Math.ceil(learningSessionStore.currentIndex / itemsPerPage.value)
    currentPage.value = page
    if (page == 1)
        loadQuestions(1)
}

onBeforeMount(() => {
    learningSessionStore.$onAction(({ after, name }) => {
        if (name == 'startNewSession') {
            after((result) => {
                if (result) {
                    loadQuestions(1)
                }
            })
        } else if (name == 'startNewSessionWithJumpToQuestion') {
            after((result) => {
                if (result) {
                    loadPageWithSpecificQuestion()
                }
            })
        }
    })
})

const currentPage = ref(1)
watch(currentPage, (p) => loadQuestions(p))

learningSessionStore.$onAction(
    ({
        name,
        after,
    }) => {
        if (name == 'addNewQuestionToList')
            after((result) => {
                loadNewQuestion(result)
            })
    }
)

async function loadNewQuestion(index: number) {
    spinnerStore.showSpinner()

    var result = await $fetch<any>(`/apiVue/TopicLearningQuestionList/LoadNewQuestion/?index=${index}`, {
        mode: 'cors',
        credentials: 'include'
    })
    if (result != null) {
        questions.value.push(result)
        learningSessionStore.lastIndexInQuestionList = index + 1
    }
    spinnerStore.hideSpinner()
}
</script>

<template>
    <div class="col-xs-12" id="QuestionListComponent" v-show="!learningSessionStore.showResult">

        <TopicLearningQuestion v-for="(q, index) in questions" :question="q" :is-last-item="index == (questions.length - 1)"
            :session-index="q.SessionIndex" :expand-question="props.expandQuestion" :key="q.Id" />

        <TopicLearningQuickCreateQuestion @new-question-created="loadNewQuestion" />

        <div id="QuestionListPagination" class="pagination" v-show="questions.length > 0">

            <vue-awesome-paginate v-if="currentPage > 0" :total-items="learningSessionStore?.activeQuestionCount"
                :items-per-page="itemsPerPage" :max-pages-shown="5" v-model="currentPage" :show-ending-buttons="false"
                :show-breakpoint-buttons="false" prev-button-content="Vorherige" next-button-content="Nächste"
                first-page-content="Erste" last-page-content="Letzte" />
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