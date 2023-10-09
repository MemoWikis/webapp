<script lang="ts" setup>
import { EditRelationData, EditTopicRelationType, useEditTopicRelationStore } from '~/components/topic/relation/editTopicRelationStore'
import { useUserStore } from '~/components/user/userStore'
import { usePublishTopicStore } from '~/components/topic/publish/publishTopicStore'
import { useAlertStore, AlertType, messages } from '~/components/alert/alertStore'
import { Visibility } from '~/components/shared/visibilityEnum'
import { GridTopicItem } from './gridTopicItem'
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { useTopicToPrivateStore } from '~/components/topic/toPrivate/topicToPrivateStore'
import { useDeleteTopicStore } from '~/components/topic/delete/deleteTopicStore'

const userStore = useUserStore()
const publishTopicStore = usePublishTopicStore()
const editTopicRelationStore = useEditTopicRelationStore()
const alertStore = useAlertStore()
const topicToPrivateStore = useTopicToPrivateStore()
const deleteTopicStore = useDeleteTopicStore()

interface Props {
    topic: GridTopicItem
    parentId: number
    parentName: string
}

const props = defineProps<Props>()

const { $urlHelper } = useNuxtApp()

async function removeParent() {
    const userStore = useUserStore()
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    const data = {
        parentIdToRemove: props.parentId,
        childId: props.topic.id,
    }

    const result = await $fetch<FetchResult<null>>('/apiVue/TopicRelationEdit/RemoveParent', {
        method: 'POST',
        body: data
    })
    if (result) {
        if (result.success == true) {
            alertStore.openAlert(AlertType.Success, {
                text: messages.getByCompositeKey(result.messageKey)
            })

            editTopicRelationStore.removeTopic(data.childId, data.parentIdToRemove)
        }
        else {
            alertStore.openAlert(AlertType.Error, {
                text: messages.getByCompositeKey(result.messageKey)
            })
        }
    }
}

function openMoveTopicModal() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const data = {
        topicIdToRemove: props.parentId,
        childId: props.topic.id,
        editCategoryRelation: EditTopicRelationType.Move,
        categoriesToFilter: [props.parentId, props.topic.id]
    } as EditRelationData

    editTopicRelationStore.openModal(data)
}

const emit = defineEmits(['addTopic'])
const showAllLinkOptions = ref<boolean>(false)
</script>

