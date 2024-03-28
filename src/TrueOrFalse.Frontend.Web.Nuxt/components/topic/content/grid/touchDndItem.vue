<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useEditTopicRelationStore } from '~~/components/topic/relation/editTopicRelationStore'
import { useDragStore, TargetPosition, DragAndDropType, DropZoneData } from '~~/components/shared/dragStore'
import { drop } from 'underscore';

const editTopicRelationStore = useEditTopicRelationStore()
const dragStore = useDragStore()

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

interface TransferData {
    movingTopicId: number
    oldParentId: number
    topicName: string
}

async function onDrop() {
    isDroppableItemActive.value = false

    hoverTopHalf.value = false
    hoverBottomHalf.value = false
    dropIn.value = false

    if (dragStore.transferData == null)
        return

    const transferData: TransferData = dragStore.transferData

    if (dragStore.dropZoneData == null)
        return

    const targetId = dragStore.dropZoneData.id

    if (transferData.movingTopicId == targetId)
        return

    const position = dragStore.dropZoneData.position
    currentPosition.value = TargetPosition.None
    dragOverTimer.value = null

    editTopicRelationStore.moveTopic(transferData.movingTopicId, targetId, position, props.parentId, transferData.oldParentId)
}

const dragging = ref(false)

function handleDragStart(e: any) {
    if (!dragStore.active) {
        const data: TransferData = {
            movingTopicId: props.topic.id,
            oldParentId: props.parentId,
            topicName: props.topic.name
        }
        dragStore.dragStart(data)
        dragging.value = true
    }

    handleDrag(e)
}

const touchTimer = ref()

function touchDown(e: TouchEvent) {
    console.log('handleDragStart')

    touchTimer.value = setTimeout(() => {
        console.log('handleDragStartTimer')

        handleDragStart(e)
    }, 500)
}

function touchRelease(e: TouchEvent) {
    console.log('touchRelease')
    clearTimeout(touchTimer.value)
    touchTimer.value = null
    handleDragEnd()
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
    console.log('handleDragEnd')
    if (dragStore.active)
        onDrop()
    dragging.value = false
    dragStore.dragEnd()
    currentPosition.value = TargetPosition.None
}

function handleDrag(e: MouseEvent | TouchEvent) {

    if (dragging.value && 'touches' in e) {
        const clientX = e.changedTouches[0].clientX;
        const clientY = e.changedTouches[0].clientY;
        dragStore.setMouseData(clientX, clientY)
    }
}

function getDropZoneData(position: TargetPosition): string {
    const data = {
        type: DragAndDropType.GridItem,
        id: props.topic.id,
        position: position
    } as DropZoneData
    return JSON.stringify(data)
}


watch(() => dragStore.dropZoneData, (data) => {
    if (data?.type == DragAndDropType.GridItem && data.id == props.topic.id) {
        isDroppableItemActive.value = true
        currentPosition.value = data.position
    }
    else {
        currentPosition.value = TargetPosition.None
        isDroppableItemActive.value = false
    }
}, { immediate: true, deep: true })

const currentPositionTimer = ref()

watch(currentPosition, (val) => {
    if (isDroppableItemActive.value) {
        if (val == TargetPosition.Before) {
            hoverTopHalf.value = true
            hoverBottomHalf.value = false
            currentPositionTimer.value = null
        }
        else if (val == TargetPosition.After) {
            hoverTopHalf.value = false
            hoverBottomHalf.value = true
            currentPositionTimer.value = setTimeout(() => {
                currentPosition.value = TargetPosition.Inner
                dropIn.value = true
            }, 300)
        }
        else if (val == TargetPosition.Inner) {
            hoverTopHalf.value = false
            hoverBottomHalf.value = true
        }
    }
    else {
        hoverTopHalf.value = false
        hoverBottomHalf.value = false
        currentPositionTimer.value = null
    }
}, { immediate: true })

</script>

<template>
    <div class="draggable" v-touch:press="touchDown" v-touch:release="touchRelease" v-touch:drag="handleDrag"
        @dragstart.stop="handleDragStart" @dragend="handleDragEnd" style="touch-action: none">
        <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

            <div v-if="dragStore.active" v-on:mouseenter="hoverTopHalf = true" class="emptydropzone"
                :class="{ 'open': hoverTopHalf && !dragging }"
                :data-dropzonedata="getDropZoneData(TargetPosition.Before)">

                <div class="inner top">
                    <LazyTopicContentGridDndPlaceholder v-if="dragStore.transferData?.topicName"
                        :name="dragStore.transferData?.topicName" />
                </div>

            </div>

            <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                :parent-name="props.parentName" :active-dragging="dragStore.active" :is-dragging="dragging"
                :drop-expand="dropIn">

                <template #topdropzone>
                    <div v-if="dragStore.active && !dragging && !props.disabled" class="dropzone top"
                        :class="{ 'hover': hoverTopHalf && !dragging }" @dragover="hoverTopHalf = true"
                        @dragleave="hoverTopHalf = false" :data-dropzonedata="getDropZoneData(TargetPosition.Before)">
                    </div>
                </template>
                <template #bottomdropzone>
                    <div v-if="dragStore.active && !dragging && !props.disabled && !dropIn" class="dropzone bottom"
                        :class="{ 'hover': hoverBottomHalf && !dragging }"
                        :data-dropzonedata="getDropZoneData(TargetPosition.After)">
                    </div>
                </template>
                <template #dropinzone>
                    <div v-if="dragStore.active && !dragging && !props.disabled && dropIn" class="dropzone inner"
                        :class="{ 'hover': hoverBottomHalf && !dragging }" @dragover="hoverBottomHalf = true"
                        @dragleave="hoverBottomHalf = false" :data-dropzonedata="getDropZoneData(TargetPosition.After)">
                        <div class="dropzone-label">Thema unterordnen</div>
                    </div>
                </template>

            </TopicContentGridItem>

            <div v-if="dragStore.active" @dragover="hoverBottomHalf = true" class="emptydropzone"
                :class="{ 'open': hoverBottomHalf && !dragging, 'inside': dropIn }"
                :data-dropzonedata="getDropZoneData(TargetPosition.After)">

                <div class="inner bottom">
                    <LazyTopicContentGridDndPlaceholder v-if="dragStore.transferData?.topicName"
                        :name="dragStore.transferData?.topicName" />
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
    z-index: 2;

    &.top {
        height: 33%;
        z-index: 4;
        top: 0px;
        // background: @memo-green;
        // background: linear-gradient(180deg, rgba(175, 213, 52, 1) 0%, rgba(175, 213, 52, 0.6) 10%, rgba(175, 213, 52, 0.33) 25%, rgba(175, 213, 52, 0) 50%);
    }

    &.bottom {
        z-index: 3;
        height: 67%;
        top: 33%;
        // background: @memo-green;
        // background: linear-gradient(0deg, rgba(175, 213, 52, 1) 0%, rgba(175, 213, 52, 0.6) 5%, rgba(175, 213, 52, 0.33) 12.5%, rgba(175, 213, 52, 0) 25%);
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

    .item {
        opacity: 1;

        &.dragging {
            opacity: 0.2;
        }

    }

    cursor: grab;

    &:active {
        cursor: grabbing;
    }
}
</style>