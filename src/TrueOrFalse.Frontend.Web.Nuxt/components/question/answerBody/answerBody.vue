<script lang="ts" setup>
import { useLearningSessionStore, AnswerState } from '~/components/topic/learning/learningSessionStore'
import { useUserStore } from '~/components/user/userStore'
import { useTabsStore, Tab } from '~/components/topic/tabs/tabsStore'
import { SolutionType } from '../solutionTypeEnum'
import { useEditQuestionStore } from '../edit/editQuestionStore'
import { useDeleteQuestionStore } from '../edit/delete/deleteQuestionStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { getHighlightedCode } from '~~/components/shared/utils'
import { Activity, useActivityPointsStore } from '~~/components/activityPoints/activityPointsStore'
import { random, handleNewLine } from '~/components/shared/utils'
import { AnswerBodyModel, SolutionData } from '~~/components/question/answerBody/answerBodyInterfaces'

const spinnerStore = useSpinnerStore()
const learningSessionStore = useLearningSessionStore()
const deleteQuestionStore = useDeleteQuestionStore()
const activityPointsStore = useActivityPointsStore()

interface Props {
    isLandingPage?: boolean
    landingPageModel?: AnswerBodyModel
    landingPageSolutionData?: SolutionData
}
const props = defineProps<Props>()

const userStore = useUserStore()
const tabsStore = useTabsStore()
const editQuestionStore = useEditQuestionStore()

const answerIsCorrect = ref(false)
const answerIsCorrectPopUp = ref(false)
watch(answerIsCorrect, (val) => {
    if (val)
        answerIsCorrectPopUp.value = true
    setTimeout(() => {
        answerIsCorrectPopUp.value = false
    }, 1200)
})

const answerIsWrong = ref(false)
const answerIsWrongPopUp = ref(false)
watch(answerIsWrong, (val) => {
    if (val)
        answerIsWrongPopUp.value = true
    setTimeout(() => {
        answerIsWrongPopUp.value = false
    }, 1200)
})


function openCommentModal() {

}

const amountOfTries = ref(0)
const amountOfTriesText = ref('')

watch(amountOfTries, (val) => {
    const tryTexts = ["0 Versuche", "ein Versuch", "zwei", "drei", "vier", "fünf", "sehr hartnäckig", "Respekt!"]

    switch (val) {
        case 0:
        case 1:
            amountOfTriesText.value = tryTexts[val]
            break
        case 2:
        case 3:
        case 4:
        case 5:
            amountOfTriesText.value = tryTexts[val] + " Versuche"
            break
        case 6:
        case 7:
            amountOfTriesText.value = tryTexts[val]
            break
        default:
            amountOfTriesText.value = tryTexts[7]
    }
})
const answersSoFar = ref<string[]>([])
const showWrongAnswers = ref(false)

const multipleChoice = ref()
const text = ref()
const matchList = ref()
const flashcard = ref()
const showAnswer = ref(false)
const showAnswerButtons = ref(true)

function flip() {
    flashcard.value?.flip()
}

const _errMsgs = [
    "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
    "Weiter, weiter, nicht aufgeben.",
    "Übung macht den Meister. Du bist auf dem richtigen Weg.",
]

const _repeatedErrMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
    "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden."]

const _successMsgs = ["Yeah!", "Du bist auf einem guten Weg.", "Sauber!", "Well done!", "Toll!", "Weiter so!", "Genau.", "Absolut.",
    "Richtiger wird's nicht.", "Fehlerlos!", "Korrrrrekt!", "Einwandfrei", "Mehr davon!", "Klasse.", "Schubidu!",
    "Wer sagt's denn!", "Exakt.", "So ist es.", "Da kannste nicht meckern.", "Sieht gut aus.", "Oha!", "Rrrrrrichtig!"]

const wellDoneMsg = ref('')

const wrongAnswerMsg = ref('')

