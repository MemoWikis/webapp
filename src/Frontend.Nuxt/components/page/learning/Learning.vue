<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { useLearningSessionStore, AnswerState } from './learningSessionStore'
import { Tab, useTabsStore } from '../tabs/tabsStore'

const userStore = useUserStore()
const learningSessionStore = useLearningSessionStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const tabsStore = useTabsStore()

const route = useRoute()
const openFilter = ref(true)
const expiryDate = new Date(Date.now() + 365 * 24 * 60 * 60 * 1000)

const alertOnMounted = ref(false)
const alertOnMountedMsg = ref('')

onBeforeMount(async () => {
    alertOnMounted.value = false
    alertOnMountedMsg.value = ''

    learningSessionConfigurationStore.checkKnowledgeSummarySelection()
    await learningSessionConfigurationStore.loadSessionFromLocalStorage()

    if (route.query.inWuWi === 'true') {
        learningSessionConfigurationStore.questionFilterOptions.inWishKnowledge.isSelected = true
        learningSessionConfigurationStore.questionFilterOptions.notInWishKnowledge.isSelected = false
        learningSessionConfigurationStore.checkQuestionFilterSelection()
    }

    if (route.params.questionId != null)
        mountNewQuestion()
    else
        await learningSessionStore.startNewSession()
})

const mountNewQuestion = async () => {
    if (route.params.questionId == null)
        return
    const questionId = parseInt(route.params.questionId.toString())
    const errorMsg = await learningSessionStore.startNewSessionWithJumpToQuestion(questionId)
    if (errorMsg) {
        if (import.meta.server) {
            alertOnMounted.value = true
            alertOnMountedMsg.value = errorMsg
        } else {
            learningSessionStore.handleQuestionNotInSessionAlert(questionId, errorMsg)
        }
    }
}

onMounted(() => {
    if (import.meta.client && alertOnMounted.value)
        learningSessionStore.handleQuestionNotInSessionAlert(parseInt(route.params.questionId?.toString()), alertOnMountedMsg.value)
})

const filterOpened = useCookie('show-top-dropdown')

onBeforeMount(() => {
    if (filterOpened.value?.toString() === 'true')
        openFilter.value = true
    else if (filterOpened.value?.toString() === 'false' || filterOpened.value == undefined)
        openFilter.value = false
})
const oldId = ref()
onMounted(() => {
    watch(() => learningSessionConfigurationStore.selectedQuestionCount, (oldNumber, newNumber) => {
        learningSessionConfigurationStore.questionCountIsInvalid = newNumber <= 0 || isNaN(newNumber)
    })

    watch(() => tabsStore.activeTab, async (tab) => {
        if (tab === Tab.Learning && route.params?.questionId != null) {
            const newId = parseInt(route.params.questionId?.toString())
            if (isNaN(newId)) {
                return
            }

            if (newId != oldId.value) {
                oldId.value = newId
                await mountNewQuestion()
            }
        }
    })
})
watch(() => learningSessionConfigurationStore.showSelectionError, (val) => {
    if (val)
        openFilter.value = true
})

watch(() => userStore.isLoggedIn, async () => {
    if (import.meta.client) {
        await learningSessionConfigurationStore.loadSessionFromLocalStorage()
    }
})

watch([() => learningSessionStore.currentStep, () => learningSessionStore.steps], () => {
    calculateProgress()
}, { deep: true })

const progressPercentage = ref(0)
const answeredWidth = ref<string>('width: 0%')
const unansweredWidth = ref('width: 100%')
function calculateProgress() {
    const answered = learningSessionStore.steps.filter(s =>
        s.state != AnswerState.Unanswered
    ).length

    progressPercentage.value = Math.round(100 / learningSessionStore.steps.length * answered * 100) / 100

    answeredWidth.value = `width: ${progressPercentage.value}%`
    unansweredWidth.value = `width: ${100 - progressPercentage.value}%`
}

function stepBack() {
    if (learningSessionStore.currentStep?.index === 0 || learningSessionStore.currentStep?.index == null)
        return
    learningSessionStore.changeActiveQuestion(learningSessionStore.currentStep?.index - 1)
}

function stepForward() {
    if (learningSessionStore.currentStep?.index == null || learningSessionStore.steps.length === learningSessionStore.currentStep?.index + 1)
        return
    learningSessionStore.changeActiveQuestion(learningSessionStore.currentStep?.index + 1)
}
</script>

