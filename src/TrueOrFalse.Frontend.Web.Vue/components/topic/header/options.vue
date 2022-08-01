<script lang="ts" setup>
import { ref } from 'vue';
import { useUserStore } from '~~/components/user/userStore';
import { useTopicStore } from '../topicStore';
const userStore = useUserStore()
const topicStore = useTopicStore()
</script>

<template>
    <VDropdown :distance="0">
        <font-awesome-icon icon="fa-solid fa-ellipsis-vertical" />
        <template #popper>
            <!-- <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
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
                    <a onclick="eventBus.$emit('create-category', <%= Model.Category.Id %>)" data-allowed="logged-in">
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
                    <a onclick="eventBus.$emit('add-to-wiki', <%= Model.Category.Id %>)" data-allowed="logged-in">
                        <div class="dropdown-icon">
                            <i class="fa fa-plus-square"></i>
                        </div>
                        Zu meinem Wiki hinzufügen
                    </a>
                </li>

                <% if (Model.IsOwnerOrAdmin && Model.Category.Visibility==CategoryVisibility.All) { %>
                    <li>
                        <a onclick="eventBus.$emit('set-category-to-private', <%= Model.Category.Id %>)"
                            data-allowed="logged-in">
                            <div class="dropdown-icon">
                                <i class="fas fa-lock"></i>
                            </div>
                            Thema auf privat setzen
                        </a>
                    </li>
                    <% } %>
                        <% if (Model.IsOwnerOrAdmin && Model.Category.Visibility==CategoryVisibility.Owner) { %>
                            <li>
                                <a onclick="eventBus.$emit('open-publish-category-modal', <%= Model.Category.Id %>)"
                                    data-allowed="logged-in">
                                    <div class="dropdown-icon">
                                        <i class="fas fa-unlock"></i>
                                    </div>
                                    Thema veröffentlichen
                                </a>
                            </li>
                            <% } %>
                                <% if (PermissionCheck.CanDelete(Model.Category)) { %>
                                    <li>
                                        <a onclick="eventBus.$emit('open-delete-category-modal', <%= Model.Category.Id %>)"
                                            data-allowed="logged-in">
                                            <div class="dropdown-icon">
                                                <i class="fas fa-trash"></i>
                                            </div>
                                            Thema löschen
                                        </a>
                                    </li>
                                    <% } %>

            </ul> -->
        </template>
    </VDropdown>
</template>