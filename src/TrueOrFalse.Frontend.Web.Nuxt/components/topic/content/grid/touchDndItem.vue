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
    console.log('onDrop')
    // isDroppableItemActive.value = false

    // hoverTopHalf.value = false
    // hoverBottomHalf.value = false
    // dropIn.value = false

    // if (dragStore.transferData == null || !dragStore.isMoveTopicTransferData)
    //     return

    // const transferData = dragStore.transferData as MoveTopicTransferData

    // if (dragStore.dropZoneData == null)
    //     return

    // const targetId = dragStore.dropZoneData.id

    // if (transferData.topic.id == targetId)
    //     return

    // const position = dragStore.dropZoneData.position
    // currentPosition.value = TargetPosition.None
    // dragOverTimer.value = null

    // await editTopicRelationStore.moveTopic(transferData.topic, targetId, position, props.parentId, transferData.oldParentId)

    // const snackbarCustomAction: SnackbarCustomAction = {
    //     label: '',
    //     action: () => {
    //         editTopicRelationStore.undoMoveTopic()
    //     }
    // }

    // snackbar.add({
    //     type: 'info',
    //     title: { text: transferData.topic.name, url: `/${transferData.topic.name}/${transferData.topic.id}` },
    //     text: { html: `wurde verschoben`, buttonLabel: snackbarCustomAction?.label, buttonId: snackbarStore.addCustomAction(snackbarCustomAction), buttonIcon: ['fas', 'rotate-left'] },
    //     dismissible: true
    // })
}

const dragging = ref(false)

function handleDragStart(e: any) {
    dragStore.showTouchSpinner = false
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

    if (!dragStore.active) {
        const data: MoveTopicTransferData = {
            topic: props.topic,
            oldParentId: props.parentId
        }
        dragStore.dragStart(data)
        dragging.value = true
    }
}

const touchTimer = ref()

function preventScroll(e: TouchEvent) {
    console.log('try prevent scroll', e.cancelable)

    if (e.cancelable)
        e.preventDefault()
}

const showTouchIndicatorTimer = ref()
async function handlePress(e: TouchEvent) {
    const x = e.changedTouches[0].clientX
    const y = e.changedTouches[0].clientY
    dragStore.setTouchPositionForDrag(x, y)
    showTouchIndicatorTimer.value = setTimeout(() => {
        dragStore.showTouchSpinner = true
    }, 100)
    console.log('press')

    document.addEventListener('touchmove', preventScroll, { passive: false })

    initialHoldPosition.x = e.changedTouches[0].pageX
    initialHoldPosition.y = e.changedTouches[0].pageY

    touchTimer.value = setTimeout(() => {
        // document.addEventListener('touchmove', preventScroll, { passive: false })
        handleDragStart(e)
    }, 1100)
}

function touchRelease() {
    dragStore.showTouchSpinner = false
    clearTimeout(touchTimer.value)
    clearTimeout(showTouchIndicatorTimer.value)
    touchTimer.value = null
    document.removeEventListener('touchmove', preventScroll, { passive: false } as any)
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
    if (dragStore.active)
        onDrop()
    dragging.value = false
    dragStore.dragEnd()
    currentPosition.value = TargetPosition.None
}

const touchDragComponent = ref<HTMLElement | null>(null)

function handleDragOnce(e: TouchEvent) {
    console.log(e)
    if (e.defaultPrevented) {
        console.log('shouldHandleDragStart')
        handleDragStart(e)
    }
}

function handleDrag(e: TouchEvent) {

    dragStore.showTouchSpinner = false

    if (!e.defaultPrevented) {
        clearTimeout(touchTimer.value)
    }

    const x = e.changedTouches[0].pageX
    const y = e.changedTouches[0].pageY - 85
    dragStore.setMouseData(e.changedTouches[0].clientX, e.changedTouches[0].clientY, x, y)

    if (dragging.value && e.defaultPrevented)
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

    if (xDifference > 5 || yDifference > 2) {
        clearTimeout(touchTimer.value)
    }

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
    <div class="draggable" v-touch:press="handlePress" v-touch:release="touchRelease" v-touch:drag="handleDrag"
        v-touch.drag.once="handleDragOnce" ref="touchDragComponent">
        <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

            <div v-if="dragStore.active" class="emptydropzone" :class="{ 'open': hoverTopHalf && !dragging }"
                :data-dropzonedata="getDropZoneData(TargetPosition.Before)">

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