<template>
    <div class="row">
        <div class="col-xs-12" v-if="learningSessionConfigurationStore?.showFilter" v-show="!learningSessionStore.showResult">
            <PageLearningSessionConfiguration :open-filter="openFilter" :expiry-date="expiryDate"
                cookie-name="show-top-dropdown">
                <slot>
                    <div class="session-progress-bar">
                        <div class="session-progress">
                            <!-- <DevOnly>
                                <div v-for="step in learningSessionStore.steps" class="step"
                                    :class="{ 'answered': step.state != AnswerState.Unanswered, 'skipped': step.state === AnswerState.Skipped, 'false': step.state === AnswerState.False }">
                                </div>
                            </DevOnly> -->
                            <div class="pager-container"></div>
                            <div class="step answered" :style="answeredWidth"></div>
                            <div class="step" :style="unansweredWidth"></div>
                        </div>

                        <div class="step-count">
                            <template v-if="learningSessionStore.currentStep">
                                <div class="pager-container left"
                                    :class="{ 'no-step': learningSessionStore.currentStep?.index === 0 }"
                                    @click="stepBack">
                                    <font-awesome-icon :icon="['fas', 'chevron-left']"
                                        v-if="learningSessionStore.currentStep?.index > 0" />
                                </div>
                                <div>
                                    {{ learningSessionStore.currentStep?.index + 1 }} /
                                    {{ learningSessionStore.steps.length }}
                                </div>
                                <div class="pager-container right"
                                    :class="{ 'no-step': learningSessionStore.steps.length === learningSessionStore.currentStep?.index + 1 }"
                                    @click="stepForward">
                                    <font-awesome-icon :icon="['fas', 'chevron-right']"
                                        v-if="learningSessionStore.steps.length > learningSessionStore.currentStep?.index + 1" />
                                </div>

                            </template>
                        </div>
                        <div class="progress-percentage">{{ progressPercentage }}%</div>
                    </div>
                </slot>
            </PageLearningSessionConfiguration>
        </div>

        <div class="col-xs-12">
            <QuestionAnswerBody />
        </div>

        <div class="col-xs-12" id="QuestionListContainer" v-show="!learningSessionStore.showResult">
            <!-- <PageLearningQuestionsSection /> -->
        </div>

        <ClientOnly>
            <QuestionEditPublishModal />
        </ClientOnly>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#QuestionListContainer {
    padding-bottom: 40px;

    @media(max-width: @screen-xxs-max) {
        padding-left: 0;
        padding-right: 0;
        margin-right: 0;
        margin-left: 0;
    }
}

.session-progress-bar {
    font-size: @font-size-base;
    line-height: @line-height-base;
    display: flex;
    width: 100%;
    margin-left: 10px;
    height: 30px;
    background: @memo-grey-lighter;
    justify-content: space-between;
    flex-wrap: nowrap;
    position: relative;

    font-size: 14px;
    font-style: normal;
    font-weight: 600;
    line-height: 20px;
    letter-spacing: 1.25px;
    text-align: center;

    .step-count {
        display: flex;
        // padding-left: 10px;
        // padding-right: 15px;
        margin-right: 15px;
        flex-wrap: nowrap;
        align-items: center;
        z-index: 2;

        .pager-container {
            width: 35px;
            height: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
            user-select: none;

            &.left {
                margin-right: 5px;
            }

            &.right {
                margin-left: 5px;
            }

            &:hover {
                cursor: pointer;
                background-color: rgba(0, 0, 0, 0.1);
            }

            &:active {
                background-color: rgba(0, 0, 0, 0.2);
            }

            &.no-step {

                &:hover,
                &:active {
                    cursor: default;
                    background: none;
                }
            }
        }
    }

    .progress-percentage {
        display: flex;
        align-items: center;
        padding-right: 10px;
        padding-left: 15px;
        z-index: 2;
    }

    .session-progress {
        position: absolute;
        height: 100%;
        left: 0;
        right: 0;
        display: flex;
        flex-wrap: nowrap;
        width: 100%;

        .step {
            width: 100%;
            flex-grow: 2;
            height: 100%;
            transition: all 0.5s ease-in-out;

            &.answered {
                background: @memo-green;
            }

            &.skipped {
                background: @memo-yellow;
            }

            &.false {
                background: @memo-salmon;
            }
        }
    }
}
</style>
