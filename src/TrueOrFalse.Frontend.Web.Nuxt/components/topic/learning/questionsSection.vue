<script lang="ts" setup>

import { useLearningSessionStore } from './learningSessionStore'
import { useTopicStore } from '../topicStore'
import { useUserStore } from '~~/components/user/userStore'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'

const learningSessionStore = useLearningSessionStore()
const topicStore = useTopicStore()
const userStore = useUserStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const editQuestionStore = useEditQuestionStore()

const openFilter = ref(false)
const filterOpened = useCookie('show-bottom-dropdown')
onBeforeMount(() => {
    if (filterOpened.value?.toString() == 'false' || filterOpened.value == undefined)
        openFilter.value = false
    else if (filterOpened.value?.toString() == 'true')
        openFilter.value = true

    if (topicStore.questionCount > 0)
        learningSessionConfigurationStore.showFilter = true
    else
        learningSessionConfigurationStore.showFilter = false
})

watch(() => topicStore.questionCount, (count) => {
    if (count > 0)
        learningSessionConfigurationStore.showFilter = true
    else
        learningSessionConfigurationStore.showFilter = false
})
const questionsExpanded = ref(false)

const { isMobile } = useDevice()

function getClass(): string {
    let str = ''
    if (isMobile)
        str += 'is-mobile'
    if (import.meta.server)
        return str
    else return !learningSessionConfigurationStore.showFilter ? `${str} no-questions` : str
}
const ariaId = useId()

</script>

<template>
    <div id="QuestionListSection" class="row" :class="getClass()">
        <div>
            <TopicLearningSessionConfiguration :is-in-question-list="true" cookie-name="show-bottom-dropdown"
                v-if="learningSessionConfigurationStore.showFilter" :open-filter="openFilter">
                <slot>
                    <div class="drop-down-question-sort col-xs-12">
                        <div class="session-config-header">
                            <span class="hidden-xs">Du lernst </span>
                            <template v-if="learningSessionStore.steps.length == topicStore.questionCount">
                                <b> alle </b>
                            </template>
                            <template v-else>
                                <b> {{ learningSessionStore.steps.length }} </b>
                            </template>
                            <template v-if="learningSessionStore.steps.length == 1"> Frage </template>
                            <template v-else> Fragen </template>
                            <span class="hidden-xs">aus diesem Thema</span>
                            ({{ topicStore.questionCount }})
                        </div>

                        <div id="ButtonAndDropdown">
                            <div id="QuestionListHeaderDropDown" class="Button dropdown">
                                <VDropdown :aria-id="ariaId" :distance="0">
                                    <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" class="btn btn-link btn-sm ButtonEllipsis" />
                                    <template #popper>

                                        <div v-if="userStore.isLoggedIn" class="dropdown-row"
                                            @click="editQuestionStore.create()">
                                            <div class="dropdown-icon">
                                                <font-awesome-icon icon="fa-solid fa-circle-plus" />
                                            </div>
                                            <div class="dropdown-label">Frage hinzufügen</div>

                                        </div>

                                        <div class="dropdown-row" @click="questionsExpanded = !questionsExpanded"
                                            v-if="questionsExpanded">
                                            <div class="dropdown-icon">
                                                <font-awesome-icon icon="fa-solid fa-angles-up" />
                                            </div>
                                            <div class="dropdown-label">
                                                Alle Fragen zuklappen
                                            </div>
                                        </div>
                                        <div class="dropdown-row" @click="questionsExpanded = !questionsExpanded"
                                            v-else>
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
                                </VDropdown>

                            </div>
                        </div>
                    </div>
                </slot>
            </TopicLearningSessionConfiguration>

            <div class="session-configurator no-questions" v-else-if="!learningSessionConfigurationStore.showFilter">
                <div class="session-config-header">
                    <div class="col-xs-12 drop-down-question-sort">
                        Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                    </div>
                </div>
            </div>

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
    max-width: calc(100vw - 20px);

    &.is-mobile {
        max-width: 100vw;
    }

    @media(max-width: @screen-xxs-max) {
        padding-left: 0;
        padding-right: 0;
    }

    @media(min-width: @screen-xs-min) and (max-width: @screen-sm-min) {
        margin-left: -10px;
        margin-right: -10px;

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