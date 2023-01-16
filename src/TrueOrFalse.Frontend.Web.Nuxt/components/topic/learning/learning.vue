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
    <TopicLearningSessionConfiguration v-show="showFilter">
        <slot>
            <div class="session-progress-bar">
                <div>2</div>
                <div>3%</div>
            </div>
        </slot>


    </TopicLearningSessionConfiguration>
    <div class="session-configurator col-xs-12" v-if="!showFilter">
        <div class="session-config-header">
            <div class="col-xs-12 drop-down-question-sort">
                <div class="session-config-header">
                    Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                </div>
            </div>
        </div>
    </div>

    <LazyQuestionAnswerBody v-if="answerBodyModel != null" :answer-body-model="answerBodyModel" />

    <TopicLearningQuestionsSection />
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.session-progress-bar {
    font-size: 14px;
    line-height: 1.42857143;
    display: flex;
    width: 100%;
    margin-left: 10px;
    height: 30px;
    background: @memo-grey-lighter;
    justify-content: space-between;
    flex-wrap: nowrap;
    position: relative;
    align-items: center;
    padding: 0 8px;
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