<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useEditTopicRelationStore } from '~~/components/topic/relation/editTopicRelationStore'
import { useDragStore, TargetPosition } from '~~/components/shared/dragStore'

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
function onDragOver() {
    isDroppableItemActive.value = true
    if (dragOverTimer.value == null)
        dragOverTimer.value = Date.now()
    else {
        const diff = Date.now() - dragOverTimer.value
        if (diff > 1000)
            dropIn.value = true
    }
}
function onDragLeave() {
    isDroppableItemActive.value = false
    dragOverTimer.value = null
    dropIn.value = false
}

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
    const targetId = props.topic.id

    if (transferData.movingTopicId == targetId)
        return

    const position = currentPosition.value
    currentPosition.value = TargetPosition.None
    dragOverTimer.value = null

    editTopicRelationStore.moveTopic(transferData.movingTopicId, targetId, position, props.parentId, transferData.oldParentId)
}


const dragging = ref(false)

function handleDragStart() {
    const data: TransferData = {
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
}
</script>

<template>
    <div class="draggable" @dragstart.stop="handleDragStart" @dragend="handleDragEnd" :draggable="true">
        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">

            <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

                <div v-if="dragStore.active" @dragover="hoverTopHalf = true" @dragleave="hoverTopHalf = false"
                    class="emptydropzone" :class="{ 'open': hoverTopHalf && !dragging }">

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
                        <LazyTopicContentGridDndPlaceholder v-if="dragStore.transferData?.topicName"
                            :name="dragStore.transferData?.topicName" />
                    </div>

                </div>
            </div>
        </SharedDroppable>
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
            border-top: none;
            z-index: 2;
        }

        &.top {
            border-bottom: none;
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
        background: @memo-green;
        background: linear-gradient(180deg, rgba(175, 213, 52, 1) 0%, rgba(175, 213, 52, 0.6) 10%, rgba(175, 213, 52, 0.33) 25%, rgba(175, 213, 52, 0) 50%);
    }

    &.bottom {
        z-index: 3;
        height: 67%;
        top: 33%;
        background: @memo-green;
        background: linear-gradient(0deg, rgba(175, 213, 52, 1) 0%, rgba(175, 213, 52, 0.6) 5%, rgba(175, 213, 52, 0.33) 12.5%, rgba(175, 213, 52, 0) 25%);
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
}
</style>