<script lang="ts" setup>
import { Visibility } from '../../shared/visibilityEnum'
import { useUserStore } from '../../user/userStore'
import { SolutionType } from '../solutionTypeEnum'
import { useEditQuestionStore } from './editQuestionStore'
import { AlertType, useAlertStore } from '../../alert/alertStore'
import { PageResult, PageItem } from '../../search/searchHelper'
import { debounce } from 'underscore'
import { useLoadingStore } from '../../loading/loadingStore'
import { useTabsStore, Tab } from '../../page/tabs/tabsStore'
import { usePageStore } from '../../page/pageStore'
import { Editor } from '@tiptap/vue-3'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { useLearningSessionConfigurationStore } from '~/components/page/learning/learningSessionConfigurationStore'
import { SearchType } from '../../search/searchHelper'
import { QuestionListItem } from '~/components/page/learning/questionListItem'

const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const learningSessionStore = useLearningSessionStore()
const userStore = useUserStore()
const loadingStore = useLoadingStore()
const editQuestionStore = useEditQuestionStore()
const pageStore = usePageStore()
const visibility = ref(Visibility.Public)
const solutionType = ref(SolutionType.Text)
const addToWuwi = ref(true)
const alertStore = useAlertStore()
const { t } = useI18n()

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

const pageIds = ref<number[]>([])
const selectedPages = ref<PageItem[]>([])
function removePage(t: PageItem) {
    if (selectedPages.value.length > 1) {
        var index = selectedPages.value.findIndex(s => s === t)
        selectedPages.value.splice(index, 1)

        var pageIdIndex = pageIds.value.findIndex(i => i === t.id)
        pageIds.value.splice(pageIdIndex, 1)
    }
}
const { $logger } = useNuxtApp()

const tabsStore = useTabsStore()
async function search() {
    showDropdown.value = true
    const data = {
        term: searchTerm.value,
    }

    const result = await $api<PageResult>('/apiVue/Search/Page', {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })

    if (result != null) {
        pages.value = result.pages
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
    if (term.length > 0 && lockDropdown.value === false) {
        showDropdown.value = true
        debounceSearch()
    }
    else
        showDropdown.value = false
})

const pages = ref([] as PageItem[])

