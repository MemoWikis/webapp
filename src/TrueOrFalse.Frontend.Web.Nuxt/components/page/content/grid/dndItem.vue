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

const dropInHovering = ref(false)

function onDropZoneEnter() {
    dropInHovering.value = true
    if (dragOverTimer.value == null)
        dragOverTimer.value = Date.now()
    else {
        const diff = Date.now() - dragOverTimer.value
        if (diff > 700) {
            dropIn.value = true
            dropInHovering.value = false
            dragOverTimer.value = null
        } else {
            dropInHovering.value = true
        }
    }
}

function onDropZoneLeave() {
    dropInHovering.value = false
}

const dragOverTimer = ref()
const isDroppableItemActive = ref(false)
function onDragOver(e: any) {
    e.preventDefault()

    isDroppableItemActive.value = true
    if (hoverPlaceholder.value === true) {
        dragOverTimer.value = null
        dropIn.value = false
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

    if (dragStore.transferData == null || !dragStore.isMovePageTransferData)
        return

    const transferData = dragStore.transferData as MovePageTransferData
    const targetId = props.page.id
    if (transferData.page.id === targetId)
        return

    const position = currentPosition.value

    const result = await editPageRelationStore.movePage(transferData.page, targetId, position, props.parentId, transferData.oldParentId)

    currentPosition.value = TargetPosition.None
    dragOverTimer.value = null

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

</script>

<template>
    <div class="draggable" @dragstart.stop="handleDragStart" @dragend="handleDragEnd" :draggable="dragStore.isDraggable"
        ref="dragComponent" @drag.stop="handleDrag">
        <div @dragover.prevent.stop="onDragOver" @dragleave="onDragLeave" @drop.stop="onDrop">

            <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

                <div v-if="dragStore.active"
                    class="emptydropzone"
                    :class="{ 'open': hoverTopHalf && !dragging }">

                    <div class="inner top">
                        <LazyPageContentGridDndPlaceholder v-if="dragStore.isMovePageTransferData"
                            :name="placeHolderPageName" />
                    </div>

                </div>

                <PageContentGridItem :page="page" :toggle-state="props.toggleState" :parent-id="props.parentId"
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
                            <div class="dropzone-label">{{ t('page.grid.dnd.labels.subordinatePage') }}</div>
                        </div>
                    </template>
                </PageContentGridItem>

                <div v-if="dragStore.active && !dragging && !props.disabled && !dropIn" class="drop-in-trigger" :class="{ 'hover-top': hoverTopHalf }" @dragover.stop.prevent="onDropZoneEnter" @dragleave.prevent="onDropZoneLeave">
                    <div class="drop-in-indicator" :class="{ 'hover-main': isDroppableItemActive, 'active': dropInHovering }">
                        <span class="loader" :class="{ 'loading': dropInHovering }"></span>
                        <div class="drop-in-icon" v-if="!dropIn">
                            <font-awesome-icon :icon="['fas', 'right-to-bracket']" rotation="90" />
                        </div>
                    </div>
                </div>

                <div v-if="dragStore.active" class="emptydropzone" :class="{ 'open': hoverBottomHalf && !dragging, 'inside': dropIn }">

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

        &.dragging {
            opacity: 0.2;
        }
    }

    &:active {
        cursor: grabbing;
    }
}

.drop-in-trigger {
    position: absolute;
    left: 50%;
    top: 5px;
    transform: translateX(-50%);
    width: 50px;
    height: 50px;
    border-radius: 50%;
    z-index: 10;
    pointer-events: auto;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 100ms ease-in;

    &.hover-top {
        top: 85px;
    }
}

.drop-in-indicator {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    border: 2px solid transparent;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    overflow: visible;
    transition: all 0.2s ease;

    .drop-in-icon {
        z-index: 1;
        color: @memo-grey-light;
        font-size: 1.6rem;
    }

    &.hover-main {
        .drop-in-icon {
            color: @memo-grey-dark;
        }
    }

    &.active {
        border-color: @memo-green;
        background-color: fade(@memo-green, 10%);

        .drop-in-icon {
            color: @memo-green;
        }
    }
}

.loader {
    width: 58px;
    height: 58px;
    border: 5px solid white;
    border-radius: 50%;
    position: absolute;
    transform: rotate(45deg);
    box-sizing: border-box;
    display: none;
    z-index: -1;
}

.loader.loading {
    display: block;
}

.loader::before {
    content: "";
    position: absolute;
    box-sizing: border-box;
    inset: -5px;
    border-radius: 50%;
    border: 5px solid @memo-green;
    animation: prixClipFix 700ms 1 linear forwards;
}

@keyframes prixClipFix {
    0% {
        clip-path: polygon(50% 50%, 0 0, 0 0, 0 0, 0 0, 0 0)
    }

    25% {
        clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 0, 100% 0, 100% 0)
    }

    50% {
        clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 100%, 100% 100%, 100% 100%)
    }

    75% {
        clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 100%, 0 100%, 0 100%)
    }

    100% {
        clip-path: polygon(50% 50%, 0 0, 100% 0, 100% 100%, 0 100%, 0 0)
    }
}
</style>