<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import { useUserStore } from '~~/components/user/userStore'
import { getHighlightedCode } from '~~/components/shared/utils'
import { useLearningSessionStore } from './learningSessionStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'

const showFullQuestion = ref(false)
const backgroundColor = ref('')

const props = defineProps({
    question: { type: Object as () => QuestionListItem, required: true },
    isLastItem: { type: Boolean, required: true },
    expandQuestion: { type: Boolean, required: true },
    sessionIndex: { type: Number, required: true }
})

const questionTitleId = ref("#QuestionTitle-" + props.question.Id)

const questionTitleHtml = ref('')
onBeforeMount(() => {
    questionTitleHtml.value = "<div class='body-m bold margin-bottom-0'>" + props.question.Title + "</div>"
})
const allDataLoaded = ref(false)

const answer = ref('')
const extendedAnswer = ref('')

const topics = ref({})
const userStore = useUserStore()

function abbreviateNumber(val: number): string {
    if (val < 1000000) {
        return val.toLocaleString("de-DE");
    }
    else if (val >= 1000000 && val < 1000000000) {
        7
        var newVal
        newVal = val / 1000000;
        return newVal.toFixed(2).toLocaleString() + " Mio."
    } else
        return ''
}

const references = reactive({ value: [] })
const author = ref('')
const authorImage = ref('')

const extendedQuestion = ref('')
const commentCount = ref(0)
const isCreator = ref(false)

const editUrl = ref('')
const historyUrl = ref('')
const authorUrl = ref('')

const answerCount = ref('')
const correctAnswers = ref('')
const wrongAnswers = ref('')

const canBeEdited = ref(false)

async function loadQuestionBody() {

    var data = await $fetch<any>('api/QuestionList/LoadQuestionBody/', {
        body: { questionId: props.question.Id },
        method: 'post',
        credentials: 'include',
    })

    if (data != null) {
        if (data.answer == null || data.answer.length <= 0) {
            if (data.extendedAnswer && data.extendedAnswer.length > 0)
                answer.value = "<div>" + data.extendedAnswer + "</div>"
            else
                answer.value = "<div> Fehler: Keine Antwort! </div>"
        } else {
            answer.value = "<div>" + data.answer + "</div>"
            if (data.extendedAnswer != null)
                extendedAnswer.value = "<div>" + data.extendedAnswer + "</div>"
        }

        if (data.categories)
            topics.value = data.categories

        references.value = data.references
        author.value = data.author
        authorImage.value = data.authorImage
        extendedQuestion.value = "<div>" + data.extendedQuestion + "</div>"
        commentCount.value = data.commentCount
        isCreator.value = data.isCreator && userStore.isLoggedIn
        editUrl.value = data.editUrl
        historyUrl.value = data.historyUrl
        authorUrl.value = data.authorUrl
        allDataLoaded.value = true
        answerCount.value = abbreviateNumber(data.answerCount)
        correctAnswers.value = abbreviateNumber(data.correctAnswerCount)
        wrongAnswers.value = abbreviateNumber(data.wrongAnswerCount)
        canBeEdited.value = data.canBeEdited
    }

}

function expandQuestion() {
    showFullQuestion.value = !showFullQuestion.value
    if (allDataLoaded.value == false) {
        loadQuestionBody()
    }
}

watch(() => props.expandQuestion, (val) => {
    if (val != showFullQuestion.value)
        expandQuestion()
})

function highlightCode(elementId: string) {
    var el = document.getElementById(elementId)
    if (el != null)
        el.querySelectorAll('code').forEach(block => {
            if (block.textContent != null)
                block.innerHTML = getHighlightedCode(block.textContent)
        })
}
const learningSessionStore = useLearningSessionStore()

function loadSpecificQuestion() {
    learningSessionStore.loadQuestion(-5, props.sessionIndex)
}
const activeQuestionId = ref(0)
const extendedQuestionId = ref('#eqId-' + props.question.Id)
const answerId = ref('#aId' + props.question.Id)
const extendedAnswerId = ref('#eaId' + props.question.Id)
const correctnessProbability = ref('')
const correctnessProbabilityLabel = ref('')

function showCommentModal() {
    // needs comment modal
}

const editQuestionStore = useEditQuestionStore()
function editQuestion() {
    editQuestionStore.editQuestion(props.question.Id, props.question.SessionIndex)
}

function deleteQuestion() {

}
</script>

