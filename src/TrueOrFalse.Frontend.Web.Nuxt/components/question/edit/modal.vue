<script lang="ts" setup>
import { Visibility } from '../../shared/visibilityEnum'
import { useUserStore } from '../../user/userStore'
import { SolutionType } from '../solutionTypeEnum'
import { useEditQuestionStore } from './editQuestionStore'
import { AlertType, useAlertStore, messages } from '../../alert/alertStore'
import { TopicResult, TopicItem } from '../../search/searchHelper'
import { debounce } from 'underscore'
import { useSpinnerStore } from '../../spinner/spinnerStore'
import { useTabsStore, Tab } from '../../topic/tabs/tabsStore'
import { useTopicStore } from '../../topic/topicStore'
import { Editor } from '@tiptap/vue-3'
import { useLearningSessionStore } from '~~/components/topic/learning/learningSessionStore'
import { useLearningSessionConfigurationStore } from '~~/components/topic/learning/learningSessionConfigurationStore'
import { SearchType } from '../../search/searchHelper'
import { QuestionListItem } from '~~/components/topic/learning/questionListItem'
// import { FetchResult } from '~~/components/fetchResult'

const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const learningSessionStore = useLearningSessionStore()
const userStore = useUserStore()
const spinnerStore = useSpinnerStore()
const editQuestionStore = useEditQuestionStore()
const topicStore = useTopicStore()
const visibility = ref(Visibility.All)
const solutionType = ref(SolutionType.Text)
const addToWuwi = ref(true)
const alertStore = useAlertStore()

const highlightEmptyFields = ref(false)

const questionJson = ref({})
const questionHtml = ref('')
function setQuestionData(editor: Editor) {
    questionJson.value = editor.getJSON()
    questionHtml.value = editor.getHTML()
}

const questionExtensionJson = ref({})
const questionExtensionHtml = ref('')
function setQuestionExtensionData(editor: Editor) {
    questionExtensionJson.value = editor.getJSON()
    questionExtensionHtml.value = editor.getHTML()
}

const descriptionJson = ref({})
const descriptionHtml = ref('')
function setDescriptionData(editor: Editor) {
    descriptionJson.value = editor.getJSON()
    descriptionHtml.value = editor.getHTML()
}

const textSolution = ref<string>()

function setTextSolution(e: { textSolution: string, solutionMetaDataJson: string }) {
    textSolution.value = e.textSolution
    solutionMetadataJson.value = e.solutionMetaDataJson
}

const multipleChoiceJson = ref<string>()

const matchListJson = ref<string>()
const flashCardAnswer = ref<string>()


const topicIds = ref<number[]>([])
const selectedTopics = ref<TopicItem[]>([])
const privateTopicIds = ref<number[]>([])
function removeTopic(t: TopicItem) {
    if (selectedTopics.value.length > 1) {
        var index = selectedTopics.value.findIndex(s => s == t)
        selectedTopics.value.splice(index, 1)

        var topicIdIndex = topicIds.value.findIndex(i => i == t.Id)
        topicIds.value.splice(topicIdIndex, 1)
    }
}
const { $logger } = useNuxtApp()

const tabsStore = useTabsStore()
async function search() {
    showDropdown.value = true
    const data = {
        term: searchTerm.value,
    }

    const result = await $fetch<TopicResult>('/apiVue/Search/Topic', {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })

    if (result != null) {
        topics.value = result.topics
        totalCount.value = result.totalCount
    }
}

const debounceSearch = debounce(() => {
    search()
}, 500)

const showDropdown = ref(false)
const searchTerm = ref('')
const lockDropdown = ref(false)

watch(searchTerm, (term) => {
    if (term.length > 0 && lockDropdown.value == false) {
        showDropdown.value = true
        debounceSearch()
    }
    else
        showDropdown.value = false
})

const topics = ref([] as TopicItem[])

function selectTopic(t: TopicItem) {
    showDropdown.value = false
    lockDropdown.value = true
    searchTerm.value = ''

    var index = topicIds.value.indexOf(t.Id)
    if (index < 0) {
        topicIds.value.push(t.Id)
        selectedTopics.value.push(t)
    }
}

const totalCount = ref(0)

const licenseIsValid = ref(false)
const isPrivate = ref(true)
const licenseConfirmation = ref(false)
const showMore = ref(false)
const licenseId = ref(0)

