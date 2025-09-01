<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { usePageStore } from '../pageStore'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { useEditPageRelationStore } from '../relation/editPageRelationStore'
import { usePageToPrivateStore } from '../toPrivate/pageToPrivateStore'
import { usePublishPageStore } from '../publish/publishPageStore'
import { useDeletePageStore } from '../delete/deletePageStore'

import { useConvertStore } from '../convert/convertStore'
import { useSharePageStore } from '../sharing/sharePageStore'
import { useUserSkills } from '~~/composables/useUserSkills'

const userStore = useUserStore()
const pageStore = usePageStore()
const editQuestionStore = useEditQuestionStore()
const editPageRelationStore = useEditPageRelationStore()
const publishPageStore = usePublishPageStore()
const deletePageStore = useDeletePageStore()
const pageToPrivateStore = usePageToPrivateStore()
const convertStore = useConvertStore()
const sharePageStore = useSharePageStore()
const { addSkill, removeSkill, checkSkill } = useUserSkills()

const hoverLock = ref(false)
const ariaId = useId()
const isSkill = ref(false)

const { t, localeProperties } = useI18n()

// Check if current page is already a skill
const checkIfSkill = async () => {
    if (userStore.id && pageStore.id) {
        const result = await checkSkill(userStore.id, pageStore.id)
        isSkill.value = result
    }
}

// Check skill status when component mounts
onMounted(checkIfSkill)

// Also check when page changes
watch(() => pageStore.id, checkIfSkill)

