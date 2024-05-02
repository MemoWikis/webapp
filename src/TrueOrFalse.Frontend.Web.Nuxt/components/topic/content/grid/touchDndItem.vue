<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useEditTopicRelationStore } from '~~/components/topic/relation/editTopicRelationStore'
import { useDragStore, TargetPosition, DragAndDropType, DropZoneData, MoveTopicTransferData } from '~~/components/shared/dragStore'
import { SnackbarCustomAction, useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { useUserStore } from '~/components/user/userStore'
import { Visibility } from '~/components/shared/visibilityEnum'

const editTopicRelationStore = useEditTopicRelationStore()
const dragStore = useDragStore()
const snackbarStore = useSnackbarStore()
const userStore = useUserStore()

const { $logger } = useNuxtApp()

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

const snackbar = useSnackbar()

async function onDrop() {
    isDroppableItemActive.value = false

    hoverTopHalf.value = false
    hoverBottomHalf.value = false
    dropIn.value = false

    if (dragStore.transferData == null || !dragStore.isMoveTopicTransferData)
        return

    const transferData = dragStore.transferData as MoveTopicTransferData

    if (dragStore.dropZoneData == null)
        return

    const targetId = dragStore.dropZoneData.id

    if (transferData.topic.id == targetId)
        return

    const position = dragStore.dropZoneData.position
    currentPosition.value = TargetPosition.None
    dragOverTimer.value = null

    const result = await editTopicRelationStore.moveTopic(transferData.topic, targetId, position, dragStore.dropZoneData.parentId, transferData.oldParentId)

    if (result) {
        const snackbarCustomAction: SnackbarCustomAction = {
            label: '',
            action: () => {
                editTopicRelationStore.undoMoveTopic()
            }
        }

        snackbar.add({
            type: 'info',
            title: { text: transferData.topic.name, url: `/${transferData.topic.name}/${transferData.topic.id}` },
            text: { html: `wurde verschoben`, buttonLabel: snackbarCustomAction?.label, buttonId: snackbarStore.addCustomAction(snackbarCustomAction), buttonIcon: ['fas', 'rotate-left'] },
            dismissible: true
        })
    }
}

const dragging = ref(false)

async function prepareDragStart(e: any) {

    $logger.error("prepare-topicName: " + props.topic.name)
    $logger.error("prepare-parentName: " + props.parentName)

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
                label: '',
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
                dismissible: false
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
    dragStore.setTransferData(data)

    shouldDrag.value = true
}

const showTouchIndicatorTimer = ref()
const holdTimer = ref()
async function handleTouchStart(e: TouchEvent) {
    console.log('handleTouchStart')
    $logger.error(`touchDnd: handleTouchStart`)
    e.stopPropagation()
    const x = e.changedTouches[0].clientX
    const y = e.changedTouches[0].clientY
    initialHoldPosition.x = e.changedTouches[0].pageX
    initialHoldPosition.y = e.changedTouches[0].pageY
    dragStore.setTouchPositionForDrag(x, y)

    holdTimer.value = setTimeout(() => {
        handleHold(e)
    }, 300)

    await nextTick()
    dragStore.showTouchSpinner = true
}

const shouldDrag = ref(false)
function handleHold(e: TouchEvent) {
    console.log('handleHold')
    $logger.error(`touchDnd: handleHold`)
    // e.preventDefault();

    // document
    //     .getElementById('TopicGrid')
    //     ?.addEventListener('touchmove', preventScroll, { passive: false })

    e.stopPropagation()

    const x = e.changedTouches[0].pageX
    const y = e.changedTouches[0].pageY - 85
    dragStore.setMouseData(e.changedTouches[0].clientX, e.changedTouches[0].clientY, x, y)

    prepareDragStart(e)
}

async function handleDragOnce(e: TouchEvent) {
    console.log('handleDragOnce')
    $logger.error(`touchDnd: handleDragStart`)
    $logger.info('handleDragOnce', [{ 'shouldDrag': shouldDrag.value, 'topicId': props.topic.id }])
    isDragStart.value = false

    dragStore.showTouchSpinner = false

    if (shouldDrag.value) {
        dragStore.active = true
        dragging.value = true
    } else
        handleTouchEnd()
}

function handleTouchEnd() {
    try {
        // Intentionally throw an error to capture the stack trace
        throw new Error("Capturing stack");
    } catch (error: any) {
        // Access the stack trace from the Error object
        const stack = error.stack;

        $logger.error(`touchDnd: handleRelease`, [{ 'stack': stack }])
        console.log('touchend', stack)
    }

    handleDragEnd()
    dragStore.showTouchSpinner = false
    clearTimeout(showTouchIndicatorTimer.value)
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
    $logger.error(`touchDnd: handleDragEnd`)

    if (dragStore.active)
        onDrop()
    dragging.value = false
    dragStore.dragEnd()
    currentPosition.value = TargetPosition.None
    isDragStart.value = true
    shouldDrag.value = false
}

const touchDragComponent = ref<HTMLElement | null>(null)

const isDragStart = ref(true)
const touchDragTime = ref(0)
async function handleTouchMove(e: TouchEvent) {

    if (shouldDrag.value)
        e.preventDefault()

    if (isDragStart.value)
        handleDragOnce(e)

    const now = e.timeStamp
    const throttle = 25
    if (touchDragTime.value == null || now > (touchDragTime.value + throttle)) {
        touchDragTime.value = now
        return
    }

    console.log('handleTouchMove', shouldDrag.value)

    // $logger.info('handleDrag', [{ 'dragstartvalue': dragStart.value }])



    // $logger.info('handleDrag')

    dragStore.showTouchSpinner = false

    const x = e.changedTouches[0].pageX
    const y = e.changedTouches[0].pageY - 85
    dragStore.setMouseData(e.changedTouches[0].clientX, e.changedTouches[0].clientY, x, y)

    if (!shouldDrag.value)
        handleTouchEnd()

    if (dragging.value && shouldDrag.value)
        handleScroll(e.changedTouches[0].clientY)
}

function handleScroll(clientY: number) {
    const threshold = 100
    const distanceFromBottom = window.innerHeight - clientY

    if (clientY <= threshold) {
        const scrollSpeed = -10 - Math.ceil(((threshold - clientY) / 10))
        window.scrollBy(0, scrollSpeed)
    } else if (distanceFromBottom <= threshold - 50) {
        const scrollSpeed = 10 + Math.ceil(((threshold - distanceFromBottom) / 10))
        window.scrollBy(0, scrollSpeed)
    }
}

function getDropZoneData(position: TargetPosition): string {
    const data = {
        type: DragAndDropType.GridItem,
        id: props.topic.id,
        position: position,
        parentId: props.parentId
    } as DropZoneData
    return JSON.stringify(data)
}

watch(() => dragStore.dropZoneData, (data) => {
    if (data?.type == DragAndDropType.GridItem && data.id == props.topic.id && data.parentId == props.parentId) {
        isDroppableItemActive.value = true
        currentPosition.value = data.position
    }
    else {
        currentPosition.value = TargetPosition.None
        isDroppableItemActive.value = false
    }
}, { immediate: true, deep: true })

watch(currentPosition, (val) => {
    if (isDroppableItemActive.value) {
        if (val == TargetPosition.Before) {
            hoverTopHalf.value = true
            hoverBottomHalf.value = false
        }
        else if (val == TargetPosition.After || val == TargetPosition.Inner) {
            hoverTopHalf.value = false
            hoverBottomHalf.value = true
        }
    }
    else {
        hoverTopHalf.value = false
        hoverBottomHalf.value = false
        dropIn.value = false
    }
}, { immediate: true })

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
}, { deep: true })

