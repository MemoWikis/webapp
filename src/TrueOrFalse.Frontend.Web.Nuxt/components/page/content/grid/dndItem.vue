<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridPageItem } from './item/gridPageItem'
import { useEditPageRelationStore } from '~/components/page/relation/editPageRelationStore'
import { useDragStore, TargetPosition, MovePageTransferData } from '~~/components/shared/dragStore'
import { SnackbarCustomAction, useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { useUserStore } from '~/components/user/userStore'
import { Visibility } from '~/components/shared/visibilityEnum'

const editPageRelationStore = useEditPageRelationStore()
const dragStore = useDragStore()
const snackbarStore = useSnackbarStore()
const userStore = useUserStore()
const { t } = useI18n()

interface Props {
    page: GridPageItem
    toggleState: ToggleState
    parentId: number
    parentName: string
    parentVisibility: Visibility
    disabled?: boolean
    userIsCreatorOfParent: boolean
}
const props = defineProps<Props>()

const dropIn = ref(false)
// Add watcher to track when dropIn changes
watch(dropIn, (newVal) => {
    console.log('dropIn changed to:', newVal, 'hoverPlaceholder:', hoverPlaceholder.value)
})
const dropInProgress = ref(0)  // 0-100% for the progress indicator
const dropInTimer = ref<number | null>(null)
const dropInHovering = ref(false)

// Functions to handle drop-in zone interaction
function startDropInTimer() {
    // Clear any existing timer
    if (dropInTimer.value) {
        clearInterval(dropInTimer.value)
    }

    // Reset progress
    dropInProgress.value = 0

    // Start a new timer that increments progress
    const startTime = Date.now()
    const duration = 1000 // 1 second to hold

    dropInTimer.value = window.setInterval(() => {
        const elapsed = Date.now() - startTime
        const progress = Math.min((elapsed / duration) * 100, 100)
        dropInProgress.value = progress

        if (progress >= 100) {
            // Once we reach 100%, activate dropIn
            dropIn.value = true
            clearInterval(dropInTimer.value as number)
            dropInTimer.value = null
        }
    }, 20) // Update every 20ms for smooth animation
}

function stopDropInTimer() {
    if (dropInTimer.value) {
        clearInterval(dropInTimer.value)
        dropInTimer.value = null
    }
    dropInProgress.value = 0
}

function onDropZoneEnter() {
    console.log('Entering drop zone')
    dropInHovering.value = true
    // startDropInTimer()
    if (dragOverTimer.value == null)
        dragOverTimer.value = Date.now()
    else {
        const diff = Date.now() - dragOverTimer.value
        if (diff > 700)
            dropIn.value = true
    }
}

function onDropZoneLeave() {
    console.log('Leaving drop zone')
    dropInHovering.value = false
    // stopDropInTimer()
    // Don't immediately reset dropIn, let it be handled by other events
}

const dragOverTimer = ref()
const isDroppableItemActive = ref(false)
function onDragOver(e: any) {
    e.preventDefault()

    isDroppableItemActive.value = true
    console.log('dragover')
    // Only start or continue the timer if not hovering over a placeholder
    if (hoverPlaceholder.value === false) {
        // if (dragOverTimer.value == null)
        //     dragOverTimer.value = Date.now()
        // else {
        //     const diff = Date.now() - dragOverTimer.value
        //     if (diff > 700)
        //         dropIn.value = true
        // }
    } else {
        // Reset timer when hovering over a placeholder
        dragOverTimer.value = null
        // Always ensure dropIn is false when over placeholders
        dropIn.value = false
    }

    handleScroll(e.clientY)
}

function onDragLeave() {
    handleDragLeavePlaceholder(hoverTopHalf.value)
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

    if (dragStore.transferData == null || !dragStore.isMovePageTransferData)
        return

    const transferData = dragStore.transferData as MovePageTransferData
    const targetId = props.page.id
    if (transferData.page.id === targetId)
        return

    const position = currentPosition.value
    currentPosition.value = TargetPosition.None
    dragOverTimer.value = null

    const result = await editPageRelationStore.movePage(transferData.page, targetId, position, props.parentId, transferData.oldParentId)

    if (result) {
        const snackbarCustomAction: SnackbarCustomAction = {
            label: t('page.grid.dnd.buttons.reset'),
            action: () => {
                editPageRelationStore.undoMovePage()
            },
            icon: ['fas', 'rotate-left']
        }

        snackbar.add({
            type: 'info',
            title: { text: transferData.page.name, url: `/${transferData.page.name}/${transferData.page.id}` },
            text: {
                html: t('page.grid.dnd.messages.moved'),
                buttonLabel: snackbarCustomAction.label,
                buttonId: snackbarStore.addCustomAction(snackbarCustomAction),
                buttonIcon: snackbarCustomAction.icon
            },
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

    if (!userStore.isAdmin && (!props.userIsCreatorOfParent && props.page.creatorId != userStore.id)) {
        if (userStore.isLoggedIn)
            snackbar.add({
                type: 'error',
                title: '',
                text: {
                    html: t('page.grid.dnd.errors.noPermission', { pageName: `<b>${props.page.name}</b>` })
                },
                dismissible: true
            })
        else {
            const snackbarCustomAction: SnackbarCustomAction = {
                label: t('page.grid.dnd.buttons.login'),
                action: () => {
                    editPageRelationStore.undoMovePage()
                },
                icon: ['fas', 'right-to-bracket']
            }

            snackbar.add({
                type: 'error',
                title: '',
                text: {
                    html: t('page.grid.dnd.errors.noPermission', { pageName: `<b>${props.page.name}</b>` }),
                    buttonLabel: snackbarCustomAction.label,
                    buttonId: snackbarStore.addCustomAction(snackbarCustomAction),
                    buttonIcon: snackbarCustomAction.icon
                },
                dismissible: true
            })
        }
        return
    }

    if (props.parentVisibility === Visibility.Public && !userStore.gridInfoShown) {
        snackbar.add({
            type: 'warning',
            title: '',
            text: {
                html: t('page.grid.dnd.messages.visibleToAll', { parentName: `<b>${props.parentName}</b>` })
            },
            dismissible: true
        })

        userStore.gridInfoShown = true
    }

    const data: MovePageTransferData = {
        page: props.page,
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

const placeHolderPageName = ref('')

onMounted(() => {
    if (dragStore.isMovePageTransferData) {
        const m = dragStore.transferData as MovePageTransferData
        placeHolderPageName.value = m.page.name
    }
})

watch(() => dragStore.transferData, (t) => {
    if (dragStore.isMovePageTransferData) {
        const m = t as MovePageTransferData
        placeHolderPageName.value = m.page.name
    }
}, { deep: true, immediate: true })

const hoverPlaceholder = ref(false)

const handleDragOverPlaceholder = (top: boolean) => {
    console.log('handleDragOverPlaceholder, isTop:', top)

    hoverPlaceholder.value = true
    // Ensure dropIn is always false when over placeholders
    dropIn.value = false
    // Reset any existing timer
    dragOverTimer.value = null

    if (top) {
        hoverTopHalf.value = true
        hoverBottomHalf.value = false
    }
    else {
        hoverTopHalf.value = false
        hoverBottomHalf.value = true
    }
}

const handleDragLeaveTimer = ref()

const handleDragLeavePlaceholder = (top: boolean) => {
    console.log('handleDragLeavePlaceholder, isTop:', top)

    if (handleDragLeaveTimer.value) {
        clearTimeout(handleDragLeaveTimer.value)
    }
    handleDragLeaveTimer.value = setTimeout(() => {
        hoverPlaceholder.value = false
        if (top) {
            hoverTopHalf.value = false
        } else {
            hoverBottomHalf.value = false
        }
    }, 800)
}

const handleMouseLeavePlaceholder = (top: boolean) => {
    console.log('handleMouseLeavePlaceholder, isTop:', top)
    // This is a more reliable way to detect when we've actually left the placeholder
    hoverPlaceholder.value = false
    // Reset the relevant half
    if (top) {
        hoverTopHalf.value = false
    } else {
        hoverBottomHalf.value = false
    }
}

watch(hoverPlaceholder, (val) => console.log('hoverPlaceholder', val))
const gridItem = ref()
</script>

<template>
    <div class="draggable" @dragstart.stop="handleDragStart" @dragend="handleDragEnd" :draggable="true"
        ref="dragComponent" @drag.stop="handleDrag">
        <div @dragover.prevent.stop="onDragOver" @dragleave="onDragLeave" @drop.stop="onDrop">

            <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

                <div v-if="dragStore.active"
                    class="emptydropzone"
                    :class="{ 'open': hoverTopHalf && !dragging }"
                    @dragover.stop.prevent="handleDragOverPlaceholder(true)">

                    <div class="inner top">
                        <LazyPageContentGridDndPlaceholder v-if="dragStore.isMovePageTransferData"
                            :name="placeHolderPageName" />
                    </div>

                </div>

                <!-- Normal item without a container -->
                <PageContentGridItem :page="page" :toggle-state="props.toggleState" :parent-id="props.parentId"
                    :parent-name="props.parentName" :is-dragging="dragging" :drop-expand="dropIn" ref="gridItem">

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
                            <div class="dropzone-label">{{ t('page.grid.dnd.labels.subordinatePage') }}</div>
                        </div>
                    </template>
                </PageContentGridItem>

                <!-- Absolute positioned drop indicator that doesn't affect layout -->
                <div v-if="dragStore.active && !dragging && !props.disabled && props.page.childrenCount > 0" class="drop-in-trigger" @dragover.stop.prevent="onDropZoneEnter" @dragleave.prevent="onDropZoneLeave">
                    <div class="drop-in-indicator" :class="{ 'active': dropInHovering }">
                        <div class="drop-in-progress" :style="{ height: dropInProgress + '%' }"></div>
                        <div class="drop-in-icon" v-if="!dropIn">
                            <i class="fas fa-chevron-down"></i>
                        </div>
                    </div>
                </div>

                <div v-if="dragStore.active" class="emptydropzone" :class="{ 'open': hoverBottomHalf && !dragging, 'inside': dropIn }" @dragover.stop.prevent="handleDragOverPlaceholder(false)">

                    <div class="inner bottom">
                        <LazyPageContentGridDndPlaceholder v-if="dragStore.isMovePageTransferData" :name="placeHolderPageName" />
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
        position: relative;
        /* Needed for absolute positioning of drop-in-trigger */

        &.dragging {
            opacity: 0.2;
        }
    }

    &:active {
        cursor: grabbing;
    }
}

/* New drop-in-trigger styles */
.drop-in-trigger {
    position: absolute;
    left: 50%;
    /* Fix position to top of item instead of center to prevent movement during expansion */
    top: 0px;
    /* Fixed distance from top instead of percentage */
    transform: translateX(-50%);
    /* Only transform horizontally now */
    width: 240px;
    height: 60px;
    z-index: 10;
    pointer-events: auto;
    display: flex;
    align-items: center;
    justify-content: center;
}

.drop-in-indicator {
    width: 230px;
    height: 50px;
    border-radius: 4;
    border: 2px solid transparent;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    overflow: hidden;
    transition: all 0.2s ease;
    background-color: rgba(255, 255, 255, 0.8);
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);

    &.active {
        border-color: @memo-green;
        background-color: fade(@memo-green, 10%);
    }

    .drop-in-progress {
        position: absolute;
        bottom: 0;
        left: 0;
        width: 100%;
        background-color: fade(@memo-green, 30%);
        transition: height 20ms linear;
    }

    .drop-in-icon {
        z-index: 1;
        color: @memo-green;
    }
}
</style>