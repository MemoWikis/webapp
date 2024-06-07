<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { useTopicStore } from '../topicStore'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { useEditTopicRelationStore } from '../relation/editTopicRelationStore'
import { useTopicToPrivateStore } from '../toPrivate/topicToPrivateStore'
import { usePublishTopicStore } from '../publish/publishTopicStore'
import { useDeleteTopicStore } from '../delete/deleteTopicStore'
import { messages } from '~/components/alert/messages'

const userStore = useUserStore()
const topicStore = useTopicStore()
const editQuestionStore = useEditQuestionStore()
const editTopicRelationStore = useEditTopicRelationStore()
const publishTopicStore = usePublishTopicStore()
const deleteTopicStore = useDeleteTopicStore()
const topicToPrivateStore = useTopicToPrivateStore()

const hoverLock = ref(false)
</script>

<template>
    <div id="TopicHeaderOptions">
        <div>
            <VDropdown :distance="0" :popperHideTriggers="(triggers: any) => []" :arrow-padding="300" placement="auto">
                <div class="topic-header-options-btn">
                    <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                </div>
                <template #popper="{ hide }">

                    <div @click="topicStore.hideOrShowText()" class="dropdown-row hide-text-option">
                        <div class="dropdown-label">
                            Keine Texteingabe <font-awesome-icon :icon="['fas', 'circle-info']" class="toggle-info"
                                v-tooltip="messages.info.category.toggleHideText" />
                        </div>
                        <div class="toggle-icon-container">
                            <font-awesome-icon :icon="['fas', 'toggle-on']" v-if="topicStore.textIsHidden"
                                class="toggle-active" />
                            <font-awesome-icon :icon="['fas', 'toggle-off']" v-else class="toggle-inactive" />
                        </div>
                    </div>
                    <div class="dropdown-divider"></div>

                    <NuxtLink :to="`/Historie/Thema/${topicStore.id}`" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-history" />
                        </div>
                        <div class="dropdown-label">Bearbeitungshistorie</div>
                    </NuxtLink>

                    <div @click="editQuestionStore.create(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-circle-plus" />
                        </div>
                        <div class="dropdown-label">Frage hinzufügen</div>
                    </div>

                    <div @click="editTopicRelationStore.createTopic(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-circle-plus" />
                        </div>
                        <div class="dropdown-label">Thema erstellen</div>
                    </div>

                    <div @click="editTopicRelationStore.addParent(topicStore.id); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            Oberthema verknüpfen
                        </div>
                    </div>

                    <div @click="editTopicRelationStore.addChild(topicStore.id); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            Unterthema verknüpfen
                        </div>
                    </div>

                    <div v-if="!topicStore.isChildOfPersonalWiki && topicStore.id != userStore.personalWiki?.id"
                        class="dropdown-row" @click="editTopicRelationStore.addToPersonalWiki(topicStore.id); hide()">
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

                    <div v-if="topicStore.isOwnerOrAdmin() && topicStore.visibility == Visibility.All"
                        class="dropdown-row" @click="topicToPrivateStore.openModal(topicStore.id); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-lock" />
                        </div>
                        <div class="dropdown-label">
                            Thema auf privat setzen
                        </div>
                    </div>
                    <div v-else-if="topicStore.isOwnerOrAdmin() && topicStore.visibility == Visibility.Owner"
                        class="dropdown-row" @click="publishTopicStore.openModal(topicStore.id); hide()">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-unlock" />
                        </div>
                        <div class="dropdown-label">
                            Thema veröffentlichen
                        </div>
                    </div>
                    <div v-if="topicStore.canBeDeleted" class="dropdown-row"
                        @click="deleteTopicStore.openModal(topicStore.id, true); hide()">

                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-trash" />
                        </div>
                        <div class="dropdown-label">
                            Thema löschen
                        </div>
                    </div>
                </template>
            </VDropdown>
        </div>
        <div class="lock-btn" v-if="topicStore.visibility == Visibility.Owner" @mouseover="hoverLock = true"
            @mouseleave="hoverLock = false" @click="publishTopicStore.openModal(topicStore.id)">
            <font-awesome-icon icon="fa-solid fa-lock" v-show="!hoverLock" />
            <font-awesome-icon icon="fa-solid fa-unlock" v-show="hoverLock" />
        </div>

    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#TopicHeaderOptions {
    min-width: 30px;
    display: flex;
    flex-direction: row-reverse;

    .topic-header-options-btn {
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
}

.dropdown-divider {
    height: 1px;
    background: @memo-grey-lighter;
    margin: 10px 0px;
}
</style>