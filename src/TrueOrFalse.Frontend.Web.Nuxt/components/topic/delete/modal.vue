<script lang="ts" setup>
import { useDeleteTopicStore } from './deleteTopicStore'
import { SearchType, TopicItem } from '~~/components/search/searchHelper'

const deleteTopicStore = useDeleteTopicStore()

const primaryBtnLabel = ref('Thema löschen')
const newParentForQuestions = ref<TopicItem>()
watch(() => deleteTopicStore.topicDeleted, (val) => {
    if (val)
        primaryBtnLabel.value = 'Weiter'
    else primaryBtnLabel.value = 'Thema löschen'
})

watch(() => deleteTopicStore.suggestedNewParent, (val) => {
    if (val)
        newParentForQuestions.value = val
})

async function handlePrimaryAction() {
    if (deleteTopicStore.topicDeleted) {
        deleteTopicStore.showModal = false
        deleteTopicStore.topicDeleted = false

        if (deleteTopicStore.redirect)
            await navigateTo(deleteTopicStore.redirectURL)
    } else {
        deleteTopicStore.deleteTopic()
    }
}

function selectNewParentForQuestions(topic: TopicItem) {
    console.log(topic, "selectNewParentForQuestions")
    newParentForQuestions.value = topic
    deleteTopicStore.suggestedNewParent = topic
}

const showSelectedTopic = ref(false)

watch(newParentForQuestions, (val) => {
    showSelectedTopic.value = val ? true : false

})

const showDropdown = ref(true)
</script>

<template>
    <LazyModal :show="deleteTopicStore.showModal" :show-cancel-btn="!deleteTopicStore.topicDeleted"
        v-if="deleteTopicStore.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handlePrimaryAction()"
        @close="deleteTopicStore.showModal = false">
        <template v-slot:header>
            <template v-if="deleteTopicStore.topicDeleted">
                Thema gelöscht
            </template>
            <template v-else>
                Thema '{{ deleteTopicStore.name }}' löschen
            </template>
        </template>
        <template v-slot:body>
            <template v-if="deleteTopicStore.topicDeleted && deleteTopicStore.redirect">
                Beim schliessen dieses Fensters wirst Du zum nächsten übergeordnetem Thema weitergeleitet.
            </template>
            <template v-else-if="deleteTopicStore.topicDeleted && !deleteTopicStore.redirect">
                Das Thema '<strong>{{ deleteTopicStore.name }}</strong>' wurde erfolgreich gelöscht.
            </template>
            <template v-else>
                <!-- <div>
                    <label class="flex">
                        <input type="radio" :value="parent?.id" v-model="selectedParent">
                        <div class="radio-button-description"> {{ parent?.name }}</div>
                    </label>
                </div>
                <div class="StickySearch">
                    <Search :placeholderLabel="'Suche hier...'" :showDefaultSearchIcon="true"
                        :search-type="SearchType.moveQuestions" :show-search="showSearch"
                        v-on:select-item="setNewTopicId" :topicIdsToFilter="[deleteTopicStore.id]" />
                </div> -->

                <div>
                    <div class="body-m">Möchtest Du '<strong>{{ deleteTopicStore.name }}</strong>' unwiderruflich
                        löschen?
                    </div>
                    <div class="body-m">
                        Du kannst das vorausgewählte Thema nehmen oder dir ein Thema aussuchen wohin deine Fragen
                        verschoben
                        werden sollen
                    </div>
                    <div class="body-s">Fragen werden nicht gelöscht.</div>
                    <div class="body-s" v-if="deleteTopicStore.hasPublicQuestion">Du musst ein Thema auswählen, wohin
                        dene Fragen verschoben werden sollen!</div>
                    <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedTopic && newParentForQuestions != null" class="searchResultItem mb-125"
                            data-toggle="tooltip" data-placement="top" :title="newParentForQuestions.name">
                            <img :src="newParentForQuestions.imageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ newParentForQuestions.name }}</div>
                                <div class="searchResultQuestionCount body-s">
                                    {{ newParentForQuestions.questionCount }} Frage<template
                                        v-if="newParentForQuestions.questionCount != 1">n</template>
                                </div>
                            </div>
                            <div class="selectedSearchResultItemContainer">
                                <div class="selectedSearchResultItem">
                                    Ausgewählt
                                    <font-awesome-icon icon="fa-solid fa-check" />
                                </div>
                            </div>
                        </div>

                        <Search :search-type="SearchType.topic" :show-search="true"
                            v-on:select-item="selectNewParentForQuestions" :topic-ids-to-filter="[deleteTopicStore.id]"
                            :public-only="deleteTopicStore.hasPublicQuestion" />
                    </div>
                </div>
                <div class="alert alert-warning" role="alert" v-if="deleteTopicStore.showErrorMsg">
                    {{ deleteTopicStore.messageKey }}
                </div>
            </template>
        </template>
        <template v-slot:footer>
        </template>
    </LazyModal>
</template>
<style scoped lang="less">
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.flex {
    display: flex;
    align-items: center;
}

.radio-button-description {
    margin-left: 4px;
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