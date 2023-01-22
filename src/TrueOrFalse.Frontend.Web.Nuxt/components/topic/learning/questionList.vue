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

const pages = ref(0)
const pageArray = ref([] as number[])
const selectedPage = ref(1)

const itemCountPerPage = ref(25)

const emit = defineEmits(['updateQuestionCount'])
const showLeftSelectionDropUp = ref(false)
const showRightSelectionDropUp = ref(false)
const questionCount = ref(0)

const hideLeftPageSelector = ref(false)
const hideRightPageSelector = ref(false)

const showLeftPageSelector = ref(false)
const showRightPageSelector = ref(false)

const centerArray = ref([] as number[])
const leftSelectorArray = ref([] as number[])
const rightSelectorArray = ref([] as number[])

function setPaginationRanges(sP: number) {
    if ((sP - 2) <= 2) {
        hideLeftPageSelector.value = true
    };
    if ((sP + 2) >= pageArray.value.length) {
        hideRightPageSelector.value = true
    };

    let leftArray = [];
    let cA = [];
    let rightArray = [];

    if (pageArray.value.length >= 8) {

        cA = _.range(sP - 2, sP + 3);
        cA = cA.filter(e => e >= 2 && e <= pageArray.value.length - 1)

        leftArray = _.range(2, cA[0]);
        rightArray = _.range(cA[cA.length - 1] + 1, pageArray.value.length)

        centerArray.value = cA
        leftSelectorArray.value = leftArray
        rightSelectorArray.value = rightArray

    } else {
        centerArray.value = pageArray.value
    }
}
async function updatePageCount(sP: number) {
    selectedPage.value = sP
    showLeftSelectionDropUp.value = false
    showRightSelectionDropUp.value = false

    if (typeof questions.value[0] != "undefined")
        pages.value = Math.ceil(questionCount.value / itemCountPerPage.value)
    else
        pages.value = 1

    await nextTick()
    setPaginationRanges(sP)
    if (tabsStore.activeTab == Tab.Learning)
        spinnerStore.hideSpinner()
}

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
    }
    spinnerStore.hideSpinner()
}
function preloadQuestions() {
    loadQuestions(1)
}
onBeforeMount(() => {
    // preloadQuestions()
})

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

function loadPreviousQuestions() {
    if (selectedPage.value != 1)
        loadQuestions(selectedPage.value - 1)
}

function loadNextQuestions() {
    if (selectedPage.value != pageArray.value.length)
        loadQuestions(selectedPage.value + 1)
}

const currentPage = ref(1)
watch(currentPage, (p) => loadQuestions(p))

</script>

<template>
    <div class="col-xs-12" id="QuestionListComponent">

        <TopicLearningQuestion v-for="(q, index) in questions" :question="q"
            :is-last-item="index == (questions.length - 1)" :session-index="index"
            :expand-question="props.expandQuestion" :key="q.Id" />

        <TopicLearningQuickCreateQuestion />

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

.pagination-container {
    display: flex;
    column-gap: 10px;
}

.paginate-buttons {
    height: 40px;
    width: 40px;
    border-radius: 20px;
    cursor: pointer;
    background-color: rgb(242, 242, 242);
    border: 1px solid rgb(217, 217, 217);
    color: black;
}

.paginate-buttons:hover {
    background-color: #d8d8d8;
}

.active-page {
    background-color: #3498db;
    border: 1px solid #3498db;
    color: white;
}

.active-page:hover {
    background-color: #2988c8;
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

    .activeQ {
        &::before {
            color: @memo-grey-darker;
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