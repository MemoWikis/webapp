<script lang="ts" setup>
import { ref } from 'vue'
import { useEditTopicRelationStore, EditTopicRelationType } from './editTopicRelationStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { useUserStore } from '~~/components/user/userStore'
import { messages } from '~~/components/alert/messages'
import { useTopicStore } from '../topicStore'
import _ from 'underscore'
import { FullSearch, TopicItem } from '~~/components/search/searchHelper'

const spinnerStore = useSpinnerStore()
const userStore = useUserStore()
const editTopicRelationStore = useEditTopicRelationStore()
const topicStore = useTopicStore()

const name = ref('')
const showErrorMsg = ref(false)
const errorMsg = ref('')
const forbbidenTopicName = ref('')
const existingTopicUrl = ref('')

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

    const nameValidationResult = await $fetch<TopicNameValidationResult>('/apiVue/EditTopic/ValidateName', { method: 'POST', body: { name: name.value }, mode: 'cors', credentials: 'include' })

    if (nameValidationResult.categoryNameAllowed) {
        type QuickCreateResult = {
            success: boolean
            url: string
            id: number
        }

        const topicData = {
            name: name.value,
            parentTopicId: editTopicRelationStore.parentId,
        }

        const result = await $fetch<QuickCreateResult>('/apiVue/EditTopic/QuickCreate', { method: 'POST', body: topicData, mode: 'cors', credentials: 'include' })
        if (result.success) {
            if (editTopicRelationStore.redirect)
                navigateTo(result.url)

            topicStore.childTopicCount++
            editTopicRelationStore.showModal = false
            spinnerStore.hideSpinner()
        }
    } else {
        showErrorMsg.value = messages.error.category[nameValidationResult.key]
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

const addToWikiHistory = ref(null)

const hideSearch = ref(true)
const topics = reactive({ value: [] as TopicItem[] })
const totalCount = ref(0)

const forbiddenTopicName = ref('')

const childId = ref(null)
const parentTopicIdToRemove = ref(null)
const parentId = ref(null)
const redirect = ref(true)

const addTopicBtnId = ref(null)

function loadTopicCard(id: number) {
    var data = {
        parentId: parentId.value,
        newCategoryId: id
    }
    // eventBus.$emit('add-category-card', data);
}

async function moveTopicToNewParent() {
    spinnerStore.showSpinner()

    if (selectedTopicId.value == parentId.value) {
        errorMsg.value = messages.error.category.loopLink
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
        return
    }

    var topicData = {
        childCategoryId: childId.value,
        parentCategoryIdToRemove: parentTopicIdToRemove.value,
        parentCategoryIdToAdd: selectedTopic.value?.Id
    }

    var data = await $fetch<any>('/apiVue/EditCategory/MoveChild', {
        body: topicData,
        method: 'POST',
    })

    if (data.success) {
        if (redirect.value)
            window.open(data.url, '_self')

        if (addTopicBtnId.value != null)
            loadTopicCard(data.id)
        else
            editTopicRelationStore.showModal = false

        topicStore.childTopicCount++
        spinnerStore.hideSpinner()
    } else {
        errorMsg.value = messages.error.category[data.key]
        showErrorMsg.value = true
        spinnerStore.hideSpinner()
    }

}

// async function addExistingTopic() {
//     spinnerStore.showSpinner()

//     if (this.selectedCategoryId == this.parentId) {
//         this.errorMsg = messages.error.category.loopLink;
//         this.showErrorMsg = true;
//         Utils.HideSpinner();
//         return;
//     }

//     var self = this;
//     var categoryData = {
//         childCategoryId: self.selectedCategoryId,
//         parentCategoryId: self.parentId,
//     }



//     $.ajax({
//         type: 'Post',
//         contentType: "application/json",
//         url: '/EditCategory/AddChild',
//         data: JSON.stringify(categoryData),
//         success(data) {
//             if (data.success) {
//                 if (self.redirect)
//                     window.open(data.url, '_self');

//                 if (self.addCategoryBtnId != null)
//                     self.loadCategoryCard(data.id);

//                 $('#AddCategoryModal').modal('hide');
//                 self.addCategoryCount();
//                 Utils.HideSpinner();
//             } else {
//                 self.errorMsg = messages.error.category[data.key];
//                 self.showErrorMsg = true;
//                 Utils.HideSpinner();
//             };
//         },
//     });
// }

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

const debounceSearch = _.debounce(() => {
    search()
}, 500)

async function search() {
    showDropdown.value = true;
    var data = {
        term: searchTerm.value,
    }

    var url = editTopicRelationStore.type == EditTopicRelationType.AddToPersonalWiki
        ? '/apiVue/Search/CategoryInWiki'
        : '/apiVue/Search/Category';

    var result = await $fetch<FullSearch>(url, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result != null) {
        topics.value = result.categories.filter(t => t.Id != parentId.value)
        totalCount.value = result.categoryCount
    }
}

editTopicRelationStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'initWikiData' && editTopicRelationStore.personalWiki) {
            selectedParentInWikiId.value = editTopicRelationStore.personalWiki.Id
        }
    })
})

