<script lang="ts" setup>
import { ref } from 'vue'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { useTopicStore } from '../topicStore'
import { useLearningSessionStore, AnswerState } from './learningSessionStore'

const learningSessionStore = useLearningSessionStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const expandAllQuestions = ref(false)
const showFilter = ref(true)

onBeforeMount(() => {
    learningSessionConfigurationStore.checkKnowledgeSummarySelection()
})

watch(() => learningSessionConfigurationStore.selectedQuestionCount, (oldNumber, newNumber) => {
    learningSessionConfigurationStore.questionCountIsInvalid = newNumber <= 0 || isNaN(newNumber) || newNumber == null
    if (oldNumber != newNumber && isNaN(newNumber)) {
        var val = newNumber as any
        learningSessionConfigurationStore.selectedQuestionCount = parseInt(val)
    }
})

const topicStore = useTopicStore()
const answerBodyModel = ref()

onMounted(async () => {
    var sessionJson = learningSessionConfigurationStore.buildSessionConfigJson(topicStore.id)
    answerBodyModel.value = await $fetch<any>(`/apiVue/Learning/GetNewAnswerBodyForTopic/`, {
        body: sessionJson,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
    })

    learningSessionConfigurationStore.setCounter(answerBodyModel.value?.counter)
    learningSessionStore.startNewSession()
})

const progressPercentage = ref(0)

function calculateProgress() {
    const answered = learningSessionStore.steps.filter(s =>
        s.state != AnswerState.Unanswered
    ).length

    progressPercentage.value = Math.round(100 / learningSessionStore.steps.length * answered * 100) / 100

}

watch([() => learningSessionStore.currentStep, () => learningSessionStore.steps], ([c, s]) => {
    calculateProgress()
}, { deep: true })

</script>

<template>
    <div class="">
        <TopicLearningSessionConfiguration v-if="showFilter">
            <slot>
                <div class="session-progress-bar">
                    <div class="session-progress">
                        <div v-for="step in learningSessionStore.steps" class="step"
                            :class="{ 'answered': step.state != AnswerState.Unanswered }"></div>
                    </div>

                    <div class="step-count">
                        {{ learningSessionStore.currentStep }} / {{ learningSessionStore.steps.length }}
                    </div>
                    <div class="progress-percentage">{{ progressPercentage }}%</div>
                </div>
            </slot>
        </TopicLearningSessionConfiguration>

    </div>

    <div class="session-configurator col-xs-12" v-if="!showFilter">
        <div class="session-config-header">
            <div class="col-xs-12 drop-down-question-sort">
                <div class="session-config-header">
                    Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                </div>
            </div>
        </div>
    </div>

    <!-- <LazyQuestionAnswerBody v-if="answerBodyModel != null" :answer-body-model="answerBodyModel" /> -->
    <div>
        <div class="col-xs-12" id="QuestionListContainer">
            <TopicLearningQuestionsSection />

        </div>
    </div>
</template>
<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.step {
    &.answered {
        background: @memo-green;
    }
}
</style>
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

            &.answered {
                background: @memo-green;
            }
        }
    }
}
</style>