<script lang="ts" setup>
import { Visibility } from '../shared/visibilityEnum'
import { useUserStore } from '../user/userStore'
import { useEditQuestionStore, SolutionType } from './editQuestionStore'
import { AlertType, useAlertStore, AlertMsg, messages } from '../alert/alertStore'
import { SearchTopicItem, TopicResult } from '../search/searchHelper'
import _ from 'underscore'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { useSessionConfigurationStore } from '../topic/learning/sessionConfigurationStore'
import { useTabsStore } from '../topic/tabs/tabsStore'
import { Tab } from '../topic/tabs/tabsEnum'
import { useTopicStore } from '../topic/topicStore'

const userStore = useUserStore()
const spinnerStore = useSpinnerStore()
const editQuestionStore = useEditQuestionStore()
const sessionConfigStore = useSessionConfigurationStore()
const topicStore = useTopicStore()
const edit = ref(false)
const visibility = ref(Visibility.All)
const solutionType = ref(SolutionType.Text)
const addToWuwi = ref(true)
const alertStore = useAlertStore()

const highlightEmptyFields = ref(false)

const questionJson = ref({})
const questionHtml = ref('')
function setQuestionData(editor) {
    questionJson.value = editor.getJSON()
    questionHtml.value = editor.getHTML()
}

const questionExtensionJson = ref({})
const questionExtensionHtml = ref('')
function setQuestionExtensionData(editor) {
    questionExtensionJson.value = editor.getJSON()
    questionExtensionHtml.value = editor.getHTML()
}

const descriptionJson = ref({})
const descriptionHtml = ref('')
function setDescriptionData(editor) {
    descriptionJson.value = editor.getJSON()
    descriptionHtml.value = editor.getHTML()
}

const textSolution = ref(null)
const multipleChoiceJson = ref(null)
const matchListJson = ref(null)
const flashCardAnswer = ref(null)


const topicIds = ref([])
const selectedTopics = ref([])
function removeTopic(t) {
    if (selectedTopics.value.length > 1) {
        selectedTopics.value.splice(t.index, 1)
        var topicIdIndex = topicIds.value.indexOf(t.topicId)
        topicIds.value.splice(topicIdIndex, 1)
    }
}

