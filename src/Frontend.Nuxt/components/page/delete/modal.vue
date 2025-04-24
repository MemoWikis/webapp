<script lang="ts" setup>
import { useDeletePageStore } from './deletePageStore'
import { SearchType, PageItem } from '~~/components/search/searchHelper'

const deletePageStore = useDeletePageStore()
const { t } = useI18n()

const primaryBtnLabel = computed(() => {
    if (deletePageStore.pageDeleted) {
        return t('page.deleteModal.button.continue')
    } else {
        return deletePageStore.isWiki
            ? t('page.deleteModal.button.deleteWiki')
            : t('page.deleteModal.button.deletePage')
    }
})

const newParentForQuestions = ref<PageItem>()

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
const { $urlHelper } = useNuxtApp()
</script>

<template>
    <LazyModal :show="deletePageStore.showModal" :show-cancel-btn="!deletePageStore.pageDeleted"
        v-if="deletePageStore.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handlePrimaryAction()"
        @close="deletePageStore.showModal = false">

        <template v-slot:header>
            <h4 class="modal-title">
                <template v-if="deletePageStore.pageDeleted">
                    {{ t(deletePageStore.isWiki
                        ? 'page.deleteModal.title.deleted.wiki'
                        : 'page.deleteModal.title.deleted.page') }}
                </template>
                <template v-else>
                    {{ t(deletePageStore.isWiki
                        ? 'page.deleteModal.title.delete.wiki'
                        : 'page.deleteModal.title.delete.page',
                        { name: deletePageStore.name }) }}
                </template>
            </h4>
        </template>

        <template v-slot:body>
            <div class="delete-modal">
                <template v-if="deletePageStore.pageDeleted && deletePageStore.redirect">
                    {{ t(deletePageStore.isWiki
                        ? 'page.deleteModal.message.redirect.wiki'
                        : 'page.deleteModal.message.redirect.page') }}
                </template>
                <template v-else-if="deletePageStore.pageDeleted && !deletePageStore.redirect">
                    <i18n-t :keypath="deletePageStore.isWiki
                        ? 'page.deleteModal.message.deleted.wiki'
                        : 'page.deleteModal.message.deleted.page'" tag="span">
                        <template #name>
                            <strong>{{ deletePageStore.name }}</strong>
                        </template>
                    </i18n-t>
                </template>
                <template v-else>
                    <div>
                        <div class="body-m">
                            <i18n-t keypath="page.deleteModal.message.confirm" tag="span">
                                <template #name>
                                    <strong>
                                        <NuxtLink :to="$urlHelper.getPageUrl(deletePageStore.name, deletePageStore.id)">
                                            {{ deletePageStore.name }}
                                        </NuxtLink>
                                    </strong>
                                </template>
                            </i18n-t>
                        </div>
                        <div v-if="deletePageStore.hasQuestion" class="new-parent-page-selection-container">
                            <div class="body-s">
                                <strong>{{ t('page.deleteModal.message.questions.notDeleted') }}</strong>
                                {{ t('page.deleteModal.message.questions.moveToAnotherPage') }}
                                <br />
                                <template v-if="!deletePageStore.hasPublicQuestion">
                                    {{ t('page.deleteModal.message.questions.publicQuestions') }}
                                    <br />
                                </template>
                            </div>
                            <div class="body-s" v-if="newParentForQuestions">
                                <i18n-t keypath="page.deleteModal.message.questions.suggestion" tag="span">
                                    <template #name>
                                        <NuxtLink :to="$urlHelper.getPageUrl(newParentForQuestions.name, newParentForQuestions.id)">
                                            {{ newParentForQuestions.name }}
                                        </NuxtLink>
                                    </template>
                                </i18n-t>
                            </div>
                            <div class="form-group dropdown pageSearchAutocomplete" :class="{ 'open': showDropdown }">
                                <div v-if="showSelectedPage && newParentForQuestions != null" class="searchResultItem mb-125" data-toggle="tooltip" data-placement="top" :title="newParentForQuestions.name">
                                    <img :src="newParentForQuestions.imageUrl" />
                                    <div class="searchResultBody">
                                        <div class="searchResultLabel body-m">{{ newParentForQuestions.name }}</div>
                                        <div class="searchResultQuestionCount body-s">
                                            {{ newParentForQuestions.questionCount }}
                                            {{ t('page.deleteModal.question', newParentForQuestions.questionCount) }}
                                        </div>
                                    </div>
                                </div>
                                <div class="body-s">{{ t('page.deleteModal.message.questions.searchOther') }}</div>
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