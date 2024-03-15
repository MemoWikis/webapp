<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useEditTopicRelationStore } from '~~/components/topic/relation/editTopicRelationStore'

const editTopicRelationStore = useEditTopicRelationStore()

interface Props {
    topic: GridTopicItem
    toggleState: ToggleState
    parentId: number
    parentName: string
}
const props = defineProps<Props>()

const transferData = ref('')

onBeforeMount(() => {
    transferData.value = JSON.stringify({
        movingTopicId: props.topic.id,
        oldParentId: props.parentId
    })
})

const isDroppableItemActive = ref(false)
function onDragOver() {
    isDroppableItemActive.value = true
}
function onDragLeave() {
    isDroppableItemActive.value = false
}
async function onDrop(event: any) {
    isDroppableItemActive.value = false

    hoverTopHalf.value = false
    hoverBottomHalf.value = false

    const { movingTopicId, oldParentId } = JSON.parse(event.dataTransfer.getData('value'))
    const targetId = props.topic.id

    if (movingTopicId == targetId)
        return

    const position = event.target.attributes['data-targetposition'].value
    editTopicRelationStore.moveTopic(movingTopicId, targetId, position, oldParentId, props.parentId)
}

const hoverTopHalf = ref(false)
const hoverBottomHalf = ref(false)

const dragging = ref(false)
watch(dragging, (val) => {
    const html = document.getElementsByTagName('html').item(0)
    if (html != null)
        html.classList.toggle('grabbing', val)
})
</script>

<template>
    <SharedDraggable :transfer-data="transferData" class="draggable" @drag-started="dragging = true"
        @drag-ended="dragging = false">
        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">

            <div class="item" :class="{ 'isDroppableItemActive': isDroppableItemActive, 'dragging': dragging }">
                <div>

                    <div @dragover="hoverTopHalf = true" @dragleave="hoverTopHalf = false" class="emptydropzone"
                        :class="{ 'open': hoverTopHalf && !dragging }">
                    </div>

                    <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                        :parent-name="props.parentName">

                        <div class="dropzone top" :class="{ 'hover': hoverTopHalf && !dragging }"
                            @dragover="hoverTopHalf = true" @dragleave="hoverTopHalf = false"
                            data-targetposition="before"></div>
                        <div class="dropzone bottom" :class="{ 'hover': hoverBottomHalf && !dragging }"
                            @dragover="hoverBottomHalf = true" @dragleave="hoverBottomHalf = false"
                            data-targetposition="after"></div>

                    </TopicContentGridItem>

                    <div @dragover="hoverBottomHalf = true" @dragleave="hoverBottomHalf = false" class="emptydropzone"
                        :class="{ 'open': hoverBottomHalf && !dragging }">
                    </div>
                </div>
            </div>
        </SharedDroppable>
    </SharedDraggable>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.emptydropzone {
    height: 0px;
    transition: all 150ms ease-in;
    opacity: 0;
    background: @memo-green;

    &.open {
        height: 30px;
        opacity: 1;
    }
}

.dropzone {
    position: absolute;
    width: 100%;
    height: 50%;
    opacity: 0;
    transition: all 150ms ease-in;

    &.top {
        top: 0px;
        background: @memo-green;
        background: linear-gradient(180deg, rgba(175, 213, 52, 0.5) 0%, rgba(175, 213, 52, 0) 100%);
    }

    &.bottom {
        top: 50%;
        background: @memo-green;
        background: linear-gradient(0deg, rgba(175, 213, 52, 0.5) 0%, rgba(175, 213, 52, 0) 100%);
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