<script lang="ts" setup>

import { useLearningSessionStore } from './learningSessionStore'

const props = defineProps([''])
const showFilter = ref(true)

const currentQuestionCount = ref(0)
const allQuestionCount = ref(0)

const topicHasNoQuestions = ref(true)
const showError = ref(false)

const questionsExpanded = ref(false)
function expandAllQuestions() {
    questionsExpanded.value = true
}

const learningSessionStore = useLearningSessionStore()

</script>

<template>
    <div>
        <TopicLearningSessionConfiguration v-if="showFilter">
            <slot>
                <div class="col-xs-12 drop-down-question-sort">
                    <div class="session-config-header">
                        <span class="hidden-xs">Du lernst</span>
                        <template v-if="currentQuestionCount == allQuestionCount">
                            <b>&nbsp;alle&nbsp;</b>
                        </template>
                        <template v-else>
                            <b>&nbsp;{{currentQuestionCount}}&nbsp;</b>
                        </template>
                        Fragen&nbsp;
                        <span class="hidden-xs">aus diesem Thema</span>
                        &nbsp;({{allQuestionCount}})
                    </div>
                    <div class="session-config-header" v-if="topicHasNoQuestions && showError">Leider hat dieses
                        Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.</div>

                    <div id="ButtonAndDropdown">
                        <div id="QuestionListHeaderDropDown" class="Button dropdown">
                            <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button"
                                data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                <i class="fa fa-ellipsis-v"></i>
                            </a>
                            <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                                <li onclick="eventBus.$emit('open-edit-question-modal', { categoryId: <%= Model.CategoryId %>, edit: false })"
                                    data-allowed="logged-in">
                                    <a>
                                        <div class="dropdown-icon">
                                            <i class="fa fa-plus-circle"></i>
                                        </div><span>Frage hinzufügen</span>
                                    </a>
                                </li>
                                <li v-if="questionsExpanded" @click="expandAllQuestions()" style="cursor: pointer">
                                    <a>
                                        <div class="dropdown-icon">
                                            <i class="fa fa-angle-double-up"></i>
                                        </div><span>Alle Fragen zuklappen</span>
                                    </a>
                                </li>
                                <li v-else @click="expandAllQuestions()" style="cursor: pointer">
                                    <a>
                                        <div class="dropdown-icon">
                                            <i class="fa fa-angle-double-down"></i>
                                        </div><span>Alle Fragen erweitern</span>
                                    </a>
                                </li>
                                <li style="cursor: pointer">
                                    <a data-allowed="logged-in" @click="learningSessionStore.startNewSession()">
                                        <div class="dropdown-icon">
                                            <i class="fa fa-play"></i>
                                        </div><span>Fragen jetzt lernen</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </slot>
        </TopicLearningSessionConfiguration>
        <div class="session-configurator col-xs-12" v-else>
            <div class="session-config-header">
                <div class="col-xs-12 drop-down-question-sort">
                    <div class="session-config-header">
                        Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

//Variables
@colorPagination: @memo-grey-lighter ;

//Less
#QuestionListApp {
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