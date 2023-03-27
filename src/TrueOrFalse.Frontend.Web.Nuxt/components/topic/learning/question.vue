<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import { useUserStore } from '~~/components/user/userStore'
import { getHighlightedCode } from '~~/components/shared/utils'
import { useLearningSessionStore } from './learningSessionStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { PinState } from '~~/components/question/pin/pinStore'
import { useCommentsStore } from '~~/components/comment/commentStore'

const commentsStore = useCommentsStore()

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

const questionTitleHtml = ref<any>()

onBeforeMount(() => {
    questionTitleHtml.value = "<div class='body-m bold margin-bottom-0'>" + props.question.Title + "</div>"
    isInWishknowledge.value = props.question.IsInWishknowledge
    hasPersonalAnswer.value = props.question.HasPersonalAnswer
    setKnowledgebarData(props.question.CorrectnessProbability)
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

    var data = await $fetch<any>('/apiVue/TopicLearningQuestion/LoadQuestionBody/', {
        body: { questionId: props.question.Id },
        method: 'POST',
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
    learningSessionStore.changeActiveQuestion(props.sessionIndex)
}
const extendedQuestionId = ref('#eqId-' + props.question.Id)
const answerId = ref('#aId' + props.question.Id)
const extendedAnswerId = ref('#eaId' + props.question.Id)
const correctnessProbability = ref('')
const correctnessProbabilityLabel = ref('')

function showCommentModal(hide: any = undefined) {
    if (hide)
        hide()
    commentsStore.openModal(props.question.Id)
}

const editQuestionStore = useEditQuestionStore()
function editQuestion(hide: any) {
    hide()
    editQuestionStore.editQuestion(props.question.Id, props.question.SessionIndex)
}

function deleteQuestion(hide: any) {
    hide()
}

function publishQuestion(hide: any | null = null) {
    if (hide)
        hide()
}

const isInWishknowledge = ref(false)
const hasPersonalAnswer = ref(false)


watch(() => props.sessionIndex, (val) => {
    if (props.isLastItem)
        learningSessionStore.lastIndexInQuestionList = val
})

function setKnowledgebarData(val: number) {
    if (isInWishknowledge.value) {
        if (hasPersonalAnswer.value) {
            if (val >= 80) {
                backgroundColor.value = "solid"
                correctnessProbabilityLabel.value = "Sicheres Wissen"
            } else if (val < 80 && val >= 50) {
                backgroundColor.value = "shouldConsolidate"
                correctnessProbabilityLabel.value = "Zu festigen"
            } else if (val < 50 && val >= 0) {
                backgroundColor.value = "shouldLearn"
                correctnessProbabilityLabel.value = "Zu lernen"
            }
        } else {
            backgroundColor.value = "inWishknowledge"
            correctnessProbabilityLabel.value = "Nicht gelernt"
        }
    } else {
        backgroundColor.value = ""
        correctnessProbabilityLabel.value = "Nicht im Wunschwissen"
    }

}

function setWuwiState(state: PinState) {
    if (state == PinState.Added)
        isInWishknowledge.value = true
    else if (state == PinState.NotAdded)
        isInWishknowledge.value = false
}

watch(isInWishknowledge, () => {
    setKnowledgebarData(props.question.CorrectnessProbability)
})

</script>

<template>
    <div class="singleQuestionRow" :class="[{ open: showFullQuestion }, backgroundColor]">
        <div class="questionSectionFlex">
            <div class="questionContainer">
                <div class="questionBodyTop row">
                    <div class="questionImg col-xs-1" @click="expandQuestion()">
                        <Image :url="props.question.ImageData" />
                    </div>
                    <div class="questionContainerTopSection col-xs-11">
                        <div class="questionHeader">
                            <div class="questionTitle col-xs-9" ref="questionTitle" :id="questionTitleId"
                                @click="expandQuestion()">
                                <div v-html="questionTitleHtml" v-if="questionTitleHtml != null">

                                </div>
                                <div v-if="props.question.Visibility == 1" class="privateQuestionIcon question-lock"
                                    @click.stop="publishQuestion()">
                                    <font-awesome-icon :icon="['fa-solid', 'lock']" />
                                    <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                                </div>
                            </div>
                            <div class="questionHeaderIcons col-xs-3" @click.self="expandQuestion()">
                                <div class="iconContainer float-right" @click="expandQuestion()">
                                    <font-awesome-icon icon="fa-solid fa-angle-down" class="rotateIcon"
                                        :class="{ open: showFullQuestion }" />
                                </div>
                                <QuestionPin :is-in-wishknowledge="isInWishknowledge" :question-id="props.question.Id"
                                    class="iconContainer" @set-wuwi-state="setWuwiState" />
                                <div class="go-to-question iconContainer">
                                    <font-awesome-icon icon="fa-solid fa-play"
                                        :class="{ 'activeQ': props.question.Id == learningSessionStore.currentStep?.id }"
                                        @click="loadSpecificQuestion()" />
                                </div>
                            </div>
                        </div>
                        <div class="extendedQuestionContainer" v-show="showFullQuestion">
                            <div class="questionBody">
                                <div class="RenderedMarkdown extendedQuestion" :id="extendedQuestionId">
                                    <div v-html="extendedQuestion" @hook:mounted="highlightCode(extendedQuestionId)">
                                    </div>
                                </div>
                                <div class="answer body-m" :id="answerId">
                                    Richtige Antwort: <div v-html="answer" @hook:mounted="highlightCode(answerId)">
                                    </div>
                                </div>
                                <div class="extendedAnswer body-m" v-if="extendedAnswer.length > 11" :id="extendedAnswerId">
                                    <strong>Ergänzungen zur Antwort:</strong><br />
                                    <div :v-html="extendedAnswer" @hook:mounted="highlightCode(extendedAnswerId)"></div>
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
                            }}</span>
                            <span class="chip" :class="backgroundColor">{{ correctnessProbabilityLabel }}</span>
                        </div>
                        <div class="answerCountFooter">
                            {{ answerCount }} mal beantwortet | {{
                                correctAnswers
                            }} richtig / {{ wrongAnswers }} falsch
                        </div>
                    </div>
                    <div id="QuestionFooterIcons" class="questionFooterIcons">
                        <div class="commentIcon" @click.stop="showCommentModal()">
                            <font-awesome-icon icon="fa-solid fa-comment" />
                            <span> {{
                                commentCount
                            }}
                            </span>
                        </div>
                        <div class="Button dropdown">

                            <V-Dropdown :distance="0">
                                <font-awesome-icon icon="fa-solid fa-ellipsis-vertical"
                                    class="btn btn-link btn-sm ButtonEllipsis" />
                                <template #popper="{ hide }">

                                    <div v-if="userStore.isAdmin || isCreator" @click="editQuestion(hide)"
                                        class="dropdown-row">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-pen" />
                                        </div>
                                        <div class="dropdown-label">Frage bearbeiten</div>

                                    </div>
                                    <NuxtLink :to="props.question.LinkToQuestion">
                                        <div class="dropdown-row">
                                            <div class="dropdown-icon">
                                                <font-awesome-icon icon="fa-solid fa-file" />
                                            </div>
                                            <div class="dropdown-label">
                                                Frageseite anzeigen
                                            </div>
                                        </div>
                                    </NuxtLink>

                                    <NuxtLink :to="props.question.LinkToQuestionVersions">
                                        <div class="dropdown-row">
                                            <div class="dropdown-icon">
                                                <font-awesome-icon icon="fa-solid fa-code-fork" />
                                            </div>
                                            <div class="dropdown-label">
                                                Bearbeitungshistorie der Frage
                                            </div>
                                        </div>
                                    </NuxtLink>

                                    <div class="dropdown-row" @click="showCommentModal(hide)">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon :icon="['fas', 'comment']" />
                                        </div>
                                        <div class="dropdown-label">
                                            Frage kommentieren
                                        </div>
                                    </div>

                                    <div class="dropdown-row" @click="deleteQuestion(hide)">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-trash" />
                                        </div>
                                        <div class="dropdown-label">
                                            Frage löschen
                                        </div>
                                    </div>
                                </template>
                            </V-Dropdown>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.activeQ {
    color: @memo-grey-darker;
}

.singleQuestionRow {
    background: linear-gradient(to right, @memo-grey-light 0px, @memo-grey-light 8px, #ffffffff 9px, #ffffffff 100%);
    font-family: "Open Sans";
    font-size: 14px;
    border: 0.05px solid @memo-grey-light;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    transition: all .2s ease-out;

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
        background: linear-gradient(to right, @memo-yellow 0px, @memo-yellow 8px, #ffffffff 8px, #ffffffff 100%);
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

                        .iconContainer {
                            min-width: 20px;
                            width: 20px;
                            max-width: 20px;
                            text-align: center;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            height: 57px;
                            margin-right: 10px;

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


                    .commentIcon {
                        text-decoration: none;
                        color: @memo-grey-dark;
                        font-size: 14px;
                        cursor: pointer;
                        display: flex;
                        align-items: center;

                        span {
                            font-weight: 400;
                            padding-left: 8px;
                        }
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

.question-lock {
    cursor: pointer;
    display: inline-flex;
    align-items: center;
    margin-right: 4px;
    margin-left: 4px;
    background: white;
    width: 24px;
    height: 24px;
    justify-content: center;
    border-radius: 15px;

    .fa-unlock {
        display: none !important;
    }

    .fa-lock {
        display: unset !important;
    }

    &:hover {

        .fa-lock {
            display: none !important;
            color: @memo-blue;
        }

        .fa-unlock {
            display: unset !important;
            color: @memo-blue;
        }

        filter: brightness(0.95)
    }

    &:active {
        filter: brightness(0.85)
    }

}

.ButtonEllipsis {
    margin-left: 0px !important;
    font-size: 18px;
    color: @memo-grey-dark;
    transition: all .1s ease-in-out;

    &:hover {
        color: @memo-blue;
        transition: all .1s ease-in-out;
    }
}
</style>