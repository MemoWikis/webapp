<script lang="ts" setup>
import { useLearningSessionStore, AnswerState } from '~/components/page/learning/learningSessionStore'
import { useUserStore } from '~/components/user/userStore'
import { useTabsStore, Tab } from '~/components/page/tabs/tabsStore'
import { SolutionType } from '../solutionTypeEnum'
import { getHighlightedCode, random } from '~/utils/utils'
import { Activity, useActivityPointsStore } from '~~/components/activityPoints/activityPointsStore'
import { AnswerBodyModel, SolutionData } from '~~/components/question/answerBody/answerBodyInterfaces'
import { usePageStore } from '~/components/page/pageStore'
import { useCommentsStore } from '~/components/comment/commentsStore'
import { usePublishQuestionStore } from '../edit/publish/publishQuestionStore'

const learningSessionStore = useLearningSessionStore()
const activityPointsStore = useActivityPointsStore()
const pageStore = usePageStore()
const userStore = useUserStore()
const tabsStore = useTabsStore()
const commentsStore = useCommentsStore()
const publishQuestionStore = usePublishQuestionStore()
const { t } = useI18n()

commentsStore.loadComments()

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

const amountOfTries = ref(0)
const amountOfTriesText = ref('')

watch(amountOfTries, (val) => {
    switch (val) {
        case 0:
        case 1:
            amountOfTriesText.value = t(`answerbody.tries.${val}`)
            break
        case 2:
        case 3:
        case 4:
        case 5:
            amountOfTriesText.value = t(`answerbody.tries.${val}`)
            break
        case 6:
        case 7:
            amountOfTriesText.value = t(`answerbody.tries.${val}`)
            break
        default:
            amountOfTriesText.value = t('answerbody.tries.many')
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
    'answerbody.errorMessages.churchill',
    'answerbody.errorMessages.keepGoing',
    'answerbody.errorMessages.practice'
]

const _repeatedErrMsgs = [
    'answerbody.repeatedErrorMessages.confucius',
    'answerbody.repeatedErrorMessages.balanced'
]

const _successMsgs = [
    'answerbody.successMessages.yeah',
    'answerbody.successMessages.goodWay',
    'answerbody.successMessages.clean',
    'answerbody.successMessages.wellDone',
    'answerbody.successMessages.great',
    'answerbody.successMessages.keepItUp',
    'answerbody.successMessages.exactly',
    'answerbody.successMessages.absolutely',
    'answerbody.successMessages.cantBeMoreCorrect',
    'answerbody.successMessages.flawless',
    'answerbody.successMessages.correct',
    'answerbody.successMessages.perfect',
    'answerbody.successMessages.more',
    'answerbody.successMessages.awesome',
    'answerbody.successMessages.schubidu',
    'answerbody.successMessages.thereYouGo',
    'answerbody.successMessages.exact',
    'answerbody.successMessages.thatsIt',
    'answerbody.successMessages.nothingToComplain',
    'answerbody.successMessages.looksGood',
    'answerbody.successMessages.oha',
    'answerbody.successMessages.rrrright'
]

const wellDoneMsg = ref('')

const wrongAnswerMsg = ref('')
const { $logger, $urlHelper } = useNuxtApp()

async function answer() {
    showWrongAnswers.value = false

    if (answerBodyModel.value?.solutionType === SolutionType.Text && text.value.getAnswerText().trim().length === 0) {
        wrongAnswerMsg.value = t('answerbody.emptyAnswerMessage')
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
        case SolutionType.Flashcard:
            solutionComponent = flashcard.value
            break
    }
    if (solutionComponent == null)
        return

    const repeatedAnswer = answersSoFar.value.indexOf(solutionComponent.getAnswerText()) >= 0
    answersSoFar.value.push(solutionComponent.getAnswerText())
    const data = {
        answer: await solutionComponent.getAnswerDataString(),
        id: answerBodyModel.value?.id,
        questionViewGuid: answerBodyModel.value?.questionViewGuid,
        inTestMode: learningSessionStore.isInTestMode,
        isLearningSession: learningSessionStore.isLearningSession
    }

    const result = await $api<any>(`/apiVue/AnswerBody/SendAnswerToLearningSession/`,
        {
            method: 'POST',
            body: data,
            credentials: 'include',
            mode: 'cors',
            onResponseError(context) {
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
            }
        })

    if (result) {
        learningSessionStore.knowledgeStatusChanged(answerBodyModel.value!.id)
        await pageStore.reloadKnowledgeSummary()
        if (result.correct) {
            activityPointsStore.addPoints(Activity.CorrectAnswer)
            learningSessionStore.markCurrentStepAsCorrect()
            answerIsWrong.value = false
            answerIsCorrect.value = true
            wellDoneMsg.value = t(_successMsgs[random(0, _successMsgs.length - 1)])
        }
        else {
            activityPointsStore.addPoints(Activity.WrongAnswer)
            learningSessionStore.markCurrentStepAsWrong()
            answerIsCorrect.value = false
            answerIsWrong.value = true
            wrongAnswerMsg.value = repeatedAnswer ? t(_repeatedErrMsgs[random(0, _repeatedErrMsgs.length - 1)]) : t(_errMsgs[random(0, _errMsgs.length - 1)])
        }

        showAnswerButtons.value = false

        if (learningSessionStore.isInTestMode
            || answerIsCorrect.value
            || (answerBodyModel.value?.solutionType != SolutionType.MultipleChoice && amountOfTries.value > 1)
            || (answerBodyModel.value?.solutionType === SolutionType.MultipleChoice && allMultipleChoiceCombinationTried.value))
            await loadSolution()

        await nextTick()
        if (result.newStepAdded)
            await learningSessionStore.loadSteps()

        learningSessionStore.currentStep!.isLastStep = result.isLastStep
    }
}

const flashCardAnswered = ref(false)
const markFlashcardAsCorrect = ref(false)

function answerFlashcard(isCorrect: boolean) {
    markFlashcardAsCorrect.value = isCorrect
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

const answerBodyModel = ref<AnswerBodyModel>()

const currentRequest = ref<AbortController | null>(null)

async function loadAnswerBodyModel() {
    if (!learningSessionStore.currentStep)
        return

    if (currentRequest.value) {
        currentRequest.value.abort()
    }

    currentRequest.value = new AbortController()

    const result = await $api<AnswerBodyModel>(`/apiVue/AnswerBody/Get/${learningSessionStore.currentIndex}`, {
        mode: 'cors',
        credentials: 'include',
        signal: currentRequest.value.signal,
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    }).catch(() => {
        return null
    })

    currentRequest.value = null

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
        if (tabsStore.isLearning)
            attachQuestionIdToUrl()
    }
}

const router = useRouter()
async function attachQuestionIdToUrl() {
    if (!tabsStore.isLearning || !answerBodyModel.value?.id || answerBodyModel.value.id <= 0)
        return

    const pathSegments = window.location.pathname.split('/')
        .filter(segment => segment.length > 0)

    const currentPageId = pathSegments.length >= 2 && !isNaN(parseInt(pathSegments[1]))
        ? parseInt(pathSegments[1])
        : null

    if (currentPageId === pageStore.id) {
        const newPath = $urlHelper.getPageUrlWithQuestionId(
            pageStore.name,
            pageStore.id,
            answerBodyModel.value.id
        )

        if (newPath !== window.location.pathname)
            router.push(newPath)
    }
}

watch(() => pageStore.id, (newId, oldId) => {
    if (newId !== oldId && currentRequest.value) {
        currentRequest.value.abort()
        currentRequest.value = null
    }
})

const route = useRoute()
watch(() => tabsStore.activeTab, () => {
    if (tabsStore.isLearning && isNaN(parseInt(route.params.questionId?.toString()))) {
        attachQuestionIdToUrl()
    }
})

async function markAsCorrect() {

    const data = {
        id: answerBodyModel.value!.id,
        questionViewGuid: answerBodyModel.value!.questionViewGuid,
        amountOfTries: amountOfTries.value,
    }

    const result = await $api<boolean>('/apiVue/AnswerBody/MarkAsCorrect', {
        method: 'POST',
        mode: 'cors',
        body: data,
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText} `, [{ response: context.response, host: context.request }])
        }
    })

    if (result != false) {
        activityPointsStore.addPoints(Activity.CountAsCorrect)
        learningSessionStore.markCurrentStepAsCorrect()
        answerIsWrong.value = false
        answerIsCorrect.value = true
        wellDoneMsg.value = t(_successMsgs[random(0, _successMsgs.length - 1)])
    }
}
const solutionData = ref<SolutionData | null>(null)

async function loadSolution(answered: boolean = true) {
    showAnswerButtons.value = false
    if (answerBodyModel.value?.solutionType === SolutionType.Text) {
        showWrongAnswers.value = true
    }
    showAnswer.value = true
    const data = {
        id: answerBodyModel.value?.id,
        questionViewGuid: answerBodyModel.value?.questionViewGuid,
        interactionNumber: amountOfTries.value,
        unanswered: !answered
    }
    const solutionResult = await $api<SolutionData>('/apiVue/AnswerBody/GetSolution',
        {
            method: 'POST',
            body: data,
            mode: 'cors',
            credentials: 'include',
            onResponseError(context) {
                $logger.error(`fetch Error: ${context.response?.statusText} `, [{ response: context.response, host: context.request }])
            }
        })
    if (solutionResult != null) {
        if (answerBodyModel.value!.solutionType === SolutionType.MultipleChoice && solutionResult.answer == null) {
            solutionResult.answer = t('answerbody.noCorrectAnswers')
            solutionResult.answerAsHTML = solutionResult.answer
        }
        solutionData.value = solutionResult

        if (!answered)
            learningSessionStore.markCurrentStep(AnswerState.ShowedSolutionOnly)

        highlightCode()
    }

}

onMounted(() => {
    watch([() => learningSessionStore.currentStep?.index, () => learningSessionStore.currentStep?.id], () => {
        loadAnswerBodyModel()
    })

    learningSessionStore.$onAction(({ name, after }) => {
        if (name === 'startNewSession') {

            after((newSession) => {
                if (newSession)
                    loadAnswerBodyModel()
            })
        }

        if (name === 'reloadAnswerBody') {

            after((result) => {
                if (result.id === answerBodyModel.value?.id && learningSessionStore.currentIndex === result.index)
                    loadAnswerBodyModel()
            })
        }
    })

    watch(() => userStore.isLoggedIn, () => learningSessionStore.startNewSession())
})

function loadResult() {
    answerBodyModel.value = undefined
    learningSessionStore.showResult = true
}

function startNewSession() {
    learningSessionStore.showResult = false
    learningSessionStore.startNewSession()
}

const allMultipleChoiceCombinationTried = computed(() => {
    if (answerBodyModel.value?.solutionType === SolutionType.MultipleChoice) {
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

watch(() => pageStore.id, () => learningSessionStore.showResult = false)

publishQuestionStore.$onAction(({ name, after }) => {
    if (name === 'confirmPublish') {
        after((id) => {
            if (id && id === answerBodyModel.value?.id)
                answerBodyModel.value!.isPrivate = false
        })
    }
})


</script>

<template>
    <div id="AnswerBody" v-if="answerBodyModel && !learningSessionStore.showResult">
        <div class="answerbody-header">

            <div class="answerbody-text">
                <h3 v-if="answerBodyModel.solutionType != SolutionType.Flashcard" class="QuestionText">
                    {{ answerBodyModel.text }}
                </h3>
            </div>

            <div class="AnswerQuestionBodyMenu">
                <div class="answerbody-btn visibility" v-if="answerBodyModel.isPrivate">
                    <div class="answerbody-btn-inner" @click="publishQuestionStore.openModal(answerBodyModel.id)">
                        <font-awesome-icon :icon="['fas', 'lock']" class="no-hover" />
                        <font-awesome-icon :icon="['fas', 'unlock']" class="hover" />
                    </div>
                </div>
                <div class="Pin answerbody-btn" :data-question-id="answerBodyModel.id">
                    <div class="answerbody-btn-inner">
                        <QuestionPin
                            :question-id="answerBodyModel.id"
                            :key="answerBodyModel.id"
                            :is-in-wishknowledge="answerBodyModel.isInWishknowledge" />
                    </div>
                </div>
                <QuestionAnswerBodyOptions
                    v-if="answerBodyModel"
                    :id="answerBodyModel.id"
                    :title="answerBodyModel.title"
                    :can-edit="answerBodyModel.isCreator || userStore.isAdmin" />
            </div>
        </div>

        <div class="row">

            <div
                id="MarkdownCol"
                v-if="answerBodyModel.solutionType != SolutionType.Flashcard && !!answerBodyModel.renderedQuestionTextExtended.length">
                <SharedRawHtml
                    class="RenderedMarkdown"
                    :html="answerBodyModel.renderedQuestionTextExtended"
                    id="ExtendedQuestionContainer" />
            </div>

            <div id="AnswerAndSolutionCol">
                <div id="AnswerAndSolution">
                    <div
                        class="row"
                        :class="{ 'hasFlashcard': answerBodyModel.solutionType === SolutionType.Flashcard }">
                        <div id="AnswerInputSection">
                            <template v-if="answerBodyModel.solutionType === SolutionType.Flashcard">
                                <Transition name="fade">
                                    <div class="answerFeedback answerFeedbackCorrect" v-show="answerIsCorrectPopUp">
                                        <font-awesome-icon icon="fa-solid fa-circle-check" />
                                        &nbsp;{{ t('answerbody.feedbackCorrectPopUp') }}
                                    </div>
                                </Transition>
                                <Transition name="fade">
                                    <div class="answerFeedback answerFeedbackWrong" v-show="answerIsWrongPopUp">
                                        <font-awesome-icon icon="fa-solid fa-circle-minus" />
                                        &nbsp;{{ t('answerbody.feedbackWrongPopUp') }}
                                    </div>
                                </Transition>
                            </template>

                            <QuestionAnswerBodyFlashcard :key="answerBodyModel.id + 'flashcard'" v-if="answerBodyModel.solutionType === SolutionType.Flashcard" ref="flashcard" :solution="answerBodyModel.solution"
                                :front-content="answerBodyModel.textHtml"
                                :marked-as-correct="markFlashcardAsCorrect" @flipped="amountOfTries++" />
                            <QuestionAnswerBodyMatchlist :key="answerBodyModel.id + 'matchlist'" v-else-if="answerBodyModel.solutionType === SolutionType.MatchList" ref="matchList" :solution="answerBodyModel.solution"
                                :show-answer="showAnswer"
                                @flipped="amountOfTries++" />
                            <QuestionAnswerBodyMultipleChoice :key="answerBodyModel.id + 'multiplechoice'" v-else-if="answerBodyModel.solutionType === SolutionType.MultipleChoice" :solution="answerBodyModel.solution"
                                :show-answer="showAnswer" ref="multipleChoice" />
                            <QuestionAnswerBodyText :key="answerBodyModel.id + 'text'" v-else-if="answerBodyModel.solutionType === SolutionType.Text" ref="text" :show-answer="showAnswer" />
                        </div>

                        <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <div id="Buttons">
                                    <template v-if="answerBodyModel.solutionType === SolutionType.Flashcard && !flashCardAnswered">
                                        <button
                                            class="btn btn-warning memo-button"
                                            @click="flip()"
                                            v-if="amountOfTries === 0">
                                            {{ t('answerbody.flashcardFlip') }}
                                        </button>
                                        <div id="buttons-answer" class="ButtonGroup flashCardAnswerButtons" v-else>
                                            <button
                                                id="btnRightAnswer"
                                                class="btn btn-warning memo-button"
                                                @click="answerFlashcard(true)">
                                                {{ t('answerbody.flashcardKnew') }}
                                            </button>
                                            <button
                                                id="btnWrongAnswer"
                                                class="btn btn-warning memo-button"
                                                @click="answerFlashcard(false)">
                                                {{ t('answerbody.flashcardDidNotKnow') }}
                                            </button>
                                            <button
                                                v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp"
                                                id="flashCard-dontCountAnswer"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="learningSessionStore.skipStep()">
                                                {{ t('answerbody.flashcardDontCount') }}
                                            </button>
                                        </div>
                                    </template>

                                    <template v-else-if="showAnswerButtons && answerBodyModel.solutionType != SolutionType.Flashcard">
                                        <div id="buttons-first-try" class="ButtonGroup">
                                            <button class="btn btn-primary memo-button" @click="answer()">
                                                {{ t('answerbody.answer') }}
                                            </button>
                                            <button
                                                v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp && !showAnswer"
                                                class="selectorShowSolution SecAction btn btn-link memo-button"
                                                @click="loadSolution(false)">
                                                <font-awesome-icon icon="fa-solid fa-lightbulb" />
                                                {{ t('answerbody.showSolution') }}
                                            </button>
                                        </div>
                                    </template>

                                    <div v-if="learningSessionStore.isLearningSession
                                        && !learningSessionStore.isInTestMode
                                        && (amountOfTries === 0 && !showAnswer && learningSessionStore.currentStep?.state != AnswerState.Skipped)">
                                        <button class="SecAction btn btn-link memo-button" @click="learningSessionStore.skipStep()">
                                            <font-awesome-icon icon="fa-solid fa-forward" />
                                            {{ t('answerbody.skipQuestion') }}
                                        </button>
                                    </div>

                                    <div id="buttons-next-question" class="ButtonGroup" v-if="(amountOfTries > 0 || showAnswer)
                                        && !learningSessionStore.currentStep?.isLastStep
                                        && !showAnswerButtons">
                                        <button v-if="!learningSessionStore.currentStep?.isLastStep" @click="learningSessionStore.loadNextQuestionInSession()" id="btnNext" class="btn btn-primary memo-button" rel="nofollow">
                                            {{ t('answerbody.nextQuestion') }}
                                        </button>

                                        <button v-if="answerBodyModel.solutionType === SolutionType.Text
                                            && !learningSessionStore.isInTestMode
                                            && learningSessionStore.answerHelp
                                            && answerIsWrong" href="#" id="aCountAsCorrect" class="SecAction btn btn-link show-tooltip memo-button" :title="t('answerBody.tooltip.markAsCorrect')" rel="nofollow" @click="markAsCorrect()">
                                            {{ t('answerbody.markAsKnown') }}
                                        </button>
                                    </div>
                                    <Transition name="fade">
                                        <div v-if="learningSessionStore.currentStep?.isLastStep
                                            && (amountOfTries > 0
                                                || learningSessionStore.currentStep?.state === AnswerState.Skipped
                                                || learningSessionStore.currentStep?.state === AnswerState.ShowedSolutionOnly)">
                                            <button @click="loadResult()" class="btn btn-primary memo-button" rel="nofollow">
                                                {{ t('answerbody.toResult') }}
                                            </button>
                                        </div>
                                    </Transition>

                                    <div v-if="answerBodyModel.solutionType != SolutionType.Flashcard" id="buttons-answer-again" class="ButtonGroup">
                                        <button v-if="!allMultipleChoiceCombinationTried
                                            && !learningSessionStore.isInTestMode
                                            && answerIsWrong
                                            && !showAnswer
                                            && !showAnswerButtons" id="btnCheckAgain" class="btn btn-warning memo-button" rel="nofollow" @click="answer()">
                                            {{ t('answerbody.answerAgain') }}
                                        </button>
                                        <button v-if="!learningSessionStore.isInTestMode
                                            && learningSessionStore.answerHelp
                                            && !showAnswer
                                            && !showAnswerButtons" class="selectorShowSolution SecAction btn btn-link memo-button" @click="loadSolution(true)">
                                            <font-awesome-icon icon="fa-solid fa-lightbulb" />
                                            {{ t('answerbody.showSolution') }}
                                        </button>
                                    </div>
                                    <div style="clear: both"></div>
                                </div>

                                <div id="AnswerFeedbackAndSolutionDetails">
                                    <div v-if="answerBodyModel.solutionType != SolutionType.Flashcard" id="AnswerFeedback">
                                        <div id="divAnsweredCorrect" v-if="answerIsCorrect">
                                            <b class="correct-answer-label">
                                                {{ t('answerbody.correctAnswerLabel') }}
                                            </b>
                                            <SharedRawHtml v-if="wellDoneMsg.length > 0" :html="wellDoneMsg" />
                                        </div>

                                        <div id="Solution" v-if="showAnswer">
                                            <div class="solution-label">
                                                {{ t('answerbody.rightAnswerHeading') }}
                                            </div>
                                            <SharedRawHtml class="Content body-m" v-if="solutionData" :html="solutionData.answerAsHTML" />
                                        </div>

                                        <div id="divWrongAnswer" v-if="answerIsWrong">
                                            <div id="spnWrongAnswer">
                                                <b class="wrong-answer-label">
                                                    {{ t('answerbody.wrongAnswerLabel') }}
                                                </b>
                                            </div>
                                            <SharedRawHtml v-if="wrongAnswerMsg.length > 0" :html="wrongAnswerMsg" />
                                            <br />

                                            <div id="divWrongAnswers" v-if="showWrongAnswers">
                                                <span class="WrongAnswersHeading">
                                                    {{ answersSoFar.length === 1
                                                        ? t('answerbody.yourAnswersHeadingSingular')
                                                        : t('answerbody.yourAnswersHeadingPlural')
                                                    }}
                                                </span>
                                                <ul id="ulAnswerHistory">
                                                    <li v-for="answer in answersSoFar">
                                                        <SharedRawHtml :html="answer" />
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="SolutionDetails" v-if="solutionData
                                        && (solutionData.answerDescription?.trim().length > 0
                                            || solutionData.answerDescriptionHtml?.trim().length > 0)
                                        && showAnswer">
                                        <div id="Description">
                                            <div class="solution-label">
                                                {{ t('answerbody.answerAdditionsHeading') }}
                                            </div>

                                            <SharedRawHtml v-if="solutionData.answerDescriptionHtml.trim().length > 0" class="Content body-m" :html="solutionData.answerDescriptionHtml" />
                                            <SharedRawHtml v-else class="Content body-m" :html="solutionData.answerDescription" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <QuestionAnswerBodyAnswerQuestionDetails :id="answerBodyModel.id" />
    </div>
    <div v-else-if="learningSessionStore.showResult === true">
        <QuestionAnswerBodyLearningSessionResult @start-new-session="startNewSession" />
    </div>
</template>



<style lang="less">
@import '~~/assets/views/answerQuestion.less';

#ButtonsAndSolution {
    .btn-primary {
        margin-right: 22px;
    }
}

.ButtonGroup {
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
}

#AnswerBody {
    transition: all 1s ease-in-out;
    overflow: hidden;
}

.answerbody-header {
    display: flex;
    flex-wrap: nowrap;
    justify-content: space-between;

    .answerbody-text {
        margin-right: 8px;

        h3 {
            margin-top: 0;
            margin-bottom: 34px;
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

            .hover {
                display: none;
            }

            .no-hover {
                display: block;
            }

            &:hover {
                filter: brightness(0.95);

                .hover {
                    display: block;
                }

                .no-hover {
                    display: none;
                }
            }

            &:active {
                filter: brightness(0.85);
            }


        }
    }
}

.activity-points-icon {
    font-size: 14px;
}

#ActivityPointsDisplay {
    min-width: 100px;

    .activitypoints-display-detail {
        display: flex;
        justify-content: flex-end;
        align-items: center;
        flex-wrap: nowrap;

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

<style lang="less" scoped>
.fade-enter-active {
    transition: opacity 0.5s ease;
}

.fade-leave-active {
    transition: opacity 0;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>