const disabled = computed(() => {
    licenseIsValid.value = licenseConfirmation.value || isPrivate.value

    var questionIsValid = questionHtml.value.length > 0
    return !questionIsValid || !solutionIsValid.value || !licenseIsValid.value
})

const lockSaveButton = ref(false)

function getSolution() {
    switch (solutionType.value) {
        case SolutionType.Text:
            return textSolution.value
        case SolutionType.MultipleChoice:
            return multipleChoiceJson.value
        case SolutionType.MatchList:
            return matchListJson.value
        case SolutionType.FlashCard:
            return flashCardAnswer.value
        default: return null
    }
}

const solutionMetadataJson = ref('')

function getData() {
    const solution = getSolution()
    if (solution == null) {
        return
    }
    if (solutionType.value == SolutionType.Numeric || solutionType.value == SolutionType.Date)
        solutionType.value = SolutionType.Text

    const editData = {
        QuestionId: editQuestionStore.id,
    }
    const createData = {
        AddToWishknowledge: addToWuwi.value,
    }
    const visibility = isPrivate.value ? 1 : 0

    const dataExtension = {
        CategoryIds: topicIds.value,
        TextHtml: questionHtml.value,
        DescriptionHtml: descriptionHtml.value,
        Solution: solution.toString(),
        SolutionType: solutionType.value,
        Visibility: visibility,
        SolutionMetadataJson: solutionMetadataJson.value,
        LicenseId: licenseId.value == 0 ? 1 : licenseId.value,
        SessionIndex: learningSessionStore.lastIndexInQuestionList,
        IsLearningTab: tabsStore.activeTab == Tab.Learning,
        SessionConfig: learningSessionConfigurationStore.buildSessionConfigJson()
    }
    const data = editQuestionStore.edit ? editData : createData

    return { ...data, ...dataExtension }
}
async function updateQuestionCount() {
    let count = await $fetch<number>(`/apiVue/QuestionEditModal/GetCurrentQuestionCount/${topicStore.id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })

    if (count) {
        topicStore.questionCount = count
        topicStore.reloadKnowledgeSummary()
    }
}
async function save() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    if (disabled.value) {
        highlightEmptyFields.value = true
        return
    }
    lockSaveButton.value = true

    spinnerStore.showSpinner()
    const url = editQuestionStore.edit ? '/apiVue/QuestionEditModal/Edit' : '/apiVue/QuestionEditModal/Create'
    const data = getData()

    // const testData = {
    //     CategoryIds: topicIds.value,
    //     TextHtml: questionHtml.value,
    //     DescriptionHtml: descriptionHtml.value,
    //     SolutionType: solutionType.value,
    //     Visibility: visibility,
    //     SolutionMetadataJson: solutionMetadataJson.value,
    //     LicenseId: licenseId.value == 0 ? 1 : licenseId.value,
    //     SessionIndex: learningSessionStore.lastIndexInQuestionList,
    //     IsLearningTab: tabsStore.activeTab == Tab.Learning,
    //     SessionConfig: learningSessionConfigurationStore.buildSessionConfigJson()
    // }

    const result = await $fetch<FetchResult<QuestionListItem>>(url, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    }).catch(error => {
        spinnerStore.hideSpinner()
        alertStore.openAlert(AlertType.Error, { text: editQuestionStore.edit ? messages.error.question.save : messages.error.question.creation })
        lockSaveButton.value = false
    })

    if (result?.success) {
        if (editQuestionStore.edit) {
            learningSessionStore.updateQuestionList(result.data)
        } else if (result.data.SessionIndex > 0) {
            learningSessionStore.lastIndexInQuestionList = result.data.SessionIndex
            learningSessionStore.getLastStepInQuestionList()
            learningSessionStore.addNewQuestionToList(learningSessionStore.lastIndexInQuestionList)
        }

        if (result.data.SessionIndex > 0 || tabsStore.activeTab != Tab.Learning || editQuestionStore.edit)
            alertStore.openAlert(AlertType.Success, {
                text: editQuestionStore.edit ? messages.success.question.saved : messages.success.question.created
            })
        else
            alertStore.openAlert(AlertType.Success, {
                text: editQuestionStore.edit ? messages.success.question.saved : messages.success.question.created,
                customHtml: '<div class="session-config-error fade in col-xs-12"><span><b>Der Fragenfilter ist aktiv.</b> Die Frage wird dir nicht angezeigt. Setze den Filter zurück, um alle Fragen anzuzeigen.</span></div>',
                customBtn: `<button class="btn memo-button btn-link pull-right cancel-alert">Filter zurücksetzen</button>`,
                customBtnKey: 'resetLearningSession'
            }, 'Ok')
        highlightEmptyFields.value = false
        spinnerStore.hideSpinner()
        editQuestionStore.showModal = false
        lockSaveButton.value = false
        updateQuestionCount()
        editQuestionStore.questionEdited(result.data.Id)
    } else if (result?.success == false) {
        highlightEmptyFields.value = false
        spinnerStore.hideSpinner()
        editQuestionStore.showModal = false
        lockSaveButton.value = false

        alertStore.openAlert(AlertType.Error, {
            text: messages.getByCompositeKey(result.messageKey)
        })
    }
}

onMounted(() => {
    alertStore.$onAction(({ name, after }) => {
        if (name == 'closeAlert')
            after((result) => {
                if (result.customKey == 'resetLearningSession')
                    learningSessionConfigurationStore.reset()
            })
    })
})

type QuestionData = {
    SolutionType: SolutionType
    Solution: string
    SolutionMetadataJson: string
    Text: string
    TextExtended: string
    TopicIds: number[]
    DescriptionHtml: string
    Topics: TopicItem[]
    LicenseId: number
    Visibility: Visibility
}

function initiateSolution(solution: string) {
    switch (solutionType.value) {
        case SolutionType.Text:
            textSolution.value = solution
            break
        case SolutionType.MultipleChoice:
            multipleChoiceJson.value = solution
            break
        case SolutionType.MatchList:
            matchListJson.value = solution;
            break
        case SolutionType.FlashCard:
            flashCardAnswer.value = solution
    }

    return solution;
}
const questionEditor = ref()
const questionExtensionEditor = ref(null)

async function getQuestionData(id: number) {
    const result = await $fetch<QuestionData>(`/apiVue/QuestionEditModal/GetData/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })
    if (result != null) {
        solutionType.value = result.SolutionType as SolutionType
        initiateSolution(result.Solution)
        questionHtml.value = result.Text
        questionExtensionHtml.value = result.TextExtended
        descriptionHtml.value = result.DescriptionHtml
        topicIds.value = result.TopicIds
        selectedTopics.value = result.Topics
        licenseId.value = result.LicenseId
        solutionMetadataJson.value = result.SolutionMetadataJson
        if (result.Visibility == 1)
            isPrivate.value = true
        else {
            isPrivate.value = false
            licenseConfirmation.value = true
        }
    }
}