function selectPage(t: PageItem) {
    showDropdown.value = false
    lockDropdown.value = true
    searchTerm.value = ''

    var index = pageIds.value.indexOf(t.id)
    if (index < 0) {
        pageIds.value.push(t.id)
        selectedPages.value.push(t)
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
        case SolutionType.Flashcard:
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
    if (solutionType.value === SolutionType.Numeric || solutionType.value === SolutionType.Date)
        solutionType.value = SolutionType.Text

    const editData = {
        QuestionId: editQuestionStore.id,
    }
    const createData = {
        AddToWishKnowledge: addToWuwi.value,
    }
    const visibility = isPrivate.value ? 1 : 0

    const dataExtension = {
        PageIds: pageIds.value,
        TextHtml: questionHtml.value,
        QuestionExtensionHtml: questionExtensionHtml.value,
        DescriptionHtml: descriptionHtml.value,
        Solution: solution.toString(),
        SolutionType: solutionType.value,
        Visibility: visibility,
        SolutionMetadataJson: solutionMetadataJson.value,
        LicenseId: licenseId.value === 0 ? 1 : licenseId.value,
        SessionIndex: learningSessionStore.lastIndexInQuestionList,
        IsLearningTab: tabsStore.activeTab === Tab.Learning,
        SessionConfig: learningSessionConfigurationStore.buildSessionConfigJson(),
        uploadedImagesMarkedForDeletion: editQuestionStore.uploadedImagesMarkedForDeletion,
        uploadedImagesInContent: editQuestionStore.uploadedImagesInContent
    }
    const data = editQuestionStore.edit ? editData : createData

    return { ...data, ...dataExtension }
}
async function updateQuestionCount() {
    let count = await $api<number>(`/apiVue/QuestionEditModal/GetCurrentQuestionCount/${pageStore.id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })

    if (count) {
        pageStore.questionCount = count
        pageStore.reloadKnowledgeSummary()
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
    loadingStore.startLoading()

    await editQuestionStore.waitUntilAllUploadsComplete()

    const url = editQuestionStore.edit ? '/apiVue/QuestionEditModal/Edit' : '/apiVue/QuestionEditModal/Create'
    const data = getData()

    const result = await $api<FetchResult<QuestionListItem>>(url, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    }).catch(error => {
        loadingStore.stopLoading()
        alertStore.openAlert(AlertType.Error, { text: editQuestionStore.edit ? t('error.question.save ') : t('error.question.creation') })
        lockSaveButton.value = false
    })

    if (result?.success) {
        if (editQuestionStore.edit) {
            learningSessionStore.updateQuestionList(result.data)
            learningSessionStore.reloadAnswerBody(result.data.id, result.data.sessionIndex)
        } else if (result.data.sessionIndex > 0) {
            learningSessionStore.lastIndexInQuestionList = result.data.sessionIndex
            learningSessionStore.getLastStepInQuestionList()
            learningSessionStore.addNewQuestionToList(learningSessionStore.lastIndexInQuestionList)
        }

        if (result.data.sessionIndex > 0 || tabsStore.activeTab != Tab.Learning || editQuestionStore.edit)
            alertStore.openAlert(AlertType.Success, {
                text: editQuestionStore.edit ? t('success.question.saved') : t('success.question.created')
            })
        else
            alertStore.openAlert(AlertType.Success, {
                text: editQuestionStore.edit ? t('success.question.saved') : t('success.question.created'),
                customHtml: `<div class="session-config-error fade in col-xs-12"><span><b>${t('question.editModal.filterAlert.active')}</b> ${t('question.editModal.filterAlert.notShown')} ${t('question.editModal.filterAlert.resetToShow')}</span></div>`,
                customBtn: `<button class="btn memo-button btn-link pull-right cancel-alert">${t('question.editModal.filterAlert.resetButton')}</button>`,
                customBtnKey: 'resetLearningSession'
            }, 'Ok')
        highlightEmptyFields.value = false
        loadingStore.stopLoading()
        editQuestionStore.showModal = false
        lockSaveButton.value = false
        updateQuestionCount()
        editQuestionStore.questionEdited(result.data.id)
    } else if (result?.success === false) {
        highlightEmptyFields.value = false
        loadingStore.stopLoading()
        editQuestionStore.showModal = false
        lockSaveButton.value = false

        alertStore.openAlert(AlertType.Error, {
            text: t(result.messageKey)
        })
    }

    editQuestionStore.uploadedImagesMarkedForDeletion = []
    editQuestionStore.uploadedImagesInContent = []
}

onMounted(() => {
    alertStore.$onAction(({ name, after }) => {
        if (name === 'closeAlert')
            after((result) => {
                if (result.customKey === 'resetLearningSession')
                    learningSessionConfigurationStore.reset()
            })
    })
})

type QuestionData = {
    solutionType: SolutionType
    solution: string
    solutionMetadataJson: string
    text: string
    textExtended: string
    pageIds: number[]
    descriptionHtml: string
    pages: PageItem[]
    licenseId: number
    visibility: Visibility
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
            matchListJson.value = solution
            break
        case SolutionType.Flashcard:
            flashCardAnswer.value = solution
    }

    return solution
}
const questionEditor = ref()
const questionExtensionEditor = ref(null)

const isInit = ref(false)

async function getQuestionData(id: number) {
    isInit.value = true

    const result = await $api<QuestionData>(`/apiVue/QuestionEditModal/GetData/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })
    if (result != null) {
        solutionType.value = result.solutionType as SolutionType
        initiateSolution(result.solution)
        questionHtml.value = result.text
        questionExtensionHtml.value = result.textExtended
        descriptionHtml.value = result.descriptionHtml
        pageIds.value = result.pageIds
        selectedPages.value = result.pages
        licenseId.value = result.licenseId
        solutionMetadataJson.value = result.solutionMetadataJson
        if (result.visibility === 1)
            isPrivate.value = true
        else {
            isPrivate.value = false
            licenseConfirmation.value = true
        }
    }

    await nextTick()
    isInit.value = false
}

