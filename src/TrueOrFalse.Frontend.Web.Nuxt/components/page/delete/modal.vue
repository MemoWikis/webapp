<script lang="ts" setup>
import { useDeletePageStore } from './deletePageStore'
import { SearchType, PageItem } from '~~/components/search/searchHelper'

const deletePageStore = useDeletePageStore()

const primaryBtnLabel = ref('Seite löschen')
const newParentForQuestions = ref<PageItem>()
watch(() => deletePageStore.pageDeleted, (val) => {
    if (val)
        primaryBtnLabel.value = 'Weiter'
    else primaryBtnLabel.value = 'Seite löschen'
})

watch(() => deletePageStore.suggestedNewParent, (val) => {
    if (val)
        newParentForQuestions.value = val
})

async function handlePrimaryAction() {
    if (deletePageStore.pageDeleted) {
        deletePageStore.showModal = false
        deletePageStore.pageDeleted = false

        if (deletePageStore.redirect)
            await navigateTo(deletePageStore.redirectURL)
    } else {
        deletePageStore.deletePage()
    }
}

function selectNewParentForQuestions(page: PageItem) {
    newParentForQuestions.value = page
    deletePageStore.suggestedNewParent = page
}

const showSelectedPage = ref(false)

watch(newParentForQuestions, (val) => {
    showSelectedPage.value = val ? true : false
})

const showDropdown = ref(true)
</script>

<template>
    <LazyModal :show="deletePageStore.showModal" :show-cancel-btn="!deletePageStore.pageDeleted"
        v-if="deletePageStore.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handlePrimaryAction()"
        @close="deletePageStore.showModal = false">
        <template v-slot:header>
            <h4 class="modal-title">
                <template v-if="deletePageStore.pageDeleted">
                    Seite gelöscht
                </template>
                <template v-else>
                    Seite '{{ deletePageStore.name }}' löschen
                </template>
            </h4>

        </template>
        <template v-slot:body>
            <div class="delete-modal">
                <template v-if="deletePageStore.pageDeleted && deletePageStore.redirect">
                    Beim Schließen dieses Fensters wirst du zur nächsten übergeordneten Seite weitergeleitet.
                </template>
                <template v-else-if="deletePageStore.pageDeleted && !deletePageStore.redirect">
                    Die Seite '<strong>{{ deletePageStore.name }}</strong>' wurde erfolgreich gelöscht.
                </template>
                <template v-else>
                    <div>
                        <div class="body-m">
                            Möchtest Du
                            '<strong>
                                <NuxtLink :to="$urlHelper.getPageUrl(deletePageStore.name, deletePageStore.id)">
                                    {{ deletePageStore.name }}
                                </NuxtLink>
                            </strong>'
                            unwiderruflich löschen?
                        </div>
                        <div v-if="deletePageStore.hasQuestion" class="new-parent-page-selection-container">
                            <div class="body-s">
                                <strong> Fragen werden nicht gelöscht. </strong>
                                Verschiebe die Fragen auf eine andere Seite.
                                <br />
                                <template v-if="!deletePageStore.hasPublicQuestion">
                                    Es gibt öffentliche Fragen auf dieser Seite.
                                    Du kannst nur öffentliche Seiten auswählen.
                                    <br />
                                </template>
                            </div>
                            <div class="body-s" v-if="newParentForQuestions">
                                Wie wäre es mit
                                <NuxtLink :to="$urlHelper.getPageUrl(newParentForQuestions.name, newParentForQuestions.id)">
                                    {{ newParentForQuestions.name }}
                                </NuxtLink>?
                            </div>
                            <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open': showDropdown }">
                                <div v-if="showSelectedPage && newParentForQuestions != null" class="searchResultItem mb-125" data-toggle="tooltip" data-placement="top" :title="newParentForQuestions.name">
                                    <img :src="newParentForQuestions.imageUrl" />
                                    <div class="searchResultBody">
                                        <div class="searchResultLabel body-m">{{ newParentForQuestions.name }}</div>
                                        <div class="searchResultQuestionCount body-s">
                                            {{ newParentForQuestions.questionCount }} Frage<template v-if="newParentForQuestions.questionCount != 1">n</template>
                                        </div>
                                    </div>
                                    <!-- <div class="selectedSearchResultItemContainer">
                                        <div class="selectedSearchResultItem">
                                            Ausgewählt
                                            <font-awesome-icon icon="fa-solid fa-check" />
                                        </div>
                                    </div> -->
                                </div>
                                <div class="body-s">Oder suche eine andere Seite aus.</div>
                                <Search :search-type="SearchType.page" :show-search="true" v-on:select-item="selectNewParentForQuestions" :page-ids-to-filter="[deletePageStore.id]" :public-only="deletePageStore.hasPublicQuestion" />
                            </div>
                        </div>
                    </div>
                    <div class="alert alert-warning" role="alert" v-if="deletePageStore.showErrorMsg">
                        {{ deletePageStore.messageKey }}
                    </div>
                </template>
            </div>
        </template>
        <template v-slot:footer>
        </template>
    </LazyModal>
</template>
<style scoped lang="less">
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.delete-modal {
    margin-bottom: 24px;
}

.new-parent-page-selection-container {
    margin-top: 20px;
    border: solid 1px @memo-grey-lighter;
    margin-left: -12px;
    margin-right: -12px;
    padding: 16px 12px 8px;
    border-radius: 2px;
}

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