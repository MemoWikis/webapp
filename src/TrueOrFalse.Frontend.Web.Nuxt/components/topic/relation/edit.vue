<script lang="ts" setup>
import { ref } from 'vue'
import { useEditTopicRelationStore, EditTopicRelationType } from './editTopicRelationStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { useUserStore } from '~~/components/user/userStore'
import { useTopicStore } from '../topicStore'
import { debounce } from 'underscore'
import { FullSearch, TopicItem, SearchType } from '~~/components/search/searchHelper'
import { messages } from '~~/components/alert/alertStore'

const spinnerStore = useSpinnerStore()
const userStore = useUserStore()
const editTopicRelationStore = useEditTopicRelationStore()
const topicStore = useTopicStore()

const name = ref('')
const showErrorMsg = ref(false)
const errorMsg = ref('')
const forbbidenTopicName = ref('')
const existingTopicUrl = ref('')
const { $logger } = useNuxtApp()

async function validateName() {
    type TopicNameValidationResult = {
        name: string
        url?: string
    }

    const result = await $api<FetchResult<TopicNameValidationResult>>('/apiVue/TopicRelationEdit/ValidateName', {
        method: 'POST', body: { name: name.value }, mode: 'cors', credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success)
        return true
    else if (result.success == false) {
        errorMsg.value = messages.getByCompositeKey(result.messageKey)
        forbbidenTopicName.value = result.data.name
        if (result.data.url)
            existingTopicUrl.value = result.data.url
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
        return false
    }
}
const privateTopicLimitReached = ref(false)
async function addTopic() {

    if (!userStore.isLoggedIn) {
        userStore.showLoginModal = true
        return
    }
    spinnerStore.showSpinner()

    const nameIsValid = await validateName()

    if (!nameIsValid)
        return

    type QuickCreateResult = {
        name: string
        id: number
        cantSavePrivateTopic?: boolean
    }

    const topicData = {
        name: name.value,
        parentTopicId: editTopicRelationStore.parentId,
    }

    const result = await $api<FetchResult<QuickCreateResult>>('/apiVue/TopicRelationEdit/QuickCreate', {
        method: 'POST', body: topicData, mode: 'cors', credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
    if (result.success) {
        spinnerStore.hideSpinner()
        topicStore.childTopicCount++
        editTopicRelationStore.showModal = false
        editTopicRelationStore.addTopic(result.data.id)

        // await nextTick()
        if (editTopicRelationStore.redirect)
            await navigateTo($urlHelper.getTopicUrl(result.data.name, result.data.id))

    } else if (result.success == false) {
        errorMsg.value = messages.getByCompositeKey(result.messageKey)
        showErrorMsg.value = true

        if (result.data.cantSavePrivateTopic) {
            privateTopicLimitReached.value = true
        }
        spinnerStore.hideSpinner()
    }
}

const disableAddButton = ref(true)

watch(name, (val) => {
    if (val.length <= 0)
        disableAddButton.value = true
    else
        disableAddButton.value = false
})

const selectedTopicId = ref(0)
watch(selectedTopicId, (id) => {
    if (id > 0 && editTopicRelationStore.type != EditTopicRelationType.Create || editTopicRelationStore.type == EditTopicRelationType.Create)
        disableAddButton.value = false
})

const showDropdown = ref(false)
const lockDropdown = ref(false)
const searchTerm = ref('')
const selectedTopic = ref(null as TopicItem | null)
const showSelectedTopic = ref(false)

function selectTopic(t: TopicItem) {
    showDropdown.value = false
    lockDropdown.value = true
    searchTerm.value = t.name
    selectedTopic.value = t
    selectedTopicId.value = t.id
    showSelectedTopic.value = true
    selectedParentInWikiId.value = t.id
}

const selectedParentInWikiId = ref(0)

const topics = reactive({ value: [] as TopicItem[] })
const totalCount = ref(0)

const forbiddenTopicName = ref('')

async function moveTopicToNewParent() {
    spinnerStore.showSpinner()

    if (selectedTopicId.value == editTopicRelationStore.parentId) {
        errorMsg.value = messages.error.category.loopLink
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
        return
    }

    const topicData = {
        childId: editTopicRelationStore.childId,
        parentIdToRemove: editTopicRelationStore.topicIdToRemove,
        parentIdToAdd: selectedTopic.value?.id
    }

    const result = await $api<FetchResult<any>>('/apiVue/TopicRelationEdit/MoveChild', {
        body: topicData,
        method: 'POST',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success == true) {
        editTopicRelationStore.parentId = selectedTopic.value?.id!
        editTopicRelationStore.addTopic(editTopicRelationStore.childId)
        editTopicRelationStore.removeTopic(editTopicRelationStore.childId, editTopicRelationStore.topicIdToRemove)
        editTopicRelationStore.showModal = false
        if (editTopicRelationStore.parentId == topicStore.id)
            topicStore.childTopicCount++
        if (editTopicRelationStore.topicIdToRemove == topicStore.id)
            topicStore.childTopicCount--

        spinnerStore.hideSpinner()
    } else {
        errorMsg.value = messages.getByCompositeKey(result.messageKey)
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
    }
}

const { $urlHelper } = useNuxtApp()

watch(() => editTopicRelationStore.type, (val) => {
    if (val == EditTopicRelationType.AddToPersonalWiki && editTopicRelationStore.personalWiki) {
        selectedTopicId.value = editTopicRelationStore.personalWiki.id
    }
})

watch(() => editTopicRelationStore.personalWiki, (val) => {
    if (val && editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki) {
        selectedTopicId.value = editTopicRelationStore.personalWiki!.id
    }
})

function getAddChildPayload() {

    if (editTopicRelationStore.type == EditTopicRelationType.AddChild)
        editTopicRelationStore.childId = selectedTopicId.value

    else if (editTopicRelationStore.type == EditTopicRelationType.AddParent || editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki)
        editTopicRelationStore.parentId = selectedTopicId.value

    return {
        childId: editTopicRelationStore.childId,
        parentId: editTopicRelationStore.parentId,
    }
}
async function addExistingTopic() {
    spinnerStore.showSpinner()
    const data = getAddChildPayload()

    if (data.childId == data.parentId) {
        errorMsg.value = messages.error.category.loopLink
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
        return
    }

    const result = await $api<FetchResult<{ name: string, id: number }>>('/apiVue/TopicRelationEdit/AddChild', {
        mode: 'cors',
        method: 'POST',
        body: data,
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success) {
        if (editTopicRelationStore.redirect)
            await navigateTo($urlHelper.getTopicUrl(result.data.name, result.data.id))
        editTopicRelationStore.showModal = false
        editTopicRelationStore.addTopic(editTopicRelationStore.childId)
        spinnerStore.hideSpinner()
    } else {
        errorMsg.value = messages.getByCompositeKey(result.messageKey)
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
    }
}


watch(searchTerm, (term) => {
    if (term.length > 0 && lockDropdown.value == false) {
        showDropdown.value = true
        debounceSearch()
    }
    else
        showDropdown.value = false
})

const debounceSearch = debounce(() => {
    search()
}, 500)

async function search() {
    showDropdown.value = true
    const data = {
        term: searchTerm.value,
        topicsIdToFilter: editTopicRelationStore.categoriesToFilter
    }

    const url = editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki
        ? '/apiVue/TopicRelationEdit/SearchTopicInPersonalWiki'
        : '/apiVue/TopicRelationEdit/SearchTopic'

    const result = await $api<FullSearch>(url, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result != null) {
        topics.value = result.topics.filter(t => t.id != editTopicRelationStore.parentId)
        totalCount.value = result.topicCount
    }
}

editTopicRelationStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'initWikiData' && editTopicRelationStore.personalWiki) {
            selectedParentInWikiId.value = editTopicRelationStore.personalWiki.id
        }
    })
})

