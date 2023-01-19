<script lang="ts" setup>
import { useLearningSessionStore } from '~/components/topic/learning/learningSessionStore'
import { useUserStore } from '~/components/user/userStore'
import { useTabsStore, Tab } from '~/components/topic/tabs/tabsStore'
import { SolutionType } from '../solutionTypeEnum'
import { useEditQuestionStore } from '../edit/editQuestionStore'
import { useDeleteQuestionStore } from '../edit/delete/deleteQuestionStore'

const props = defineProps(['question', 'answerBodyModel'])
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
</script>

<template>


    <div id="AnswerBody">

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
            {{ props.answerBodyModel.Name }}
        </div>
        <div class="AnswerQuestionBodyMenu">

            <div v-if="showPinButton" class="Pin" :data-question-id="props.answerBodyModel.Id">
                <QuestionPin :questionId="props.answerBodyModel.Id"
                    :is-in-wishknowledge="props.answerBodyModel.IsInWishknowledge" />
            </div>
            <div class="Button dropdown">
                <span class="margin-top-4">
                    <V-Dropdown :distance="0">
                        <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                        <template #popper>

                            <div class="dropdown-row"
                                v-if="tabsStore.activeTab == Tab.Learning && (props.answerBodyModel.IsCreator || userStore.isAdmin)"
                                @click="editQuestionStore.editQuestion(props.answerBodyModel.Id)">
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

        <h3 v-if="props.answerBodyModel.SolutionType != SolutionType.FlashCard" class="QuestionText">
            {{ props.answerBodyModel.Name }}
        </h3>

        <div class="row">

            <div id="MarkdownCol"
                v-if="props.answerBodyModel.SolutionType != SolutionType.FlashCard && props.answerBodyModel.QuestionTextMarkdown != null">
                <div class="RenderedMarkdown">
                    {{ props.answerBodyModel.QuestionTextMarkdown }}
                </div>
            </div>


            <div id="AnswerAndSolutionCol">
                <div id="AnswerAndSolution">
                    <div class="row"
                        :class="{ 'hasFlashCard': props.answerBodyModel.SolutionType == SolutionType.FlashCard }">
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
                            <QuestionAnswerBodyFlashcard v-else ref="flashcard" :question="question" />

                        </div>
                        <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <div id="Buttons">
                                    <div id="btnGoToTestSession" style="display: none">

                                        <a v-if="props.answerBodyModel.HasCategories && !props.answerBodyModel.IsInWidget && !props.answerBodyModel.IsForVideo &&
                                        props.answerBodyModel.IsLastQuestion"
                                            href="<%= Links.CategoryDetailLearningTab(Model.PrimaryCategory) %>"
                                            id="btnStartTestSession" class="btn btn-primary show-tooltip" rel="nofollow"
                                            data-original-title='<%= Model.IsLoggedIn ? "Lerne alle Fragen im Thema " : "Lerne 5 zufällig ausgewählte Fragen aus dem Thema " %><%= Model.PrimaryCategory.Name  %>'>
                                            <b>Weiterlernen</b>
                                        </a>
                                    </div>
                                    <div v-if="props.question.SolutionType == SolutionType.FlashCard"
                                        class="btn btn-warning memo-button" @click="answer()">
                                        Umdrehen
                                    </div>

                                    <div v-if="props.answerBodyModel.SolutionType != SolutionType.FlashCard"
                                        id="buttons-first-try" class="ButtonGroup">
                                        <a href="#" id="btnCheck" class="btn btn-primary memo-button"
                                            rel="nofollow">Antworten</a>
                                        <a v-if="!props.answerBodyModel.IsIntestMode && props.answerBodyModel.AnswerHelp"
                                            href="#" class="selectorShowSolution SecAction btn btn-link memo-button"><i
                                                class="fa fa-lightbulb-o">&nbsp;</i>Lösung
                                            anzeigen</a>
                                    </div>
                                    <div v-else id="buttons-answer" class="ButtonGroup flashCardAnswerButtons"
                                        style="display: none">
                                        <a href="#" id="btnRightAnswer" class="btn btn-warning memo-button"
                                            rel="nofollow">
                                            Wusste ich!
                                        </a>
                                        <a href="#" id="btnWrongAnswer" class="btn btn-warning memo-button"
                                            rel="nofollow">
                                            Wusste ich nicht!
                                        </a>
                                        <a v-if="!props.answerBodyModel.IsIntestMode && props.answerBodyModel.AnswerHelp"
                                            href="#" id="flashCard-dontCountAnswer"
                                            class="selectorShowSolution SecAction btn btn-link memo-button">
                                            Nicht werten!
                                        </a>
                                    </div>
                                    <div
                                        v-if="props.answerBodyModel.IsLearningSession && props.answerBodyModel.NextUrl != null && !props.answerBodyModel.IsIntestMode">

                                        <a id="aSkipStep" href="<%= Model.NextUrl(Url) %>"
                                            class="SecAction btn btn-link memo-button"><i
                                                class="fa fa-step-forward">&nbsp;</i>Frage überspringen</a>
                                    </div>

                                    <div id="buttons-next-question" class="ButtonGroup" style="display: none;">
                                        <a v-if="props.answerBodyModel.NextUrl && !props.answerBodyModel.IsLastQuestion"
                                            href="<%= Model.NextUrl(Url) %>" id="btnNext"
                                            class="btn btn-primary memo-button" rel="nofollow">
                                            Nächste Frage
                                        </a>
                                        <button
                                            v-else-if="props.answerBodyModel.NextUrl == null && props.answerBodyModel.IsForVideo"
                                            id="continue" class="btn btn-primary clickToContinue memo-button"
                                            style="display: none">
                                            Weiter
                                        </button>

                                        <a v-if="props.answerBodyModel.SolutionType != SolutionType.FlashCard && !props.answerBodyModel.IsIntestMode && !props.answerBodyModel.AnswerHelp"
                                            href="#" id="aCountAsCorrect"
                                            class="SecAction btn btn-link show-tooltip memo-button"
                                            title="Drücke hier und die Frage wird als richtig beantwortet gewertet"
                                            rel="nofollow" style="display: none;">
                                            Hab ich gewusst!
                                        </a>

                                    </div>

                                    <div v-if="props.answerBodyModel.SolutionType != SolutionType.FlashCard"
                                        id="buttons-answer-again" class="ButtonGroup" style="display: none">
                                        <a href="#" id="btnCheckAgain" class="btn btn-warning memo-button"
                                            rel="nofollow">Nochmal
                                            Antworten</a>
                                        <a v-if="!props.answerBodyModel.IsIntestMode && props.answerBodyModel.AnswerHelp"
                                            href="#" class="selectorShowSolution SecAction btn btn-link memo-button"><i
                                                class="fa fa-lightbulb-o">&nbsp;</i>
                                            Lösung anzeigen</a>
                                    </div>
                                    <div style="clear: both">
                                    </div>
                                </div>
                                <div id="AnswerFeedbackAndSolutionDetails">
                                    <div v-if="props.answerBodyModel.SolutionType != SolutionType.FlashCard"
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
                                            <span id="spnWrongAnswer" style="color: #B13A48"><b>Falsch
                                                    beantwortet
                                                </b></span>
                                            <a href="#" id="CountWrongAnswers" style="float: right;">(zwei
                                                Versuche)</a><br />

                                            <div style="margin-top: 5px;" id="answerFeedbackTry">
                                                Du könntest es
                                                wenigstens
                                                probieren!</div>

                                            <div id="divWrongAnswers" style="margin-top: 7px; display: none;">
                                                <span class="WrongAnswersHeading">Deine
                                                    bisherigen
                                                    Antwortversuche:</span>
                                                <ul style="padding-top: 5px;" id="ulAnswerHistory">
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="SolutionDetailsSpinner" style="display: none;">
                                        <i class="fa fa-spinner fa-spin" style="color: #b13a48;"></i>
                                    </div>



                                    <div id="SolutionDetails"
                                        v-if="props.answerBodyModel.QuestionDescription != null && props.answerBodyModel.QuestionDescription.trim().length > 0"
                                        style="display: none; background-color: white;">
                                        <div id="Description" class="Detail">
                                            <div class="Label">
                                                Ergänzungen
                                                zur
                                                Antwort:
                                            </div>
                                            <div class="Content body-m">
                                                {{ props.answerBodyModel.QuestionDescription }}
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
                {{ props.answerBodyModel.TotalActivityPoints }}
            </span>
            <span style="display: inline-block; white-space: nowrap;" class="show-tooltip" data-placement="bottom"
                title="Du bekommst Lernpunkte für das Beantworten von Fragen">
                <i class="fa fa-info-circle"></i>
            </span>
        </div>
        <QuestionAnswerQuestionDetails :answer-body-model="props.answerBodyModel" />
    </div>

</template>