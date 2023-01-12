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
    <div class="col-xs-12">
        <div id="QuestionListSection" class="row">
            <template v-if="showFilter">
                <TopicLearningSessionConfiguration>
                    <slot>
                        <div class="drop-down-question-sort">
                            <div class="session-config-header">
                                <span class="hidden-xs">Du lernst</span>
                                <template v-if="currentQuestionCount == allQuestionCount">
                                    <b>&nbsp;alle&nbsp;</b>
                                </template>
                                <template v-else>
                                    <b>&nbsp;{{ currentQuestionCount }}&nbsp;</b>
                                </template>
                                Fragen&nbsp;
                                <span class="hidden-xs">aus diesem Thema</span>
                                &nbsp;({{ allQuestionCount }})
                            </div>
                            <div class="session-config-header" v-if="(topicHasNoQuestions && showError == true)">Leider
                                hat
                                dieses
                                Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                            </div>

                            <div id="ButtonAndDropdown">
                                <div id="QuestionListHeaderDropDown" class="Button dropdown">
                                    <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis"
                                        type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
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
                                        <li v-if="questionsExpanded" @click="expandAllQuestions()"
                                            style="cursor: pointer">
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
                <TopicLearningQuestionList />
            </template>

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
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

//Variables
@colorPagination: @memo-grey-lighter ;

//Less
#QuestionListSection {

    margin-top: 103px;
    background-color: @memo-grey-lighter;
    padding: 0px 0px 33px 20px;
    margin-right: 0;
    margin-left: 0;
    margin-bottom: 46px;

    @media(max-width: @screen-xxs-max) {
        padding-left: 0;
        padding-right: 0;
    }

}
</style>