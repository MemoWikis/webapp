<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useEditTopicRelationStore } from '~~/components/topic/relation/editTopicRelationStore'
import { useDragStore, TargetPosition, MoveTopicTransferData } from '~~/components/shared/dragStore'
import { SnackbarCustomAction, useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { useUserStore } from '~/components/user/userStore'
import { Visibility } from '~/components/shared/visibilityEnum'

const editTopicRelationStore = useEditTopicRelationStore()
const dragStore = useDragStore()
const snackbarStore = useSnackbarStore()
const userStore = useUserStore()

interface Props {
    topic: GridTopicItem
    toggleState: ToggleState
    parentId: number
    parentName: string
    parentVisibility: Visibility
    disabled?: boolean
    userIsCreatorOfParent: boolean
}
const props = defineProps<Props>()

const dropIn = ref(false)
const dragOverTimer = ref()
const isDroppableItemActive = ref(false)
function onDragOver(e: any) {
    e.preventDefault()

    isDroppableItemActive.value = true
    if (dragOverTimer.value == null)
        dragOverTimer.value = Date.now()
    else {
        const diff = Date.now() - dragOverTimer.value
        if (diff > 700)
            dropIn.value = true
    }

    handleScroll(e.clientY)
}

function onDragLeave() {
    isDroppableItemActive.value = false
    dragOverTimer.value = null
    dropIn.value = false
}

const snackbar = useSnackbar()

async function onDrop() {
    isDroppableItemActive.value = false

    hoverTopHalf.value = false
    hoverBottomHalf.value = false
    dropIn.value = false

    if (dragStore.transferData == null || !dragStore.isMoveTopicTransferData)
        return

    const transferData = dragStore.transferData as MoveTopicTransferData
    const targetId = props.topic.id
    if (transferData.topic.id == targetId)
        return

    const position = currentPosition.value
    currentPosition.value = TargetPosition.None
    dragOverTimer.value = null

    const result = await editTopicRelationStore.moveTopic(transferData.topic, targetId, position, props.parentId, transferData.oldParentId)

    if (result) {
        const snackbarCustomAction: SnackbarCustomAction = {
            label: 'Zurücksetzen',
            action: () => {
                editTopicRelationStore.undoMoveTopic()
            },
            icon: ['fas', 'rotate-left']
        }

        snackbar.add({
            type: 'info',
            title: { text: transferData.topic.name, url: `/${transferData.topic.name}/${transferData.topic.id}` },
            text: { html: `wurde verschoben`, buttonLabel: snackbarCustomAction.label, buttonId: snackbarStore.addCustomAction(snackbarCustomAction), buttonIcon: snackbarCustomAction.icon },
            dismissible: true
        })
    }
}

const dragging = ref(false)
const customDragImage = ref()
function handleDragStart(e: DragEvent) {

    const cdi = document.createElement('div')
    cdi.textContent = ''
    cdi.style.position = 'absolute'
    cdi.style.top = '-99999px'
    customDragImage.value = cdi

    document.body.appendChild(cdi)

    e.dataTransfer?.setDragImage(cdi, 0, 0)

    if (!userStore.isAdmin && (!props.userIsCreatorOfParent && props.topic.creatorId != userStore.id)) {
        if (userStore.isLoggedIn)
            snackbar.add({
                type: 'error',
                title: '',
                text: { html: `Leider hast du keine Rechte um <b>${props.topic.name}</b> zu verschieben` },
                dismissible: true
            })
        else {
            const snackbarCustomAction: SnackbarCustomAction = {
                label: 'Anmelden',
                action: () => {
                    editTopicRelationStore.undoMoveTopic()
                },
                icon: ['fas', 'right-to-bracket']
            }

            snackbar.add({
                type: 'error',
                title: '',
                text: {
                    html: `Leider hast du keine Rechte um <b>${props.topic.name}</b> zu verschieben`, buttonLabel: snackbarCustomAction.label, buttonId: snackbarStore.addCustomAction(snackbarCustomAction), buttonIcon: snackbarCustomAction.icon
                },
                dismissible: true
            })
        }
        return
    }

    if (props.parentVisibility == Visibility.All && !userStore.gridInfoShown) {
        snackbar.add({
            type: 'warning',
            title: '',
            text: { html: `Änderung im Thema <b>${props.parentName}</b> sind für alle sichtbar` },
            dismissible: true
        })

        userStore.gridInfoShown = true
    }

    const data: MoveTopicTransferData = {
        topic: props.topic,
        oldParentId: props.parentId
    }
    dragStore.dragStart(data)
    dragging.value = true
}

const currentPosition = ref<TargetPosition>(TargetPosition.None)
const hoverTopHalf = ref(false)
const hoverBottomHalf = ref(false)

watch([hoverTopHalf, hoverBottomHalf], ([t, b]) => {
    if (t)
        currentPosition.value = TargetPosition.Before
    else if (b && dropIn.value)
        currentPosition.value = TargetPosition.Inner
    else if (b && !dropIn.value)
        currentPosition.value = TargetPosition.After
})

function handleDragEnd() {
    dragging.value = false
    dragStore.dragEnd()
    currentPosition.value = TargetPosition.None
    customDragImage.value.remove()
}

const dragComponent = ref<HTMLElement | null>(null)

function handleDrag(e: DragEvent) {
    if (dragComponent.value) {
        const x = e.clientX
        const y = e.clientY
        dragStore.setMousePosition(x, y)
    }
}

function handleScroll(clientY: number) {
    const threshold = 100
    const distanceFromBottom = window.innerHeight - clientY

    if (clientY <= threshold) {
        const scrollSpeed = - Math.ceil(((threshold - clientY) / 10))
        window.scrollBy(0, scrollSpeed)
    } else if (distanceFromBottom <= threshold) {
        const scrollSpeed = Math.ceil(((threshold - distanceFromBottom) / 10))
        window.scrollBy(0, scrollSpeed)
    }
}

const placeHolderTopicName = ref('')

onMounted(() => {
    if (dragStore.isMoveTopicTransferData) {
        const m = dragStore.transferData as MoveTopicTransferData
        placeHolderTopicName.value = m.topic.name
    }
})

watch(() => dragStore.transferData, (t) => {
    if (dragStore.isMoveTopicTransferData) {
        const m = t as MoveTopicTransferData
        placeHolderTopicName.value = m.topic.name
    }
}, { deep: true, immediate: true })
</script>

<template>
    <div class="draggable" @dragstart.stop="handleDragStart" @dragend="handleDragEnd" :draggable="true"
        ref="dragComponent" @drag.stop="handleDrag">
        <div @dragover="onDragOver" @dragleave="onDragLeave" @drop.stop="onDrop">

            <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

                <div v-if="dragStore.active" @dragover="hoverTopHalf = true" @dragleave="hoverTopHalf = false"
                    class="emptydropzone" :class="{ 'open': hoverTopHalf && !dragging }">

                    <div class="inner top">
                        <LazyTopicContentGridDndPlaceholder v-if="dragStore.isMoveTopicTransferData"
                            :name="placeHolderTopicName" />
                    </div>

                </div>

                <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                    :parent-name="props.parentName" :is-dragging="dragging" :drop-expand="dropIn">

                    <template #topdropzone>
                        <div v-if="dragStore.active && !dragging && !props.disabled" class="dropzone top"
                            :class="{ 'hover': hoverTopHalf && !dragging }" @dragover="hoverTopHalf = true"
                            @dragleave="hoverTopHalf = false">
                        </div>
                    </template>
                    <template #bottomdropzone>
                        <div v-if="dragStore.active && !dragging && !props.disabled && !dropIn" class="dropzone bottom"
                            :class="{ 'hover': hoverBottomHalf && !dragging }" @dragover="hoverBottomHalf = true"
                            @dragleave="hoverBottomHalf = false">
                        </div>
                    </template>
                    <template #dropinzone>
                        <div v-if="dragStore.active && !dragging && !props.disabled && dropIn" class="dropzone inner"
                            :class="{ 'hover': hoverBottomHalf && !dragging }" @dragover="hoverBottomHalf = true"
                            @dragleave="hoverBottomHalf = false">
                            <div class="dropzone-label">Thema unterordnen</div>
                        </div>
                    </template>

                </TopicContentGridItem>

                <div v-if="dragStore.active" @dragover="hoverBottomHalf = true" @dragleave="hoverBottomHalf = false"
                    class="emptydropzone" :class="{ 'open': hoverBottomHalf && !dragging, 'inside': dropIn }">

                    <div class="inner bottom">
                        <LazyTopicContentGridDndPlaceholder v-if="dragStore.isMoveTopicTransferData"
                            :name="placeHolderTopicName" />
                    </div>

                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.emptydropzone {
    height: 0px;
    transition: all 100ms ease-in;
    opacity: 0;
    pointer-events: none;

    &.open {
        height: 80px;
        opacity: 1;
    }

    &.inside {
        padding-left: 30px;
    }

    .inner {
        height: 100%;
        width: 100%;
        border: 1px solid @memo-green;

        &.bottom {
            z-index: 2;
        }

        &.top {
            z-index: 3;
        }
    }
}

.dropzone {
    position: absolute;
    width: 100%;
    opacity: 0;
    transition: all 90ms ease-in;

    &.top {
        height: 33%;
        z-index: 4;
        top: 0px;

        &.hover {
            height: calc(33% + 80px);
            top: -80px;
        }
    }

    &.bottom {
        z-index: 3;
        height: 67%;
        top: 33%;

        &.hover {
            height: calc(67% + 80px);
        }
    }

    &.inner {
        z-index: 3;
        height: 100%;
        top: 0px;
        background: rgba(175, 213, 52, 0.5);

        display: flex;
        justify-content: center;
        align-items: center;

        .dropzone-label {
            font-size: 18px;
            font-weight: bold;
        }
    }

    &.hover {
        opacity: 1;
    }
}


.draggable {
    transition: all 0.5s;
    cursor: grab;

    .item {
        opacity: 1;

        &.dragging {
            opacity: 0.2;
        }

    }

    &:active {
        cursor: grabbing;
    }
}
</style>