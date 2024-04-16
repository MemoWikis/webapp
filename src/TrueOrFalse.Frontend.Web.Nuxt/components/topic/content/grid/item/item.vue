<script lang="ts" setup>
import { useAlertStore, AlertType, messages } from '~/components/alert/alertStore'
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { ToggleState } from '../toggleStateEnum'
import { GridTopicItem } from './gridTopicItem'
import { useUserStore } from '~/components/user/userStore'
import { EditRelationData, EditTopicRelationType, useEditTopicRelationStore } from '~/components/topic/relation/editTopicRelationStore'
import { useSpinnerStore } from '~/components/spinner/spinnerStore'
import { usePublishTopicStore } from '~/components/topic/publish/publishTopicStore'
import { useTopicToPrivateStore } from '~/components/topic/toPrivate/topicToPrivateStore'
import { useDeleteTopicStore } from '~/components/topic/delete/deleteTopicStore'
import { TargetPosition } from '~/components/shared/dragStore'

const userStore = useUserStore()
const alertStore = useAlertStore()
const editTopicRelationStore = useEditTopicRelationStore()
const spinnerStore = useSpinnerStore()
const publishTopicStore = usePublishTopicStore()
const topicToPrivateStore = useTopicToPrivateStore()
const deleteTopicStore = useDeleteTopicStore()

interface Props {
    topic: GridTopicItem
    toggleState: ToggleState
    parentId: number
    parentName: string
    isDragging: boolean
    dropExpand: boolean
}

const props = defineProps<Props>()

watch(() => props.topic.id, async () => {
    children.value = []
    if (childrenLoaded.value)
        await loadChildren(true)
    expanded.value = false
})

watch(() => props.toggleState, (state) => {
    if (state == ToggleState.Collapsed)
        expanded.value = false
    else if (state == ToggleState.Expanded)
        expanded.value = true
})

const expanded = ref<boolean>(false)
watch(expanded, async (val) => {
    if (val && !childrenLoaded.value && props.topic.childrenCount > 0)
        await loadChildren()
})

watch(() => props.dropExpand, val => {
    if (val && expanded.value == false)
        expanded.value = true
})

const children = ref<GridTopicItem[]>([])
const childrenLoaded = ref<boolean>(false)

