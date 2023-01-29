<script lang="ts" setup>
import { useLearningSessionStore } from '~/components/topic/learning/learningSessionStore'
import { useUserStore } from '~/components/user/userStore'
import { useTabsStore, Tab } from '~/components/topic/tabs/tabsStore'
import { SolutionType } from '../solutionTypeEnum'
import { useEditQuestionStore } from '../edit/editQuestionStore'
import { useDeleteQuestionStore } from '../edit/delete/deleteQuestionStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'

const spinnerStore = useSpinnerStore()
const learningSessionStore = useLearningSessionStore()
const deleteQuestionStore = useDeleteQuestionStore()

const showPinButton = ref(true)

const userStore = useUserStore()
const tabsStore = useTabsStore()
const editQuestionStore = useEditQuestionStore()

const answerIsCorrect = ref(false)
const answerIsWrong = ref(false)


function openCommentModal() {

}

const amountOfTries = ref(0)
const answersSoFar = ref<string[]>([])

const multipleChoice = ref()
const text = ref()
const matchList = ref()
const flashcard = ref()

async function answer() {
    amountOfTries.value++
    let solutionComponent
    switch (answerBodyModel.value?.solutionType) {
        case SolutionType.MultipleChoice:
            solutionComponent = multipleChoice.value
            break
        case SolutionType.Text:
            solutionComponent = text.value
            break
        case SolutionType.MatchList:
            solutionComponent = matchList.value
            break
        case SolutionType.FlashCard:
            solutionComponent = flashcard.value
            break
    }

    if (solutionComponent == null)
        return

    answersSoFar.value.push(solutionComponent.getAnswerText())

    const data = {
        answer: solutionComponent.getAnswerDataString(),
        id: answerBodyModel.value?.id,
        questionViewGuid: answerBodyModel.value?.questionViewGuid,
        inTestMode: learningSessionStore.isInTestMode,
        isLearningSession: learningSessionStore.isLearningSession
    }

    const result = await $fetch<any>(`/apiVue/AnswerBody/SendAnswerToLearningSession/`,
        {
            method: 'POST',
            body: data,
            credentials: 'include',
            mode: 'cors',
        })

    console.log(result)
}

interface AnswerBodyModel {
    id: number
    text: string
    title: string
    solutionType: SolutionType
    renderedQuestionTextExtended: string
    description: string
    hasTopics: boolean
    primaryTopicUrl: string
    primaryTopicName: string
    solution: string

    isCreator: boolean
    isInWishknowledge: boolean

    questionViewGuid: number
    isLastStep: boolean
}

const flashCardSolution = ref('')

const answerBodyModel = ref<AnswerBodyModel | null>(null)

async function loadAnswerBodyModel() {
    if (!learningSessionStore.currentStep) {
        return
    }
    const result = await $fetch<AnswerBodyModel>(`/apiVue/AnswerBody/Get/?index=${learningSessionStore.currentStep.index}`, {
        mode: 'cors',
        credentials: 'include'
    })
    if (result != null) {
        answerBodyModel.value = result
    }
}

const interactionNumber = ref(0)

async function markAsCorrect() {
    const data = {
        questionViewGuid: answerBodyModel.value?.questionViewGuid,
        interactionNumber: interactionNumber.value,
    }
}

async function markAsWrong() {

}

async function skipMarking() {

}

function showSolution() {

}

async function loadNextQuestion() {

}

async function continueSession() {

}

watch(() => learningSessionStore.currentStep?.index, () => {
    loadAnswerBodyModel()
})

</script>

