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

async function loadQuestions(page: Number) {
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
        updatePageCount(selectedPage.value)
        emit('updateQuestionCount')
    }
}
function preloadQuestions() {
    loadQuestions(1)
}
onBeforeMount(() => {
    // preloadQuestions()
})

onMounted(() => {
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

function selectPage(page: number) {

}
</script>

<template>
    <div class="col-xs-12" id="QuestionListComponent">

        <TopicLearningQuestion v-for="(q, index) in questions" :question="q"
            :is-last-item="index == (questions.length - 1)" :session-index="index"
            :expand-question="props.expandQuestion" />

        <!-- <vue-awesome-paginate :total-items="50" :items-per-page="5" :max-pages-shown="5" v-model="pageArray.length"
            :on-click="selectPage" /> -->

        <div id="QuestionListPagination" v-show="questions.length > 0">
            <ul class="pagination col-xs-12 row justify-content-xs-center" v-if="pageArray.length <= 8">
                <li class="page-item page-btn" :class="{ disabled: selectedPage == 1 }">
                    <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                </li>
                <li class="page-item" v-for="(p, key) in pageArray" @click="loadQuestions(p)"
                    :class="{ selected: selectedPage == p }">
                    <span class="page-link">{{ p }}</span>
                </li>
                <li class="page-item page-btn" :class="{ disabled: selectedPage == pageArray.length }">
                    <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                </li>
            </ul>

            <ul class="pagination col-xs-12 row justify-content-xs-center" v-else>
                <li class="page-item col-auto page-btn" :class="{ disabled: selectedPage == 1 }">
                    <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                </li>
                <li class="page-item col-auto" @click="loadQuestions(1)" :class="{ selected: selectedPage == 1 }">
                    <span class="page-link">1</span>
                </li>
                <li class="page-item col-auto" v-show="selectedPage == 5">
                    <span class="page-link">2</span>
                </li>
                <li class="page-item col-auto" v-show="showLeftPageSelector" data-toggle="dropdown" aria-haspopup="true"
                    aria-expanded="true">
                    <span class="page-link" @click.this="showLeftSelectionDropUp = !showLeftSelectionDropUp">
                        <div class="dropup" @click.this="showLeftSelectionDropUp = !showLeftSelectionDropUp">
                            <div class="dropdown-toggle" type="button" id="DropUpMenuLeft" data-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false"
                                @click="showLeftSelectionDropUp = !showLeftSelectionDropUp">
                                ...
                            </div>
                            <ul id="DropUpMenuLeftList" class="pagination dropdown-menu"
                                aria-labelledby="DropUpMenuLeft" v-show="showLeftSelectionDropUp">
                                <li class="page-item" v-for="p in leftSelectorArray" @click="loadQuestions(p)">
                                    <span class="page-link">{{ p }}</span>
                                </li>
                            </ul>
                        </div>
                    </span>
                </li>
                <li class="page-item col-auto" v-for="(p, key) in centerArray" @click="loadQuestions(p)"
                    :class="{ selected: selectedPage == p }">
                    <span class="page-link">{{ p }}</span>
                </li>

                <li class="page-item col-auto" v-show="showRightPageSelector" data-toggle="dropdown"
                    aria-haspopup="true" aria-expanded="true">
                    <span class="page-link" @click.this="showRightSelectionDropUp = !showRightSelectionDropUp">
                        <div class="dropup" @click.this="showRightSelectionDropUp = !showRightSelectionDropUp">
                            <div class="dropdown-toggle" type="button" id="DropUpMenuRight" data-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false"
                                @click="showRightSelectionDropUp = !showRightSelectionDropUp">
                                ...
                            </div>
                            <ul id="DropUpMenuRightList" class="pagination dropdown-menu"
                                aria-labelledby="DropUpMenuLeft" v-show="showRightSelectionDropUp">
                                <li class="page-item" v-for="p in rightSelectorArray" @click="loadQuestions(p)">
                                    <span class="page-link">{{ p }}</span>
                                </li>
                            </ul>
                        </div>
                    </span>
                </li>
                <li class="page-item col-auto" v-show="selectedPage == pageArray.length - 4">
                    <span class="page-link">{{ pageArray.length - 1 }}</span>
                </li>
                <li class="page-item col-auto" @click="loadQuestions(pageArray.length)"
                    :class="{ selected: selectedPage == pageArray.length }">
                    <span class="page-link">{{ pageArray.length }}</span>
                </li>
                <li class="page-item col-auto page-btn" :class="{ disabled: selectedPage == pageArray.length }">
                    <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                </li>
            </ul>
        </div>
    </div>
</template>

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



    #AddInlineQuestionContainer {
        background: white;
        margin-top: 20px;
        padding: 22px 21px 30px;
        font-size: 16px;
        margin-right: 20px;

        @media(max-width: @screen-xxs-max) {
            margin-right: 0;
        }

        .add-inline-question-label {
            font-weight: 700;

            &.s-label {
                font-size: 14px;
                margin-bottom: 5px;
            }

            span,
            a {
                font-weight: 400;
            }

            a {
                cursor: pointer;
            }
        }

        #AddQuestionHeader {
            display: flex;

            .main-label {
                padding-bottom: 6px;
                flex: 1;
            }

            .heart-container {
                display: flex;
                align-content: center;
                align-items: center;
                flex-direction: column;
                width: 100px;
                margin-right: -22px;
                cursor: pointer;

                .Text {
                    padding: 0 3px !important;
                    font-size: 8px !important;
                    line-height: 14px !important;
                    font-weight: 600 !important;
                    text-transform: uppercase !important;
                    letter-spacing: 0.1em;
                    color: #FF001F !important;
                }
            }
        }

        #AddQuestionBody {
            display: flex;

            #AddQuestionFormContainer {
                flex: 1;

                @media(max-width: @screen-xxs-max) {
                    padding-right: 0;
                }

                .ProseMirror {
                    padding: 11px 15px 0;
                }

                .overline-s {
                    margin-top: 26px;
                }
            }

            #AddQuestionPrivacyContainer {
                padding: 20px;
                border: solid 1px @memo-grey-light;
                border-radius: 4px;
                min-width: 216px;
                max-height: 143px;
                margin-top: 24px;

                .form-check-label,
                form-check-radio {
                    cursor: pointer;
                }

                .form-check-label {
                    .fa-lock {
                        font-size: 14px;
                    }
                }
            }

            .btn-container {
                display: flex;
                justify-content: flex-end;

                @media(max-width: @screen-xxs-max) {
                    justify-content: center;
                    flex-wrap: wrap-reverse;
                }
            }
        }

        .wuwi-red {
            color: rgb(255, 0, 31);
        }

        .wuwi-grey {
            color: @memo-grey-dark;
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