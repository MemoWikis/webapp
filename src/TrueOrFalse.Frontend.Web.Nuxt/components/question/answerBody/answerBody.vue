<script lang="ts" setup>
import { useLearningSessionStore } from '~/components/topic/learning/learningSessionStore'
import { useUserStore } from '~/components/user/userStore'
import { useTabsStore, Tab } from '~/components/topic/tabs/tabsStore'
import { SolutionType } from '../solutionTypeEnum'
import { useEditQuestionStore } from '../edit/editQuestionStore'
import { useDeleteQuestionStore } from '../edit/delete/deleteQuestionStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'

const props = defineProps(['question'])

const spinnerStore = useSpinnerStore()
const learningSessionStore = useLearningSessionStore()
const deleteQuestionStore = useDeleteQuestionStore()

const showPinButton = ref(true)

const userStore = useUserStore()
const tabsStore = useTabsStore()
const editQuestionStore = useEditQuestionStore()

const answerIsCorrect = ref(false)
const answerIsWrong = ref(false)

const flashcard = ref()

function openCommentModal() {

}

function answer() {

}

interface AnswerBodyModel {
    id: number,
    title: string,
    isInWishknowledge: boolean,
    solutionType: SolutionType,
    name: string,
    questionViewGuid: number,
    isCreator: boolean,
    learningSessionStepGuid: number,
}

async function loadAnswerBodyModel(): Promise<AnswerBodyModel | null> {
    const id = learningSessionStore.currentStep?.id
    const result = await $fetch<AnswerBodyModel>(`/apiVue/AnswerBodyController/Get/?id=${id}`, {
        mode: 'cors',
        credentials: 'include'
    })
    if (result != null)
        return result
    else return null
}

const answerBodyModel = ref<AnswerBodyModel | null>(await loadAnswerBodyModel())
const interactionNumber = ref(0)

async function countAsCorrect() {
    const data = {
        questionViewGuid: answerBodyModel.value?.questionViewGuid,
        interactionNumber: interactionNumber.value,
        learningSessionStepGuid: answerBodyModel.value?.learningSessionStepGuid
    }
}

</script>
<template>
    