<template>
    <div class="grid-item-options-container">

        <div class="grid-item-option" v-if="props.topic.parents.length > 1">
            <VDropdown :distance="6">
                <button v-show="props.topic.parents.length > 1" class="" @click.stop>
                    <font-awesome-icon :icon="['fas', 'sitemap']" rotation=180 />
                </button>
                <template #popper>
                    <div class="dropdown-row">
                        <div class="overline-s no-line"> Übergeordnete Themen</div>

                    </div>
                    <template v-for="parent in props.topic.parents">
                        <LazyNuxtLink class="dropdown-row" v-if="parent.id > 0"
                            :to="$urlHelper.getTopicUrl(parent.name, parent.id)">
                            <div class="dropdown-icon">
                                <Image :src="parent.imgUrl" :format="ImageFormat.Topic" class="header-topic-icon" />
                            </div>
                            <div class="dropdown-label">{{ parent.name }}</div>
                        </LazyNuxtLink>
                    </template>
                </template>
            </VDropdown>
        </div>

        <div class="grid-item-option" v-if="props.topic.visibility != Visibility.All">
            <button class="" @click.stop="publishTopicStore.openModal(props.topic.id)">
                <font-awesome-icon :icon="['fas', 'lock']" />
            </button>
        </div>

        <div class="grid-item-option">
            <VDropdown :distance="1">
                <button class="" @click.stop>
                    <font-awesome-icon :icon="['fa-solid', 'ellipsis-vertical']" />
                </button>
                <template #popper="{ hide }">

                    <div @click="emit('addTopic', true); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon :icon="['fas', 'plus']" />
                        </div>
                        <div class="dropdown-label">Unterthema erstellen</div>
                    </div>
                    <div class="divider"></div>
                    <div @click="showAllLinkOptions = !showAllLinkOptions" class="dropdown-row"
                        :class="{ 'extended': showAllLinkOptions }">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            Verknüpfungen bearbeiten
                        </div>
                        <div class="dropdown-icon collapse">
                            <font-awesome-icon :icon="['fas', 'chevron-up']" v-if="showAllLinkOptions" />
                            <font-awesome-icon :icon="['fas', 'chevron-down']" v-else />
                        </div>
                    </div>

                    <div v-if="showAllLinkOptions" class="link-options">
                        <div @click="editTopicRelationStore.addParent(props.topic.id, false); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon icon="fa-solid fa-link" />
                            </div>
                            <div class="dropdown-label">
                                Oberthema verknüpfen
                            </div>
                        </div>

                        <div @click="emit('addTopic', false); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fas', 'link']" />
                            </div>
                            <div class="dropdown-label">Unterthema verknüpfen</div>
                        </div>

                        <div @click="removeParent(); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'link-slash']" />
                            </div>
                            <div class="dropdown-label">'{{ props.parentName }}' <br /> als Oberthema entfernen </div>
                        </div>

                        <div @click="openMoveTopicModal(); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'circle-right']" />
                            </div>
                            <div class="dropdown-label">Thema verschieben</div>
                        </div>
                        <!-- <div @click="editTopicRelationStore.removeFromPersonalWiki(props.topic.id); hide()" class="dropdown-row"
                        v-if="props.topic.isChildOfPersonalWiki">
                        <div class="dropdown-icon">
                            <font-awesome-layers>
                                <font-awesome-icon :icon="['fas', 'house']" />
                                <font-awesome-icon :icon="['fas', 'square']" transform="shrink-2 down-2 right-1" />
                                <font-awesome-icon :icon="['fas', 'minus']" transform="shrink-6 down-1 right-1"
                                    style="color: white;" />
                                <font-awesome-icon :icon="['fas', 'minus']" transform="shrink-6 down-2 right-1"
                                    style="color: white;" />
                            </font-awesome-layers>
                        </div>
                        <div class="dropdown-label">Aus deinem Wiki entfernen</div>
                    </div> -->
                        <div @click="editTopicRelationStore.addToPersonalWiki(props.topic.id); hide()" class="dropdown-row"
                            v-if="!props.topic.isChildOfPersonalWiki && props.topic.id != userStore.personalWiki?.Id">
                            <div class="dropdown-icon">
                                <font-awesome-layers>
                                    <font-awesome-icon :icon="['fas', 'house']" />
                                    <font-awesome-icon :icon="['fas', 'square']" transform="shrink-2 down-2 right-1" />
                                    <font-awesome-icon :icon="['fas', 'plus']" transform="shrink-3 down-1 right-1"
                                        style="color: white;" />
                                </font-awesome-layers>
                            </div>
                            <div class="dropdown-label">Zu deinem Wiki hinzufügen</div>
                        </div>
                    </div>

                    <template v-if="userStore.id == props.topic.creatorId || userStore.isAdmin">
                        <div class="divider"></div>

                        <div v-if="props.topic.visibility == Visibility.All"
                            @click="topicToPrivateStore.openModal(props.topic.id); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'lock']" />
                            </div>
                            <div class="dropdown-label">Thema privat stellen</div>
                        </div>
                        <div v-else @click="publishTopicStore.openModal(props.topic.id); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                            </div>
                            <div class="dropdown-label">Thema veröffentlichen</div>
                        </div>

                        <div @click="deleteTopicStore.openModal(props.topic.id); hide()" data-allowed="logged-in"
                            class="dropdown-row" v-if="props.topic.canDelete">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fas', 'trash']" />
                            </div>
                            <div class="dropdown-label">Thema löschen</div>
                        </div>
                    </template>

                </template>
            </VDropdown>

        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.dropdown-row {
    &.extended {
        margin-bottom: 8px;
    }

    .dropdown-icon {
        &.collapse {
            margin-left: 8px;
        }
    }
}

.link-options {
    .dropdown-row {
        padding-left: 44px;
    }
}

.grid-item-options-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-wrap: nowrap;
    height: 40px;
    padding-right: 4px;
    color: @memo-grey-dark;

    .grid-item-option {
        width: 32px;
        display: flex;
        justify-content: center;
        align-items: center;
        margin-left: -4px;

        button {
            background: white;
            width: 32px;
            height: 32px;
            border-radius: 32px;
            color: @memo-grey-dark;

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }
        }
    }

}
</style>

<style lang="less">
.open-modal {
    .grid-item-options-container {
        .grid-item-option {
            button {
                background: none;
            }
        }
    }
}
</style>