</script>

<template>
    <!-- <LazyModal @close="editTopicRelationStore.showModal = false" :show="editTopicRelationStore.showModal">
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
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddParent" class="modal-title">Neues
                Oberthema verknüpfen</h4>
            <h4 v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddToWiki" class="modal-title">Thema zu
                meinem Wiki hinzufügen</h4>
        </template>
        <template v-slot:body>
            <template v-if="editTopicRelationStore.type == EditTopicRelationType.Create">
                <form v-on:submit.prevent="addTopic">
                    <div class="form-group">
                        <input class="form-control" v-model="name" placeholder="Bitte gib den Namen des Themas ein" />
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


            <template v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddToWiki">
                <div class="mb-250">
                    <p>Wo soll das Thema hinzugefügt werden?</p>
                </div>
                <form v-on:submit.prevent="selectTopic">
                    <div class="categorySearchAutocomplete mb-250" v-if="editTopicRelationStore.personalWiki != null"
                        @click="selectedParentInWikiId = editTopicRelationStore.personalWiki.Id">
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
                                    <i class="fas fa-check"></i>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="categorySearchAutocomplete mb-250"
                        v-if="addToWikiHistory != null && addToWikiHistory.length > 0">
                        <div class="overline-s mb-125 no-line">Zuletzt ausgewählte Themen</div>
                        <template v-for="previousTopic in addToWikiHistory" v-if="addToWikiHistory != null">
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
                                        <i class="fas fa-check"></i>
                                    </div>
                                </div>
                            </div>
                        </template>
                    </div>
                    <div class="mb-125">
                        <a v-if="hideSearch" @click="hideSearch = false">Anderes Thema auswählen</a>
                        <span v-else>Anderes Thema auswählen</span>
                    </div>
                    <div v-if="!hideSearch" class="form-group dropdown categorySearchAutocomplete"
                        :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic" class="searchResultItem mb-125"
                            :class="{ 'selectedSearchResultItem': selectedParentInWikiId == selectedTopic.Id }"
                            @click="selectedParentInWikiId = selectedTopic.Id" data-toggle="tooltip"
                            data-placement="top" :title="selectedTopic.Name">
                            <img :src="selectedTopic.ImageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ selectedTopic.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.QuestionCount }}
                                    Frage<template v-if="selectedTopic.QuestionCount != 1">n</template></div>
                            </div>
                            <div v-show="selectedParentInWikiId == selectedTopic.Id"
                                class="selectedSearchResultItemContainer">
                                <div class="selectedSearchResultItem">
                                    Ausgewählt
                                    <i class="fas fa-check"></i>
                                </div>
                            </div>
                        </div>
                        <input ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm"
                            id="searchInWikiList" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true"
                            placeholder="Bitte gib den Namen des Themas ein" />
                        <ul class="dropdown-menu" aria-labelledby="searchList">
                            <li class="searchResultItem" v-for="t in topics.value" @click="selectTopic(t)"
                                data-toggle="tooltip" data-placement="top" :title="t.Name"
                                :data-original-title="t.Name">
                                <img :src="t.ImageUrl" />
                                <div>
                                    <div class="searchResultLabel body-m">{{ t.Name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ t.QuestionCount }} Frage<template
                                            v-if="t.QuestionCount != 1">n</template></div>
                                </div>
                            </li>
                            <li class="dropdownFooter body-m">
                                <b>{{ totalCount }}</b> Treffer. <br />
                                Deins ist nicht dabei? <span class="dropdownLink"
                                    @click="editTopicRelationStore.type = EditTopicRelationType.Create">Erstelle hier
                                    dein
                                    Thema</span>
                            </li>
                        </ul>
                    </div>
                </form>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <a :href="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}</a>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else>
                <form v-on:submit.prevent="selectTopic">
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic" class="searchResultItem mb-125" data-toggle="tooltip"
                            data-placement="top" :title="selectedTopic.Name">
                            <img :src="selectedTopic.ImageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedTopic.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedTopic.QuestionCount }}
                                    Frage<template v-if="selectedTopic.QuestionCount != 1">n</template></div>
                            </div>
                        </div>
                        <input ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm"
                            id="searchList" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true"
                            placeholder="Bitte gib den Namen des Themas ein" />
                        <ul class="dropdown-menu" aria-labelledby="searchList">
                            <li class="searchResultItem" v-for="t in topics.value" @click="selectTopic(t)"
                                data-toggle="tooltip" data-placement="top" :title="t.Name"
                                :data-original-title="t.Name">
                                <img :src="t.ImageUrl" />
                                <div>
                                    <div class="searchResultLabel body-m">{{ t.Name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ t.QuestionCount }} Frage<template
                                            v-if="t.QuestionCount != 1">n</template></div>
                                </div>
                            </li>
                            <li class="dropdownFooter body-m">
                                <b>{{ totalCount }}</b> Treffer. <br />
                                Deins ist nicht dabei? <span class="dropdownLink"
                                    @click="editTopicRelationStore.type = EditTopicRelationType.Create">Erstelle hier
                                    dein
                                    Thema</span>
                            </li>
                        </ul>
                    </div>
                </form>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <a :href="existingTopicUrl" target="_blank" class="alert-link">{{ forbiddenTopicName }}</a>
                    {{ errorMsg }}
                </div>
            </template>


        </template>
        <template v-slot:footer>
            <div v-if="editTopicRelationStore.type == EditTopicRelationType.Create" id="AddNewTopicBtn"
                class="btn btn-primary memo-button" @click="addTopic" :class="{ 'disabled': disableAddButton }">Thema
                erstellen
            </div>
            <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.Move" id="MoveTopicToNewParentBtn"
                class="btn btn-primary memo-button" @click="moveTopicToNewParent"
                :class="{ 'disabled': disableAddButton }">
                Thema verschieben</div>
            <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddChild" id="AddExistingTopicBtn"
                class="btn btn-primary memo-button" @click="addExistingTopic" :class="{ 'disabled': disableAddButton }">
                Thema
                verknüpfen</div>
            <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddParent" id="AddNewParentBtn"
                class="btn btn-primary memo-button" @click="addNewParentToTopic"
                :class="{ 'disabled': disableAddButton }">Thema
                verknüpfen</div>
            <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddToWiki" id="AddToWiki"
                class="btn btn-primary memo-button" @click="addNewParentToTopic"
                :class="{ 'disabled': disableAddButton }">Thema
                verknüpfen</div>
            <div class="btn btn-link memo-button" @click="editTopicRelationStore.showModal = false">Abbrechen</div>
        </template>

    </LazyModal> -->
</template>