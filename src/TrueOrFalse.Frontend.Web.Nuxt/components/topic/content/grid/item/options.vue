<script lang="ts" setup>
import { EditRelationData, EditTopicRelationType, useEditTopicRelationStore } from '~/components/topic/relation/editTopicRelationStore'
import { useUserStore } from '~/components/user/userStore'
import { usePublishTopicStore } from '~/components/topic/publish/publishTopicStore'
import { useAlertStore, AlertType, messages } from '~/components/alert/alertStore'
import { Visibility } from '~/components/shared/visibilityEnum'
import { GridTopicItem } from './gridTopicItem'
import { ImageFormat } from '~/components/image/imageFormatEnum'

const userStore = useUserStore()
const publishTopicStore = usePublishTopicStore()
const editTopicRelationStore = useEditTopicRelationStore()
const alertStore = useAlertStore()

interface Props {
    topic: GridTopicItem
    parentId: number
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

    const result = await $fetch<any>('/apiVue/TopicRelationEdit/RemoveParent', {
        method: 'POST',
        body: data
    })
    if (result) {
        if (result.success == true) {
            alertStore.openAlert(AlertType.Success, {
                text: messages.success.category[result.key]
            })
        }
        else {
            alertStore.openAlert(AlertType.Error, {
                text: messages.error.category[result.key]
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
    } as EditRelationData

    editTopicRelationStore.openModal(data)
}

function openAddToWikiModal() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    const data = {
        parentId: props.topic.id,
        editCategoryRelation: EditTopicRelationType.AddToPersonalWiki
    } as EditRelationData

    editTopicRelationStore.openModal(data)
}

</script>

<template>
    <div class="grid-item-options-container">

        <div class="grid-item-option">
            <VDropdown :distance="6">
                <button v-show="props.topic.parents.length > 1" class="">
                    <font-awesome-icon :icon="['fas', 'sitemap']" rotation=180 />
                </button>
                <template #popper>
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

        <div class="grid-item-option">
            <font-awesome-icon :icon="['fas', 'lock']" v-show="props.topic.visibility != Visibility.All" />
        </div>

        <div class="grid-item-option">
            <VDropdown :distance="1">
                <button class="">
                    <font-awesome-icon :icon="['fa-solid', 'ellipsis-vertical']" />
                </button>
                <template #popper="{ hide }">
                    <div @click="removeParent(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon :icon="['fa-solid', 'link-slash']" />
                        </div>
                        <div class="dropdown-label">Verknüpfung entfernen </div>
                    </div>
                    <div v-if="props.topic.visibility != Visibility.All"
                        @click="publishTopicStore.openModal(props.topic.id); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                        </div>
                        <div class="dropdown-label">Thema veröffentlichen</div>
                    </div>
                    <div @click="openMoveTopicModal(); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon :icon="['fa-solid', 'circle-right']" />
                        </div>
                        <div class="dropdown-label">Thema verschieben</div>
                    </div>
                    <div @click="openAddToWikiModal(); hide()" data-allowed="logged-in" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon :icon="['fa-solid', 'plus']" />
                        </div>
                        <div class="dropdown-label">Zu meinem Wiki hinzufügen</div>
                    </div>
                </template>
            </VDropdown>

        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-item-options-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-wrap: nowrap;

    .grid-item-option {
        width: 34px;

        button {
            background: white;
            width: 34px;
            height: 34px;
            border-radius: 36px;
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