<template>
    <div class="singleQuestionRow" :class="[{ open: showFullQuestion }, backgroundColor]">
        <div class="questionSectionFlex">
            <div class="questionContainer">
                <div class="questionBodyTop">
                    <div class="questionImg col-xs-1" @click="expandQuestion()">
                        <Image :src="props.question.ImageData" />
                    </div>
                    <div class="questionContainerTopSection col-xs-11">
                        <div class="questionHeader row">
                            <div class="questionTitle col-xs-9" ref="questionTitle" :id="questionTitleId"
                                @click="expandQuestion()">
                                <component :is="questionTitleHtml && { template: questionTitleHtml }"
                                    @hook:mounted="highlightCode(questionTitleId)"></component>
                                <div v-if="props.question.Visibility == 1" class="privateQuestionIcon">
                                    <p>
                                        <i class="fas fa-lock"></i>
                                    </p>
                                </div>
                            </div>
                            <div class="questionHeaderIcons col-xs-3" @click.self="expandQuestion()">
                                <div class="iconContainer float-right" @click="expandQuestion()">
                                    <i class="fas fa-angle-down rotateIcon" :class="{ open: showFullQuestion }"></i>
                                </div>
                                <div>
                                    <pin-wuwi-component :is-in-wishknowledge="props.question.IsInWishknowledge"
                                        :question-id="props.question.Id" />
                                </div>
                                <div class="go-to-question iconContainer">
                                    <span class="fas fa-play"
                                        :class="{ 'activeQ': activeQuestionId == props.question.Id }"
                                        :data-question-id="props.question.Id" @click="loadSpecificQuestion()">
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="extendedQuestionContainer" v-show="showFullQuestion">
                            <div class="questionBody">
                                <div class="RenderedMarkdown extendedQuestion" :id="extendedQuestionId">
                                    <component :is="extendedQuestion && { template: extendedQuestion }"
                                        @hook:mounted="highlightCode(extendedQuestionId)"></component>
                                </div>
                                <div class="answer body-m" :id="answerId">
                                    Richtige Antwort: <component :is="answer && { template: answer }"
                                        @hook:mounted="highlightCode(answerId)"></component>
                                </div>
                                <div class="extendedAnswer body-m" v-if="extendedAnswer.length > 11"
                                    :id="extendedAnswerId">
                                    <strong>Ergänzungen zur Antwort:</strong><br />
                                    <component :is="extendedAnswer && { template: extendedAnswer }"
                                        @hook:mounted="highlightCode(extendedAnswerId)"></component>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="questionBodyBottom" v-show="showFullQuestion">
                    <div class="questionStats questionStatsInQuestionList">
                        <div class="probabilitySection">
                            <span class="percentageLabel" :class="backgroundColor">{{ correctnessProbability
                            }}&nbsp</span>
                            <span class="chip" :class="backgroundColor">{{ correctnessProbabilityLabel }}</span>
                        </div>
                        <div class="answerCountFooter">
                            {{ answerCount }}&nbspmal&nbspbeantwortet&nbsp|&nbsp{{ correctAnswers
                            }}&nbsprichtig&nbsp/&nbsp{{ wrongAnswers }}&nbspfalsch
                        </div>
                    </div>
                    <div id="QuestionFooterIcons" class="questionFooterIcons">
                        <div>
                            <a class="commentIcon" @click.stop="showCommentModal()">
                                <i class="fa fa-comment"><span style="font-weight: 400">&nbsp{{ commentCount
                                }}</span></i>
                            </a>
                        </div>
                        <div class="Button dropdown">
                            <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button"
                                data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                <i class="fa fa-ellipsis-v"></i>
                            </a>
                            <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                                <li v-if="userStore.isAdmin || isCreator">
                                    <a @click="editQuestion()">
                                        <div class="dropdown-icon"><i class="fa fa-pen"></i></div><span>Frage
                                            bearbeiten</span>
                                    </a>
                                </li>
                                <li v-if="userStore.isAdmin"><a :href="props.question.LinkToQuestion">
                                        <div class="dropdown-icon"><i class="fas fa-file"></i></div><span>Frageseite
                                            anzeigen</span>
                                    </a></li>
                                <li><a :href="props.question.LinkToQuestionVersions" data-allowed="logged-in">
                                        <div class="dropdown-icon"><i class="fa fa-code-fork"></i></div>
                                        <span>Bearbeitungshistorie der Frage</span>
                                    </a></li>
                                <li><a @click="showCommentModal()">
                                        <div class="dropdown-icon"><i class="fas fa-comment"></i></div><span>Frage
                                            kommentieren</span>
                                    </a></li>
                                <li>
                                    <a @click="deleteQuestion()">
                                        <div class="dropdown-icon"><i class="fas fa-trash"></i></div><span>Frage
                                            löschen</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>