async function loadChildren(force: boolean = false) {

    if (childrenLoaded.value && !force)
        return

    spinnerStore.showSpinner()
    const result = await $fetch<FetchResult<GridTopicItem[]>>(`/apiVue/GridItem/GetChildren/${props.topic.id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success == true) {
        children.value = result.data
    } else if (result.success == false) {
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
    }

    childrenLoaded.value = true
    spinnerStore.hideSpinner()
}

const { $urlHelper } = useNuxtApp()

const detailLabel = computed(() => {
    const { questionCount, childrenCount } = props.topic

    const childrenLabel = `${childrenCount} ${childrenCount === 1 ? 'Unterthema' : 'Unterthemen'}`
    const questionLabel = `${questionCount} ${questionCount === 1 ? 'Frage' : 'Fragen'}`

    if (childrenCount > 0 && questionCount > 0)
        return `${childrenLabel} und ${questionLabel}`

    if (childrenCount > 0)
        return childrenLabel

    if (questionCount > 0)
        return questionLabel

    return ''
})

const topicsToFilter = computed(async () => {
    if (!childrenLoaded.value)
        await loadChildren()

    let topicsToFilter = children.value.map(c => c.id)
    topicsToFilter.push(props.topic.id)

    return topicsToFilter
})

async function addTopic(newTopic: boolean) {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const parent: EditRelationData = {
        parentId: props.topic.id,
        editCategoryRelation: newTopic
            ? EditTopicRelationType.Create
            : EditTopicRelationType.AddChild,
        categoriesToFilter: await topicsToFilter.value,
    }
    editTopicRelationStore.openModal(parent)
}

editTopicRelationStore.$onAction(({ after, name }) => {
    if (name == 'addTopic') {
        after((result) => {

            if (result.parentId == props.topic.id) {
                if (children.value.some(c => c.id == result.childId))
                    reloadGridItem(result.childId)
                else
                    addGridItem(result.childId)
            } else if (children.value.some(c => c.id == result.childId))
                reloadGridItem(result.childId)
        })
    }
    if (name == 'removeTopic') {
        after((result) => {
            if (result.parentId == props.topic.id) {
                removeGridItem(result.childId)
            }
        })
    }
    if (name == 'addToPersonalWiki' || name == 'removeFromPersonalWiki') {
        after((result) => {
            if (result?.success && result.id && children.value.some(c => c.id == result.id))
                reloadGridItem(result.id)
        })
    }
})

publishTopicStore.$onAction(({ after, name }) => {
    if (name == 'publish') {
        after((result) => {
            if (result?.success && result.id && children.value.some(c => c.id == result.id))
                reloadGridItem(result.id)
        })
    }
})

topicToPrivateStore.$onAction(({ after, name }) => {
    if (name == 'setToPrivate') {
        after((result) => {
            if (result?.success && result.id && children.value.some(c => c.id == result.id))
                reloadGridItem(result.id)
        })
    }
})

deleteTopicStore.$onAction(({ after, name }) => {
    if (name == 'deleteTopic') {
        after((result) => {
            if (result && result.id && children.value.some(c => c.id == result.id)) {
                removeGridItem(result.id)
            }
        })
    }
})

function removeGridItem(id: number) {
    const filteredGridItems = children.value.filter(i => i.id != id)
    children.value = filteredGridItems
}

async function addGridItem(id: number) {

    if (!childrenLoaded.value) {
        await loadChildren()
    }
    await nextTick()
    if (children.value.findIndex(c => c.id == id) > 0)
        return

    const result = await loadGridItem(id)

    if (result.success == true) {
        if (children.value.some(c => c.id == result.data.id))
            reloadGridItem(result.data.id)
        else
            children.value.push(result.data)
    } else if (result.success == false)
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
}

async function loadGridItem(id: number) {
    const result = await $fetch<FetchResult<GridTopicItem>>(`/apiVue/GridItem/GetItem/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })
    return result
}
async function reloadGridItem(id: number) {
    const result = await loadGridItem(id)

    if (result.success == true) {
        children.value = children.value.map(i => i.id === result.data.id ? result.data : i)
    } else if (result.success == false)
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
}

const dragActive = ref(false)
watch(() => props.isDragging, (val) => {
    dragActive.value = val
}, { immediate: true })

editTopicRelationStore.$onAction(({ name, after }) => {
    if (name == 'moveTopic' || name == 'cancelMoveTopic') {

        after(async (result) => {
            if (result) {
                if (result.oldParentId == props.topic.id || result.newParentId == props.topic.id)
                    await loadChildren(true)

                const parentHasChanged = result.oldParentId != result.newParentId

                if (children.value.find(c => c.id == result.oldParentId))
                    await reloadGridItem(result.oldParentId)
                if (children.value.find(c => c.id == result.newParentId) && parentHasChanged)
                    await reloadGridItem(result.newParentId)
            }
        })
    }

    if (name == 'tempInsert') {
        after((result) => {

            if (result.oldParentId == props.topic.id) {
                const index = children.value.findIndex(c => c.id === result.moveTopic.id)
                if (index !== -1) {
                    children.value.splice(index, 1)
                }
            }

            if (result.newParentId == props.topic.id) {
                const index = children.value.findIndex(c => c.id == result.targetId)
                if (result.position == TargetPosition.Before)
                    children.value.splice(index, 0, result.moveTopic)
                else if (result.position == TargetPosition.After)
                    children.value.splice(index + 1, 0, result.moveTopic)
                else if (result.position == TargetPosition.Inner)
                    children.value.push(result.moveTopic)
            }
        })
    }
})
const { isDesktop } = useDevice()
</script>

