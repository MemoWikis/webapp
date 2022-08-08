<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore';
import { useTopicStore } from '../topicStore';
import { Visibility } from '~~/components/shared/visibilityEnum';

const userStore = useUserStore()
const topicStore = useTopicStore()
</script>

<template>
    <div id="TopicHeaderOptions">
        <div>
            <V-Dropdown :distance="0">
                <div class="topic-header-options-btn">
                    <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />

                </div>
                <template #popper>
                    <ul>
                        <li>
                            <a href="<%= Links.CategoryHistory(Model.Id) %>">
                                <div class="dropdown-icon">
                                    <i class="fas fa-history"></i>
                                </div>
                                Bearbeitungshistorie
                            </a>
                        </li>
                        <li>
                            <a
                                onclick="eventBus.$emit('open-edit-question-modal', { categoryId: <%= Model.Category.Id %>, edit: false })">
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-circle"></i>
                                </div>
                                Frage hinzufügen
                            </a>
                        </li>
                        <li>
                            <a onclick="eventBus.$emit('create-category', <%= Model.Category.Id %>)"
                                data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-circle"></i>
                                </div>
                                Thema Erstellen
                            </a>
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
</style>