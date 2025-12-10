<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import { useUserStore } from '~~/components/user/userStore'
import { getHighlightedCode } from '~/utils/utils'
import { useLearningSessionStore } from './learningSessionStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { PinState } from '~~/components/question/pin/pinStore'
import { useCommentsStore } from '~~/components/comment/commentsStore'
import { KnowledgeStatus } from '~~/components/question/knowledgeStatusEnum'
import { useDeleteQuestionStore } from '~~/components/question/edit/delete/deleteQuestionStore'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { useAlertStore, AlertType } from '~/components/alert/alertStore'
import { usePublishQuestionStore } from '~/components/question/edit/publish/publishQuestionStore'

const alertStore = useAlertStore()
const commentsStore = useCommentsStore()
const deleteQuestionStore = useDeleteQuestionStore()
const publishQuestionStore = usePublishQuestionStore()
const { t } = useI18n()

const showFullQuestion = ref(false)
const backgroundColor = ref('')

interface Props {
    question: QuestionListItem,
    isLastItem: boolean,
    expandQuestion: boolean,
    sessionIndex: number
}

const props = defineProps<Props>()

const questionTitleId = ref("#QuestionTitle-" + props.question.id)

const questionTitleHtml = ref<any>()

const allDataLoaded = ref(false)

const answer = ref('')
const extendedAnswer = ref('')

const userStore = useUserStore()

function abbreviateNumber(val: number): string {
    if (val < 1000000) {
        return val.toLocaleString("de-DE")
    }
    else if (val >= 1000000 && val < 1000000000) {
        7
        var newVal
        newVal = val / 1000000
        return newVal.toFixed(2).toLocaleString() + " Mio."
    } else
        return ''
}

const authorName = ref('')
const authorImageUrl = ref('')

const extendedQuestion = ref('')
const commentCount = ref(0)
const isCreator = ref(false)

const answerCount = ref('')
const correctAnswers = ref('')
const wrongAnswers = ref('')

const canBeEdited = ref(false)

interface QuestionDataResult {
    answer: string
    extendedAnswer?: string
    authorName: string
    authorId: number
    authorImageUrl: string
    extendedQuestion: string
    commentCount: number
    isCreator: boolean
    answerCount: number
    correctAnswerCount: number
    wrongAnswerCount: number
    canBeEdited: boolean
    title: string
    visibility: Visibility
}
const { $logger } = useNuxtApp()
async function loadQuestionData() {

    const result = await $api<FetchResult<QuestionDataResult>>(`/apiVue/PageLearningQuestion/LoadQuestionData/${props.question.id}`, {
        method: 'POST',
        credentials: 'include',

        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result?.success) {
        if (result.data.answer == null || result.data.answer.length <= 0) {
            if (result.data.extendedAnswer && result.data.extendedAnswer.length > 0)
                answer.value = `<div>${result.data.extendedAnswer}</div>`
            else
                answer.value = `<div>${t('page.questionsSection.list.question.error.noAnswer')}</div>`
        } else {
            answer.value = `<div>${result.data.answer}</div>`
            if (result.data.extendedAnswer != null)
                extendedAnswer.value = `<div>${result.data.extendedAnswer}</div>`
        }

        authorName.value = result.data.authorName
        authorImageUrl.value = result.data.authorImageUrl
        extendedQuestion.value = `<div>${result.data.extendedQuestion}</div>`
        commentCount.value = result.data.commentCount
        isCreator.value = result.data.isCreator && userStore.isLoggedIn
        allDataLoaded.value = true
        answerCount.value = abbreviateNumber(result.data.answerCount)
        correctAnswers.value = abbreviateNumber(result.data.correctAnswerCount)
        wrongAnswers.value = abbreviateNumber(result.data.wrongAnswerCount)
        canBeEdited.value = result.data.canBeEdited
        setTitle(result.data.title)
        showLock.value = result.data.visibility != Visibility.Public
    } else if (result?.success === false) {
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) ?? t('error.default') })
    }

}

function expandQuestionContainer() {
    showFullQuestion.value = !showFullQuestion.value
    if (allDataLoaded.value === false) {
        loadQuestionData()
    }
}

