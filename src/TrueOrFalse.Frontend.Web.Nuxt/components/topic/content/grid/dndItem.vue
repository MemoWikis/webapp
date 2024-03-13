<script lang="ts" setup>
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'

interface Props {
    topic: GridTopicItem
    toggleState: ToggleState
    parentId: number
    parentName: string
}
const props = defineProps<Props>()
const isDroppableItemActive = ref(false)
function onDragOver() {
    isDroppableItemActive.value = true
}
function onDragLeave() {
    isDroppableItemActive.value = false
}
function onMouseLeave() {
    isDroppableItemActive.value = false
}
const emit = defineEmits(['setNewArr'])

const newArr = ref<GridTopicItem[]>([])

function onDrop(event: any) {
    isDroppableItemActive.value = false
    console.log("dropped")
}
// function getPayload(index: number) {
//     const payload = {
//         item: props.item.children![index],
//         index: [...props.index, index]
//     }
//     return payload
// }

const open = ref(false)
</script>

<template>
    <SharedDraggable :transfer-data="topic.id" class="draggable">
        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop, onMouseLeave }">

            <div class="item" @click.self="open = !open"
                :class="{ 'open': open, 'isDroppableItemActive': isDroppableItemActive }">
                <div>
                    <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                        :parent-name="props.parentName" />
                </div>
            </div>
        </SharedDroppable>
    </SharedDraggable>

    <!-- <Draggable :key="index.toString() + item.id" class="draggable">
        <div class="item">
            {{ item.name }}

            <Container @drop="emit('onDndDrop', { event: $event, targetPath: index })" group-name="test"
                :get-child-payload="getPayload">
                <TopicContentGridItem v-for="c, i in item.children" :item="c" :index="[...index, i]"
                    @on-dnd-drop="emit('onDndDrop', $event)" />
            </Container>
        </div>
    </Draggable> -->
</template>

<style lang="less" scoped>
.draggable {
    // transition: all 2s;
    transform: scale(1);


    .item {
        width: 100%;
        border: solid 1px silver;
        padding: 10px;
        background: white;
        margin-left: 10px;
        transition: all 0.1s;
        border-right: none;
        margin-bottom: 10px;
        // transform: scale(1);

        &.open {
            padding-top: 50px;
            padding-bottom: 50px;

            background-color: mediumspringgreen;
        }

        &.isDroppableItemActive {
            background-color: lightpink;
        }
    }

    .is-moving {
        // transform: scale(0);
    }
}
</style>