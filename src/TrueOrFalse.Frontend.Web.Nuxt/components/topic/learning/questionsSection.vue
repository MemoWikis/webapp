<script lang="ts" setup>

import { useLearningSessionStore } from './learningSessionStore'
import { useTopicStore } from '../topicStore'
import { useUserStore } from '~~/components/user/userStore'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

const learningSessionStore = useLearningSessionStore()
const topicStore = useTopicStore()
const userStore = useUserStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()


const currentQuestionCount = ref(0)
const allQuestionCount = ref(0)

onBeforeMount(() => {
    allQuestionCount.value = topicStore.questionCount
    console.log(topicStore.questionCount)
    if (topicStore.questionCount > 0)
        learningSessionConfigurationStore.showFilter = true
    else
        learningSessionConfigurationStore.showFilter = false
})

watch(() => learningSessionStore.steps.length, (v) => {
    currentQuestionCount.value = learningSessionStore.steps.length
})
const topicHasNoQuestions = ref(true)
const showError = ref(false)

const questionsExpanded = ref(false)
function expandAllQuestions() {
    questionsExpanded.value = !questionsExpanded.value
}

function createQuestion() {

}
function getClass(): string {
    if (process.server)
        return ''
    else return !learningSessionConfigurationStore.showFilter ? 'no-questions' : ''
}

</script>

<template>
    <div id="QuestionListSection" class="row" :class="getClass()">
        <div>
            <ClientOnly>
                <TopicLearningSessionConfiguration :is-in-question-list="true"
                    v-if="learningSessionConfigurationStore.showFilter">
                    <slot>
                        <div class="drop-down-question-sort col-xs-12">
                            <div class="session-config-header">
                                <span class="hidden-xs">Du lernst </span>
                                <template v-if="currentQuestionCount == allQuestionCount">
                                    <b> alle </b>
                                </template>
                                <template v-else>
                                    <b> {{ currentQuestionCount }} </b>
                                </template>
                                <template v-if="currentQuestionCount == 1"> Frage </template>
                                <template v-else> Fragen </template>
                                <span class="hidden-xs">aus diesem Thema</span>
                                ({{ allQuestionCount }})
                            </div>
                            <div class="session-config-header" v-if="(topicHasNoQuestions && showError == true)">Leider
                                hat
                                dieses
                                Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                            </div>



                            <div id="ButtonAndDropdown">
                                <div id="QuestionListHeaderDropDown" class="Button dropdown">
                                    <V-Dropdown :distance="0">
                                        <font-awesome-icon icon="fa-solid fa-ellipsis-vertical"
                                            class="btn btn-link btn-sm ButtonEllipsis" />
                                        <template #popper="{ hide }">

                                            <div v-if="userStore.isLoggedIn" class="dropdown-row" @click="createQuestion()">
                                                <div class="dropdown-icon">
                                                    <font-awesome-icon icon="fa-solid fa-circle-plus" />
                                                </div>
                                                <div class="dropdown-label">Frage hinzufügen</div>

                                            </div>

                                            <div class="dropdown-row" @click="expandAllQuestions()"
                                                v-if="questionsExpanded">
                                                <div class="dropdown-icon">
                                                    <font-awesome-icon icon="fa-solid fa-angles-up" />
                                                </div>
                                                <div class="dropdown-label">
                                                    Alle Fragen zuklappen
                                                </div>
                                            </div>
                                            <div class="dropdown-row" @click="expandAllQuestions()" v-else>
                                                <div class="dropdown-icon">
                                                    <font-awesome-icon icon="fa-solid fa-angles-down" />
                                                </div>
                                                <div class="dropdown-label">
                                                    Alle Fragen erweitern
                                                </div>
                                            </div>

                                            <div class="dropdown-row" @click="learningSessionStore.startNewSession()">
                                                <div class="dropdown-icon">
                                                    <font-awesome-icon icon="fa-solid fa-play" />
                                                </div>
                                                <div class="dropdown-label">
                                                    Fragen jetzt lernen
                                                </div>
                                            </div>
                                        </template>
                                    </V-Dropdown>

                                </div>
                            </div>
                        </div>
                    </slot>
                </TopicLearningSessionConfiguration>

                <div class="session-configurator missing-questions" v-if="!learningSessionConfigurationStore.showFilter">
                    <div class="session-config-header">
                        <div class="col-xs-12 drop-down-question-sort">
                            <div class="session-config-header">
                                Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                            </div>
                        </div>
                    </div>
                </div>
            </ClientOnly>
            <TopicLearningQuestionList :expand-question="questionsExpanded" />
        </div>
    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

#QuestionListSection {

    margin-top: 100px;
    background-color: @memo-grey-lighter;
    padding: 0px 20px 33px 20px;
    margin-right: 0;
    margin-left: 0;

    @media(max-width: @screen-xxs-max) {
        padding-left: 0;
        padding-right: 0;
    }

    &.no-questions {
        margin-top: 20px;

        .session-config-header {
            padding-top: 12px;
            padding-bottom: 24px;
        }
    }


    .drop-down-question-sort {
        display: flex;
        flex-wrap: wrap;
        font-size: 18px;
        justify-content: space-between;
        padding-right: 0;

        #ButtonAndDropdown {
            cursor: pointer;
            background: @memo-grey-lighter;
            border-radius: 24px;
            display: flex;
            justify-content: center;
            align-items: center;
            font-size: 18px;
            height: 30px;
            width: 30px;
            min-width: 30px;
            transition: filter 0.1s;

            margin-top: -10px;

            @media(max-width: 768px) {
                padding-left: 10px;
            }

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }


        }
    }

}
</style>