async function answer() {
    showWrongAnswers.value = false

    if (answerBodyModel.value?.solutionType == SolutionType.Text && text.value.getAnswerText().trim().length == 0) {
        wrongAnswerMsg.value = 'Du könntest es ja wenigstens probieren ... (Wird nicht als Antwortversuch gewertet.)'
        showWrongAnswers.value = true
        return
    }
    wrongAnswerMsg.value = ''
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

    const repeatedAnswer = answersSoFar.value.indexOf(solutionComponent.getAnswerText()) >= 0
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

    if (result) {
        if (result.correct) {
            activityPointsStore.addPoints(Activity.CorrectAnswer)
            learningSessionStore.markCurrentStepAsCorrect()
            answerIsWrong.value = false
            answerIsCorrect.value = true
            wellDoneMsg.value = _successMsgs[random(0, _successMsgs.length - 1)]
        }
        else {
            activityPointsStore.addPoints(Activity.WrongAnswer)
            learningSessionStore.markCurrentStepAsWrong()
            answerIsCorrect.value = false
            answerIsWrong.value = true
            wrongAnswerMsg.value = repeatedAnswer ? _repeatedErrMsgs[random(0, _repeatedErrMsgs.length - 1)] : _errMsgs[random(0, _errMsgs.length - 1)]
        }

        showAnswerButtons.value = false

        if (learningSessionStore.isInTestMode
            || answerIsCorrect.value
            || (answerBodyModel.value?.solutionType != SolutionType.MultipleChoice && amountOfTries.value > 1)
            || (answerBodyModel.value?.solutionType == SolutionType.MultipleChoice && allMultipleChoiceCombinationTried.value))
            loadSolution()

        if (result.newStepAdded)
            learningSessionStore.loadSteps()

        learningSessionStore.currentStep!.isLastStep = result.isLastStep
    }
}

const flashCardAnswered = ref(false)
const markFlashCardAsCorrect = ref(false)

function answerFlashcard(isCorrect: boolean) {
    markFlashCardAsCorrect.value = isCorrect
    flashCardAnswered.value = true
    answer()
}

function highlightCode() {
    const el = document.getElementById('AnswerBody')
    if (el != null)
        el.querySelectorAll('code').forEach(block => {
            if (block.textContent != null)
                block.innerHTML = getHighlightedCode(block.textContent)
        })
}

const answerBodyModel = ref<AnswerBodyModel | null>(null)
async function loadAnswerBodyModel() {
    if (!learningSessionStore.currentStep)
        return

    const result = await $fetch<AnswerBodyModel>(`/apiVue/AnswerBody/Get/?index=${learningSessionStore.currentIndex}`, {
        mode: 'cors',
        credentials: 'include'
    })
    if (result != null) {
        flashCardAnswered.value = false
        answerIsCorrect.value = false
        answerIsWrong.value = false
        wellDoneMsg.value = ('')
        wrongAnswerMsg.value = ('')
        showAnswer.value = false
        amountOfTries.value = 0
        showAnswerButtons.value = true
        answerBodyModel.value = result
        solutionData.value = null
        answersSoFar.value = []
        await nextTick()
        highlightCode()
    }
}

async function markAsCorrect() {

    const data = {
        questionViewGuid: answerBodyModel.value!.questionViewGuid,
        amountOfTries: amountOfTries.value,
    }

    const result = $fetch<any>('/apiVue/AnswerBody/MarkAsCorrect', {
        method: 'POST',
        mode: 'cors',
        body: data,
    })

    if (result != null) {
        activityPointsStore.addPoints(Activity.CountAsCorrect)
        learningSessionStore.markCurrentStepAsCorrect()
        answerIsWrong.value = false
        answerIsCorrect.value = true
        wellDoneMsg.value = _successMsgs[random(0, _successMsgs.length - 1)]
    }
}
const solutionData = ref<SolutionData | null>(null)

async function loadSolution(answered: boolean = true) {
    showAnswerButtons.value = false
    if (answerBodyModel.value?.solutionType == SolutionType.Text) {
        showWrongAnswers.value = true
    }
    showAnswer.value = true
    const data = {
        id: answerBodyModel.value?.id,
        questionViewGuid: answerBodyModel.value?.questionViewGuid,
        interactionNumber: amountOfTries.value,
        unanswered: !answered
    }
    const solutionResult = await $fetch<SolutionData>('/apiVue/AnswerBody/GetSolution',
        {
            method: 'POST',
            body: data,
            mode: 'cors',
            credentials: 'include'
        })
    if (solutionResult != null) {
        if (answerBodyModel.value!.solutionType == SolutionType.MultipleChoice && solutionResult.answer == null) {
            solutionResult.answer = 'Keine der Antworten ist richtig!'
            solutionResult.answerAsHTML = solutionResult.answer
        }
        solutionData.value = solutionResult

        if (!answered)
            learningSessionStore.markCurrentStep(AnswerState.ShowedSolutionOnly)

        highlightCode()
    }

}

