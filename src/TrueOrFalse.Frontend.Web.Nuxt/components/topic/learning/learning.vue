<script lang="ts" setup>
import { ref } from 'vue'
import { useLearningSessionConfigurationStore, StoredSessionConfig } from './learningSessionConfigurationStore'
import { useTopicStore } from '../topicStore'
import { useLearningSessionStore, AnswerState } from './learningSessionStore'

const learningSessionStore = useLearningSessionStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()

onBeforeMount(async () => {
    learningSessionConfigurationStore.checkKnowledgeSummarySelection()
    if (process.client)
        learningSessionConfigurationStore.loadSessionFromLocalStorage()

    var sessionJson = learningSessionConfigurationStore.buildSessionConfigJson(topicStore.id)
    const count = await $fetch<number>(`/apiVue/Learning/GetCount/`, {
        body: sessionJson,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
    })

    learningSessionConfigurationStore.setCounter(count)
    learningSessionStore.startNewSession()
})

const topicStore = useTopicStore()

onMounted(() => {
    watch(() => learningSessionConfigurationStore.selectedQuestionCount, (oldNumber, newNumber) => {
        learningSessionConfigurationStore.questionCountIsInvalid = newNumber <= 0 || isNaN(newNumber)
    })
})


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

watch([() => learningSessionStore.currentStep, () => learningSessionStore.steps], ([c, s]) => {
    calculateProgress()
}, { deep: true })

</script>

<template>
    <div class="col-xs-12" v-if="learningSessionConfigurationStore.showFilter">
        <ClientOnly>
            <TopicLearningSessionConfiguration>
                <slot>
                    <div class="session-progress-bar">
                        <div class="session-progress">
                            <!-- <div v-for="step in learningSessionStore.steps" class="step"
                            :class="{ 'answered': step.state != AnswerState.Unanswered, 'skipped': step.state == AnswerState.Skipped, 'false': step.state == AnswerState.False }">
                        </div> -->

                            <div class="step answered" :style="answeredWidth"></div>
                            <div class="step" :style="unansweredWidth"></div>

                        </div>

                        <div class="step-count">
                            <template v-if="learningSessionStore.currentStep">
                                {{ learningSessionStore.currentStep?.index + 1 }} / {{
                                    learningSessionStore.steps.length
                                }}
                            </template>
                        </div>
                        <div class="progress-percentage">{{ progressPercentage }}%</div>
                    </div>
                </slot>
            </TopicLearningSessionConfiguration>
        </ClientOnly>
    </div>

    <div class="col-xs-12">
        <QuestionAnswerBody />
    </div>

    <div class="col-xs-12" id="QuestionListContainer">
        <TopicLearningQuestionsSection />
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
        padding-left: 10px;
        padding-right: 15px;
        flex-wrap: nowrap;
        align-items: center;
        z-index: 2;
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