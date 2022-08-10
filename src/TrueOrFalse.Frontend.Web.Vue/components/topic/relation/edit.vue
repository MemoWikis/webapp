<script lang="ts" setup>
import { ref } from 'vue'
import { useEditTopicRelationStore, EditTopicRelationType } from './editTopicRelationStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { useUserStore } from '~~/components/user/userStore'
import { messages } from '~~/components/alert/messages'
import { useTopicStore } from '../topicStore'


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

    const nameValidationResult = await $fetch<TopicNameValidationResult>('/api/EditTopic/ValidateName', { method: 'POST', body: { name: name.value }, mode: 'cors', credentials: 'include' })

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
        console.log(topicData)
        console.log(editTopicRelationStore)
        console.log(editTopicRelationStore.parentId)

        const result = await $fetch<QuickCreateResult>('/api/EditTopic/QuickCreate', { method: 'POST', body: topicData, mode: 'cors', credentials: 'include' })
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
        disableAddButton.value = true;
    else
        disableAddButton.value = false;
})

const selectedCategoryId = ref(0)
watch(selectedCategoryId, (id) => {
    if (id > 0 && editTopicRelationStore.type != EditTopicRelationType.Create || editTopicRelationStore.type == EditTopicRelationType.Create)
        disableAddButton.value = false;
})
</script>

<template>
    <LazyModal @close="editTopicRelationStore.showModal = false" :show="editTopicRelationStore.showModal">
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

            <!-- 
            <template v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddToWiki">
                <div class="mb-250">
                    <p>Wo soll das Thema hinzugefügt werden?</p>
                </div>
                <form v-on:submit.prevent="selectCategory">
                    <div class="categorySearchAutocomplete mb-250" v-if="userStore.personalWiki != null"
                        @click="selectedParentInWikiId = userStore.personalWiki.Id">
                        <div class="searchResultItem"
                            :class="{ 'selectedSearchResultItem': selectedParentInWikiId == userStore.personalWiki.Id }">
                            <img :src="userStore.personalWiki.ImgUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ userStore.personalWiki.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ userStore.personalWiki.QuestionCount }}
                                    Frage<template v-if="userStore.personalWiki.QuestionCount != 1">n</template></div>
                            </div>
                            <div v-show="selectedParentInWikiId == userStore.personalWiki.Id"
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
                        <template v-for="previousCategory in addToWikiHistory">
                            <div class="searchResultItem"
                                :class="{ 'selectedSearchResultItem': selectedParentInWikiId == previousCategory.Id }"
                                @click="selectedParentInWikiId = previousCategory.Id">
                                <img :src="previousCategory.ImageUrl" />
                                <div class="searchResultBody">
                                    <div class="searchResultLabel body-m">{{ previousCategory.Name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ previousCategory.QuestionCount }}
                                        Frage<template v-if="previousCategory.QuestionCount != 1">n</template></div>
                                </div>
                                <div v-show="selectedParentInWikiId == previousCategory.Id"
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
                        <div v-if="showSelectedCategory" class="searchResultItem mb-125"
                            :class="{ 'selectedSearchResultItem': selectedParentInWikiId == selectedCategory.Id }"
                            @click="selectedParentInWikiId = selectedCategory.Id" data-toggle="tooltip"
                            data-placement="top" :title="selectedCategory.Name">
                            <img :src="selectedCategory.ImageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ selectedCategory.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedCategory.QuestionCount }}
                                    Frage<template v-if="selectedCategory.QuestionCount != 1">n</template></div>
                            </div>
                            <div v-show="selectedParentInWikiId == selectedCategory.Id"
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
                            <li class="searchResultItem" v-for="c in categories" @click="selectCategory(c)"
                                data-toggle="tooltip" data-placement="top" :title="c.Name"
                                :data-original-title="c.Name">
                                <img :src="c.ImageUrl" />
                                <div>
                                    <div class="searchResultLabel body-m">{{ c.Name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ c.QuestionCount }} Frage<template
                                            v-if="c.QuestionCount != 1">n</template></div>
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
                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{ forbiddenCategoryName }}</a>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else>
                <form v-on:submit.prevent="selectCategory">
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedCategory" class="searchResultItem mb-125" data-toggle="tooltip"
                            data-placement="top" :title="selectedCategory.Name">
                            <img :src="selectedCategory.ImageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedCategory.Name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedCategory.QuestionCount }}
                                    Frage<template v-if="selectedCategory.QuestionCount != 1">n</template></div>
                            </div>
                        </div>
                        <input ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm"
                            id="searchList" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true"
                            placeholder="Bitte gib den Namen des Themas ein" />
                        <ul class="dropdown-menu" aria-labelledby="searchList">
                            <li class="searchResultItem" v-for="c in categories" @click="selectCategory(c)"
                                data-toggle="tooltip" data-placement="top" :title="c.Name"
                                :data-original-title="c.Name">
                                <img :src="c.ImageUrl" />
                                <div>
                                    <div class="searchResultLabel body-m">{{ c.Name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ c.QuestionCount }} Frage<template
                                            v-if="c.QuestionCount != 1">n</template></div>
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
                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{ forbiddenCategoryName }}</a>
                    {{ errorMsg }}
                </div>
            </template> -->


        </template>
        <template v-slot:footer>
            <div v-if="editTopicRelationStore.type == EditTopicRelationType.Create" id="AddNewCategoryBtn"
                class="btn btn-primary memo-button" @click="addTopic" :disabled="disableAddButton">Thema erstellen
            </div>
            <!-- <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.Move" id="MoveCategoryToNewParentBtn"
                class="btn btn-primary memo-button" @click="moveCategoryToNewParent" :disabled="disableAddButton">
                Thema verschieben</div>
            <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddChild" id="AddExistingCategoryBtn"
                class="btn btn-primary memo-button" @click="addExistingCategory" :disabled="disableAddButton">Thema
                verknüpfen</div>
            <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddParent" id="AddNewParentBtn"
                class="btn btn-primary memo-button" @click="addNewParentToCategory" :disabled="disableAddButton">Thema
                verknüpfen</div>
            <div v-else-if="editTopicRelationStore.type == EditTopicRelationType.AddToWiki" id="AddToWiki"
                class="btn btn-primary memo-button" @click="addNewParentToCategory" :disabled="disableAddButton">Thema
                verknüpfen</div> -->
            <div class="btn btn-link memo-button" @click="editTopicRelationStore.showModal = false">Abbrechen</div>
        </template>

    </LazyModal>
</template>