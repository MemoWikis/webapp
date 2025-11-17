<script lang="ts" setup>

import { useLearningSessionStore } from './learningSessionStore'
import { usePageStore } from '../pageStore'
import { useUserStore } from '~~/components/user/userStore'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'

const learningSessionStore = useLearningSessionStore()
const pageStore = usePageStore()
const userStore = useUserStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const editQuestionStore = useEditQuestionStore()
const route = useRoute()

// Check if we're in wishknowledge mode
const isWishknowledgeMode = computed(() => true)

const openFilter = ref(false)
const filterOpened = useCookie('show-bottom-dropdown')
onBeforeMount(() => {
    if (filterOpened.value?.toString() === 'false' || filterOpened.value == undefined)
        openFilter.value = false
    else if (filterOpened.value?.toString() === 'true')
        openFilter.value = true

    // For wishknowledge mode, show filter if user has wishknowledge questions
    // For page mode, use pageStore.questionCount
    if (isWishknowledgeMode.value) {
        // For wishknowledge, we'll show the filter if the user is logged in
        learningSessionConfigurationStore.showFilter = userStore.isLoggedIn
    } else {
        learningSessionConfigurationStore.showFilter = pageStore.questionCount > 0
    }
})

watch(() => pageStore.questionCount, (count) => {
    // Only apply page question count logic when not in wishknowledge mode
    if (!isWishknowledgeMode.value) {
        learningSessionConfigurationStore.showFilter = count > 0
    }
})

// For wishknowledge mode, show filter based on whether there are learning session steps
watch(() => learningSessionStore.steps.length, (count) => {
    if (isWishknowledgeMode.value && userStore.isLoggedIn) {
        learningSessionConfigurationStore.showFilter = count > 0
    }
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
const { t } = useI18n()
</script>

<template>
    <div id="QuestionListSection" :class="getClass()">
        <div>
            <div v-if="learningSessionConfigurationStore.showFilter" id="PageLearnignSessionContainer">
                <PageLearningSessionConfiguration cookie-name="show-bottom-dropdown"
                    :open-filter="openFilter">
                    <slot>
                        <div class="drop-down-question-sort">
                            <div class="session-config-header">
                                <span class="hidden-xs">{{ t('page.questionsSection.youAreLearning') }}&nbsp;</span>
                                <b v-if="learningSessionStore.steps.length === pageStore.questionCount">{{ t('page.questionsSection.all') }}&nbsp;</b>
                                <b v-else>{{ learningSessionStore.steps.length }}&nbsp;</b>
                                <template v-if="learningSessionStore.steps.length === 1"> {{ t('page.questionsSection.question') }}&nbsp;</template>
                                <template v-else>{{ t('page.questionsSection.questions') }}&nbsp;</template>
                                <span class="hidden-xs">{{ t('page.questionsSection.onThisPage') }}</span>
                                ({{ pageStore.questionCount }})
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
                                                <div class="dropdown-label">{{ t('page.questionsSection.addQuestion') }}</div>

                                            </div>

                                            <div class="dropdown-row" @click="questionsExpanded = !questionsExpanded"
                                                v-if="questionsExpanded">
                                                <div class="dropdown-icon">
                                                    <font-awesome-icon icon="fa-solid fa-angles-up" />
                                                </div>
                                                <div class="dropdown-label">
                                                    {{ t('page.questionsSection.collapseAllQuestions') }}
                                                </div>
                                            </div>
                                            <div class="dropdown-row" @click="questionsExpanded = !questionsExpanded"
                                                v-else>
                                                <div class="dropdown-icon">
                                                    <font-awesome-icon icon="fa-solid fa-angles-down" />
                                                </div>
                                                <div class="dropdown-label">
                                                    {{ t('page.questionsSection.expandAllQuestions') }}
                                                </div>
                                            </div>

                                            <div class="dropdown-row" @click="learningSessionStore.startNewSession()">
                                                <div class="dropdown-icon">
                                                    <font-awesome-icon icon="fa-solid fa-play" />
                                                </div>
                                                <div class="dropdown-label">
                                                    {{ t('page.questionsSection.learnQuestionsNow') }}
                                                </div>
                                            </div>
                                        </template>
                                    </VDropdown>

                                </div>
                            </div>
                        </div>
                    </slot>
                </PageLearningSessionConfiguration>
            </div>

            <div class="session-configurator no-questions" v-else-if="!learningSessionConfigurationStore.showFilter">
                <div class="session-config-header">
                    <div class="drop-down-question-sort">
                        {{ t('page.questionsSection.noQuestionsYet') }}
                    </div>
                </div>
            </div>

            <PageLearningQuestionList :expand-question="questionsExpanded" />

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
    border-radius: 8px;

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
        align-items: center;
        width: 100%;

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

            @media(max-width: @screen-sm) {
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

<style lang="less" scoped>
#PageLearnignSessionContainer {
    display: flex;
    justify-content: center;
}
</style>