const initialHoldPosition = reactive({
    x: 0,
    y: 0
})
const touchNotMovedTimer = ref()
watch([() => dragStore.touchX, () => dragStore.touchY], ([x, y]) => {

    const xDifference = Math.abs(initialHoldPosition.x - x)
    const yDifference = Math.abs(initialHoldPosition.x - y)

    if (currentPosition.value != TargetPosition.None && dragStore.active) {

        if (xDifference > 5 || yDifference > 2) {
            clearTimeout(touchNotMovedTimer.value)
        }

        if (touchNotMovedTimer.value == null) {
            if (currentPosition.value == TargetPosition.After)
                setTimeout(() => {
                    currentPosition.value = TargetPosition.Inner
                    dropIn.value = true
                }, 1000)
        }

        initialHoldPosition.x = x
        initialHoldPosition.y = y
    }
}, { immediate: true })

</script>

<template>
    <div class="draggable" v-on:touchstart="handleTouchStart" v-on:touchcancel="handleTouchEnd"
        v-on:touchend="handleTouchEnd" v-on:touchmove="handleTouchMove" v-on:contextmenu.prevent
        ref="touchDragComponent">
        <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

            <div v-if="dragStore.active" class="emptydropzone" :class="{ 'open': hoverTopHalf && !dragging }"
                :data-dropzonedata="getDropZoneData(TargetPosition.Before)">

                <div class="inner top">
                    <LazyTopicContentGridDndPlaceholder v-if="dragStore.isMoveTopicTransferData"
                        :name="placeHolderTopicName" />
                </div>

            </div>

            <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                :parent-name="props.parentName" :is-dragging="dragging" :drop-expand="false">

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
                        @dragleave="hoverBottomHalf = false" :data-dropzonedata="getDropZoneData(TargetPosition.Inner)">
                        <div class="dropzone-label">Thema unterordnen</div>
                    </div>
                </template>

            </TopicContentGridItem>

            <div v-if="dragStore.active" class="emptydropzone"
                :class="{ 'open': hoverBottomHalf && !dragging, 'inside': dropIn }"
                :data-dropzonedata="getDropZoneData(TargetPosition.After)">

                <div class="inner bottom">
                    <LazyTopicContentGridDndPlaceholder v-if="dragStore.isMoveTopicTransferData"
                        :name="placeHolderTopicName" />
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
        height: 40%;
        z-index: 4;
        top: 0px;

        &.hover {
            height: calc(40% + 80px);
            top: -80px;
        }
    }

    &.bottom {
        z-index: 3;
        height: 60%;
        top: 40%;

        &.hover {
            height: calc(60% + 80px);
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