const primaryBtnLabel = ref('Seite erstellen')
watch(() => editTopicRelationStore.type, (type) => {
    switch (type) {
        case EditTopicRelationType.Create:
            primaryBtnLabel.value = 'Seite erstellen'
            break
        case EditTopicRelationType.Move:
            primaryBtnLabel.value = 'Seite verschieben'
            break
        case EditTopicRelationType.AddChild:
        case EditTopicRelationType.AddParent:
            primaryBtnLabel.value = 'Seite verknüpfen'
            break
        case EditTopicRelationType.AddToPersonalWiki:
            primaryBtnLabel.value = 'Seite verknüpfen'
            editTopicRelationStore.initWikiData()
            break
    }
})

function handleMainBtn() {
    switch (editTopicRelationStore.type) {
        case EditTopicRelationType.Create:
            addTopic()
            break
        case EditTopicRelationType.Move:
            moveTopicToNewParent()
            break
        case EditTopicRelationType.AddChild:
        case EditTopicRelationType.AddParent:
        case EditTopicRelationType.AddToPersonalWiki:
            addExistingTopic()
            break
    }
}

watch(() => editTopicRelationStore.showModal, (val) => {
    if (val == false) {
        name.value = ''
        showErrorMsg.value = false
        privateTopicLimitReached.value = false
    }
})

</script>

