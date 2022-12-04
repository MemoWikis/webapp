<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { useTopicStore } from '../topicStore'
import { Visibility } from '~~/components/shared/visibilityEnum'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { useEditTopicRelationStore } from '../relation/editTopicRelationStore'

const userStore = useUserStore()
const topicStore = useTopicStore()
const editQuestionStore = useEditQuestionStore()
const editTopicRelationStore = useEditTopicRelationStore()
</script>

<template>
    <div id="TopicHeaderOptions">
        <div>
            <V-Dropdown :distance="0" :popperHideTriggers="(triggers: any) => [...triggers, 'click']">
                <div class="topic-header-options-btn">
                    <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
                </div>
                <template #popper>
                    <ul>
                        <li>
                            <NuxtLink :to="`/history/topic/${topicStore.id}`">
                                <div class="dropdown-icon">
                                    <font-awesome-icon icon="fa-solid fa-history" />
                                </div>
                                Bearbeitungshistorie
                            </NuxtLink>
                        </li>
                        <li>
                            <div @click="editQuestionStore.create()" class="dropdown-item">
                                <div class="dropdown-icon">
                                    <font-awesome-icon icon="fa-solid fa-circle-plus" />
                                </div>
                                Frage hinzufügen
                            </div>
                        </li>
                        <li>
                            <div @click="editTopicRelationStore.createTopic()" class="dropdown-item">
                                <div class="dropdown-icon">
                                    <font-awesome-icon icon="fa-solid fa-circle-plus" />
                                </div>
                                Thema Erstellen
                            </div>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('add-parent-category', <%= Model.Category.Id %>)"
                                data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-link"></i>
                                </div>
                                Oberthema verknüpfen
                            </a>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('add-child-category', <%= Model.Category.Id %>)"
                                data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-link"></i>
                                </div>
                                Unterthema verknüpfen
                            </a>
                        </li>
                        <li v-if="userStore.isLoggedIn && topicStore.id != userStore.wikiId">
                            <a onclick="eventBus.$emit('add-to-wiki', <%= Model.Category.Id %>)"
                                data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-square"></i>
                                </div>
                                Zu meinem Wiki hinzufügen
                            </a>
                        </li>

                        <li v-if="topicStore.isOwnerOrAdmin() && topicStore.visibility == Visibility.All">
                            <a onclick="eventBus.$emit('set-category-to-private', <%= Model.Category.Id %>)"
                                data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fas fa-lock"></i>
                                </div>
                                Thema auf privat setzen
                            </a>
                        </li>

                        <li v-if="topicStore.isOwnerOrAdmin() && topicStore.visibility == Visibility.Owner">
                            <a onclick="eventBus.$emit('open-publish-category-modal', <%= Model.Category.Id %>)"
                                data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fas fa-unlock"></i>
                                </div>
                                Thema veröffentlichen
                            </a>
                        </li>
                        <li v-if="topicStore.canBeDeleted">
                            <a onclick="eventBus.$emit('open-delete-category-modal', <%= Model.Category.Id %>)"
                                data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fas fa-trash"></i>
                                </div>
                                Thema löschen
                            </a>
                        </li>
                    </ul>

                </template>
            </V-Dropdown>
        </div>
        <div v-if="topicStore.visibility == Visibility.Owner">
            <font-awesome-icon icon="fa-solid fa-lock" />
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
</style>