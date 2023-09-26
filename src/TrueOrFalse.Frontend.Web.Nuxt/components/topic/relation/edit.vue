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
async function addTopic() {

    if (!userStore.isLoggedIn) {
        userStore.showLoginModal = true
        return
    }
    spinnerStore.showSpinner()

    type TopicNameValidationResult = {
        categoryNameAllowed: boolean
        name: string
        url: string
        key: string
    }

    const nameValidationResult = await $fetch<TopicNameValidationResult>('/apiVue/TopicRelationEdit/ValidateName', {
        method: 'POST', body: { name: name.value }, mode: 'cors', credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (nameValidationResult.categoryNameAllowed) {
        type QuickCreateResult = {
            success: boolean
            name: string
            id: number
        }

        const topicData = {
            name: name.value,
            parentTopicId: editTopicRelationStore.parentId,
        }

        const result = await $fetch<QuickCreateResult>('/apiVue/TopicRelationEdit/QuickCreate', {
            method: 'POST', body: topicData, mode: 'cors', credentials: 'include',
            onResponseError(context) {
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
            },
        })
        if (result.success) {
            spinnerStore.hideSpinner()
            topicStore.childTopicCount++
            editTopicRelationStore.showModal = false
            editTopicRelationStore.addTopic(result.id)

            // await nextTick()
            if (editTopicRelationStore.redirect)
                await navigateTo($urlHelper.getTopicUrl(result.name, result.id))
        }
    } else {
        errorMsg.value = messages.error.category[nameValidationResult.key]
        forbbidenTopicName.value = nameValidationResult.name
        existingTopicUrl.value = nameValidationResult.url
        showErrorMsg.value = true
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

function selectTopic(t: any) {
    showDropdown.value = false
    lockDropdown.value = true
    searchTerm.value = t.Name
    selectedTopic.value = t
    selectedTopicId.value = t.Id
    showSelectedTopic.value = true
    selectedParentInWikiId.value = t.Id;
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
        parentIdToAdd: selectedTopic.value?.Id
    }

    const result = await $fetch<FetchResult<any>>('/apiVue/TopicRelationEdit/MoveChild', {
        body: topicData,
        method: 'POST',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success == true) {
        editTopicRelationStore.parentId = selectedTopic.value?.Id!
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

async function addExistingTopic() {
    spinnerStore.showSpinner()
    editTopicRelationStore.childId = selectedTopicId.value

    if (editTopicRelationStore.parentId == editTopicRelationStore.childId) {
        errorMsg.value = messages.error.category.loopLink
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
        return
    }

    const data = {
        childId: editTopicRelationStore.childId,
        parentId: editTopicRelationStore.parentId,
    }

    const result = await $fetch<FetchResult<{ name: string, id: number }>>('/apiVue/TopicRelationEdit/AddChild', {
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

async function addNewParentToTopic() {

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

    const result = await $fetch<FullSearch>(url, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result != null) {
        topics.value = result.topics.filter(t => t.Id != editTopicRelationStore.parentId)
        totalCount.value = result.topicCount
    }
}

editTopicRelationStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'initWikiData' && editTopicRelationStore.personalWiki) {
            selectedParentInWikiId.value = editTopicRelationStore.personalWiki.Id
        }
    })
})

const primaryBtnLabel = ref('Thema erstellen')
watch(() => editTopicRelationStore.type, (type) => {
    switch (type) {
        case EditTopicRelationType.Create:
            primaryBtnLabel.value = 'Thema erstellen'
            break
        case EditTopicRelationType.Move:
            primaryBtnLabel.value = 'Thema verschieben'
            break
        case EditTopicRelationType.AddChild:
        case EditTopicRelationType.AddParent:
            primaryBtnLabel.value = 'Thema verknüpfen'
            break
        case EditTopicRelationType.AddToPersonalWiki:
            primaryBtnLabel.value = 'Thema verknüpfen'
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
            addExistingTopic()
            break
        case EditTopicRelationType.AddParent:
        case EditTopicRelationType.AddToPersonalWiki:
            addNewParentToTopic()
            break
    }
}

watch(() => editTopicRelationStore.showModal, (val) => {
    if (val == false) {
        name.value = ''
        showErrorMsg.value = false
    }
})

</script>

<template>
    <LazyModal @close="editTopicRelationStore.showModal = false" :show="editTopicRelationStore.showModal"
        v-if="editTopicRelationStore.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handleMainBtn()"
        :show-cancel-btn="true">
        <template v-slot:header>
            <h4 v-if="editTopicRelationStore.type == EditTopicRelationType.Create" class="modal-title">Neues Thema
                erstellen
            </h4>
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.Move" class="modal-title">Thema
                verschieben
                nach</h4>
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddChild" class="modal-title">
                Bestehendes
                Thema verknüpfen</h4>
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddParent || editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki"
                class="modal-title">Neues
                Oberthema verknüpfen</h4>
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
                <div class="categoryPrivate">
                    <p><b> Das Thema ist privat.</b> Du kannst es später im das Dreipunkt-Menü oder direkt über das
                        Schloss-Icon veröffentlichen.</p>
                </div>
            </template>


            <template v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki">
                <div class="mb-250">
                    <p>Wo soll das Thema hinzugefügt werden?</p>
                </div>
                <div>
                    <div class="categorySearchAutocomplete mb-250" v-if="editTopicRelationStore.personalWiki != null"
                        @click="selectedParentInWikiId = userStore.personalWiki?.Id ?? 0">
                        <div class="searchResultItem"
                            :class="{ 'selectedSearchResultItem': selectedParentInWikiId == editTopicRelationStore.personalWiki.Id }">
                            <img :src="editTopicRelationStore.personalWiki.MiniImageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ editTopicRelationStore.personalWiki.Name }}
                                </div>
                                <div class="searchResultQuestionCount body-s">{{
                                    editTopicRelationStore.personalWiki.QuestionCount
                                }}
                                    Frage<template
                                        v-if="editTopicRelationStore.personalWiki.QuestionCount != 1">n</template></div>
                            </div>
                            <div v-show="selectedParentInWikiId == editTopicRelationStore.personalWiki.Id"
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
                                :class="{ 'selectedSearchResultItem': selectedParentInWikiId == previousTopic.Id }"
                                @click="selectedParentInWikiId = previousTopic.Id">
                                <img :src="previousTopic.ImageUrl" />
                                <div class="searchResultBody">
                                    <div class="searchResultLabel body-m">{{ previousTopic.Name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ previousTopic.QuestionCount }}
                                        Frage<template v-if="previousTopic.QuestionCount != 1">n</template></div>
                                </div>
                                <div v-show="selectedParentInWikiId == previousTopic.Id"
                                    class="selectedSearchResultItemContainer">
                                    <div class="selectedSearchResultItem">
                                        Ausgewählt
                                        <font-awesome-icon icon="fa-solid fa-check" />
                                    </div>
                                </div>
                            </div>
                        </template>
                    </div>
                    <div class="mb-125">
                        <p>Anderes Thema auswählen</p>
                    </div>
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic && selectedTopic != null" class="searchResultItem mb-125"
                            :class="{ 'selectedSearchResultItem': selectedParentInWikiId == selectedTopic.Id }"
                            @click="selectedParentInWikiId = selectedTopic?.Id ?? 0" data-toggle="tooltip"
                            data-placement="top" :title="selectedTopic?.Name">
                            <img :src="selectedTopic?.ImageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ selectedTopic?.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.QuestionCount }}
                                    Frage<template v-if="selectedTopic?.QuestionCount != 1">n</template></div>
                            </div>
                            <div v-show="selectedParentInWikiId == selectedTopic.Id"
                                class="selectedSearchResultItemContainer">
                                <div class="selectedSearchResultItem">
                                    Ausgewählt
                                    <font-awesome-icon icon="fa-solid fa-check" />
                                </div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.TopicInWiki" :show-search="true" v-on:select-item="selectTopic"
                            :topic-ids-to-filter="editTopicRelationStore.categoriesToFilter" />

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
                    <NuxtLink :to="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}</NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddParent">
                <div class="mb-250">
                    <p>Wo soll das Thema hinzugefügt werden?</p>
                </div>
                <div>
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic && selectedTopic != null" class="searchResultItem mb-125"
                            data-toggle="tooltip" data-placement="top" :title="selectedTopic.Name">
                            <img :src="selectedTopic.ImageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedTopic.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.QuestionCount }}
                                    Frage<template v-if="selectedTopic.QuestionCount != 1">n</template></div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.Topic" :show-search="true" v-on:select-item="selectTopic"
                            :topic-ids-to-filter="editTopicRelationStore.categoriesToFilter" />

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
                    <NuxtLink :to="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}</NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else>
                <div>
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic && selectedTopic != null" class="searchResultItem mb-125"
                            data-toggle="tooltip" data-placement="top" :title="selectedTopic.Name">
                            <img :src="selectedTopic.ImageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedTopic.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.QuestionCount }}
                                    Frage<template v-if="selectedTopic.QuestionCount != 1">n</template></div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.Topic" :show-search="true" v-on:select-item="selectTopic"
                            :topic-ids-to-filter="editTopicRelationStore.categoriesToFilter" />
                    </div>
                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :to="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}</NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>

        </template>

    </LazyModal>
</template>

<style lang="less" scoped>
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

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