const addToSkills = async () => {
    if (!userStore.id || !userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    try {
        const result = await addSkill(pageStore.id)

        if (result.success) {
            // Show success message or toast
            console.log('Page added to skills successfully')
        } else {
            // Show error message
            console.error('Failed to add to skills:', result.errorMessageKey)
            alert(`Error: ${t(result.errorMessageKey)}`)
        }
    } catch (error) {
        console.error('Error adding to skills:', error)
        alert('Failed to add page to skills')
    }
}

const removeFromSkills = async () => {
    if (!userStore.id || !userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    try {
        const result = await removeSkill(userStore.id, pageStore.id)

        if (result.success) {
            // Show success message or toast
            console.log('Page removed from skills successfully')
        } else {
            // Show error message
            console.error('Failed to remove from skills:', result.errorMessageKey)
            alert(`Error: ${t(result.errorMessageKey)}`)
        }
    } catch (error) {
        console.error('Error removing from skills:', error)
        alert('Failed to remove page from skills')
    }
}

</script>

<template>
    <div id="PageHeaderOptions">
        <div>
            <VDropdown :aria-id="ariaId" :distance="0" :popperHideTriggers="(triggers: any) => []" :arrow-padding="300" placement="auto">
                <div class="page-header-options-btn">
                    <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                </div>
                <template #popper="{ hide }">

                    <template v-if="pageStore.visibility != Visibility.Public">
                        <div @click="sharePageStore.openModal(pageStore.id, pageStore.name); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon icon="fa-solid fa-link" />
                            </div>
                            <div class="dropdown-label">
                                {{ t('page.header.sharePage') }}
                            </div>
                        </div>

                        <div class="dropdown-divider"></div>
                    </template>

                    <div @click="pageStore.hideOrShowText()" class="dropdown-row hide-text-option" :class="{ 'page-has-content': pageStore.content?.length > 0 || pageStore.contentHasChanged }">
                        <div class="dropdown-label">
                            {{ t('page.header.noTextInput') }} <font-awesome-icon :icon="['fas', 'circle-info']" class="toggle-info" v-tooltip="t('info.page.toggleHideText')" />
                        </div>
                        <div class="toggle-icon-container">
                            <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="pageStore.textIsHidden" class="toggle-active" />
                            <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="toggle-inactive" />
                        </div>
                    </div>
                    <div class="dropdown-divider"></div>

                    <div @click="editQuestionStore.create(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-circle-plus" />
                        </div>
                        <div class="dropdown-label">{{ t('page.header.addQuestion') }}</div>
                    </div>

                    <div @click="editPageRelationStore.createPage(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-circle-plus" />
                        </div>
                        <div class="dropdown-label">{{ t('page.header.createPage') }}</div>
                    </div>

                    <div @click="editPageRelationStore.addParent(pageStore.id); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            {{ t('page.header.addParentPage') }}
                        </div>
                    </div>

                    <div @click="editPageRelationStore.addChild(pageStore.id); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            {{ t('page.header.addChildPage') }}
                        </div>
                    </div>

                    <div class="dropdown-divider"></div>

                    <div v-if="!pageStore.isChildOfPersonalWiki && pageStore.id != userStore.personalWiki?.id" class="dropdown-row" @click="editPageRelationStore.addToPersonalWiki(pageStore.id); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-layers>
                                <font-awesome-icon :icon="['fas', 'house']" />
                                <font-awesome-icon :icon="['fas', 'square']" transform="shrink-2 down-2 right-1" />
                                <font-awesome-icon :icon="['fas', 'plus']" transform="shrink-3 down-1 right-1" style="color: white;" />
                            </font-awesome-layers>
                        </div>
                        <div class="dropdown-label">
                            {{ t('page.header.addToPersonalWiki') }}
                        </div>
                    </div>

                    <div v-if="isSkill" class="dropdown-row" @click="removeFromSkills(); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-layers>
                                <font-awesome-icon icon="fa-solid fa-circle-plus" />

                            </font-awesome-layers>
                        </div>
                        <div class="dropdown-label">
                            {{ t('page.header.removeFromSkills') }}
                        </div>
                    </div>

                    <div v-else class="dropdown-row" @click="addToSkills(); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-layers>
                                <font-awesome-icon icon="fa-solid fa-circle-plus" />

                            </font-awesome-layers>
                        </div>
                        <div class="dropdown-label">
                            {{ t('page.header.addToSkills') }}
                        </div>
                    </div>

                    <template v-if="pageStore.isOwnerOrAdmin()">
                        <div class="dropdown-divider"></div>

                        <div v-if="pageStore.visibility === Visibility.Public" class="dropdown-row" @click="pageToPrivateStore.openModal(pageStore.id); hide()">
                            <div class="dropdown-icon">
                                <font-awesome-icon icon="fa-solid fa-lock" />
                            </div>
                            <div class="dropdown-label">
                                {{ t('page.header.setToPrivate') }}
                            </div>
                        </div>
                        <div v-else-if="pageStore.visibility === Visibility.Private" class="dropdown-row" @click="publishPageStore.openModal(pageStore.id); hide()">
                            <div class="dropdown-icon">
                                <font-awesome-icon icon="fa-solid fa-unlock" />
                            </div>
                            <div class="dropdown-label">
                                {{ t('page.header.publishPage') }}
                            </div>
                        </div>

                        <div v-if="pageStore.isWiki" class="dropdown-row" @click="convertStore.openModal(pageStore.id); hide()">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fas', 'file']" />
                            </div>
                            <div class="dropdown-label">
                                {{ t('page.header.convertToPage') }}
                            </div>
                        </div>
                        <div v-else class="dropdown-row" @click="convertStore.openModal(pageStore.id); hide()">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fas', 'folder']" />
                            </div>
                            <div class="dropdown-label">
                                {{ t('page.header.convertToWiki') }}
                            </div>
                        </div>

                        <template v-if="pageStore.canBeDeleted">
                            <div class="dropdown-row"
                                @click="deletePageStore.openModal(pageStore.id, true); hide()">

                                <div class="dropdown-icon">
                                    <font-awesome-icon icon="fa-solid fa-trash" />
                                </div>
                                <div class="dropdown-label">
                                    {{ t('page.header.deletePage') }}
                                </div>
                            </div>
                        </template>
                    </template>
                    <div class="dropdown-divider"></div>

                    <div class="dropdown-row no-hover">
                        <div class="dropdown-label content-language-info">
                            {{ t('page.header.contentLanguage') }}:
                            <div>
                                {{ localeProperties.name }} ({{ localeProperties.code }})
                            </div>
                        </div>
                    </div>

                </template>
            </VDropdown>
        </div>
        <div class="lock-btn" v-if="pageStore.visibility === Visibility.Private" @mouseover="hoverLock = true" @mouseleave="hoverLock = false" @click="publishPageStore.openModal(pageStore.id)">
            <font-awesome-icon icon="fa-solid fa-lock" v-show="!hoverLock" />
            <font-awesome-icon icon="fa-solid fa-unlock" v-show="hoverLock" />
        </div>

        <div class="share-btn" v-if="pageStore.visibility != Visibility.Public" @click="sharePageStore.openModal(pageStore.id, pageStore.name)">
            <font-awesome-icon :icon="['fas', 'user-group']" v-if="pageStore.isShared" />
            <font-awesome-icon :icon="['fas', 'user-plus']" v-else />
        </div>

    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#PageHeaderOptions {
    min-width: 30px;
    display: flex;
    flex-direction: row-reverse;

    .page-header-options-btn {
        cursor: pointer;
        background: white;
        border-radius: 24px;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 18px;
        height: 30px;
        width: 30px;
        min-width: 30px;
        transition: filter 0.1s;
        color: @memo-grey-dark;

        &:hover {
            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }

    .lock-btn,
    .share-btn {
        cursor: pointer;
        background: white;
        border-radius: 24px;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 18px;
        height: 30px;
        width: 30px;
        min-width: 30px;
        transition: filter 0.1s;

        &:hover {
            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }
}

li {
    color: @memo-blue-link;

    .dropdown-item {
        cursor: pointer;
        display: flex;
        flex-wrap: nowrap;
    }
}

.hide-text-option {

    // background: @memo-grey-lighter !important;
    justify-content: space-between;

    .dropdown-label {
        padding-left: 0px !important;
    }

    .toggle-info {
        margin-left: 4px;
        color: @memo-grey-light;
        outline: none;
    }

    .toggle-icon-container {
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 18px;

        .toggle-active {
            color: @memo-blue-link;
        }

        .toggle-inactive {
            color: @memo-grey-dark;
        }
    }

    &.page-has-content {
        color: @memo-grey-dark;
        user-select: none;
        pointer-events: none;

        .toggle-inactive {
            color: @memo-grey-light;
        }

        .toggle-info {
            pointer-events: all;
        }

        &:hover {
            color: @memo-grey-dark;
            filter: none;
        }

        &:active {
            filter: none;
        }
    }
}

.dropdown-divider {
    height: 1px;
    background: @memo-grey-lighter;
    margin: 10px 0px;
}

.content-language-info {
    font-size: 1.25rem;
    color: @memo-grey;

}
</style>