const tabsStore = useTabsStore()
async function search() {
    showDropdown.value = true
    var data = {
        term: searchTerm.value,
    }

    var result = await $fetch<TopicResult>('/api/Search/Category', {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result != null) {
        topics.value = result.topics
        totalCount.value = result.totalCount
    }
}

const debounceSearch = _.debounce(() => {
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

const topics = ref([])

function selectTopic(t) {
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

const disabled = ref(true)
const lockSaveButton = ref(false)

function getSolution() {
    let solution = ""
    switch (solutionType.value) {
        case SolutionType.Text:
            return textSolution.value
        case SolutionType.MultipleChoice: solution = multipleChoiceJson.value
            break
        case SolutionType.MatchList: solution = matchListJson.value
            break
        case SolutionType.FlashCard: return flashCardAnswer.value
    }

    return JSON.stringify(solution)
}

const id = ref(0)
const solutionMetadataJson = ref('')

function getSaveJson() {
    let solution = getSolution()

    if (solutionType.value == SolutionType.Numeric || solutionType.value == SolutionType.Date)
        solutionType.value = SolutionType.Text


    var editJson = {
        QuestionId: id.value,
    }
    var createJson = {
        AddToWishknowledge: addToWuwi,
    }
    var visibility = isPrivate ? 1 : 0

    let sessionConfigJson = localStorage.getItem('sessionConfigJson') != null ? JSON.parse(localStorage.getItem('sessionConfigJson')) : ''

    var jsonExtension = {
        CategoryIds: topicIds.value,
        TextHtml: questionHtml.value,
        DescriptionHtml: descriptionHtml.value,
        Solution: solution,
        SolutionType: solutionType.value,
        Visibility: visibility,
        SolutionMetadataJson: solutionMetadataJson,
        LicenseId: licenseId.value == 0 ? 1 : licenseId.value,
        SessionIndex: sessionIndex,
        IsLearningTab: tabsStore.activeTab == Tab.Learning,
        SessionConfig: sessionConfigJson
    }
    var json = edit ? editJson : createJson

    return _.merge(json, jsonExtension)
}
async function updateQuestionCount() {
    let count = await $fetch<number>(`/Category/GetCurrentQuestionCount/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })

    if (count != null)
        topicStore.questionCount = count

}
async function save() {
    if (!userStore.isLoggedIn) {
        // NotLoggedIn.ShowErrorMsg("EditQuestion")

        return
    }

    if (disabled.value) {
        highlightEmptyFields.value = true
        return
    }
    lockSaveButton.value = true
    spinnerStore.showSpinner()
    var url = edit.value ? '/Question/Edit' : '/Question/Create'
    var json = getSaveJson()

    let result = await $fetch<any>(`/Question/GetData/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    }).catch(error => {
        spinnerStore.hideSpinner()
        alertStore.openAlert(AlertType.Error, { text: edit.value ? messages.error.question.save : messages.error.question.creation })
        lockSaveButton.value = false
    })

    if (result != null) {
        if (isLearningSession) {
            var answerBody = new AnswerBody()
            var skipIndex = 0

            answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                "?skipStepIdx=" +
                skipIndex +
                "&index=" +
                sessionIndex)

            eventBus.$emit('change-active-question', sessionIndex)
            eventBus.$emit('update-question-count')
        }

        highlightEmptyFields.value = false
        spinnerStore.hideSpinner()
        if (result.error) {
            alertStore.openAlert(AlertType.Error, {
                text: messages.error.question[result.key]
            })
        } else {
            if (result.SessionIndex > 0 || tabsStore.activeTab != Tab.Learning || edit.value)
                alertStore.openAlert(AlertType.Success, {
                    text: edit ? messages.success.question.saved : messages.success.question.created
                })
            else
                alertStore.openAlert(AlertType.Success, {
                    text: edit ? messages.success.question.saved : messages.success.question.created,
                    customHtml: '<div class="session-config-error fade in col-xs-12"><span><b>Der Fragenfilter ist aktiv.</b> Die Frage wird dir nicht angezeigt. Setze den Filter zurück, um alle Fragen anzuzeigen.</span></div>',
                    customBtn: '<div class="btn memo-button col-xs-4 btn-link" data-dismiss="modal" onclick="eventBus.$emit(\'reset-session-config\')">Filter zurücksetzen</div>',
                })
        }
        editQuestionStore.showModal = false
        lockSaveButton.value = false
        updateQuestionCount()
    }
}

type QuestionData = {
    SolutionType: SolutionType
    Solution: string,
    SolutionMetadataJson: string,
    Text: string,
    TextExtended: string,
    TopicIds: number[],
    DescriptionHtml: string,
    Topics: SearchTopicItem[],
    LicenseId: number,
    Visibility: Visibility,
}

function initiateSolution(solution) {
    switch (solutionType.value) {
        case SolutionType.Text:
            this.textSolution = solution;
            break;
        case SolutionType.MultipleChoice:
            this.multipleChoiceJson = solution;
            break;
        case SolutionType.MatchList:
            this.matchListJson = solution;
            break;
        case SolutionType.FlashCard:
            this.flashCardJson = solution;
    }

    return solution;
}
const questionEditor = ref(null)
const questionExtensionEditor = ref(null)

async function getQuestionData(id) {

    let result = await $fetch<QuestionData>(`/Question/GetData/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })

    if (result != null) {
        solutionType.value = result.SolutionType

        initiateSolution(result.Solution)
        questionHtml.value = result.Text
        questionExtensionHtml.value = result.TextExtended
        descriptionHtml.value = result.DescriptionHtml

        topicIds.value = result.TopicIds
        topics.value = result.Topics
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

watch(() => editQuestionStore.showModal, () => {

})
</script>

<template>
    <div id="EditQuestionModal">
        <LazyModal :showCloseButton="true" :modalWidth="600" button1Text="Speichern" :isFullSizeButtons="true"
            @close="editQuestionStore.showModal = false" @mainBtn="save()" :show="editQuestionStore.showModal">
            <template slot:header></template>
            <template slot:body>
                <div class="edit-question-modal-header overline-m overline-title">

                    <div class="main-header">
                        <div class="add-inline-question-label main-label">
                            <template v-if="edit">Frage bearbeiten</template>
                            <template v-else>Frage erstellen</template>
                            <font-awesome-icon v-if="visibility == Visibility.All" icon="fa-solid fa-lock" />
                        </div>

                        <div class="solutionType-selector">
                            <select v-if="!edit" v-model="solutionType">
                                <option :value="SolutionType.Text">Text</option>
                                <option :value="SolutionType.MultipleChoice">MultipleChoice</option>
                                <option :value="SolutionType.MatchList">Zuordnung (Liste)</option>
                                <option :value="SolutionType.FlashCard">Karteikarte</option>
                            </select>
                        </div>
                    </div>

                    <div class="heart-container wuwi-red" @click="addToWuwi = !addToWuwi" v-if="!edit">
                        <div>
                            <font-awesome-icon v-if="addToWuwi" icon="fa-solid fa-heart" />
                            <font-awesome-icon v-else icon="fa-regular fa-heart" />
                        </div>
                    </div>
                </div>
                <div class="inline-question-editor">

                    <div class="input-container">
                        <div class="overline-s no-line">Frage</div>
                        <QuestionEditor :highlightEmptyFields="highlightEmptyFields" @setQuestionData="setQuestionData"
                            ref="questionEditor" :content="questionHtml" />
                    </div>

                    <div class="input-container" v-if="solutionType != SolutionType.FlashCard">
                        <QuestionExtensionEditor :highlightEmptyFields="highlightEmptyFields"
                            @setQuestionExtensionData="setQuestionExtensionData" ref="questionExtensionEditor"
                            :content="questionExtensionHtml" />
                    </div>
                    <QuestionText v-if="solutionType == SolutionType.Text" :solution="textSolution"
                        :highlightEmptyFields="highlightEmptyFields" />
                    <QuestionMultipleChoice v-if="solutionType == SolutionType.MultipleChoice"
                        :solution="multipleChoiceJson" :highlightEmptyFields="highlightEmptyFields" />
                    <QuestionMatchtList v-if="solutionType == SolutionType.MatchList" :solution="matchListJson"
                        :highlightEmptyFields="highlightEmptyFields" />
                    <QuestionFlashCard v-if="solutionType == SolutionType.FlashCard" :solution="flashCardAnswer"
                        :highlightEmptyFields="highlightEmptyFields" />

                    <div class="input-container description-container">
                        <div class="overline-s no-line">Ergänzungen zur Antwort</div>
                        <QuestionDescriptionEditor :highlightEmptyFields="highlightEmptyFields"
                            @setDescriptionData="setDescriptionData" />
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">Themenzuordnung</div>
                        <form class="" v-on:submit.prevent>
                            <div class="form-group dropdown categorySearchAutocomplete"
                                :class="{ 'open': showDropdown }">
                                <div class="related-categories-container">
                                    <TopicChip v-for="(t, index) in selectedTopics" :key="index" :category="t"
                                        :index="index" @removeTopic="removeTopic" />

                                </div>
                                <input ref="searchInput" class="form-control dropdown-toggle" type="text"
                                    v-model="searchTerm" id="questionCategoriesList" autocomplete="off"
                                    @click="lockDropdown = false" aria-haspopup="true"
                                    placeholder="Bitte gib den Namen des Themas ein" />
                                <ul class="dropdown-menu" aria-labelledby="questionCategoriesList">
                                    <li class="searchResultItem" v-for="t in topics" @click="selectTopic(t)"
                                        data-toggle="tooltip" data-placement="top" :title="t.Name">
                                        <img :src="t.ImageUrl" />
                                        <div>
                                            <div class="searchResultLabel body-m">{{ t.Name }}</div>
                                            <div class="searchResultQuestionCount body-s">{{ t.QuestionCount }}
                                                Frage<template v-if="t.QuestionCount != 1">n</template></div>
                                        </div>
                                    </li>
                                    <li class="dropdownFooter body-m">
                                        <b>{{ totalCount }}</b> Treffer. <br />
                                        <!-- Deins ist nicht dabei? <span class="dropdownLink"
                                            @click="createCategory = true">Erstelle hier dein Thema</span> -->
                                    </li>
                                </ul>
                            </div>

                        </form>
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">
                            Sichtbarkeit
                        </div>
                        <div class="privacy-selector"
                            :class="{ 'not-selected': !licenseIsValid && highlightEmptyFields }">
                            <div class="checkbox-container">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="isPrivate" :value="1"> Private Frage <i
                                            class="fas fa-lock show-tooltip tooltip-min-200" title=""
                                            data-placement="top" data-html="true" data-original-title="
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
            </template>
            <template slot:footer-text></template>


        </LazyModal>
    </div>

</template>