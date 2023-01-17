<script lang="ts" setup>
import { ref } from 'vue'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { useTopicStore } from '../topicStore'

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
})

</script>

<template>
    <div class="">
        <TopicLearningSessionConfiguration v-if="showFilter">
            <slot>
                <div class="session-progress-bar">
                    <div class="progress-bar"></div>

                    <div class="step-count">2</div>
                    <div class="progress-percentage">3%</div>
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

    .progress-bar {
        position: absolute;
        background: @memo-green;
        height: 100%;
        left: 0;
        right: 0;
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
</style>