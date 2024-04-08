<script lang="ts" setup>
import { useTopicStore } from '~/components/topic/topicStore'
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useRootTopicChipStore } from '~/components/header/rootTopicChipStore'
import { TopicItem } from '~/components/search/searchHelper'
import { EditRelationData, EditTopicRelationType, useEditTopicRelationStore } from '~/components/topic/relation/editTopicRelationStore'
import { useUserStore } from '~/components/user/userStore'
import { AlertType, messages, useAlertStore } from '~/components/alert/alertStore'
import { usePublishTopicStore } from '~/components/topic/publish/publishTopicStore'
import { useTopicToPrivateStore } from '~/components/topic/toPrivate/topicToPrivateStore'
import { useDeleteTopicStore } from '~/components/topic/delete/deleteTopicStore'
import { useDragStore } from '~/components/shared/dragStore'

const topicStore = useTopicStore()
const rootTopicChipStore = useRootTopicChipStore()
const editTopicRelationStore = useEditTopicRelationStore()
const userStore = useUserStore()
const alertStore = useAlertStore()
const publishTopicStore = usePublishTopicStore()
const topicToPrivateStore = useTopicToPrivateStore()
const deleteTopicStore = useDeleteTopicStore()
const dragStore = useDragStore()

interface Props {
    children: GridTopicItem[]
}
const props = defineProps<Props>()

const toggleState = ref(ToggleState.Collapsed)
const { $urlHelper } = useNuxtApp()

const rootTopicItem = ref<TopicItem>()

onMounted(() => {
    rootTopicItem.value = {
        type: 'TopicItem',
        id: rootTopicChipStore.id,
        name: rootTopicChipStore.name,
        url: $urlHelper.getTopicUrl(rootTopicChipStore.name, rootTopicChipStore.id),
        questionCount: 0,
        imageUrl: rootTopicChipStore.imgUrl,
        miniImageUrl: rootTopicChipStore.imgUrl,
        visibility: 0,
    }
})

const topicsToFilter = computed<number[]>(() => {

    let topicsToFilter = process.server ? props.children.map(c => c.id) : topicStore.gridItems.map(c => c.id)
    topicsToFilter.push(topicStore.id)

    return topicsToFilter
})

function addTopic(newTopic: boolean) {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const parent: EditRelationData = {
        parentId: topicStore.id,
        editCategoryRelation: newTopic
            ? EditTopicRelationType.Create
            : EditTopicRelationType.AddChild,
        categoriesToFilter: topicsToFilter.value,
    }
    editTopicRelationStore.openModal(parent)
}

editTopicRelationStore.$onAction(({ after, name }) => {
    if (name == 'addTopic') {
        after((result) => {
            if (result.parentId == topicStore.id) {
                addGridItem(result.childId)
            } else if (topicStore.gridItems.some(c => c.id == result.parentId)) {
                reloadGridItem(result.parentId)
            }

        })
    }
    if (name == 'removeTopic') {
        after((result) => {
            if (result.parentId == topicStore.id) {
                removeGridItem(result.childId)
            }
        })
    }
    if (name == 'addToPersonalWiki' || name == 'removeFromPersonalWiki') {
        after((result) => {
            if (result) {
                reloadGridItem(result.id)
            }
        })
    }
})

publishTopicStore.$onAction(({ after, name }) => {
    if (name == 'publish') {
        after((result) => {
            if (result?.success && result.id && topicStore.gridItems.some(c => c.id == result.id))
                reloadGridItem(result.id)
        })
    }
})

topicToPrivateStore.$onAction(({ after, name }) => {
    if (name == 'setToPrivate') {
        after((result) => {
            if (result?.success && result.id && topicStore.gridItems.some(c => c.id == result.id))
                reloadGridItem(result.id)
        })
    }
})

deleteTopicStore.$onAction(({ after, name }) => {
    if (name == 'deleteTopic') {
        after((result) => {
            if (result && result.id && topicStore.gridItems.some(c => c.id == result.id)) {
                removeGridItem(result.id)
            }
        })
    }
})

async function addGridItem(id: number) {
    const result = await loadGridItem(id)

    if (result.success == true) {
        topicStore.gridItems.push(result.data)
    } else if (result.success == false)
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
}

