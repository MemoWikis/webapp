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
    if (process.client) {
        answerBodyModel.value = await $fetch<any>(`/api/Learning/GetNewAnswerBodyForTopic/`, {
            body: sessionJson,
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
            headers: useRequestHeaders(['cookie'])
        })
    } else if (process.server) {
        const config = useRuntimeConfig()
        answerBodyModel.value = await useFetch<any>(`/Learning/GetNewAnswerBodyForTopic/`, {
            body: {
                categoryId: topicStore.id,
                isInLearningTab: true
            },
            baseURL: config.apiBase,
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
            headers: useRequestHeaders(['cookie'])
        })
    }
})

</script>

<template>
    <TopicLearningSessionConfiguration v-show="showFilter">
        <slot>
            <input id="hdnIsTestMode" hidden :value="learningSessionConfigurationStore.isTestMode" />
            <div class="col-xs-12 drop-down-question-sort">
                <div class="session-config-header">
                    <span class="hidden-xs">Du lernst</span>
                    <template
                        v-if="learningSessionConfigurationStore.currentQuestionCount == learningSessionConfigurationStore.allQuestionCount">
                        <b>&nbsp;alle&nbsp;</b>
                    </template>
                    <template v-else>
                        <b>&nbsp;{{ learningSessionConfigurationStore.currentQuestionCount }}&nbsp;</b>
                    </template>
                    Fragen&nbsp;
                    <span class="hidden-xs">aus diesem Thema</span>
                    &nbsp;({{ learningSessionConfigurationStore.allQuestionCount }})
                </div>
                <div class="session-config-header"
                    v-if="learningSessionConfigurationStore.categoryHasNoQuestions && learningSessionConfigurationStore.showError">
                    Leider
                    hat dieses Thema noch keine
                    Fragen, erstelle oder füge eine Frage hinzu.
                </div>



                <div id="ButtonAndDropdown">
                    <V-Dropdown :distance="0">
                        <div id="QuestionListHeaderDropDown" class="Button dropdown">
                            <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />

                        </div>
                        <template #popper>
                            <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                                <li onclick="eventBus.$emit('open-edit-question-modal', { categoryId: <%= Model.CategoryId %>, edit: false })"
                                    data-allowed="logged-in">
                                    <a>
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-circle-plus" />
                                        </div><span>Frage hinzufügen</span>
                                    </a>
                                </li>
                                <li @click="expandAllQuestions = !expandAllQuestions" style="cursor: pointer">
                                    <a>
                                        <div class="dropdown-icon">
                                            <font-awesome-icon v-if="expandAllQuestions" icon="fa-solid fa-angles-up" />
                                            <font-awesome-icon v-else icon="fa-solid fa-angles-down" />
                                        </div>
                                        <span>{{ expandAllQuestions ? 'Alle Fragen zuklappen' : 'Alle Fragen erweitern'
                                        }}</span>
                                    </a>
                                </li>
                                <li style="cursor: pointer">
                                    <a data-allowed="logged-in"
                                        @click="learningSessionConfigurationStore.startNewSession()">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-play" />
                                        </div><span>Fragen jetzt lernen</span>
                                    </a>
                                </li>
                            </ul>

                        </template>
                    </V-Dropdown>
                </div>
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