watch(() => editQuestionStore.showModal, (e) => {
    if (e) {
        if (editQuestionStore.edit) {
            getQuestionData(editQuestionStore.id)
        }
        else {
            if (editQuestionStore.topicId == topicStore.id)
                selectedTopics.value = [topicStore.searchTopicItem!]

            topicIds.value = [editQuestionStore.topicId]
            questionHtml.value = editQuestionStore.questionHtml
            solutionType.value = SolutionType.FlashCard
            initiateSolution(editQuestionStore.flashCardAnswerHtml)
        }
    }
})

const solutionIsValid = ref(true)

function setFlashCardContent(e: { solution: string, solutionIsValid: boolean }) {
    flashCardAnswer.value = e.solution
    solutionIsValid.value = e.solutionIsValid
}

function setMultipleChoiceContent(e: { solution: string, solutionIsValid: boolean }) {
    multipleChoiceJson.value = e.solution
    solutionIsValid.value = e.solutionIsValid
}

function setMatchlistContent(e: { solution: string, solutionIsValid: boolean }) {
    matchListJson.value = e.solution
    solutionIsValid.value = e.solutionIsValid
}

</script>

<template>
    <LazyModal :primary-btn-label="editQuestionStore.edit ? ' Speichern' : 'Hinzufügen'" :is-full-size-buttons="false"
        v-if="editQuestionStore.showModal" secondary-btn="Abbrechen" @close="editQuestionStore.showModal = false"
        @primary-btn="save()" :show="editQuestionStore.showModal" :disabled="disabled" :show-cancel-btn="true">
        <template v-slot:header>

        </template>
        <template v-slot:body>
            <div id="EditQuestionModal">

                <div class="edit-question-modal-header overline-m overline-title">

                    <div class="main-header">
                        <div class="add-inline-question-label main-label">
                            {{ editQuestionStore.edit ? 'Frage bearbeiten' : 'Frage erstellen' }}
                            <font-awesome-icon v-if="visibility == Visibility.All" icon="fa-solid fa-lock" />
                        </div>

                        <div class="solutionType-selector">
                            <select v-if="!editQuestionStore.edit" v-model="solutionType">
                                <option :value="SolutionType.Text">Text</option>
                                <option :value="SolutionType.MultipleChoice">MultipleChoice</option>
                                <option :value="SolutionType.MatchList">Zuordnung (Liste)</option>
                                <option :value="SolutionType.FlashCard">Karteikarte</option>
                            </select>
                        </div>
                    </div>

                    <div class="heart-container wuwi-red" @click="addToWuwi = !addToWuwi" v-if="!editQuestionStore.edit">
                        <div>
                            <font-awesome-icon v-if="addToWuwi" icon="fa-solid fa-heart" />
                            <font-awesome-icon v-else icon="fa-regular fa-heart" />
                        </div>
                    </div>
                </div>
                <div class="inline-question-editor">

                    <div class="input-container">
                        <div class="overline-s no-line">Frage</div>
                        <QuestionEditEditor :highlight-empty-fields="highlightEmptyFields"
                            @set-question-data="setQuestionData" ref="questionEditor" :content="questionHtml" />
                    </div>

                    <div class="input-container" v-if="solutionType != SolutionType.FlashCard">
                        <QuestionEditExtensionEditor :highlightEmptyFields="highlightEmptyFields"
                            @setQuestionExtensionData="setQuestionExtensionData" ref="questionExtensionEditor"
                            :content="questionExtensionHtml" />
                    </div>
                    <QuestionEditText v-if="solutionType == SolutionType.Text" :solution="textSolution"
                        :highlightEmptyFields="highlightEmptyFields" @set-solution="setTextSolution" />
                    <QuestionEditMultipleChoice v-if="solutionType == SolutionType.MultipleChoice"
                        :solution="multipleChoiceJson" :highlightEmptyFields="highlightEmptyFields"
                        @set-multiple-choice-json="setMultipleChoiceContent" />
                    <QuestionEditMatchList v-if="solutionType == SolutionType.MatchList" :solution="matchListJson"
                        :highlightEmptyFields="highlightEmptyFields" @set-matchlist-json="setMatchlistContent" />
                    <QuestionEditFlashCard v-if="solutionType == SolutionType.FlashCard" :solution="flashCardAnswer"
                        :highlightEmptyFields="highlightEmptyFields" @set-flash-card-content="setFlashCardContent"
                        ref="flashCardComponent" />

                    <div class="input-container description-container">
                        <div class="overline-s no-line">Ergänzungen zur Antwort</div>
                        <QuestionEditDescriptionEditor :highlightEmptyFields="highlightEmptyFields"
                            :content="descriptionHtml" @setDescriptionData="setDescriptionData" />
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">Themenzuordnung</div>
                        <form class="" v-on:submit.prevent>
                            <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                                <div class="related-categories-container">
                                    <TopicChip v-for="(t, index) in selectedTopics" :key="index" :topic="t" :index="index"
                                        @removeTopic="removeTopic" :removable-chip="selectedTopics.length > 1" />

                                </div>
                                <Search :search-type="SearchType.Category" :show-search-icon="false" :show-search="true"
                                    :topic-ids-to-filter="topicIds" placement="bottom" :auto-hide="true"
                                    placeholder-label="Bitte gib den Namen des Themas ein" :show-default-search-icon="true"
                                    @select-item="selectTopic" />
                            </div>

                        </form>
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">
                            Sichtbarkeit
                        </div>
                        <div class="privacy-selector" :class="{ 'not-selected': !licenseIsValid && highlightEmptyFields }">
                            <div class="checkbox-container">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="isPrivate" :value="1"> Private Frage <i
                                            class="fas fa-lock show-tooltip tooltip-min-200" title="" data-placement="top"
                                            data-html="true" data-original-title="
                                            <ul class='show-tooltip-ul'>
                                                <li>Die Frage kann nur von dir genutzt werden.</li>
                                                <li>Niemand sonst kann die Frage sehen oder nutzen.</li>
                                            </ul>">
                                        </i>
                                    </label>
                                </div>
                            </div>
                            <div class="checkbox-container license-confirmation-box" v-if="!isPrivate">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="licenseConfirmation" value="false">
                                        Dieser Eintrag wird veröffentlicht unter CC BY 4.0. <span class="btn-link"
                                            @click.prevent="showMore = !showMore">mehr</span>
                                        <template v-if="showMore">
                                            <br />
                                            <br />
                                            Ich stelle diesen Eintrag unter die Lizenz "Creative Commons -
                                            Namensnennung 4.0 International" (CC BY 4.0, Lizenztext, deutsche
                                            Zusammenfassung).
                                            Der Eintrag kann bei angemessener Namensnennung ohne Einschränkung weiter
                                            genutzt werden.
                                            Die Texte und ggf. Bilder sind meine eigene Arbeit und nicht aus
                                            urheberrechtlich geschützten Quellen kopiert.
                                        </template>

                                    </label>
                                </div>
                            </div>
                        </div>
                        <div v-if="!licenseIsValid && highlightEmptyFields"></div>
                    </div>
                </div>
                <div v-if="userStore.isAdmin">
                    <select v-model="licenseId">
                        <option value="0">Keine Lizenz</option>
                        <option value="1">CC BY 4.0</option>
                        <option value="2">Amtliches Werk BAMF</option>
                        <option value="3">ELWIS</option>
                        <option value="4">BLAC</option>
                    </select>
                </div>
            </div>

        </template>

    </LazyModal>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.modal-footer {
    display: flex;
    align-items: center;
    flex-direction: row-reverse;
}