watch(() => props.expandQuestion, (val) => {
    if (val != showFullQuestion.value)
        expandQuestionContainer()
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
    const el = document.getElementById('AnswerBody')
    if (el) {
        const headerOffset = 120
        const elementPosition = el.getBoundingClientRect().top
        const offsetPosition = elementPosition + window.scrollY - headerOffset

        window.scrollTo({
            top: offsetPosition,
            behavior: "smooth"
        })
    }

}
const extendedQuestionId = ref('#eqId-' + props.question.id)
const answerId = ref('#aId' + props.question.id)
const extendedAnswerId = ref('#eaId' + props.question.id)
const correctnessProbability = ref('')
const correctnessProbabilityLabel = ref('Nicht gelernt')

function showCommentModal() {
    commentsStore.openModal(props.question.id)
}

const editQuestionStore = useEditQuestionStore()
function editQuestion() {
    editQuestionStore.editQuestion(props.question.id, props.question.sessionIndex)
}

const isInWishKnowledge = ref(false)
const hasPersonalAnswer = ref(false)

watch(() => props.sessionIndex, (val) => {
    if (props.isLastItem && val != undefined)
        learningSessionStore.lastIndexInQuestionList = val
})

const currentKnowledgeStatus = ref<KnowledgeStatus>(KnowledgeStatus.NotLearned)
function setKnowledgebarData() {

    switch (currentKnowledgeStatus.value) {
        case KnowledgeStatus.Solid:
            backgroundColor.value = "solid"
            correctnessProbabilityLabel.value = t('page.questionsSection.list.question.knowledgeStatus.solid')
            break
        case KnowledgeStatus.NeedsConsolidation:
            backgroundColor.value = "needsConsolidation"
            correctnessProbabilityLabel.value = t('page.questionsSection.list.question.knowledgeStatus.needsConsolidation')
            break
        case KnowledgeStatus.NeedsLearning:
            backgroundColor.value = "needsLearning"
            correctnessProbabilityLabel.value = t('page.questionsSection.list.question.knowledgeStatus.needsLearning')
            break
        default:
            backgroundColor.value = "notLearned"
            correctnessProbabilityLabel.value = t('page.questionsSection.list.question.knowledgeStatus.notLearned')
            break
    }
}

function setWishKnowledgeState(state: PinState) {
    if (state === PinState.Added)
        isInWishKnowledge.value = true
    else if (state === PinState.NotAdded)
        isInWishKnowledge.value = false
}
watch(currentKnowledgeStatus, () => {
    setKnowledgebarData()
})
const showLock = ref(false)
onBeforeMount(() => {
    showLock.value = props.question.visibility != Visibility.Public

    isInWishKnowledge.value = props.question.isInWishKnowledge
    hasPersonalAnswer.value = props.question.hasPersonalAnswer
    currentKnowledgeStatus.value = props.question.knowledgeStatus
})

onMounted(() => setTitle(props.question.title))

function setTitle(title: string) {
    questionTitleHtml.value = `<div class='body-m bold margin-bottom-0'>${title}</div>`
}

async function getNewKnowledgeStatus() {
    currentKnowledgeStatus.value = await $api<KnowledgeStatus>(`/apiVue/PageLearningQuestion/GetKnowledgeStatus/${props.question.id}`, {
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
}
learningSessionStore.$onAction(({ name, after }) => {
    if (name === 'knowledgeStatusChanged')
        after((result) => {
            if (result === props.question.id) {
                getNewKnowledgeStatus()
            }
        })
})

editQuestionStore.$onAction(({ name, after }) => {
    if (name === 'questionEdited')
        after((result) => {
            if (result === props.question.id) {
                loadQuestionData()
            }
        })
})

watch(showFullQuestion, async (val) => {
    if (val) {
        await nextTick()
        highlightCode(extendedQuestionId.value)
        highlightCode(answerId.value)
        if (extendedAnswer.value.length > 0)
            highlightCode(extendedAnswerId.value)
    }
})

function hasContent(str: string) {
    return str.replace(/<div>/, '').replace(/<\/div>(?![\s\S]*<\/div>[\s\S]*$)/, '').length > 0
}
const ariaId = useId()

publishQuestionStore.$onAction(({ name, after }) => {
    if (name === 'confirmPublish') {
        after((id) => {
            if (id && id === props.question.id)
                showLock.value = false
        })
    }
})
const { $urlHelper } = useNuxtApp()

</script>

<template>
    <div class="singleQuestionRow" :class="[{ open: showFullQuestion }, backgroundColor]">
        <div class="questionSectionFlex">
            <div class="questionContainer">
                <div class="questionBodyTop row">
                    <div class="questionImg col-xs-1" @click="expandQuestionContainer()">
                        <Image :src="props.question.imageData" />
                    </div>
                    <div class="questionContainerTopSection col-xs-11">
                        <div class="questionHeader">
                            <div class="questionTitleContainer col-xs-9" ref="questionTitle" :id="questionTitleId"
                                @click="expandQuestionContainer()">
                                <div v-html="questionTitleHtml" v-if="questionTitleHtml != null" class="questionTitle">

                                </div>
                                <div v-if="showLock" class="privateQuestionIcon question-lock" @click.stop="publishQuestionStore.openModal(props.question.id)">
                                    <font-awesome-icon :icon="['fa-solid', 'lock']" />
                                    <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                                </div>
                            </div>
                            <div class="questionHeaderIcons col-xs-3" @click.self="expandQuestionContainer()">
                                <div class="iconContainer float-right" @click="expandQuestionContainer()">
                                    <font-awesome-icon icon="fa-solid fa-angle-down" class="rotateIcon"
                                        :class="{ open: showFullQuestion }" />
                                </div>
                                <QuestionPin :is-in-wish-knowledge="isInWishKnowledge" :question-id="props.question.id"
                                    class="iconContainer" @set-wish-knowledge-state="setWishKnowledgeState" />
                                <div class="go-to-question iconContainer">
                                    <font-awesome-icon icon="fa-solid fa-play"
                                        :class="{ 'activeQ': props.question.id === learningSessionStore.currentStep?.id }"
                                        @click="loadSpecificQuestion()" />
                                </div>
                            </div>
                        </div>
                        <div class="extendedQuestionContainer" v-show="showFullQuestion">
                            <div class="questionBody">
                                <div class="RenderedMarkdown extendedQuestion" :id="extendedQuestionId">
                                    <div v-html="extendedQuestion">
                                    </div>
                                </div>
                                <div class="answer body-m" :id="answerId">
                                    {{ t('page.questionsSection.list.question.answer.correct') }}
                                    <div v-html="answer"></div>
                                </div>
                                <div class="extendedAnswer body-m" v-if="hasContent(extendedAnswer)"
                                    :id="extendedAnswerId">
                                    <strong>{{ t('page.questionsSection.list.question.answer.additionalInfo') }}</strong><br />
                                    <div v-html="extendedAnswer"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="questionBodyBottom" v-show="showFullQuestion">
                    <div class="questionStats questionStatsInQuestionList">
                        <div class="probabilitySection">
                            <span class="percentageLabel" :class="backgroundColor">{{ correctnessProbability }}</span>
                            <span class="chip" :class="backgroundColor">{{ correctnessProbabilityLabel }}</span>
                        </div>
                        <div class="answerDetails">
                            <div>{{ answerCount }} {{ t('page.questionsSection.list.question.stats.answered') }}</div>
                            <div class="spacercontainer">
                                <div class="spacer"></div>
                            </div>
                            <div>
                                {{ t('page.questionsSection.list.question.stats.correct', correctAnswers) }}
                                {{ t('page.questionsSection.list.question.stats.wrong', wrongAnswers) }}
                            </div>
                        </div>
                    </div>
                    <div id="QuestionFooterIcons" class="questionFooterIcons">
                        <div class="commentIcon" @click.stop="showCommentModal()">
                            <font-awesome-icon icon="fa-solid fa-comment" />
                            <span> {{ commentCount }} </span>
                        </div>
                        <div class="Button dropdown">

                            <VDropdown :aria-id="ariaId" :distance="0">
                                <font-awesome-icon icon="fa-solid fa-ellipsis-vertical"
                                    class="btn btn-link btn-sm ButtonEllipsis" />
                                <template #popper="{ hide }">

                                    <div v-if="userStore.isAdmin || isCreator || canBeEdited"
                                        @click="editQuestion(); hide()" class="dropdown-row">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-pen" />
                                        </div>
                                        <div class="dropdown-label">{{ t('page.questionsSection.list.question.actions.edit') }}</div>
                                    </div>

                                    <NuxtLink v-if="userStore.isAdmin"
                                        :to="$urlHelper.getQuestionUrl(props.question.title, props.question.id)">
                                        <div class="dropdown-row">
                                            <div class="dropdown-icon">
                                                <font-awesome-icon icon="fa-solid fa-file" />
                                            </div>
                                            <div class="dropdown-label">
                                                {{ t('page.questionsSection.list.question.actions.viewPage') }}
                                            </div>
                                        </div>
                                    </NuxtLink>

                                    <NuxtLink v-if="userStore.isAdmin" :to="props.question.linkToQuestionVersions">
                                        <div class="dropdown-row">
                                            <div class="dropdown-icon">
                                                <font-awesome-icon icon="fa-solid fa-code-fork" />
                                            </div>
                                            <div class="dropdown-label">
                                                {{ t('page.questionsSection.list.question.actions.history') }}
                                            </div>
                                        </div>
                                    </NuxtLink>

                                    <div class="dropdown-row" @click="showCommentModal(); hide()">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon :icon="['fas', 'comment']" />
                                        </div>
                                        <div class="dropdown-label">
                                            {{ t('page.questionsSection.list.question.actions.comment') }}
                                        </div>
                                    </div>

                                    <div class="dropdown-row" v-if="isCreator || userStore.isAdmin"
                                        @click="deleteQuestionStore.openModal(props.question.id); hide()">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-trash" />
                                        </div>
                                        <div class="dropdown-label">
                                            {{ t('page.questionsSection.list.question.actions.delete') }}
                                        </div>
                                    </div>

                                </template>
                            </VDropdown>
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

    &.needsConsolidation {
        background: linear-gradient(to right, @memo-yellow 0px, @memo-yellow 8px, #ffffffff 8px, #ffffffff 100%);
    }

    &.needsLearning {
        background: linear-gradient(to right, @memo-salmon 0px, @memo-salmon 8px, #ffffffff 8px, #ffffffff 100%);
    }

    &.notLearned {
        background: linear-gradient(to right, @memo-grey-light 0px, @memo-grey-light 8px, #ffffffff 8px, #ffffffff 100%);
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
            max-height: 44px;
            max-width: 44px;

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

                        .questionTitleContainer {
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

                            .questionTitle {
                                word-break: break-all;
                            }
                        }

                        @media (max-width: @screen-sm-min) {
                            .col-xs-3 {
                                width: 33%;
                                min-width: 120px;
                            }

                            .col-xs-9 {
                                width: calc(100% - 33%);
                                max-width: calc(100% - 120px);
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

                        strong {
                            padding: 0 8px;
                        }

                        :deep(p) {
                            padding: 0 8px;

                            .tiptapImgMixin(true);
                        }
                    }

                    .notes {
                        padding-top: 16px;
                        padding-bottom: 8px;
                        font-size: 12px;

                        a {
                            cursor: pointer;
                        }

                        .relatedPages {
                            padding-bottom: 16px;
                        }

                        .sources {
                            overflow-wrap: break-word;
                        }
                    }

                    .questionBody {
                        .answer {
                            padding: 0 8px;
                        }

                        :deep(li) {
                            p {
                                margin: 0;
                            }
                        }

                        .extendedQuestion {
                            :deep(p) {
                                padding: 0 8px;

                                .tiptapImgMixin(true);
                            }
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

                    .answerDetails {
                        width: 260px;
                        padding-bottom: 16px;
                        display: flex;
                        justify-content: flex-start;
                        align-items: center;
                        flex-direction: row;
                        color: @memo-grey-dark;

                        @media(max-width: @screen-xxs-max) {
                            padding-right: 0px;
                        }

                        .spacercontainer {
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            height: 100%;

                            .spacer {
                                margin-left: 8px;
                                margin-right: 8px;
                                height: 8px;
                                width: 1px;
                                background-color: @memo-grey-light;
                            }
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

                                &.needsConsolidation {
                                    color: @memo-yellow;
                                }

                                &.needsLearning {
                                    color: @memo-salmon;
                                }

                                &.notLearned {
                                    color: @memo-grey-light;
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

                                &.needsConsolidation {
                                    background: @memo-yellow;
                                }

                                &.needsLearning {
                                    background: @memo-salmon;
                                }

                                &.notLearned {
                                    background: @memo-grey-light;
                                    color: @memo-grey-darker;
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
    height: 20px;
    justify-content: center;
    border-radius: 15px;
    cursor: pointer;

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