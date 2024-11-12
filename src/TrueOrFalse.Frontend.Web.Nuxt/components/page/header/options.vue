<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { usePageStore } from '../pageStore'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { useEditPageRelationStore } from '../relation/editPageRelationStore'
import { usePageToPrivateStore } from '../toPrivate/pageToPrivateStore'
import { usePublishPageStore } from '../publish/publishPageStore'
import { useDeletePageStore } from '../delete/deletePageStore'
import { messages } from '~/components/alert/messages'

const userStore = useUserStore()
const pageStore = usePageStore()
const editQuestionStore = useEditQuestionStore()
const editPageRelationStore = useEditPageRelationStore()
const publishPageStore = usePublishPageStore()
const deletePageStore = useDeletePageStore()
const pageToPrivateStore = usePageToPrivateStore()

const hoverLock = ref(false)
const ariaId = useId()

</script>

<template>
    <div id="PageHeaderOptions">
        <div>
            <VDropdown :aria-id="ariaId" :distance="0" :popperHideTriggers="(triggers: any) => []" :arrow-padding="300" placement="auto">
                <div class="page-header-options-btn">
                    <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                </div>
                <template #popper="{ hide }">

                    <div @click="pageStore.hideOrShowText()" class="dropdown-row hide-text-option"
                        :class="{ 'page-has-content': pageStore.content?.length > 0 || pageStore.contentHasChanged }">
                        <div class="dropdown-label">
                            Keine Texteingabe <font-awesome-icon :icon="['fas', 'circle-info']" class="toggle-info"
                                v-tooltip="messages.info.page.toggleHideText" />
                        </div>
                        <div class="toggle-icon-container">
                            <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="pageStore.textIsHidden"
                                class="toggle-active" />
                            <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="toggle-inactive" />
                        </div>
                    </div>
                    <div class="dropdown-divider"></div>

                    <div @click="editQuestionStore.create(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-circle-plus" />
                        </div>
                        <div class="dropdown-label">Frage hinzufügen</div>
                    </div>

                    <div @click="editPageRelationStore.createPage(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-circle-plus" />
                        </div>
                        <div class="dropdown-label">Seite erstellen</div>
                    </div>

                    <div @click="editPageRelationStore.addParent(pageStore.id); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            Übergeordnete Seite verknüpfen
                        </div>
                    </div>

                    <div @click="editPageRelationStore.addChild(pageStore.id); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            Unterseite verknüpfen
                        </div>
                    </div>

                    <div v-if="!pageStore.isChildOfPersonalWiki && pageStore.id != userStore.personalWiki?.id"
                        class="dropdown-row" @click="editPageRelationStore.addToPersonalWiki(pageStore.id); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-layers>
                                <font-awesome-icon :icon="['fas', 'house']" />
                                <font-awesome-icon :icon="['fas', 'square']" transform="shrink-2 down-2 right-1" />
                                <font-awesome-icon :icon="['fas', 'plus']" transform="shrink-3 down-1 right-1"
                                    style="color: white;" />
                            </font-awesome-layers>
                        </div>
                        <div class="dropdown-label">
                            Zu meinem Wiki hinzufügen
                        </div>
                    </div>

                    <div v-if="pageStore.isOwnerOrAdmin() && pageStore.visibility == Visibility.All"
                        class="dropdown-row" @click="pageToPrivateStore.openModal(pageStore.id); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-lock" />
                        </div>
                        <div class="dropdown-label">
                            Seite auf privat setzen
                        </div>
                    </div>
                    <div v-else-if="pageStore.isOwnerOrAdmin() && pageStore.visibility == Visibility.Owner"
                        class="dropdown-row" @click="publishPageStore.openModal(pageStore.id); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-unlock" />
                        </div>
                        <div class="dropdown-label">
                            Seite veröffentlichen
                        </div>
                    </div>
                    <div v-if="pageStore.canBeDeleted" class="dropdown-row"
                        @click="deletePageStore.openModal(pageStore.id, true); hide()">

                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-trash" />
                        </div>
                        <div class="dropdown-label">
                            Seite löschen
                        </div>
                    </div>
                </template>
            </VDropdown>
        </div>
        <div class="lock-btn" v-if="pageStore.visibility == Visibility.Owner" @mouseover="hoverLock = true" @mouseleave="hoverLock = false" @click="publishPageStore.openModal(pageStore.id)">
            <font-awesome-icon icon="fa-solid fa-lock" v-show="!hoverLock" />
            <font-awesome-icon icon="fa-solid fa-unlock" v-show="hoverLock" />
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

    .lock-btn {
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
</style>