async function loadGridItem(id: number) {
    const result = await $fetch<FetchResult<GridTopicItem>>(`/apiVue/Grid/GetItem/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })
    return result
}

async function reloadGridItem(id: number) {
    const result = await loadGridItem(id)
    if (result.success == true) {
        topicStore.gridItems = topicStore.gridItems.map(i => i.id === result.data.id ? result.data : i)
    } else if (result.success == false)
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
}

function removeGridItem(id: number) {
    const filteredGridItems = topicStore.gridItems.filter(i => i.id != id)
    topicStore.gridItems = filteredGridItems
}

const { isMobile, isDesktop } = useDevice()

editTopicRelationStore.$onAction(({ name, after }) => {
    if (name == 'moveTopic') {

        after(async (result) => {
            if (result) {
                const parentHasChanged = result.oldParentId != result.newParentId

                if (props.children.find(c => c.id == result.oldParentId))
                    reloadGridItem(result.oldParentId)
                if (props.children.find(c => c.id == result.newParentId) && parentHasChanged)
                    reloadGridItem(result.newParentId)
            }
        })
    }
})

</script>

<template>
    <div class="row grid-row" id="TopicGrid">
        <div class="col-xs-12">
            <div class="grid-container">
                <div class="grid-header ">
                    <div class="grid-title no-line" :class="{ 'overline-m': !isMobile, 'overline-s': isMobile }">
                        {{ isMobile ? 'Unterthemen' : 'Untergeordnete Themen' }} ({{ topicStore.childTopicCount }})
                    </div>

                    <div class="grid-options">
                        <div class="grid-divider"></div>
                        <div class="grid-option">
                            <button @click="addTopic(true)">
                                <font-awesome-icon :icon="['fas', 'plus']" />
                            </button>
                        </div>
                        <!-- <div class="grid-divider"></div> -->
                        <div class="grid-option">
                            <button @click="addTopic(false)">
                                <font-awesome-icon :icon="['fas', 'link']" />
                            </button>
                        </div>

                        <template v-if="rootTopicChipStore.showRootTopicChip && rootTopicItem">
                            <div class="grid-divider"></div>
                            <div class="root-chip grid-option">
                                <TopicChip :topic="rootTopicItem" class="no-margin" :hide-label="isMobile" />
                            </div>
                        </template>

                    </div>
                </div>

                <div class="grid-items">
                    <template v-if="isDesktop">
                        <TopicContentGridDndItem v-for="c in props.children" :topic="c" :toggle-state="toggleState"
                            :parent-id="topicStore.id" :parent-name="topicStore.name"
                            :user-is-creator-of-parent="topicStore.currentUserIsCreator" />
                    </template>
                    <template v-else>
                        <TopicContentGridTouchDndItem v-for="c in props.children" :topic="c" :toggle-state="toggleState"
                            :parent-id="topicStore.id" :parent-name="topicStore.name"
                            :user-is-creator-of-parent="topicStore.currentUserIsCreator" />
                    </template>
                </div>


                <div class="grid-footer">
                    <div class="grid-option overline-m no-line no-margin">
                        <button @click="addTopic(true)">
                            <font-awesome-icon :icon="['fas', 'plus']" />
                            <span class="button-label" :class="{ 'is-mobile': isMobile }">
                                {{ isMobile ? 'Thema erstellen' : 'Unterthema erstellen' }}
                            </span>
                        </button>
                    </div>
                    <div class="grid-divider" :class="{ 'is-mobile': isMobile }"></div>
                    <div class="grid-option overline-m no-line no-margin">
                        <button @click="addTopic(false)">
                            <font-awesome-icon :icon="['fas', 'link']" />
                            <span class="button-label" :class="{ 'is-mobile': isMobile }">
                                {{ isMobile ? 'Thema verknüpfen' : 'Unterthema verknüpfen' }}
                            </span>
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <LazyClientOnly>
            <TopicContentGridGhost v-show="dragStore.active" />
        </LazyClientOnly>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-row {
    margin-top: 45px;
    max-width: calc(100vw - 20px);
    margin-bottom: 45px;

    .grid-container {
        margin-bottom: 45px;
    }

    .no-margin {
        margin-right: 0px;
        margin-left: 0px;
        margin-top: 0px;
        margin-bottom: 0px;
    }

    .grid-header {
        justify-content: space-between;

        .grid-options {
            display: flex;
            justify-content: flex-end;
            align-items: center;

            .root-chip {
                align-items: center;
                color: @memo-grey-darker;

            }
        }

        .grid-title {
            margin-bottom: 0px;
        }
    }

    .grid-header,
    .grid-footer {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;

        button {
            color: @memo-grey-darker;
            background: white;
            height: 32px;
            border-radius: 32px;
            min-width: 32px;
            display: flex;
            flex-wrap: nowrap;
            align-items: center;
            justify-content: center;

            .button-label {
                color: @memo-grey-dark;
                padding: 0 4px;
                text-wrap: nowrap;

                &.is-mobile {
                    // font-size: 12px;
                }
            }

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }
        }

        .grid-divider {
            margin: 0 8px;
            height: 22px;
            border-left: solid 1px @memo-grey-light;
        }
    }

    .grid-footer {
        border-top: solid 1px @memo-grey-light;
        padding-top: 4px;

        .grid-divider {
            &.is-mobile {
                margin: 0 4px;

            }
        }
    }
}
</style>

<style lang="less">
.open-modal {

    .grid-container,
    .grid-header {
        .grid-option {
            button {
                background: none;
            }
        }
    }
}
</style>