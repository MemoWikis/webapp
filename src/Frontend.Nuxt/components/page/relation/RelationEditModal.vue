<script lang="ts" setup>
import { useEditPageRelationStore, EditPageRelationType } from './editPageRelationStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { useUserStore } from '~~/components/user/userStore'
import { usePageStore } from '../pageStore'
import { debounce } from 'underscore'
import { FullSearch, PageItem, SearchType } from '~~/components/search/searchHelper'
import { useAiCreatePageStore } from '../content/ai/aiCreatePageStore'
'~~/components/alert/alertStore'

const loadingStore = useLoadingStore()
const userStore = useUserStore()
const editPageRelationStore = useEditPageRelationStore()
const pageStore = usePageStore()
const aiCreatePageStore = useAiCreatePageStore()
const { t, locale } = useI18n()

const name = ref('')
const showErrorMsg = ref(false)
const errorMsg = ref('')
const forbbidenPageName = ref('')
const existingPageUrl = ref('')
const { $logger } = useNuxtApp()

async function validateName() {
    type PageNameValidationResult = {
        name: string
        url?: string
    }

    const result = await $api<FetchResult<PageNameValidationResult>>('/apiVue/PageRelationEdit/ValidateName', {
        method: 'POST', body: { name: name.value }, mode: 'cors', credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success)
        return true
    else if (result.success === false) {
        errorMsg.value = t(result.messageKey)
        forbbidenPageName.value = result.data.name
        if (result.data.url)
            existingPageUrl.value = result.data.url
        showErrorMsg.value = true
        loadingStore.stopLoading()
        return false
    }
}
const privatePageLimitReached = ref(false)
async function addPage() {

    if (!userStore.isLoggedIn) {
        userStore.showLoginModal = true
        return
    }
    loadingStore.startLoading()

    const nameIsValid = await validateName()

    if (!nameIsValid)
        return

    type QuickCreateResult = {
        name: string
        id: number
        cantSavePrivatePage?: boolean
    }

    const pageData = {
        name: name.value,
        parentPageId: editPageRelationStore.parentId,
    }

    const result = await $api<FetchResult<QuickCreateResult>>('/apiVue/PageRelationEdit/QuickCreate', {
        method: 'POST', body: pageData, mode: 'cors', credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
    if (result.success) {
        loadingStore.stopLoading()
        pageStore.childPageCount++
        editPageRelationStore.showModal = false
        editPageRelationStore.addPage(result.data.id)

        // await nextTick()
        if (editPageRelationStore.redirect)
            await navigateTo($urlHelper.getPageUrl(result.data.name, result.data.id))

    } else if (result.success === false) {
        errorMsg.value = t(result.messageKey)
        showErrorMsg.value = true

        if (result.data.cantSavePrivatePage) {
            privatePageLimitReached.value = true
        }
        loadingStore.stopLoading()
    }
}

const disableAddButton = ref(true)

watch(name, (val) => {
    if (val.length <= 0)
        disableAddButton.value = true
    else
        disableAddButton.value = false
})

const selectedPageId = ref(0)
watch(selectedPageId, (id) => {
    if (id > 0 && editPageRelationStore.type != EditPageRelationType.Create || editPageRelationStore.type === EditPageRelationType.Create)
        disableAddButton.value = false
})

const showDropdown = ref(false)
const lockDropdown = ref(false)
const searchTerm = ref('')
const selectedPage = ref(null as PageItem | null)
const showSelectedPage = ref(false)

function selectPage(t: PageItem) {
    showDropdown.value = false
    lockDropdown.value = true
    searchTerm.value = t.name
    selectedPage.value = t
    selectedPageId.value = t.id
    showSelectedPage.value = true
    selectedParentInWikiId.value = t.id
}

const selectedParentInWikiId = ref(0)

const pages = reactive({ value: [] as PageItem[] })
const totalCount = ref(0)

const forbiddenPageName = ref('')

async function movePageToNewParent() {
    loadingStore.startLoading()

    if (selectedPageId.value === editPageRelationStore.parentId) {
        errorMsg.value = t('error.page.loopLink')
        showErrorMsg.value = true
        loadingStore.stopLoading()
        return
    }

    const pageData = {
        childId: editPageRelationStore.childId,
        parentIdToRemove: editPageRelationStore.pageIdToRemove,
        parentIdToAdd: selectedPage.value?.id
    }

    const result = await $api<FetchResult<any>>('/apiVue/PageRelationEdit/MoveChild', {
        body: pageData,
        method: 'POST',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success === true) {
        editPageRelationStore.parentId = selectedPage.value?.id!
        editPageRelationStore.addPage(editPageRelationStore.childId)
        editPageRelationStore.removePage(editPageRelationStore.childId, editPageRelationStore.pageIdToRemove)
        editPageRelationStore.showModal = false
        if (editPageRelationStore.parentId === pageStore.id)
            pageStore.childPageCount++
        if (editPageRelationStore.pageIdToRemove === pageStore.id)
            pageStore.childPageCount--

        loadingStore.stopLoading()
    } else {
        errorMsg.value = t(result.messageKey)
        showErrorMsg.value = true
        loadingStore.stopLoading()
    }
}

const { $urlHelper } = useNuxtApp()

watch(() => editPageRelationStore.type, (val) => {
    if (val === EditPageRelationType.AddToPersonalWiki && editPageRelationStore.personalWiki) {
        selectedPageId.value = editPageRelationStore.personalWiki.id
    }
})

watch(() => editPageRelationStore.personalWiki, (val) => {
    if (val && editPageRelationStore.type === EditPageRelationType.AddToPersonalWiki) {
        selectedPageId.value = editPageRelationStore.personalWiki!.id
    }
})

function getAddChildPayload() {

    if (editPageRelationStore.type === EditPageRelationType.AddChild)
        editPageRelationStore.childId = selectedPageId.value

    else if (editPageRelationStore.type === EditPageRelationType.AddParent || editPageRelationStore.type === EditPageRelationType.AddToPersonalWiki)
        editPageRelationStore.parentId = selectedPageId.value

    return {
        childId: editPageRelationStore.childId,
        parentId: editPageRelationStore.parentId,
    }
}
async function addExistingPage() {
    loadingStore.startLoading()
    const data = getAddChildPayload()

    if (data.childId === data.parentId) {
        errorMsg.value = t('error.page.loopLink')
        showErrorMsg.value = true
        loadingStore.stopLoading()
        return
    }

    if (data.childId <= 0) {
        errorMsg.value = t('error.page.noChildSelected')
        showErrorMsg.value = true
        loadingStore.stopLoading()
        return
    }

    const result = await $api<FetchResult<{ name: string, id: number }>>('/apiVue/PageRelationEdit/AddChild', {
        mode: 'cors',
        method: 'POST',
        body: data,
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })

    if (result.success) {
        if (editPageRelationStore.redirect)
            await navigateTo($urlHelper.getPageUrl(result.data.name, result.data.id))
        editPageRelationStore.showModal = false
        editPageRelationStore.addPage(editPageRelationStore.childId)
        loadingStore.stopLoading()
    } else {
        errorMsg.value = t(result.messageKey)
        showErrorMsg.value = true
        loadingStore.stopLoading()
    }
}


watch(searchTerm, (term) => {
    if (term.length > 0 && lockDropdown.value === false) {
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
        pagesIdToFilter: editPageRelationStore.pagesToFilter
    }

    const url = editPageRelationStore.type === EditPageRelationType.AddToPersonalWiki
        ? '/apiVue/PageRelationEdit/SearchPageInPersonalWiki'
        : '/apiVue/PageRelationEdit/SearchPage'

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
        pages.value = result.pages.filter(t => t.id != editPageRelationStore.parentId)
        totalCount.value = result.pageCount
    }
}

editPageRelationStore.$onAction(({ name, after }) => {
    after(() => {
        if (name === 'initWikiData' && editPageRelationStore.personalWiki) {
            selectedParentInWikiId.value = editPageRelationStore.personalWiki.id
        }
    })
})

const primaryBtnLabel = ref(t('page.relationEdit.button.createPage'))
const setLabel = () => {
    const type = editPageRelationStore.type
    switch (type) {
        case EditPageRelationType.Create:
            primaryBtnLabel.value = t('page.relationEdit.button.createPage')
            break
        case EditPageRelationType.Move:
            primaryBtnLabel.value = t('page.relationEdit.button.movePage')
            break
        case EditPageRelationType.AddChild:
        case EditPageRelationType.AddParent:
            primaryBtnLabel.value = t('page.relationEdit.button.linkPage')
            break
        case EditPageRelationType.AddToPersonalWiki:
            primaryBtnLabel.value = t('page.relationEdit.button.linkPage')
            editPageRelationStore.initWikiData()
            break
    }
}
watch(() => editPageRelationStore.type, () => {
    setLabel()
})

watch(locale, () => {
    setLabel()
})

function handleMainBtn() {
    switch (editPageRelationStore.type) {
        case EditPageRelationType.Create:
            addPage()
            break
        case EditPageRelationType.Move:
            movePageToNewParent()
            break
        case EditPageRelationType.AddChild:
        case EditPageRelationType.AddParent:
        case EditPageRelationType.AddToPersonalWiki:
            addExistingPage()
            break
    }
}

watch(() => editPageRelationStore.showModal, (val) => {
    if (val === false) {
        name.value = ''
        showErrorMsg.value = false
        privatePageLimitReached.value = false
    }
})

function openAiCreatePage() {
    editPageRelationStore.showModal = false
    aiCreatePageStore.openModal(editPageRelationStore.parentId)
}
</script>

<template>
    <LazyModal @close="editPageRelationStore.showModal = false" :show="editPageRelationStore.showModal"
        v-if="editPageRelationStore.showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handleMainBtn()"
        :show-cancel-btn="true">

        <template v-slot:header>
            <h4 v-if="editPageRelationStore.type === EditPageRelationType.Create" class="modal-title">
                {{ t('page.relationEdit.modal.createPage') }}
            </h4>
            <h4 v-else-if="editPageRelationStore.type === EditPageRelationType.Move" class="modal-title">
                {{ t('page.relationEdit.modal.movePage') }}
            </h4>
            <h4 v-else-if="editPageRelationStore.type === EditPageRelationType.AddChild" class="modal-title">
                {{ t('page.relationEdit.modal.linkExistingPage') }}
            </h4>
            <h4 v-else-if="editPageRelationStore.type === EditPageRelationType.AddParent || editPageRelationStore.type === EditPageRelationType.AddToPersonalWiki"
                class="modal-title">
                {{ t('page.relationEdit.modal.linkParentPage') }}
            </h4>
        </template>

        <template v-slot:body>
            <template v-if="editPageRelationStore.type === EditPageRelationType.Create">
                <form v-on:submit.prevent="addPage">
                    <div class="form-group">
                        <input class="form-control create-input" v-model="name"
                            :placeholder="t('page.relationEdit.form.namePlaceholder')" />
                        <small class="form-text text-muted"></small>
                    </div>
                </form>
                <div class="ai-create-option">
                    <button class="btn btn-link ai-create-btn" @click="openAiCreatePage">
                        <font-awesome-icon icon="fa-solid fa-wand-magic-sparkles" />
                        {{ t('page.relationEdit.button.createWithAi') }}
                    </button>
                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :href="existingPageUrl" class="alert-link">{{ forbbidenPageName }}</NuxtLink>
                    {{ errorMsg }}
                </div>
                <div class="link-to-sub-container" v-if="privatePageLimitReached">
                    <NuxtLink to="/Preise" class="btn-link link-to-sub"><b>{{ t('info.joinNow') }}</b></NuxtLink>
                </div>
                <div class="pageIsPrivate" v-else>
                    <p>
                        <b>{{ t('page.relationEdit.text.pageIsPrivate') }}</b>
                    </p>
                </div>
            </template>


            <template v-else-if="editPageRelationStore.type === EditPageRelationType.AddToPersonalWiki">
                <div class="mb-250">
                    <p>{{ t('page.relationEdit.text.whereToAdd') }}</p>
                </div>
                <div>
                    <div class="pageSearchAutocomplete mb-250" v-if="editPageRelationStore.personalWiki != null"
                        @click="selectedParentInWikiId = userStore.personalWiki?.id ?? 0">
                        <div class="searchResultItem"
                            :class="{ 'selectedSearchResultItem': selectedParentInWikiId === editPageRelationStore.personalWiki.id }">
                            <img :src="editPageRelationStore.personalWiki.miniImageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ editPageRelationStore.personalWiki.name }}
                                </div>
                                <div class="searchResultQuestionCount body-s">
                                    {{ editPageRelationStore.personalWiki.questionCount }}
                                    {{ editPageRelationStore.personalWiki.questionCount != 1 ? t('page.relationEdit.text.questions') : t('page.relationEdit.text.question') }}
                                </div>
                            </div>
                            <div v-show="selectedParentInWikiId === editPageRelationStore.personalWiki.id"
                                class="selectedSearchResultItemContainer">
                                <div class="selectedSearchResultItem">
                                    {{ t('page.relationEdit.text.selected') }}
                                    <font-awesome-icon icon="fa-solid fa-check" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="pageSearchAutocomplete mb-250"
                        v-if="editPageRelationStore.recentlyUsedRelationTargetPages != null && editPageRelationStore.recentlyUsedRelationTargetPages.length > 0">
                        <div class="overline-s mb-125 no-line">{{ t('page.relationEdit.text.recentlySelectedPages') }}</div>
                        <template v-for="previousPage in editPageRelationStore.recentlyUsedRelationTargetPages">
                            <div class="searchResultItem"
                                :class="{ 'selectedSearchResultItem': selectedParentInWikiId === previousPage.id }"
                                @click="selectedParentInWikiId = previousPage.id">
                                <img :src="previousPage.imageUrl" />
                                <div class="searchResultBody">
                                    <div class="searchResultLabel body-m">{{ previousPage.name }}</div>
                                    <div class="searchResultQuestionCount body-s">{{ previousPage.questionCount }}
                                        {{ previousPage.questionCount != 1 ? t('page.relationEdit.text.questions') : t('page.relationEdit.text.question') }}
                                    </div>
                                </div>
                                <div v-show="selectedParentInWikiId === previousPage.id" class="selectedSearchResultItemContainer">
                                    <div class="selectedSearchResultItem">
                                        {{ t('page.relationEdit.text.selected') }}
                                        <font-awesome-icon icon="fa-solid fa-check" />
                                    </div>
                                </div>
                            </div>
                        </template>
                    </div>
                    <div class="mb-125">
                        <p>{{ t('page.relationEdit.text.selectOtherPage') }}</p>
                    </div>
                    <div class="form-group dropdown pageSearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedPage && selectedPage != null" class="searchResultItem mb-125" :class="{ 'selectedSearchResultItem': selectedParentInWikiId === selectedPage.id }"
                            @click="selectedParentInWikiId = selectedPage?.id ?? 0"
                            data-toggle="tooltip" data-placement="top" :title="selectedPage?.name">
                            <img :src="selectedPage?.imageUrl" />
                            <div class="searchResultBody">
                                <div class="searchResultLabel body-m">{{ selectedPage?.name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedPage.questionCount }}
                                    {{ selectedPage.questionCount != 1 ? t('page.relationEdit.text.questions') : t('page.relationEdit.text.question') }}</div>
                            </div>
                            <div v-show="selectedParentInWikiId === selectedPage.id" class="selectedSearchResultItemContainer">
                                <div class="selectedSearchResultItem">
                                    {{ t('page.relationEdit.text.selected') }}
                                    <font-awesome-icon icon="fa-solid fa-check" />
                                </div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.pageInWiki" :show-search="true" v-on:select-item="selectPage" :page-ids-to-filter="editPageRelationStore.pagesToFilter" />

                        <div class="swap-type-target">
                            <button @click="editPageRelationStore.type = EditPageRelationType.AddParent">
                                <label>
                                    <div class="checkbox-container">
                                        <input type="checkbox" name="addToParent" class="hidden" />
                                        <font-awesome-icon icon="fa-solid fa-square-check" class="checkbox-icon active" />
                                        <span class="checkbox-label">
                                            {{ t('page.relationEdit.text.searchOnlyInWiki') }}
                                        </span>
                                    </div>
                                </label>
                            </button>
                        </div>

                    </div>


                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :to="existingPageUrl" target="_blank" class="alert-link">{{ forbiddenPageName }}
                    </NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else-if="editPageRelationStore.type === EditPageRelationType.AddParent">
                <div class="mb-250">
                    <p>{{ t('page.relationEdit.text.whereToAdd') }}</p>
                </div>
                <div>
                    <div class="form-group dropdown pageSearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedPage && selectedPage != null" class="searchResultItem mb-125"
                            data-toggle="tooltip" data-placement="top" :title="selectedPage.name">
                            <img :src="selectedPage.imageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedPage.name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedPage.questionCount }}
                                    {{ selectedPage.questionCount != 1 ? t('page.relationEdit.text.questions') : t('page.relationEdit.text.question') }}</div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.page" :show-search="true" v-on:select-item="selectPage" :page-ids-to-filter="editPageRelationStore.pagesToFilter" />

                        <div class="swap-type-target">
                            <button @click="editPageRelationStore.type = EditPageRelationType.AddToPersonalWiki">
                                <label>
                                    <div class="checkbox-container">
                                        <input type="checkbox" name="addToWiki" class="hidden" />
                                        <font-awesome-icon icon="fa-regular fa-square" class="checkbox-icon" />
                                        <span class="checkbox-label">
                                            {{ t('page.relationEdit.text.searchOnlyInWiki') }}
                                        </span>
                                    </div>
                                </label>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :to="existingPageUrl" target="_blank" class="alert-link">{{ forbiddenPageName }}
                    </NuxtLink>
                    {{ errorMsg }}
                </div>
            </template>
            <template v-else>
                <div>
                    <div class="form-group dropdown pageSearchAutocomplete" :class="{ 'open': showDropdown }">
                        <div v-if="showSelectedPage && selectedPage != null" class="searchResultItem mb-125"
                            data-toggle="tooltip" data-placement="top" :title="selectedPage.name">
                            <img :src="selectedPage.imageUrl" />
                            <div>
                                <div class="searchResultLabel body-m">{{ selectedPage.name }}</div>
                                <div class="searchResultQuestionCount body-s">{{ selectedPage.questionCount }}
                                    {{ selectedPage.questionCount != 1 ? t('page.relationEdit.text.questions') : t('page.relationEdit.text.question') }}</div>
                            </div>
                        </div>
                        <Search :search-type="SearchType.page" :show-search="true" v-on:select-item="selectPage" :page-ids-to-filter="editPageRelationStore.pagesToFilter" />
                    </div>
                </div>
                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                    <NuxtLink :to="existingPageUrl" target="_blank" class="alert-link">{{ forbiddenPageName }}
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

.pageSearchAutocomplete {
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