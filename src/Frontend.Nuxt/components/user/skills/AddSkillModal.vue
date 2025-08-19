<script lang="ts" setup>
import { debounce } from 'underscore'
import { useUserStore } from '~/components/user/userStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { PageItem } from '~~/components/search/searchHelper'

const props = defineProps<{
    show: boolean
    userId: number
}>()

const emit = defineEmits<{
    close: []
    skillAdded: [skillId: number]
}>()

const { t } = useI18n()
const { $logger } = useNuxtApp()
const userStore = useUserStore()
const loadingStore = useLoadingStore()
const { addSkill } = useUserSkills()

const searchTerm = ref('')
const selectedPage = ref(null as PageItem | null)
const selectedPageId = ref(0)
const showDropdown = ref(false)
const lockDropdown = ref(false)
const showSelectedPage = ref(false)
const showErrorMsg = ref(false)
const errorMsg = ref('')
const disableAddButton = ref(true)

// Search results
const pages = reactive({ value: [] as PageItem[] })
const totalCount = ref(0)

function selectPage(page: PageItem) {
    showDropdown.value = false
    lockDropdown.value = true
    searchTerm.value = page.name
    selectedPage.value = page
    selectedPageId.value = page.id
    showSelectedPage.value = true
    disableAddButton.value = false
}

async function search() {
    if (searchTerm.value.length === 0) {
        pages.value = []
        showDropdown.value = false
        return
    }

    showDropdown.value = true

    const data = {
        term: searchTerm.value,
        pageIdsToFilter: []
    }

    try {
        const result = await $api<any>('/apiVue/Search/Page', {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
            onResponseError(context) {
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
            },
        })

        if (result != null) {
            pages.value = result.pages || []
            totalCount.value = result.totalCount || 0
        }
    } catch (error) {
        $logger.error('Search error:', error)
        pages.value = []
    }
}

const debounceSearch = debounce(() => {
    search()
}, 500)

watch(searchTerm, (term) => {
    if (term.length > 0 && lockDropdown.value === false) {
        showDropdown.value = true
        debounceSearch()
    } else {
        showDropdown.value = false
    }
})

watch(selectedPageId, (id) => {
    disableAddButton.value = id <= 0
})

async function handleAddSkill() {
    if (!userStore.isLoggedIn) {
        userStore.showLoginModal = true
        return
    }

    if (selectedPageId.value <= 0) {
        errorMsg.value = t('user.skills.error.noPageSelected')
        showErrorMsg.value = true
        return
    }

    loadingStore.startLoading()

    try {
        const result = await addSkill(props.userId, selectedPageId.value)

        if (result.success) {
            emit('skillAdded', selectedPageId.value)
            emit('close')
            resetForm()
        } else {
            errorMsg.value = t(result.errorMessageKey)
            showErrorMsg.value = true
        }
    } catch (error) {
        console.error('Error adding skill:', error)
        errorMsg.value = t('user.skills.error.addFailed')
        showErrorMsg.value = true
    } finally {
        loadingStore.stopLoading()
    }
}

function resetForm() {
    searchTerm.value = ''
    selectedPage.value = null
    selectedPageId.value = 0
    showDropdown.value = false
    lockDropdown.value = false
    showSelectedPage.value = false
    showErrorMsg.value = false
    errorMsg.value = ''
    disableAddButton.value = true
    pages.value = []
}

function handleClose() {
    resetForm()
    emit('close')
}

watch(() => props.show, (show) => {
    if (!show) {
        resetForm()
    }
})
</script>

<template>
    <LazyModal
        @close="handleClose"
        :show="show"
        v-if="show"
        :primary-btn-label="t('user.skills.addSkillModal.addButton')"
        @primary-btn="handleAddSkill()"
        :show-cancel-btn="true"
        :disable-primary-btn="disableAddButton">

        <template v-slot:header>
            <h4 class="modal-title">
                {{ t('user.skills.addSkillModal.title') }}
            </h4>
        </template>

        <template v-slot:body>
            <div class="mb-250">
                <p>{{ t('user.skills.addSkillModal.searchLabel') }}</p>
            </div>

            <div class="form-group dropdown pageSearchAutocomplete" :class="{ 'open': showDropdown }">
                <div v-if="showSelectedPage && selectedPage != null" class="searchResultItem mb-125"
                    data-toggle="tooltip" data-placement="top" :title="selectedPage.name">
                    <img :src="selectedPage.imageUrl" />
                    <div class="searchResultBody">
                        <div class="searchResultLabel body-m">{{ selectedPage.name }}</div>
                        <div class="searchResultQuestionCount body-s">
                            {{ selectedPage.questionCount }}
                            {{ selectedPage.questionCount != 1 ? t('user.skills.questions') : t('user.skills.question') }}
                        </div>
                    </div>
                </div>

                <input
                    v-model="searchTerm"
                    type="text"
                    class="form-control"
                    :placeholder="t('user.skills.addSkillModal.searchPlaceholder')"
                    @focus="lockDropdown = false" />

                <!-- Search dropdown results -->
                <div v-if="showDropdown && !showSelectedPage" class="dropdown-menu" style="display: block; position: relative; width: 100%; box-shadow: none; border: 1px solid #ddd; margin-top: 0;">
                    <div v-if="pages.value.length === 0" class="dropdown-item-text text-center text-muted">
                        {{ t('user.skills.addSkillModal.noResults') }}
                    </div>
                    <div
                        v-for="page in pages.value"
                        :key="page.id"
                        class="searchResultItem"
                        @click="selectPage(page)">
                        <img :src="page.imageUrl" alt="" />
                        <div class="searchResultBody">
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ page.name }}</div>
                            </div>
                            <div class="searchResultQuestionCount body-s">
                                {{ page.questionCount }}
                                {{ page.questionCount != 1 ? t('user.skills.questions') : t('user.skills.question') }}
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Error message -->
            <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                {{ errorMsg }}
            </div>
        </template>

    </LazyModal>
</template>

<style lang="less" scoped>
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.pageSearchAutocomplete {
    .form-control {
        border-radius: 24px;
    }
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

.mb-125 {
    margin-bottom: 12.5px;
}

.mb-250 {
    margin-bottom: 25px;
}
</style>