</template>
<template>
    <div id="AnswerBody" v-if="answerBodyModel">

        <!-- <input type="hidden" id="hddQuestionViewGuid" value="<%= Model.QuestionViewGuid.ToString() %>" />
        <input type="hidden" id="hddInteractionNumber" value="1" />
        <input type="hidden" id="questionId" value="<%= Model.QuestionId %>" />
        <input type="hidden" id="isLastQuestion" value="<%= Model.IsLastQuestion %>" />
        <input type="hidden" id="ajaxUrl_GetSolution" value="<%= Model.AjaxUrl_GetSolution(Url) %>" />
        <input type="hidden" id="ajaxUrl_CountLastAnswerAsCorrect"
            value="<%= Model.AjaxUrl_CountLastAnswerAsCorrect(Url) %>" />
        <input type="hidden" id="ajaxUrl_CountUnansweredAsCorrect"
            value="<%= Model.AjaxUrl_CountUnansweredAsCorrect(Url) %>" />

        <template v-if="learningSessionStore.isTestMode">
            <input type="hidden" id="ajaxUrl_TestSessionRegisterAnsweredQuestion"
                value="<%= Model.AjaxUrl_TestSessionRegisterAnsweredQuestion(Url) %>" />
            <input type="hidden" id="TestSessionProgessAfterAnswering"
                value="<%= Model.TestSessionProgessAfterAnswering %>" />
        </template>
        <template v-if="learningSessionStore.isLearningSession">
            <input type="hidden" id="ajaxUrl_LearningSessionAmendAfterShowSolution"
                value="<%= Model.AjaxUrl_LearningSessionAmendAfterShowSolution(Url) %>" />
        </template>
        <input type="hidden" id="disableAddKnowledgeButton" value="<%= Model.DisableAddKnowledgeButton %>" />

        <input type="hidden" id="hddTimeRecords" />
        <input type="hidden" id="hddQuestionId" value="<%=Model.QuestionId %>" />
        <input type="hidden" id="isInTestMode" value="<%=Model.IsInTestMode %>" /> -->
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

                            <LazyNuxtLink :to="`/question/${props.question.Name}/${props.question.Id}`"
                                v-if="tabsStore.activeTab == Tab.Learning && userStore.isAdmin">
                                <div class="dropdown-row">
                                    <div class="dropdown-icon">
                                        <font-awesome-icon icon="fa-solid fa-file" />
                                    </div>
                                    <span>Frageseite anzeigen</span>

                                </div>
                            </LazyNuxtLink>

                            <LazyNuxtLink :to="`/QuestionHistory/${props.question.Name}/${props.question.Id}`"
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

                            <div class="dropdown-row" @click="deleteQuestionStore.openModal(props.question.id)">
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
            {{ answerBodyModel.name }}
        </h3>

        <div class="row">

            <div id="MarkdownCol"
                v-if="answerBodyModel.solutionType != SolutionType.FlashCard && answerBodyModel.QuestionTextMarkdown != null">
                <div class="RenderedMarkdown">
                    {{ answerBodyModel.QuestionTextMarkdown }}
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
                            <template v-if="props.question.SolutionType != SolutionType.FlashCard">
                                <div class="answerFeedback answerFeedbackCorrect" v-if="answerIsCorrect">
                                    <font-awesome-icon icon="fa-solid fa-circle-check" />&nbsp;Richtig!
                                </div>
                                <div class="answerFeedback answerFeedbackWrong" v-else-if="answerIsWrong">
                                    <font-awesome-icon icon="fa-solid fa-circle-minus" />&nbsp;Leider falsch
                                </div>
                            </template>
                            <QuestionAnswerBodyFlashcard
                                v-else-if="props.question.SolutionType == SolutionType.FlashCard" ref="flashcard"
                                :question="question" />
                            <QuestionAnswerBodyMatchlist
                                v-else-if="props.question.SolutionType == SolutionType.MatchList" ref="flashcard"
                                :question="question" />
                            <QuestionAnswerBodyMultipleChoice
                                v-else-if="props.question.SolutionType == SolutionType.MultipleChoice" ref="flashcard"
                                :question="question" />
                            <QuestionAnswerBodyText v-else-if="props.question.SolutionType == SolutionType.Text"
                                ref="flashcard" :question="question" />


                        </div>
                        <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <div id="Buttons">
                                    <div id="btnGoToTestSession" style="display: none">

                                        <NuxtLink v-if="answerBodyModel.hasCategories && !answerBodyModel.IsForVideo &&
                                        answerBodyModel.isLastQuestion"
                                            :to="`${answerBodyModel.primaryTopic.EncodedName}/${answerBodyModel.primaryTopic.Id}`"
                                            id="btnStartTestSession" class="btn btn-primary show-tooltip" rel="nofollow"
                                            v-tooltip="userStore.isLoggedIn ? 'Lerne alle Fragen im Thema' : 'Lerne 5 zufällig ausgewählte Fragen aus dem Thema ' + answerBodyModel.primaryTopic.Name">
                                            <b>Weiterlernen</b>
                                        </NuxtLink>
                                    </div>
                                    <template v-if="props.question.SolutionType == SolutionType.FlashCard">
                                        <div class="btn btn-warning memo-button" @click="flipCard()">
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
                                            <button v-if="!answerBodyModel.IsIntestMode && answerBodyModel.AnswerHelp"
                                                id="flashCard-dontCountAnswer"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="doNotMark()">
                                                Nicht werten!
                                            </button>
                                        </div>
                                    </template>

                                    <template v-else>
                                        <div id="buttons-first-try" class="ButtonGroup">
                                            <button class="btn btn-primary memo-button" @click="answer()">
                                                Antworten
                                            </button>
                                            <button v-if="!answerBodyModel.IsIntestMode && answerBodyModel.AnswerHelp"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="showSolution()">
                                                <font-awesome-icon icon="fa-solid fa-lightbulb" /> Lösung anzeigen
                                            </button>
                                        </div>
                                    </template>

                                    <div
                                        v-if="learningSessionStore.isLearningSession
                                        && !learningSessionStore.isTestMode && learningSessionStore.steps.length - 1 < learningSessionStore.currentIndex">
                                        <button id="aSkipStep" class="SecAction btn btn-link memo-button">
                                            <font-awesome-icon icon="fa-solid fa-forward" /> Frage überspringen
                                        </button>
                                    </div>

                                    <div id="buttons-next-question" class="ButtonGroup" style="display: none;">
                                        <button v-if="answerBodyModel.NextUrl && !answerBodyModel.IsLastQuestion"
                                            @click="loadNextQuestion()" id="btnNext" class="btn btn-primary memo-button"
                                            rel="nofollow">
                                            Nächste Frage
                                        </button>
                                        <button
                                            v-else-if="answerBodyModel.NextUrl == null && answerBodyModel.IsForVideo"
                                            id="continue" class="btn btn-primary clickToContinue memo-button"
                                            style="display: none" @click="continueSession()">
                                            Weiter
                                        </button>

                                        <button
                                            v-if="answerBodyModel.SolutionType != SolutionType.FlashCard && !answerBodyModel.IsIntestMode && !answerBodyModel.AnswerHelp"
                                            href="#" id="aCountAsCorrect"
                                            class="SecAction btn btn-link show-tooltip memo-button"
                                            title="Drücke hier und die Frage wird als richtig beantwortet gewertet"
                                            rel="nofollow" style="display: none;" @click="markAsCorrect()">
                                            Hab ich gewusst!
                                        </button>

                                    </div>

                                    <div v-if="answerBodyModel.SolutionType != SolutionType.FlashCard"
                                        id="buttons-answer-again" class="ButtonGroup" style="display: none">
                                        <button id="btnCheckAgain" class="btn btn-warning memo-button"
                                            rel="nofollow">Nochmal
                                            Antworten</button>
                                        <button v-if="!answerBodyModel.IsIntestMode && answerBodyModel.AnswerHelp"
                                            class="selectorShowSolution SecAction btn btn-link memo-button">
                                            <font-awesome-icon icon="fa-solid fa-lightbulb" />
                                            Lösung anzeigen
                                        </button>
                                    </div>
                                    <div style="clear: both">
                                    </div>
                                </div>
                                <div id="AnswerFeedbackAndSolutionDetails">
                                    <div v-if="answerBodyModel.SolutionType != SolutionType.FlashCard"
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



                                    <div id="SolutionDetails"
                                        v-if="answerBodyModel.QuestionDescription != null && answerBodyModel.QuestionDescription.trim().length > 0"
                                        style="display: none; background-color: white;">
                                        <div id="Description" class="Detail">
                                            <div class="Label">
                                                Ergänzungen
                                                zur
                                                Antwort:
                                            </div>
                                            <div class="Content body-m">
                                                {{ answerBodyModel.QuestionDescription }}
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
                {{ answerBodyModel.TotalActivityPoints }}
            </span>
            <font-awesome-icon icon="fa-solid fa-circle-info"
                v-tooltip="'Du bekommst Lernpunkte für das Beantworten von Fragen'" />
        </div>
        <QuestionAnswerQuestionDetails :answer-body-model="answerBodyModel" />
    </div>

</template>