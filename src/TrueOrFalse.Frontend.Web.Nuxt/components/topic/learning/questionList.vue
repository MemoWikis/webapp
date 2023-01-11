<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import _ from 'underscore'
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

const pageIsLoading = ref(false)
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
    pageIsLoading.value = false
}

async function loadQuestions(page: Number) {
    pageIsLoading.value = true
    var result = await $fetch<any>('/apiVue/TopicLearningQuestionList/LoadQuestions/', {
        method: 'POST', body: {
            itemCountPerPage: itemCountPerPage.value,
            pageNumber: page,
        }, mode: 'cors', credentials: 'include'
    })
    if (result != null) {
        questions.value = result
        updatePageCount(selectedPage.value)
        emit('updateQuestionCount')
    }
}

onMounted(() => {
    loadQuestions(1)
})
function loadPreviousQuestions() {
    if (selectedPage.value != 1)
        loadQuestions(selectedPage.value - 1)
}

function loadNextQuestions() {
    if (selectedPage.value != pageArray.value.length)
        loadQuestions(selectedPage.value + 1)
}
</script>

<template>
    <div class="col-xs-12 questionListComponent" id="QuestionListComponent">

        <TopicLearningQuestion v-for="(q, index) in questions" :question="q"
            :is-last-item="index == (questions.length - 1)" :session-index="index"
            :expand-question="props.expandQuestion" />

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
    margin-top: 103px;
    background-color: @memo-grey-lighter;
    padding: 0px 0px 33px 20px;
    margin-right: 0;
    margin-left: 0;

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

    @media(max-width: @screen-xxs-max) {
        padding-left: 0;
        padding-right: 0;
    }

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
            margin-right: 28px;

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

    .singleQuestionRow {
        background: linear-gradient(to right, @memo-grey-light 0px, @memo-grey-light 8px, #ffffffff 9px, #ffffffff 100%);
        font-family: "Open Sans";
        font-size: 14px;
        border: 0.05px solid @memo-grey-light;
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        transition: all .2s ease-out;
        margin-right: 20px;

        @media(max-width: @screen-xxs-max) {
            margin-right: 0;
        }

        &.open {
            height: unset;
            margin-top: 20px;
            margin-bottom: 20px;
            transition: all .2s ease-out;
            box-shadow: 0px 1px 6px 0px #C4C4C4;
        }

        &.solid {
            background: linear-gradient(to right, @memo-green 0px, @memo-green 8px, #ffffffff 8px, #ffffffff 100%);
        }

        &.shouldConsolidate {
            background: linear-gradient(to right, #FDD648 0px, #FDD648 8px, #ffffffff 8px, #ffffffff 100%);
        }

        &.shouldLearn {
            background: linear-gradient(to right, @memo-salmon 0px, @memo-salmon 8px, #ffffffff 8px, #ffffffff 100%);
        }

        &.inWishknowledge {
            background: linear-gradient(to right, #949494 0px, #949494 8px, #ffffffff 8px, #ffffffff 100%);
        }

        .knowledgeState {
            width: 8px;
            min-width: 8px;
            z-index: 10;
            position: relative;
        }

        .questionSectionFlex {
            width: 100%;

            .questionSection {
                display: flex;
                flex-wrap: nowrap;
                width: 100%;
            }

            .questionImg {
                min-width: 75px;
                height: 100%;
                padding: 8px 0 8px 32px;

                @media(max-width: @screen-xxs-max) {
                    display: none;
                }
            }

            .questionContainer {
                width: 100%;

                .questionBodyTop {
                    display: flex;
                    max-width: 100%;

                    .questionContainerTopSection {
                        flex: 1 1 100%;
                        min-width: 0;
                        padding-right: 0;

                        .questionHeader {
                            display: flex;
                            flex-wrap: nowrap;
                            justify-content: space-between;
                            min-width: 0;
                            min-height: 57px;

                            &:hover {
                                cursor: pointer;
                            }

                            .questionTitle {
                                padding: 8px;
                                min-width: 0;
                                color: @memo-grey-darker;
                                font-weight: 600;
                                align-self: center;
                                display: inline-flex;

                                .privateQuestionIcon {
                                    padding-left: 8px;
                                    padding-right: 8px;
                                }
                            }

                            @media (max-width: 640px) {
                                .col-xs-3 {
                                    width: 33%;
                                }

                                .col-xs-9 {
                                    width: 66%;
                                }
                            }

                            @media (max-width: @screen-sm-min) {
                                .col-xs-3 {
                                    width: 50%;
                                }

                                .col-xs-9 {
                                    width: 50%;
                                }
                            }
                        }

                        .questionHeaderIcons {
                            flex: 0 0 auto;
                            font-size: 18px;
                            min-width: 77px;
                            display: flex;
                            flex-wrap: nowrap;
                            color: @memo-grey-light;
                            flex-direction: row-reverse;
                            padding: 0;

                            .iAdded,
                            .iAddedNot {
                                padding: 0;
                            }

                            .fa-heart,
                            .fa-spinner,
                            .fa-heart-o {
                                font-size: 18px;
                                padding-top: 18px;
                            }

                            .fa-spinner,
                            .fa-play {
                                padding-right: 10px;
                            }

                            .iconContainer {
                                padding: 8px 8px 0px 8px;
                                min-width: 40px;
                                // height: 57px;
                                width: 40px;
                                max-width: 40px;
                                text-align: center;

                                .fa-play {
                                    margin-top: 10px;
                                }

                                .rotateIcon {
                                    transition: all .2s ease-out;
                                    line-height: 41px;

                                    &.open {
                                        transform: rotate(180deg);
                                    }
                                }
                            }

                            @media (max-width: 767px) {
                                .iconContainer {
                                    padding-right: 2px;
                                    padding-left: 2px;
                                }
                            }
                        }

                        @media(max-width: @screen-xxs-max) {
                            padding-left: 20px;
                        }
                    }

                    .extendedQuestionContainer {
                        padding: 0 0 8px 0;

                        @media (max-width: 550px) {
                            padding: 8px 8px 8px 4px;
                        }

                        .extendedAnswer {
                            padding-top: 16px;
                        }

                        .notes {
                            padding-top: 16px;
                            padding-bottom: 8px;
                            font-size: 12px;

                            a {
                                cursor: pointer;
                            }

                            .relatedCategories {
                                padding-bottom: 16px;
                            }

                            .author {}

                            .sources {
                                overflow-wrap: break-word;
                            }
                        }
                    }
                }

                .questionBodyBottom {
                    display: flex;
                    justify-content: space-between;
                    padding-right: 10px;
                    padding-left: 72px;
                    align-items: center;

                    .questionDetails {
                        font-size: 12px;
                        padding-left: 24px;

                        #StatsHeader {
                            display: none;
                        }
                    }

                    .questionStats {
                        display: flex;
                        font-size: 11px;
                        padding-left: 12px;
                        padding-right: 12px;
                        width: 100%;

                        .answerCountFooter {
                            width: 260px;
                            padding-bottom: 16px;


                            @media(max-width: @screen-xxs-max) {
                                padding-right: 0px;
                            }
                        }

                        .probabilitySection {
                            padding-right: 10px;
                            display: flex;
                            justify-content: center;
                            padding-bottom: 16px;

                            span {
                                &.percentageLabel {
                                    font-weight: bold;
                                    color: @memo-grey-light;

                                    &.solid {
                                        color: @memo-green;
                                    }

                                    &.shouldConsolidate {
                                        color: #FDD648;
                                    }

                                    &.shouldLearn {
                                        color: @memo-salmon;
                                    }

                                    &.inWishknowledge {
                                        color: #949494;
                                    }
                                }

                                &.chip {
                                    padding: 1px 10px;
                                    border-radius: 20px;
                                    background: @memo-grey-light;
                                    color: @memo-grey-darker;
                                    white-space: nowrap;

                                    &.solid {
                                        background: @memo-green;
                                    }

                                    &.shouldConsolidate {
                                        background: #FDD648;
                                    }

                                    &.shouldLearn {
                                        background: @memo-salmon;
                                    }

                                    &.inWishknowledge {
                                        background: #949494;
                                        color: white;
                                    }
                                }
                            }

                            &.open {
                                height: unset;
                                margin-top: 20px;
                                margin-bottom: 20px;
                                transition: all .2s ease-out;
                                box-shadow: 0px 1px 6px 0px #C4C4C4;
                            }
                        }

                        @media(max-width: @screen-sm-min) {
                            flex-wrap: wrap;
                        }
                    }

                    .questionFooterIcons {
                        color: @memo-grey-dark;
                        font-size: 11px;
                        margin: 0;
                        display: flex;
                        align-items: center;
                        margin-top: -21px;


                        a.commentIcon {
                            text-decoration: none;
                            color: @memo-grey-dark;
                            font-size: 14px;
                            cursor: pointer;
                        }

                        .dropdown-menu {
                            text-align: left;
                        }

                        span {
                            line-height: 36px;
                            font-family: Open Sans;
                        }

                        .ellipsis {
                            padding-left: 16px;
                        }
                    }

                    @media (max-width: @screen-xxs-max) {
                        flex-wrap: wrap;
                        padding-left: 10px;
                        justify-content: flex-end;
                    }
                }
            }
        }

        .questionFooter {
            height: 52px;
            border-top: 0.5px solid @memo-grey-light;
            display: flex;
            flex-direction: row-reverse;
            font-size: 20px;

            .questionFooterLabel {
                padding: 16px;
                line-height: 20px;
            }
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

    @media(max-width: @screen-xxs-max) {
        margin-right: -10px;
        margin-left: -10px;
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
                    i.fa-lock {
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