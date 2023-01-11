<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import { useUserStore } from '~~/components/user/userStore'
import { getHighlightedCode } from '~~/components/shared/utils'
import { useLearningSessionStore } from './learningSessionStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'

const showFullQuestion = ref(false)
const backgroundColor = ref('')

interface Props {
    question: QuestionListItem,
    isLastItem: boolean,
    expandQuestion: boolean,
    sessionIndex: number
}

const props = defineProps<Props>()

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

    var data = await $fetch<any>('/apiVue/TopicLearningQuestionList/LoadQuestionBody/', {
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
                        <Image :url="props.question.ImageData" />
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
                            <span class="percentageLabel" :class="backgroundColor">{{
                                correctnessProbability
                            }}&nbsp</span>
                            <span class="chip" :class="backgroundColor">{{ correctnessProbabilityLabel }}</span>
                        </div>
                        <div class="answerCountFooter">
                            {{ answerCount }}&nbspmal&nbspbeantwortet&nbsp|&nbsp{{
                                correctAnswers
                            }}&nbsprichtig&nbsp/&nbsp{{ wrongAnswers }}&nbspfalsch
                        </div>
                    </div>
                    <div id="QuestionFooterIcons" class="questionFooterIcons">
                        <div>
                            <a class="commentIcon" @click.stop="showCommentModal()">
                                <i class="fa fa-comment"><span style="font-weight: 400">&nbsp{{
                                    commentCount
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


<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';


.singleQuestionRow {
    background: linear-gradient(to right, @memo-grey-light 0px, @memo-grey-light 8px, #ffffffff 9px, #ffffffff 100%);
    font-family: "Open Sans";
    font-size: 14px;
    border: 0.05px solid @memo-grey-light;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    transition: all .2s ease-out;
    margin-right: 20px;

    @media(max-width: @screen-xxs-max) {
        margin-right: 0;
    }

    &.open {
        height: unset;
        margin-top: 20px;
        margin-bottom: 20px;
        transition: all .2s ease-out;
        box-shadow: 0px 1px 6px 0px #C4C4C4;
    }

    &.solid {
        background: linear-gradient(to right, @memo-green 0px, @memo-green 8px, #ffffffff 8px, #ffffffff 100%);
    }

    &.shouldConsolidate {
        background: linear-gradient(to right, #FDD648 0px, #FDD648 8px, #ffffffff 8px, #ffffffff 100%);
    }

    &.shouldLearn {
        background: linear-gradient(to right, @memo-salmon 0px, @memo-salmon 8px, #ffffffff 8px, #ffffffff 100%);
    }

    &.inWishknowledge {
        background: linear-gradient(to right, #949494 0px, #949494 8px, #ffffffff 8px, #ffffffff 100%);
    }

    .knowledgeState {
        width: 8px;
        min-width: 8px;
        z-index: 10;
        position: relative;
    }

    .questionSectionFlex {
        width: 100%;

        .questionSection {
            display: flex;
            flex-wrap: nowrap;
            width: 100%;
        }

        .questionImg {
            min-width: 75px;
            height: 100%;
            padding: 8px 0 8px 32px;

            @media(max-width: @screen-xxs-max) {
                display: none;
            }
        }

        .questionContainer {
            width: 100%;

            .questionBodyTop {
                display: flex;
                max-width: 100%;

                .questionContainerTopSection {
                    flex: 1 1 100%;
                    min-width: 0;
                    padding-right: 0;

                    .questionHeader {
                        display: flex;
                        flex-wrap: nowrap;
                        justify-content: space-between;
                        min-width: 0;
                        min-height: 57px;

                        &:hover {
                            cursor: pointer;
                        }

                        .questionTitle {
                            padding: 8px;
                            min-width: 0;
                            color: @memo-grey-darker;
                            font-weight: 600;
                            align-self: center;
                            display: inline-flex;

                            .privateQuestionIcon {
                                padding-left: 8px;
                                padding-right: 8px;
                            }
                        }

                        @media (max-width: 640px) {
                            .col-xs-3 {
                                width: 33%;
                            }

                            .col-xs-9 {
                                width: 66%;
                            }
                        }

                        @media (max-width: @screen-sm-min) {
                            .col-xs-3 {
                                width: 50%;
                            }

                            .col-xs-9 {
                                width: 50%;
                            }
                        }
                    }

                    .questionHeaderIcons {
                        flex: 0 0 auto;
                        font-size: 18px;
                        min-width: 77px;
                        display: flex;
                        flex-wrap: nowrap;
                        color: @memo-grey-light;
                        flex-direction: row-reverse;
                        padding: 0;

                        .iAdded,
                        .iAddedNot {
                            padding: 0;
                        }

                        .fa-heart,
                        .fa-spinner,
                        .fa-heart-o {
                            font-size: 18px;
                            padding-top: 18px;
                        }

                        .fa-spinner,
                        .fa-play {
                            padding-right: 10px;
                        }

                        .iconContainer {
                            padding: 8px 8px 0px 8px;
                            min-width: 40px;
                            // height: 57px;
                            width: 40px;
                            max-width: 40px;
                            text-align: center;

                            .fa-play {
                                margin-top: 10px;
                            }

                            .rotateIcon {
                                transition: all .2s ease-out;
                                line-height: 41px;

                                &.open {
                                    transform: rotate(180deg);
                                }
                            }
                        }

                        @media (max-width: 767px) {
                            .iconContainer {
                                padding-right: 2px;
                                padding-left: 2px;
                            }
                        }
                    }

                    @media(max-width: @screen-xxs-max) {
                        padding-left: 20px;
                    }
                }

                .extendedQuestionContainer {
                    padding: 0 0 8px 0;

                    @media (max-width: 550px) {
                        padding: 8px 8px 8px 4px;
                    }

                    .extendedAnswer {
                        padding-top: 16px;
                    }

                    .notes {
                        padding-top: 16px;
                        padding-bottom: 8px;
                        font-size: 12px;

                        a {
                            cursor: pointer;
                        }

                        .relatedCategories {
                            padding-bottom: 16px;
                        }

                        .author {}

                        .sources {
                            overflow-wrap: break-word;
                        }
                    }
                }
            }

            .questionBodyBottom {
                display: flex;
                justify-content: space-between;
                padding-right: 10px;
                padding-left: 72px;
                align-items: center;

                .questionDetails {
                    font-size: 12px;
                    padding-left: 24px;

                    #StatsHeader {
                        display: none;
                    }
                }

                .questionStats {
                    display: flex;
                    font-size: 11px;
                    padding-left: 12px;
                    padding-right: 12px;
                    width: 100%;

                    .answerCountFooter {
                        width: 260px;
                        padding-bottom: 16px;


                        @media(max-width: @screen-xxs-max) {
                            padding-right: 0px;
                        }
                    }

                    .probabilitySection {
                        padding-right: 10px;
                        display: flex;
                        justify-content: center;
                        padding-bottom: 16px;

                        span {
                            &.percentageLabel {
                                font-weight: bold;
                                color: @memo-grey-light;

                                &.solid {
                                    color: @memo-green;
                                }

                                &.shouldConsolidate {
                                    color: #FDD648;
                                }

                                &.shouldLearn {
                                    color: @memo-salmon;
                                }

                                &.inWishknowledge {
                                    color: #949494;
                                }
                            }

                            &.chip {
                                padding: 1px 10px;
                                border-radius: 20px;
                                background: @memo-grey-light;
                                color: @memo-grey-darker;
                                white-space: nowrap;

                                &.solid {
                                    background: @memo-green;
                                }

                                &.shouldConsolidate {
                                    background: #FDD648;
                                }

                                &.shouldLearn {
                                    background: @memo-salmon;
                                }

                                &.inWishknowledge {
                                    background: #949494;
                                    color: white;
                                }
                            }
                        }

                        &.open {
                            height: unset;
                            margin-top: 20px;
                            margin-bottom: 20px;
                            transition: all .2s ease-out;
                            box-shadow: 0px 1px 6px 0px #C4C4C4;
                        }
                    }

                    @media(max-width: @screen-sm-min) {
                        flex-wrap: wrap;
                    }
                }

                .questionFooterIcons {
                    color: @memo-grey-dark;
                    font-size: 11px;
                    margin: 0;
                    display: flex;
                    align-items: center;
                    margin-top: -21px;


                    a.commentIcon {
                        text-decoration: none;
                        color: @memo-grey-dark;
                        font-size: 14px;
                        cursor: pointer;
                    }

                    .dropdown-menu {
                        text-align: left;
                    }

                    span {
                        line-height: 36px;
                        font-family: Open Sans;
                    }

                    .ellipsis {
                        padding-left: 16px;
                    }
                }

                @media (max-width: @screen-xxs-max) {
                    flex-wrap: wrap;
                    padding-left: 10px;
                    justify-content: flex-end;
                }
            }
        }
    }

    .questionFooter {
        height: 52px;
        border-top: 0.5px solid @memo-grey-light;
        display: flex;
        flex-direction: row-reverse;
        font-size: 20px;

        .questionFooterLabel {
            padding: 16px;
            line-height: 20px;
        }
    }
}
</style>