<template>
    <div class="grid-item" @click="expanded = !expanded"
        :class="{ 'no-children': props.topic.childrenCount <= 0 && children.length <= 0 }">

        <slot name="topdropzone"></slot>
        <slot name="bottomdropzone"
            v-if="!expanded || ((children.length == 0 && childrenLoaded) || props.topic.childrenCount == 0)"></slot>
        <slot name="dropinzone"></slot>

        <div class="grid-item-caret-container">
            <font-awesome-icon :icon="['fas', 'caret-right']" class="expand-caret" v-if="props.topic.childrenCount > 0"
                :class="{ 'expanded': expanded }" />
        </div>

        <div class="grid-item-body-container">
            <div class="item-img-container">
                <Image :src="props.topic.imageUrl" :format="ImageFormat.Topic" />
            </div>
            <div class="item-body">

                <div class="item-name">
                    <NuxtLink :to="$urlHelper.getTopicUrl(props.topic.name, props.topic.id)">
                        {{ props.topic.name }}
                    </NuxtLink>
                </div>

                <template v-if="detailLabel.length > 0">

                    <div class="item-detaillabel">
                        {{ detailLabel }}
                    </div>
                    <TopicContentGridKnowledgebar :knowledgebar-data="props.topic.knowledgebarData"
                        v-if="props.topic.questionCount > 0" />

                </template>

            </div>
        </div>

        <TopicContentGridItemOptions :topic="props.topic" :parent-id="props.parentId" @add-topic="addTopic"
            :parent-name="props.parentName" />
    </div>

    <div v-if="props.topic.childrenCount > 0 && expanded && !dragActive" class="grid-item-children">
        <template v-if="isDesktop">
            <TopicContentGridDndItem v-for="child in children" :topic="child" :toggle-state="props.toggleState"
                :parent-id="props.topic.id" :parent-name="props.topic.name"
                :user-is-creator-of-parent="props.topic.creatorId == userStore.id"
                :parent-visibility="props.topic.visibility" />
        </template>
        <template v-else>
            <TopicContentGridTouchDndItem v-for="child in children" :topic="child" :toggle-state="props.toggleState"
                :parent-id="props.topic.id" :parent-name="props.topic.name"
                :user-is-creator-of-parent="props.topic.creatorId == userStore.id"
                :parent-visibility="props.topic.visibility" />
        </template>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-item {
    user-select: none;
    display: flex;
    justify-content: space-between;
    flex-wrap: nowrap;
    padding: 10px 0px;
    background: white;
    border-top: solid 1px @memo-grey-light;
    position: relative;

    &:hover {
        filter: brightness(0.975)
    }

    &:active {
        filter: brightness(0.95)
    }

    .grid-item-caret-container {
        width: 32px;
        height: 100%;
        min-height: 40px;
        min-width: 32px;

        display: flex;
        justify-content: center;
        align-items: center;
        color: @memo-grey-dark;

        .expand-caret {
            // transition: 0.1s ease-in-out;

            &.expanded {
                transform: rotate(90deg)
            }

            &.no-children {
                color: @memo-grey-light;
            }
        }
    }

    .grid-item-body-container {
        display: flex;
        justify-content: flex-start;
        flex-wrap: nowrap;
        width: 100%;

        .item-img-container {
            width: 40px;
            height: 40px;
            min-width: 40px;
        }

        .item-body {
            padding-left: 8px;
            width: 100%;

            .item-name {
                font-size: 18px;

                a {
                    cursor: pointer;
                }
            }

            .item-detaillabel {
                color: @memo-grey-dark;
                font-size: 12px;
                min-height: 18px;
            }
        }
    }

    &.no-children {

        // &:hover {
        //     filter: brightness(1)
        // }

        // &:active {
        //     filter: brightness(1)
        // }

        // .grid-item-caret-container {
        //     color: @memo-grey-light;
        //     cursor: default;
        // }

        // .grid-item-body-container {
        //     cursor: default;
        // }
    }
}

.grid-item-children {
    user-select: none;
    padding-left: 16px;
}
</style>

<style lang="less">
.open-modal {
    .grid-item {
        background: none;
    }
}
</style>