<template>
    <LazyModal @close="editTopicRelationStore.showModal = false" :show="editTopicRelationStore.showModal"
        v-if="editTopicRelationStore.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handleMainBtn()"
        :show-cancel-btn="true">

        <template v-slot:header>
            <h4 v-if="editTopicRelationStore.type == EditTopicRelationType.Create" class="modal-title">
                Neue Seite erstellen
            </h4>
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.Move" class="modal-title">
                Seite verschieben nach
            </h4>
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddChild" class="modal-title">
                Bestehende Seite verknüpfen
            </h4>
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddParent || editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki"
                class="modal-title">
                Neue Übergeordnete Seite verknüpfen
            </h4>
        </template>

        <template v-slot:body>
            <template v-if="editTopicRelationStore.type == EditTopicRelationType.Create">
                <form v-on:submit.prevent="addTopic">
                    <div class="form-group">
                        <input class="form-control create-input" v-model="name"
                            placeholder="Bitte gib den Namen des Themas ein" />
                        <small class="form-text text-muted"></small>
                    </div>
                </form>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :href="existingTopicUrl" class="alert-link">{{ forbbidenTopicName }}</NuxtLink>
                    {{ errorMsg }}
                </div>
                <div class="link-to-sub-container" v-if="privateTopicLimitReached">
                    <NuxtLink to="/Preise" class="btn-link link-to-sub"><b>{{ messages.info.joinNow }}</b></NuxtLink>
                </div>
                <div class="categoryPrivate" v-else>
                    <p>
                        <b>Diese Seite ist privat.</b> Du kannst sie später im Dreipunkt-Menü oder direkt über das
                        Schloss-Icon veröffentlichen.
                    </p>
                </div>
            </template>


            <template v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki">
                <div class="mb-250">
                    <p>Wo soll die Seite hinzugefügt werden?</p>
                </div>
                <div>
                    <div class="categorySearchAutocomplete mb-250" v-if="editTopicRelationStore.personalWiki != null"
                        @click="selectedParentInWikiId = userStore.personalWiki?.id ?? 0">
                        <div class="searchResultItem"
                            :class="{ 'selectedSearchResultItem': selectedParentInWikiId == editTopicRelationStore.personalWiki.id }">
                            <img :src="editTopicRelationStore.personalWiki.miniImageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ editTopicRelationStore.personalWiki.name }}
                                </div>
                                <div class="searchResultQuestionCount body-s">
                                    {{ editTopicRelationStore.personalWiki.questionCount + `
                                    Frage${editTopicRelationStore.personalWiki.questionCount != 1 ? 'n' : ''}` }}
                                </div>
                            </div>
                            <div v-show="selectedParentInWikiId == editTopicRelationStore.personalWiki.id"
                                class="selectedSearchResultItemContainer">
                                <div class="selectedSearchResultItem">
                                    Ausgewählt
                                    <font-awesome-icon icon="fa-solid fa-check" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="categorySearchAutocomplete mb-250"
                        v-if="editTopicRelationStore.recentlyUsedRelationTargetTopics != null && editTopicRelationStore.recentlyUsedRelationTargetTopics.length > 0">
                        <div class="overline-s mb-125 no-line">Zuletzt ausgewählte Themen</div>
                        <template v-for="previousTopic in editTopicRelationStore.recentlyUsedRelationTargetTopics">
                            <div class="searchResultItem"
                                :class="{ 'selectedSearchResultItem': selectedParentInWikiId == previousTopic.id }"
                                @click="selectedParentInWikiId = previousTopic.id">
                                <img :src="previousTopic.imageUrl" />
                                <div class="searchResultBody">
                                    <div class="searchResultLabel body-m">{{ previousTopic.name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ previousTopic.questionCount }}
                                        Frage<template v-if="previousTopic.questionCount != 1">n</template>
                                    </div>
                                </div>
                                <div v-show="selectedParentInWikiId == previousTopic.id" class="selectedSearchResultItemContainer">
                                    <div class="selectedSearchResultItem">
                                        Ausgewählt
                                        <font-awesome-icon icon="fa-solid fa-check" />
                                    </div>
                                </div>
                            </div>
                        </template>
                    </div>
                    <div class="mb-125">
                        <p>Andere Seite auswählen</p>
                    </div>
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic && selectedTopic != null" class="searchResultItem mb-125" :class="{ 'selectedSearchResultItem': selectedParentInWikiId == selectedTopic.id }"
                            @click="selectedParentInWikiId = selectedTopic?.id ?? 0"
                            data-toggle="tooltip" data-placement="top" :title="selectedTopic?.name">
                            <img :src="selectedTopic?.imageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ selectedTopic?.name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.questionCount }}
                                    Frage<template v-if="selectedTopic?.questionCount != 1">n</template></div>
                            </div>
                            <div v-show="selectedParentInWikiId == selectedTopic.id" class="selectedSearchResultItemContainer">
                                <div class="selectedSearchResultItem">
                                    Ausgewählt
                                    <font-awesome-icon icon="fa-solid fa-check" />
                                </div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.topicInWiki" :show-search="true" v-on:select-item="selectTopic" :topic-ids-to-filter="editTopicRelationStore.categoriesToFilter" />

                        <div class="swap-type-target">
                            <button @click="editTopicRelationStore.type = EditTopicRelationType.AddParent">
                                <label>
                                    <div class="checkbox-container">
                                        <input type="checkbox" name="addToParent" class="hidden" />
                                        <font-awesome-icon icon="fa-solid fa-square-check" class="checkbox-icon active" />
                                        <span class="checkbox-label">
                                            Nur im eigenen Wiki suchen
                                        </span>
                                    </div>
                                </label>
                            </button>
                        </div>

                    </div>


                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :to="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}
                    </NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddParent">
                <div class="mb-250">
                    <p>Wo soll die Seite hinzugefügt werden?</p>
                </div>
                <div>
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic && selectedTopic != null" class="searchResultItem mb-125"
                            data-toggle="tooltip" data-placement="top" :title="selectedTopic.name">
                            <img :src="selectedTopic.imageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedTopic.name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.questionCount }}
                                    Frage<template v-if="selectedTopic.questionCount != 1">n</template></div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.topic" :show-search="true" v-on:select-item="selectTopic" :topic-ids-to-filter="editTopicRelationStore.categoriesToFilter" />

                        <div class="swap-type-target">
                            <button @click="editTopicRelationStore.type = EditTopicRelationType.AddToPersonalWiki">
                                <label>
                                    <div class="checkbox-container">
                                        <input type="checkbox" name="addToWiki" class="hidden" />
                                        <font-awesome-icon icon="fa-regular fa-square" class="checkbox-icon" />
                                        <span class="checkbox-label">
                                            Nur im eigenen Wiki suchen
                                        </span>
                                    </div>
                                </label>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :to="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}
                    </NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else>
                <div>
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic && selectedTopic != null" class="searchResultItem mb-125"
                            data-toggle="tooltip" data-placement="top" :title="selectedTopic.name">
                            <img :src="selectedTopic.imageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedTopic.name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.questionCount }}
                                    Frage<template v-if="selectedTopic.questionCount != 1">n</template></div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.topic" :show-search="true" v-on:select-item="selectTopic" :topic-ids-to-filter="editTopicRelationStore.categoriesToFilter" />
                    </div>
                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :to="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}
                    </NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>

        </template>

    </LazyModal>
