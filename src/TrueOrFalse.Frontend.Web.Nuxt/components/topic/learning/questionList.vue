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

const selectedPage = ref(1)

const itemCountPerPage = ref(25)

const emit = defineEmits(['updateQuestionCount'])

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

onBeforeMount(() => {
    learningSessionStore.$onAction(({ after, name }) => {
        if (name == 'startNewSession') {
            after((result) => {
                if (result) {
                    loadQuestions(1)
                }
            })
        }
    })
})

const currentPage = ref(1)
watch(currentPage, (p) => loadQuestions(p))

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
    <div class="col-xs-12" id="QuestionListComponent">

        <TopicLearningQuestion v-for="(q, index) in questions" :question="q"
            :is-last-item="index == (questions.length - 1)" :session-index="index"
            :expand-question="props.expandQuestion" :key="q.Id" />

        <TopicLearningQuickCreateQuestion @new-question-created="loadNewQuestion" />

        <div id="QuestionListPagination" v-show="questions.length > 0">

            <vue-awesome-paginate :total-items="learningSessionStore?.activeQuestionCount" :items-per-page="24"
                :max-pages-shown="5" v-model="currentPage" :show-ending-buttons="true" :show-breakpoint-buttons="false"
                prev-button-content="Vorherige" next-button-content="Nächste" first-page-content="Erste"
                last-page-content="Letzte" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#QuestionListPagination {
    display: flex;
    justify-content: center;
    align-items: center;

    :deep(.pagination-container) {
        display: flex;
    }

    :deep(.paginate-buttons) {
        height: 30px;
        min-width: 30px;
        border-radius: 20px;
        cursor: pointer;
        color: @memo-grey-dark;
        background: @memo-grey-lighter;
    }

    :deep(.paginate-buttons:hover) {
        filter: brightness(0.85);
    }

    :deep(.paginate-buttons:active) {
        filter: brightness(0.5);
    }

    :deep(.active-page) {
        &::before {
            background-color: @memo-grey-darker;
        }

        color: @memo-grey-darker;
    }

    :deep(.active-page:hover) {
        color: @memo-blue;
    }

    :deep(.li:has(.active-page)) {
        background-color: green;
    }
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