<template>
    <div id="AnswerBody" v-if="answerBodyModel">
        <div id="QuestionTitle" style="display:none">
            {{ answerBodyModel.title }}
        </div>
        <div class="AnswerQuestionBodyMenu">

            <div v-if="showPinButton" class="Pin" :data-question-id="answerBodyModel.id">
                <QuestionPin :questionId="answerBodyModel.id"
                    :is-in-wishknowledge="answerBodyModel.isInWishknowledge" />
            </div>
            <div class="Button dropdown">
                <span class="margin-top-4">
                    <V-Dropdown :distance="0">
                        <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                        <template #popper>

                            <div class="dropdown-row"
                                v-if="tabsStore.activeTab == Tab.Learning && (answerBodyModel.isCreator || userStore.isAdmin)"
                                @click="editQuestionStore.editQuestion(answerBodyModel!.id)">
                                <div class="dropdown-icon">
                                    <font-awesome-icon icon="fa-solid fa-pen" />
                                </div>
                                <div class="dropdown-label">Frage bearbeiten</div>

                            </div>

                            <LazyNuxtLink :to="`/question/${answerBodyModel.title}/${answerBodyModel.id}`"
                                v-if="tabsStore.activeTab == Tab.Learning && userStore.isAdmin">
                                <div class="dropdown-row">
                                    <div class="dropdown-icon">
                                        <font-awesome-icon icon="fa-solid fa-file" />
                                    </div>
                                    <span>Frageseite anzeigen</span>

                                </div>
                            </LazyNuxtLink>

                            <LazyNuxtLink :to="`/QuestionHistory/${answerBodyModel.title}/${answerBodyModel.id}`"
                                v-if="tabsStore.activeTab == Tab.Learning && userStore.isAdmin">
                                <div class="dropdown-row">
                                    <div class="dropdown-icon">
                                        <font-awesome-icon icon="fa-solid fa-code-fork" />
                                    </div>
                                    <span>Bearbeitungshistorie der Frage</span>

                                </div>
                            </LazyNuxtLink>

                            <div class="dropdown-row" @click="openCommentModal()">
                                <div class="dropdown-icon">
                                    <font-awesome-icon icon="fa-solid fa-comment" />
                                </div>
                                <div class="dropdown-label">Frage kommentieren</div>
                            </div>

                            <div class="dropdown-row" @click="deleteQuestionStore.openModal(answerBodyModel!.id)">
                                <div class="dropdown-icon">
                                    <font-awesome-icon icon="fa-solid fa-trash" />
                                </div>
                                <div class="dropdown-label">Frage löschen</div>
                            </div>

                        </template>
                    </V-Dropdown>


                </span>
            </div>

        </div>

        <h3 v-if="answerBodyModel.solutionType != SolutionType.FlashCard" class="QuestionText">
            {{ answerBodyModel.text }}
        </h3>

        <div class="row">

            <div id="MarkdownCol"
                v-if="answerBodyModel.solutionType != SolutionType.FlashCard && !!answerBodyModel.renderedQuestionTextExtended">
                <div class="RenderedMarkdown" v-html="answerBodyModel.renderedQuestionTextExtended">
                </div>
            </div>


            <div id="AnswerAndSolutionCol">
                <div id="AnswerAndSolution">
                    <div class="row"
                        :class="{ 'hasFlashCard': answerBodyModel.solutionType == SolutionType.FlashCard }">
                        <div id="AnswerInputSection">
                            <!-- <input type="hidden" id="hddSolutionMetaDataJson"
                                value="<%: Model.SolutionMetaDataJson %>" />
                            <input type="hidden" id="hddSolutionTypeNum" value="<%: Model.SolutionTypeInt %>" /> -->
                            <template v-if="answerBodyModel.solutionType != SolutionType.FlashCard">
                                <div class="answerFeedback answerFeedbackCorrect" v-if="answerIsCorrect">
                                    <font-awesome-icon icon="fa-solid fa-circle-check" />&nbsp;Richtig!
                                </div>
                                <div class="answerFeedback answerFeedbackWrong" v-else-if="answerIsWrong">
                                    <font-awesome-icon icon="fa-solid fa-circle-minus" />&nbsp;Leider falsch
                                </div>
                            </template>
                            <QuestionAnswerBodyFlashcard
                                v-else-if="answerBodyModel.solutionType == SolutionType.FlashCard" ref="flashcard"
                                :solution="answerBodyModel.solution" :text="answerBodyModel.text" />
                            <QuestionAnswerBodyMatchlist v-if="answerBodyModel.solutionType == SolutionType.MatchList"
                                ref="matchList" :solution="answerBodyModel.solution" />
                            <QuestionAnswerBodyMultipleChoice
                                v-if="answerBodyModel.solutionType == SolutionType.MultipleChoice"
                                :solution="answerBodyModel.solution" ref="multipleChoice" />
                            <QuestionAnswerBodyText v-if="answerBodyModel.solutionType == SolutionType.Text"
                                ref="text" />


                        </div>
                        <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <div id="Buttons">
                                    <!-- <div id="btnGoToTestSession" style="display: none">

                                        <NuxtLink v-if="answerBodyModel.hasTopics &&
                                        answerBodyModel.isLastQuestion" :to="`${answerBodyModel.primaryTopicUrl}`"
                                            id="btnStartTestSession" class="btn btn-primary show-tooltip" rel="nofollow"
                                            v-tooltip="userStore.isLoggedIn ? 'Lerne alle Fragen im Thema' : 'Lerne 5 zufällig ausgewählte Fragen aus dem Thema ' + answerBodyModel.primaryTopicName">
                                            <b>Weiterlernen</b>
                                        </NuxtLink>
                                    </div> -->
                                    <template v-if="answerBodyModel.solutionType == SolutionType.FlashCard">
                                        <div class="btn btn-warning memo-button" @click="flashcard.flip()">
                                            Umdrehen
                                        </div>

                                        <div id="buttons-answer" class="ButtonGroup flashCardAnswerButtons"
                                            style="display: none">
                                            <button id="btnRightAnswer" class="btn btn-warning memo-button"
                                                @click="markAsCorrect()">
                                                Wusste ich!
                                            </button>
                                            <button id="btnWrongAnswer" class="btn btn-warning memo-button"
                                                @click="markAsWrong()">
                                                Wusste ich nicht!
                                            </button>
                                            <button
                                                v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp"
                                                id="flashCard-dontCountAnswer"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="skipMarking()">
                                                Nicht werten!
                                            </button>
                                        </div>
                                    </template>

                                    <template v-else>
                                        <div id="buttons-first-try" class="ButtonGroup">
                                            <button class="btn btn-primary memo-button" @click="answer()">
                                                Antworten
                                            </button>
                                            <button
                                                v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="showSolution()">
                                                <font-awesome-icon icon="fa-solid fa-lightbulb" /> Lösung anzeigen
                                            </button>
                                        </div>
                                    </template>

                                    <div
                                        v-if="learningSessionStore.isLearningSession
                                        && !learningSessionStore.isInTestMode && learningSessionStore.steps.length - 1 < learningSessionStore.currentIndex">
                                        <button id="aSkipStep" class="SecAction btn btn-link memo-button">
                                            <font-awesome-icon icon="fa-solid fa-forward" /> Frage überspringen
                                        </button>
                                    </div>

                                    <div id="buttons-next-question" class="ButtonGroup" style="display: none;">
                                        <button v-if="!answerBodyModel.isLastStep" @click="loadNextQuestion()"
                                            id="btnNext" class="btn btn-primary memo-button" rel="nofollow">
                                            Nächste Frage
                                        </button>

                                        <button
                                            v-if="answerBodyModel.solutionType != SolutionType.FlashCard && !learningSessionStore.isInTestMode && !learningSessionStore.answerHelp"
                                            href="#" id="aCountAsCorrect"
                                            class="SecAction btn btn-link show-tooltip memo-button"
                                            title="Drücke hier und die Frage wird als richtig beantwortet gewertet"
                                            rel="nofollow" style="display: none;" @click="markAsCorrect()">
                                            Hab ich gewusst!
                                        </button>

                                    </div>

                                    <div v-if="answerBodyModel.solutionType != SolutionType.FlashCard"
                                        id="buttons-answer-again" class="ButtonGroup" style="display: none">
                                        <button id="btnCheckAgain" class="btn btn-warning memo-button"
                                            rel="nofollow">Nochmal
                                            Antworten</button>
                                        <button
                                            v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp"
                                            class="selectorShowSolution SecAction btn btn-link memo-button">
                                            <font-awesome-icon icon="fa-solid fa-lightbulb" />
                                            Lösung anzeigen
                                        </button>
                                    </div>
                                    <div style="clear: both">
                                    </div>
                                </div>
                                <div id="AnswerFeedbackAndSolutionDetails">
                                    <div v-if="answerBodyModel.solutionType != SolutionType.FlashCard"
                                        id="AnswerFeedback">
                                        <div class="" id="divAnsweredCorrect" style="display: none; margin-top: 5px;">
                                            <b style="color: green;">Richtig!</b>
                                            <span id="wellDoneMsg"></span>
                                        </div>
                                        <div id="Solution" class="Detail" style="display: none;">
                                            <div class="Label">
                                                Richtige
                                                Antwort:</div>
                                            <div class="Content body-m">
                                            </div>
                                        </div>
                                        <div id="divWrongAnswerPlay" class="Detail"
                                            style="display: none; background-color: white;">
                                            <span style="color: #B13A48"><b>Deine
                                                    Antwort war
                                                    falsch</b></span>
                                            <div>Deine Eingabe:
                                            </div>
                                            <div style="margin-top: 7px;" id="divWrongEnteredAnswer">
                                            </div>
                                        </div>
                                        <div id="divWrongAnswer" class="Detail"
                                            style="display: none; background-color: white;">
                                            <span id="spnWrongAnswer" style="color: #B13A48">
                                                <b>Falsch
                                                    beantwortet
                                                </b>
                                            </span>
                                            <span id="CountWrongAnswers" style="float: right;">
                                                (zwei Versuche)
                                            </span>
                                            <br />

                                            <div style="margin-top: 5px;" id="answerFeedbackTry">
                                                Du könntest es
                                                wenigstens
                                                probieren!
                                            </div>

                                            <div id="divWrongAnswers" style="margin-top: 7px; display: none;">
                                                <span class="WrongAnswersHeading">Deine
                                                    bisherigen
                                                    Antwortversuche:</span>
                                                <ul style="padding-top: 5px;" id="ulAnswerHistory">
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- <div id="SolutionDetailsSpinner" style="display: none;">
                                        <i class="fa fa-spinner fa-spin" style="color: #b13a48;"></i>
                                    </div> -->



                                    <div id="SolutionDetails" v-if="answerBodyModel.description?.trim().length > 0"
                                        style="display: none; background-color: white;">
                                        <div id="Description" class="Detail">
                                            <div class="Label">
                                                Ergänzungen
                                                zur
                                                Antwort:
                                            </div>
                                            <div class="Content body-m">
                                                {{ answerBodyModel.description }}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="ActivityPointsDisplay">
            <small>Dein Punktestand</small>
            <span id="ActivityPoints">
                {{ userStore.totalActivityPoints }}
            </span>
            <font-awesome-icon icon="fa-solid fa-circle-info"
                v-tooltip="'Du bekommst Lernpunkte für das Beantworten von Fragen'" />
        </div>
        <QuestionAnswerQuestionDetails :answer-body-model="answerBodyModel" />
    </div>

</template>

<style lang="less">
@import '~~/assets/views/answerQuestion.less';
</style>