select {
    border: solid 1px @memo-grey-light;
    border-radius: 0;
    padding: 0 4px;
    outline: none !important;
}

.edit-question-modal-header {
    display: flex;
    justify-content: space-between;

    .heart-container {
        display: flex;
        align-content: center;
        align-items: center;
        flex-direction: column;
        width: 100px;
        margin-right: -22px;
        cursor: pointer;
        color: @memo-wuwi-red;
        font-size: 22px;
    }

    .main-header {
        display: flex;
        align-items: baseline;
        padding-bottom: 4px;

        .main-label {
            padding-right: 20px;
        }

        .solutionType-selector {
            select {
                border: none;
                background: @memo-grey-lighter;
                padding: 0 4px;
                outline: none !important;
                height: 34px;
                width: 190px;
            }
        }

        @media (max-width:576px) {
            flex-direction: column;
        }
    }
}

.form-group {
    margin-bottom: 16px;
}

.is-empty {

    .ProseMirror {
        border: solid 1px @memo-salmon;
    }
}

.btn {
    &.is-empty {
        border: solid 1px @memo-salmon;
    }
}

.input-group-addon {
    border: none;
    border-radius: 0;
}

input,
.input-group-addon {
    box-shadow: none;
    background: none;

    &.toggle-correctness {
        min-width: 43px;

        &.active {
            color: white;

            &.is-correct {
                background-color: @memo-green;
            }

            &.is-wrong {
                background-color: @memo-salmon;
            }
        }
    }
}

input,
.ProseMirror-focused,
textarea,
.form-control {

    &:focus,
    &:focus-visible {
        outline: none !important;
        border: solid 1px @memo-green;
        box-shadow: none;
    }
}

.ProseMirror {
    padding: 11px 15px 0;
}

.related-categories-container {
    display: flex;
    flex-wrap: wrap;

    .topic-chip-component {
        display: flex;
        align-items: center;
        margin-right: 15px;
        overflow: hidden;

        .topic-chip-container {
            padding: 4px 0;
        }

        .topic-chip-deleteBtn {
            display: flex;
            justify-content: center;
            align-items: center;
            transition: all 0.2s ease-in-out;
            width: 16px;
            height: 100%;
            cursor: pointer;
            color: @memo-salmon;
        }
    }
}

.input-group-addon {
    background-color: @memo-grey-lighter;
}

textarea {
    width: 100%;
    padding: 11px 15px 0;
}

.description-container {
    .ProseMirror {
        min-height: 62px;
    }
}

.col-spacer {
    min-width: 38px;
}

.form-control {
    max-width: 100%;
}
</style>