</template>

<style lang="less" scoped>
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.link-to-sub-container {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 24px;
}

.categorySearchAutocomplete {
    .swap-type-target {
        display: flex;
        flex-direction: row-reverse;
        align-items: center;
        width: 100%;
        padding-top: 4px;

        button {
            background: white;
            padding: 4px 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 12px;
            cursor: pointer;

            &:hover {
                filter: brightness(0.975)
            }

            &:active {
                filter: brightness(0.95)
            }

            label {
                margin-bottom: 0px;
            }

            .checkbox-container {
                padding: 0px;
                cursor: pointer;

                .checkbox-icon {
                    margin-right: 4px;

                    &.active {
                        color: @memo-blue-link;
                    }
                }

                .checkbox-label {
                    cursor: pointer;

                }
            }
        }
    }
}

.create-input {
    border-radius: 0px;
}

.searchResultItem {
    padding: 4px;
    display: flex;
    width: 100%;
    height: 70px;
    transition: .2s ease-in-out;
    cursor: pointer;
    margin-left: -4px;
    margin-right: -4px;

    .searchResultBody {
        width: 100%;
    }

    .searchResultLabelContainer {
        width: 100%;
        height: 100%;
    }

    .searchResultLabel {
        height: 40px;
        line-height: normal;
        color: @memo-grey-darker;
        text-overflow: ellipsis;
        margin: 0;
        overflow: hidden;
        max-width: 408px;
    }

    &:hover {
        background: @memo-grey-lighter;
        color: @memo-blue;

        .searchResultLabel {
            color: @memo-blue;
        }
    }


    .searchResultQuestionCount {
        color: @memo-grey-dark;
        font-style: italic;
        height: 20px;
        line-height: normal;
    }


    img {
        max-height: 70px;
        max-width: 70px;
        height: auto;
        margin-right: 10px;

        &.authorImg {
            border-radius: 50%;
        }
    }

    &.selectedSearchResultItem {
        color: @memo-blue;
        background-color: @memo-blue-light-transparent;

        .searchResultLabel {
            color: @memo-blue;
        }
    }

    .selectedSearchResultItemContainer {
        position: relative;

        .selectedSearchResultItem {
            position: absolute;
            right: 0;
            bottom: 0;
            display: flex;
            align-items: center;
            flex-wrap: nowrap;

            i {
                padding-left: 4px;
            }
        }
    }
}
</style>