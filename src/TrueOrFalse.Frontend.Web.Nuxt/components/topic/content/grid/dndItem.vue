<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useEditTopicRelationStore } from '~~/components/topic/relation/editTopicRelationStore'
import { useDragStore, TargetPosition, MoveTopicTransferData } from '~~/components/shared/dragStore'
import { SnackbarCustomAction, useSnackbarStore } from '~/components/snackBar/snackBarStore'

const editTopicRelationStore = useEditTopicRelationStore()
const dragStore = useDragStore()
const snackbarStore = useSnackbarStore()

interface Props {
    topic: GridTopicItem
    toggleState: ToggleState
    parentId: number
    parentName: string
    disabled?: boolean
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

    if (dragStore.transferData == null || dragStore.isMoveTopicTransferData)
        return

    const transferData = dragStore.transferData as MoveTopicTransferData
    const targetId = props.topic.id
    if (transferData.movingTopicId == targetId)
        return

    const position = currentPosition.value
    currentPosition.value = TargetPosition.None
    dragOverTimer.value = null

    await editTopicRelationStore.moveTopic(transferData.movingTopicId, targetId, position, props.parentId, transferData.oldParentId)

    const snackbarCustomAction: SnackbarCustomAction = {
        label: 'Zurücksetzen',
        action: () => {
            editTopicRelationStore.undoMoveTopic()
        }
    }

    snackbar.add({
        type: 'info',
        title: { text: transferData.topicName, url: `/${transferData.topicName}/${transferData.movingTopicId}` },
        text: { message: `wurde verschoben`, buttonLabel: snackbarCustomAction?.label, buttonId: snackbarStore.addCustomAction(snackbarCustomAction), buttonIcon: ['fas', 'rotate-left'] },
        dismissible: true
    })
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

    e.dataTransfer?.setDragImage(cdi, 0, 0);
    const data: MoveTopicTransferData = {
        movingTopicId: props.topic.id,
        oldParentId: props.parentId,
        topicName: props.topic.name
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
        const el = dragComponent.value.getBoundingClientRect()
        const x = e.pageX - el.left
        const y = e.pageY - el.height
        dragStore.setMousePosition(x, y)
    }
}

const placeHolderTopicName = ref('')

watch(() => dragStore.transferData, (t) => {
    if (dragStore.isMoveTopicTransferData) {
        const m = t as MoveTopicTransferData
        placeHolderTopicName.value = m.topicName

    }
}, { deep: true })
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
                    :parent-name="props.parentName" :active-dragging="dragStore.active" :is-dragging="dragging"
                    :drop-expand="dropIn">

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
    transition: all 100ms ease-in;

    &.top {
        height: 33%;
        z-index: 4;
        top: 0px;
    }

    &.bottom {
        z-index: 3;
        height: 67%;
        top: 33%;
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