watch(() => learningSessionStore.currentStep?.index, () => {
    loadAnswerBodyModel()
})
learningSessionStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'startNewSession')
            loadAnswerBodyModel()
    })
})

function loadResult() {
    answerBodyModel.value = null
    learningSessionStore.showResult = true
}

function startNewSession() {
    learningSessionStore.showResult = false
    learningSessionStore.startNewSession()
}

onBeforeMount(() => {
    if (props.isLandingPage && props.landingPageModel && props.landingPageSolutionData) {
        answerBodyModel.value = props.landingPageModel
        solutionData.value = props.landingPageSolutionData
        showAnswer.value = true
    }
})

const allMultipleChoiceCombinationTried = computed(() => {
    if (answerBodyModel.value?.solutionType == SolutionType.MultipleChoice) {
        interface Choice {
            Text: string,
            IsCorrect: boolean
        }
        const json: { Choices: Choice[], isSolutionOrdered: boolean } = JSON.parse(answerBodyModel.value.solution)
        const maxCombinationCount = 2 ** json.Choices.length
        const uniqueAnswerCount = [...new Set(answersSoFar.value)].length
        return uniqueAnswerCount >= maxCombinationCount
    }
    return false
})
</script>

<template>
    <div id="AnswerBody" v-if="answerBodyModel && (!learningSessionStore.showResult || props.isLandingPage)"
        class="col-xs-12">
        <div class="answerbody-header">

            <div class="answerbody-text">
                <h3 v-if="answerBodyModel.solutionType != SolutionType.FlashCard" class="QuestionText">
                    {{ answerBodyModel.text }}
                </h3>
            </div>

            <div class="AnswerQuestionBodyMenu">

                <div class="Pin answerbody-btn" :data-question-id="answerBodyModel.id">
                    <div class="answerbody-btn-inner">
                        <QuestionPin :question-id="answerBodyModel.id" :key="answerBodyModel.id"
                            :is-in-wishknowledge="answerBodyModel.isInWishknowledge" />
                    </div>
                </div>
                <div class="Button dropdown answerbody-btn">
                    <div class="answerbody-btn-inner">
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

                    </div>
                </div>

            </div>


        </div>

        <div class="row">

            <div id="MarkdownCol"
                v-if="answerBodyModel.solutionType != SolutionType.FlashCard && !!answerBodyModel.renderedQuestionTextExtended">
                <div class="RenderedMarkdown" v-html="handleNewLine(answerBodyModel.renderedQuestionTextExtended)">
                </div>
            </div>


            <div id="AnswerAndSolutionCol">
                <div id="AnswerAndSolution">
                    <div class="row"
                        :class="{ 'hasFlashCard': answerBodyModel.solutionType == SolutionType.FlashCard }">
                        <div id="AnswerInputSection">
                            <template v-if="answerBodyModel.solutionType != SolutionType.FlashCard">
                                <Transition name="fade">
                                    <div class="answerFeedback answerFeedbackCorrect" v-show="answerIsCorrectPopUp">
                                        <font-awesome-icon icon="fa-solid fa-circle-check" />&nbsp;Richtig!
                                    </div>
                                </Transition>
                                <Transition name="fade">
                                    <div class="answerFeedback answerFeedbackWrong" v-show="answerIsWrongPopUp">
                                        <font-awesome-icon icon="fa-solid fa-circle-minus" />&nbsp;Leider falsch
                                    </div>
                                </Transition>
                            </template>

                            <Transition name="fade">
                                <QuestionAnswerBodyFlashcard :key="answerBodyModel.id + 'flashcard'"
                                    v-if="answerBodyModel.solutionType == SolutionType.FlashCard" ref="flashcard"
                                    :solution="answerBodyModel.solution" :text="answerBodyModel.text"
                                    :marked-as-correct="markFlashCardAsCorrect" @flipped="amountOfTries++" />
                                <QuestionAnswerBodyMatchlist :key="answerBodyModel.id + 'matchlist'"
                                    v-else-if="answerBodyModel.solutionType == SolutionType.MatchList" ref="matchList"
                                    :solution="answerBodyModel.solution" :show-answer="showAnswer"
                                    @flipped="amountOfTries++" />
                                <QuestionAnswerBodyMultipleChoice :key="answerBodyModel.id + 'multiplechoice'"
                                    v-else-if="answerBodyModel.solutionType == SolutionType.MultipleChoice"
                                    :solution="answerBodyModel.solution" :show-answer="showAnswer"
                                    ref="multipleChoice" />
                                <QuestionAnswerBodyText :key="answerBodyModel.id + 'text'"
                                    v-else-if="answerBodyModel.solutionType == SolutionType.Text" ref="text"
                                    :show-answer="showAnswer" />
                            </Transition>

                        </div>
                        <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <div id="Buttons" v-if="props.isLandingPage">
                                    <div id="btnGoToTestSession">

                                        <NuxtLink v-if="answerBodyModel.hasTopics"
                                            :to="`${answerBodyModel.primaryTopicUrl}/Lernen`" id="btnStartTestSession"
                                            class="btn btn-primary show-tooltip" rel="nofollow"
                                            v-tooltip="userStore.isLoggedIn ? 'Lerne alle Fragen im Thema' : 'Lerne 5 zufällig ausgewählte Fragen aus dem Thema ' + answerBodyModel.primaryTopicName">
                                            <b>Weiterlernen</b>
                                        </NuxtLink>
                                    </div>
                                </div>
                                <div id="Buttons" v-else>

                                    <template
                                        v-if="answerBodyModel.solutionType == SolutionType.FlashCard && !flashCardAnswered">

                                        <div class="btn btn-warning memo-button" @click="flip()"
                                            v-if="amountOfTries == 0">
                                            Umdrehen
                                        </div>
                                        <div id="buttons-answer" class="ButtonGroup flashCardAnswerButtons" v-else>
                                            <button id="btnRightAnswer" class="btn btn-warning memo-button"
                                                @click="answerFlashcard(true)">
                                                Wusste ich!
                                            </button>
                                            <button id="btnWrongAnswer" class="btn btn-warning memo-button"
                                                @click="answerFlashcard(false)">
                                                Wusste ich nicht!
                                            </button>
                                            <button
                                                v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp"
                                                id="flashCard-dontCountAnswer"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="learningSessionStore.skipStep()">
                                                Nicht werten!
                                            </button>
                                        </div>


                                    </template>

                                    <template v-else-if="showAnswerButtons">
                                        <div id="buttons-first-try" class="ButtonGroup">
                                            <button class="btn btn-primary memo-button" @click="answer()">
                                                Antworten
                                            </button>
                                            <button
                                                v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp && !showAnswer"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="loadSolution(false)">
                                                <font-awesome-icon icon="fa-solid fa-lightbulb" /> Lösung anzeigen
                                            </button>
                                        </div>
                                    </template>

                                    <div v-if="learningSessionStore.isLearningSession
                                    && !learningSessionStore.isInTestMode && amountOfTries == 0 && !showAnswer">
                                        <button class="SecAction btn btn-link memo-button"
                                            @click="learningSessionStore.skipStep()">
                                            <font-awesome-icon icon="fa-solid fa-forward" /> Frage überspringen
                                        </button>
                                    </div>

                                    <div id="buttons-next-question" class="ButtonGroup"
                                        v-if="(amountOfTries > 0 || showAnswer) && !learningSessionStore.currentStep?.isLastStep && !showAnswerButtons">
                                        <button v-if="!learningSessionStore.currentStep?.isLastStep"
                                            @click="learningSessionStore.loadNextQuestionInSession()" id="btnNext"
                                            class="btn btn-primary memo-button" rel="nofollow">
                                            Nächste Frage
                                        </button>

                                        <button
                                            v-if="answerBodyModel.solutionType == SolutionType.Text && !learningSessionStore.isInTestMode && learningSessionStore.answerHelp && answerIsWrong"
                                            href="#" id="aCountAsCorrect"
                                            class="SecAction btn btn-link show-tooltip memo-button"
                                            title="Drücke hier und die Frage wird als richtig beantwortet gewertet"
                                            rel="nofollow" @click="markAsCorrect()">
                                            Hab ich gewusst!
                                        </button>
                                    </div>

                                    <div v-else-if="learningSessionStore.currentStep?.isLastStep && amountOfTries > 0">
                                        <button @click="loadResult()" class="btn btn-primary memo-button"
                                            rel="nofollow">
                                            Zum Ergebnis
                                        </button>
                                    </div>

                                    <div v-if="answerBodyModel.solutionType != SolutionType.FlashCard"
                                        id="buttons-answer-again" class="ButtonGroup">
                                        <button
                                            v-if="!allMultipleChoiceCombinationTried && !learningSessionStore.isInTestMode && answerIsWrong && !showAnswer && !showAnswerButtons"
                                            id="btnCheckAgain" class="btn btn-warning memo-button" rel="nofollow"
                                            @click="answer()">
                                            Nochmal Antworten
                                        </button>
                                        <button
                                            v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp && !showAnswer && !showAnswerButtons"
                                            class="selectorShowSolution SecAction btn btn-link memo-button"
                                            @click="loadSolution(true)">
                                            <font-awesome-icon icon="fa-solid fa-lightbulb" /> Lösung anzeigen
                                        </button>
                                    </div>
                                    <div style="clear: both">
                                    </div>
                                </div>

                                <div id="AnswerFeedbackAndSolutionDetails">
                                    <div v-if="answerBodyModel.solutionType != SolutionType.FlashCard"
                                        id="AnswerFeedback">
                                        <div id="divAnsweredCorrect" v-if="answerIsCorrect">
                                            <b class="correct-answer-label">Richtig! </b>
                                            <div v-html="wellDoneMsg" v-if="wellDoneMsg.length > 0"></div>
                                        </div>

                                        <div id="Solution" v-if="showAnswer">
                                            <div class="solution-label">
                                                Richtige Antwort:
                                            </div>
                                            <div class="Content body-m"
                                                v-html="handleNewLine(solutionData?.answerAsHTML)">
                                            </div>
                                        </div>

                                        <div id="divWrongAnswer" v-if="answerIsWrong">
                                            <div id="spnWrongAnswer">
                                                <b class="wrong-answer-label">Falsch beantwortet </b>
                                            </div>
                                            <div v-html="wrongAnswerMsg" v-if="wrongAnswerMsg.length > 0">
                                            </div>
                                            <br />

                                            <div id="divWrongAnswers" v-if="showWrongAnswers">
                                                <span class="WrongAnswersHeading">
                                                    Deine {{ answersSoFar.length == 1 ? 'Antwort' : 'Antworten' }}:
                                                </span>
                                                <ul id="ulAnswerHistory">
                                                    <li v-for="answers in answersSoFar" v-html="answers"></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="SolutionDetails"
                                        v-if="answerBodyModel.description?.trim().length > 0 && showAnswer">
                                        <div id="Description">
                                            <div class="solution-label">
                                                Ergänzungen zur Antwort:
                                            </div>
                                            <div class="Content body-m"
                                                v-html="handleNewLine(solutionData?.answerDescription)">
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
            <div>
                <small>Dein Punktestand</small>

            </div>
            <div class="activitypoints-display-detail">
                <span id="ActivityPoints">
                    {{ activityPointsStore.points }}
                </span>
                <font-awesome-icon icon="fa-solid fa-circle-info" class="activity-points-icon"
                    v-tooltip="'Du bekommst Lernpunkte für das Beantworten von Fragen'" />
            </div>

        </div>
        <QuestionAnswerQuestionDetails :id="answerBodyModel.id" />
    </div>
    <div v-else-if="learningSessionStore.showResult">
        <QuestionAnswerBodyLearningSessionResult @start-new-session="startNewSession" />
    </div>


</template>


<style lang="less">
@import '~~/assets/views/answerQuestion.less';

#AnswerBody {
    transition: all 1s ease-in-out;
}

.answerbody-header {
    display: flex;
    flex-wrap: nowrap;
    justify-content: space-between;
    margin-bottom: 24px;

    .answerbody-text {
        margin-right: 8px;

        h3 {
            margin-top: 0;
        }
    }

    .answerbody-btn {
        font-size: 18px;

        .answerbody-btn-inner {
            cursor: pointer;
            background: white;
            height: 32px;
            width: 32px;
            display: flex;
            justify-content: center;
            align-items: center;
            border-radius: 15px;

            .fa-ellipsis-vertical {
                color: @memo-grey-dark;
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

.activity-points-icon {
    font-size: 14px;
}

#ActivityPointsDisplay {
    .activitypoints-display-detail {
        display: flex;
        justify-content: flex-end;
        align-items: center;

        #ActivityPoints {
            margin-right: 8px;
        }
    }
}

#ulAnswerHistory {
    padding-top: 5px;
}

.solution-label {
    font-weight: bold;
    padding-right: 5px;
}

.correct-answer-label {
    color: @memo-green-correct;
}

.wrong-answer-label {
    color: @memo-red-wrong;
}
</style>