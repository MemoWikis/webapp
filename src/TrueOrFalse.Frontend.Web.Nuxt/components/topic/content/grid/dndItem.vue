<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useEditTopicRelationStore } from '~~/components/topic/relation/editTopicRelationStore'
import { useDragStore } from '~~/components/shared/dragStore'

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

function handleDragStart() {
    dragging.value = true
}

watch(() => dragStore.active, (val) => {
    if (!val)
        dragging.value = false
})
</script>

<template>
    <SharedDraggable :transfer-data="transferData" class="draggable" @self-drag-started="handleDragStart"
        @drag-ended="dragging = false">
        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">

            <div class="item" :class="{ 'active-drag': isDroppableItemActive, 'dragging': dragging }">

                <div v-if="dragStore.active" @dragover="hoverTopHalf = true" @dragleave="hoverTopHalf = false"
                    class="emptydropzone" :class="{ 'open': hoverTopHalf && !dragging }">
                    <!-- <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                        :parent-name="props.parentName" :active-dragging="nuxtApp.$dragstarted"
                        :is-dragging="dragging" /> -->

                    <div class="inner top">

                    </div>
                </div>

                <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                    :parent-name="props.parentName" :active-dragging="dragStore.active" :is-dragging="dragging">

                    <template #topdropzone>
                        <div v-if="dragStore.active && !dragging && !props.disabled" class="dropzone top"
                            :class="{ 'hover': hoverTopHalf && !dragging }" @dragover="hoverTopHalf = true"
                            @dragleave="hoverTopHalf = false" data-targetposition="before">
                        </div>
                    </template>
                    <template #bottomdropzone>
                        <div v-if="dragStore.active && !dragging && !props.disabled" class="dropzone bottom"
                            :class="{ 'hover': hoverBottomHalf && !dragging }" @dragover="hoverBottomHalf = true"
                            @dragleave="hoverBottomHalf = false" data-targetposition="after">
                        </div>
                    </template>


                </TopicContentGridItem>

                <div v-if="dragStore.active" @dragover="hoverBottomHalf = true" @dragleave="hoverBottomHalf = false"
                    class="emptydropzone" :class="{ 'open': hoverBottomHalf && !dragging }">

                    <div class="inner bottom">

                    </div>
                    <!-- <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                        :parent-name="props.parentName" :active-dragging="nuxtApp.$dragstarted"
                        :is-dragging="dragging" /> -->
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

    &.open {
        height: 80px;
        opacity: 1;
    }

    .inner {
        height: 100%;
        width: 100%;
        border: 1px solid @memo-green;

        &.bottom {
            border-top: none;
        }

        &.top {
            border-bottom: none;
        }
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
        background: linear-gradient(180deg, rgba(175, 213, 52, 1) 0%, rgba(175, 213, 52, 0.6) 10%, rgba(175, 213, 52, 0.33) 25%, rgba(175, 213, 52, 0) 50%);
    }

    &.bottom {
        top: 50%;
        background: @memo-green;
        background: linear-gradient(0deg, rgba(175, 213, 52, 1) 0%, rgba(175, 213, 52, 0.6) 10%, rgba(175, 213, 52, 0.33) 25%, rgba(175, 213, 52, 0) 50%);
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