watch(() => editQuestionStore.showModal, (showModal) => {
    if (showModal) {
        if (editQuestionStore.edit) {
            getQuestionData(editQuestionStore.id)
        }
        else {
            if (editQuestionStore.pageId === pageStore.id)
                selectedPages.value = [pageStore.searchPageItem!]

            pageIds.value = [editQuestionStore.pageId]
            questionHtml.value = editQuestionStore.questionHtml
            solutionType.value = SolutionType.Flashcard
            initiateSolution(editQuestionStore.flashCardAnswerHtml)
        }
    }
})

const solutionIsValid = ref(true)

function setFlashcardContent(e: { solution: string, solutionIsValid: boolean }) {
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
    <LazyModal :primary-btn-label="editQuestionStore.edit ? t('question.editModal.button.save') : t('question.editModal.button.add')" :is-full-size-buttons="false"
        v-if="editQuestionStore.showModal" :secondary-btn="t('question.editModal.button.cancel')" @close="editQuestionStore.showModal = false"
        @primary-btn="save()" :show="editQuestionStore.showModal" :disabled="disabled" :show-cancel-btn="true">
        <template v-slot:header>

        </template>
        <template v-slot:body>
            <div id="EditQuestionModal">

                <div class="edit-question-modal-header overline-m overline-title">

                    <div class="main-header">
                        <div class="add-inline-question-label main-label">
                            {{ editQuestionStore.edit ? t('question.editModal.title.edit') : t('question.editModal.title.create') }}
                            <font-awesome-icon v-if="visibility === Visibility.Public" icon="fa-solid fa-lock" />
                        </div>

                        <div class="solutionType-selector">
                            <select v-if="!editQuestionStore.edit" v-model="solutionType">
                                <option :value="SolutionType.Text">{{ t('question.editModal.solutionType.text') }}</option>
                                <option :value="SolutionType.MultipleChoice">{{ t('question.editModal.solutionType.multipleChoice') }}</option>
                                <option :value="SolutionType.MatchList">{{ t('question.editModal.solutionType.matchList') }}</option>
                                <option :value="SolutionType.Flashcard">{{ t('question.editModal.solutionType.flashCard') }}</option>
                            </select>
                        </div>
                    </div>

                    <div class="heart-container wuwi-red" @click="addToWuwi = !addToWuwi"
                        v-if="!editQuestionStore.edit">
                        <div>
                            <font-awesome-icon v-if="addToWuwi" icon="fa-solid fa-heart" />
                            <font-awesome-icon v-else icon="fa-regular fa-heart" />
                        </div>
                    </div>
                </div>
                <div class="inline-question-editor">

                    <div class="input-container">
                        <div class="overline-s no-line">{{ t('question.editModal.label.question') }}</div>
                        <QuestionEditFlashcardFront v-if="solutionType === SolutionType.Flashcard"
                            :highlight-empty-fields="highlightEmptyFields" @set-question-data="setQuestionData"
                            ref="questionEditor" :content="questionHtml" :is-init="isInit" />
                        <QuestionEditEditor v-else :highlight-empty-fields="highlightEmptyFields"
                            @set-question-data="setQuestionData" ref="questionEditor" :content="questionHtml" :is-init="isInit" />
                    </div>

                    <div class="input-container" v-if="solutionType != SolutionType.Flashcard">
                        <QuestionEditExtensionEditor :highlightEmptyFields="highlightEmptyFields"
                            @setQuestionExtensionData="setQuestionExtensionData" ref="questionExtensionEditor"
                            :content="questionExtensionHtml" :is-init="isInit" />
                    </div>
                    <QuestionEditText v-if="solutionType === SolutionType.Text" :solution="textSolution"
                        :highlightEmptyFields="highlightEmptyFields" @set-solution="setTextSolution" />
                    <QuestionEditMultipleChoice v-if="solutionType === SolutionType.MultipleChoice"
                        :solution="multipleChoiceJson" :highlightEmptyFields="highlightEmptyFields"
                        @set-multiple-choice-json="setMultipleChoiceContent" />
                    <QuestionEditMatchList v-if="solutionType === SolutionType.MatchList" :solution="matchListJson"
                        :highlightEmptyFields="highlightEmptyFields" @set-matchlist-json="setMatchlistContent" />
                    <QuestionEditFlashcard v-if="solutionType === SolutionType.Flashcard" :solution="flashCardAnswer"
                        :highlightEmptyFields="highlightEmptyFields" @set-flashcard-content="setFlashcardContent"
                        ref="flashCardComponent" :is-init="isInit" />

                    <div class="input-container description-container">
                        <div class="overline-s no-line">{{ t('question.editModal.label.description') }}</div>
                        <QuestionEditDescriptionEditor :highlightEmptyFields="highlightEmptyFields"
                            :content="descriptionHtml" @set-description-data="setDescriptionData" :is-init="isInit" />
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">{{ t('question.editModal.label.pageAssignment') }}</div>
                        <form class="" v-on:submit.prevent>
                            <div class="form-group dropdown pageSearchAutocomplete"
                                :class="{ 'open': showDropdown }">
                                <div class="related-pages-container">
                                    <PageChip v-for="(t, index) in selectedPages" :key="index" :page="t"
                                        :index="index" @removePage="removePage"
                                        :removable-chip="selectedPages.length > 1" />

                                </div>
                                <Search :search-type="SearchType.page" :show-search-icon="false" :show-search="true"
                                    :page-ids-to-filter="pageIds" placement="bottom" :auto-hide="true"
                                    :placeholder-label="t('question.editModal.placeholder.pageSearch')"
                                    :show-default-search-icon="true" @select-item="selectPage" />
                            </div>

                        </form>
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">
                            {{ t('question.editModal.label.visibility') }}
                        </div>
                        <div class="privacy-selector"
                            :class="{ 'not-selected': !licenseIsValid && highlightEmptyFields }">
                            <div class="checkbox-container">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="isPrivate" :value="1"> {{ t('question.editModal.visibility.private') }} <i
                                            class="fas fa-lock show-tooltip tooltip-min-200" title=""
                                            data-placement="top" data-html="true"
                                            :data-original-title="t('question.editModal.visibility.privateTooltip')">
                                        </i>
                                    </label>
                                </div>
                            </div>
                            <div class="checkbox-container license-confirmation-box" v-if="!isPrivate">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="licenseConfirmation" value="false">
                                        {{ t('question.editModal.license.ccBy') }} <span class="btn-link"
                                            @click.prevent="showMore = !showMore">{{ t('question.editModal.button.more') }}</span>
                                        <template v-if="showMore">
                                            <br />
                                            <br />
                                            {{ t('question.editModal.license.fullText') }}
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
                        <option value="0">{{ t('question.editModal.license.none') }}</option>
                        <option value="1">{{ t('question.editModal.license.ccBy40') }}</option>
                        <option value="2">{{ t('question.editModal.license.bamf') }}</option>
                        <option value="3">{{ t('question.editModal.license.elwis') }}</option>
                        <option value="4">{{ t('question.editModal.license.blac') }}</option>
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

#EditQuestionModal {
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

    .related-pages-container {
        display: flex;
        flex-wrap: wrap;

        .page-chip-component {
            display: flex;
            align-items: center;
            margin-right: 15px;
            overflow: hidden;

            .page-chip-container {
                padding: 4px 0;
            }

            .page-chip-deleteBtn {
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
}
</style>
