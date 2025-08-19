<script lang="ts" setup>
import { useUserStore } from '~/components/user/userStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { PageItem, SearchType } from '~~/components/search/searchHelper'

const props = defineProps<{
    show: boolean
    userId: number
}>()

const emit = defineEmits<{
    close: []
    skillAdded: [skillId: number]
}>()

const { t } = useI18n()
const userStore = useUserStore()
const loadingStore = useLoadingStore()
const { addSkill } = useUserSkills()

const selectedPage = ref(null as PageItem | null)
const selectedPageId = ref(0)
const showSelectedPage = ref(false)
const showErrorMsg = ref(false)
const errorMsg = ref('')
const disableAddButton = ref(true)

function selectPage(page: PageItem) {
    selectedPage.value = page
    selectedPageId.value = page.id
    showSelectedPage.value = true
    disableAddButton.value = false
}

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
    selectedPage.value = null
    selectedPageId.value = 0
    showSelectedPage.value = false
    showErrorMsg.value = false
    errorMsg.value = ''
    disableAddButton.value = true
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

            <div class="form-group dropdown pageSearchAutocomplete">
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

                <Search :search-type="SearchType.page" :show-search="true" v-on:select-item="selectPage" :page-ids-to-filter="[]" />
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
