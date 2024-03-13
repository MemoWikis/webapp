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
const isDroppableItemActive = ref(false)
function onDragOver() {
    isDroppableItemActive.value = true

    // if (!hoverTopFake.value && !hoverTopHalf.value)
    //     setTimeout(() => showTopFake.value = false, 300)
    // if (!hoverBottomHalf.value && !hoverBottomFake.value)
    //     setTimeout(() => showBottomFake.value = false, 300)

    if (hoverTopFake.value || hoverTopHalf.value) {
        showBottomFake.value = false
        showTopFake.value = true
    }
    else if (hoverBottomFake.value || hoverBottomHalf.value) {
        showTopFake.value = false
        showBottomFake.value = true
    }
}
function onDragLeave() {
    isDroppableItemActive.value = false
}
async function onDrop(event: any) {
    console.log(props.topic.id)
    isDroppableItemActive.value = false
    console.log(event.dataTransfer.getData("value"))
    console.log(event.target.attributes["data-targetposition"].value)

    const moveId = event.dataTransfer.getData("value")
    const targetId = props.topic.id
    const position = event.target.attributes["data-targetposition"].value
    editTopicRelationStore.moveTopic(moveId, targetId, position)
}
// function getPayload(index: number) {
//     const payload = {
//         item: props.item.children![index],
//         index: [...props.index, index]
//     }
//     return payload
// }

const showTopFake = ref(false)

const hoverTopHalf = ref(false)
const hoverTopFake = ref(false)

// watch([hoverTopHalf, hoverTopFake], ([h, f]) => {
//     console.log(isDroppableItemActive.value)
//     console.log('toptrigger')
//     if (isDroppableItemActive.value) {
//         if (h || f)
//             showTopFake.value = true
//         else {
//             if (hoverBottomHalf.value && hoverBottomFake.value)
//                 showTopFake.value = false
//             else
//                 setTimeout(() => showTopFake.value = false, 300)
//         }
//     } else showTopFake.value = false

// })

const showBottomFake = ref(false)
const hoverBottomHalf = ref(false)
const hoverBottomFake = ref(false)

// watch([hoverBottomHalf, hoverBottomFake], ([h, f]) => {
//     console.log(isDroppableItemActive.value)
//     console.log('bottomtrigger')
//     if (isDroppableItemActive.value) {
//         if (h || f)
//             showBottomFake.value = true
//         else {
//             if (hoverTopHalf.value && hoverTopFake.value)
//                 showBottomFake.value = false
//             else
//                 setTimeout(() => showBottomFake.value = false, 300)
//         }
//     } else showBottomFake.value = false
// })

watch(isDroppableItemActive, (val) => {
    if (!val) {
        showBottomFake.value = false
        showTopFake.value = false
    }
})
</script>

<template>
    <SharedDraggable :transfer-data="topic.id" class="draggable">
        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">

            <div class="item" :class="{ 'isDroppableItemActive': isDroppableItemActive }">
                <div>
                    <!-- <div v-if="showTopFake" @dragover="hoverTopFake = true" @drageleave="hoverTopFake = false"> hello
                        world</div> -->


                    <TopicContentGridItem :topic="topic" :toggle-state="props.toggleState" :parent-id="props.parentId"
                        :parent-name="props.parentName">
                        <div style="position:absolute; width: 100%; height: 50%; top: 0px; background:#3344BB20;"
                            @dragover="hoverTopHalf = true" @mouseleave="hoverTopHalf = false"
                            data-targetposition="before">
                        </div>
                        <div style="position:absolute; width: 100%; height: 50%; top: 50%; background:#99443320;"
                            @dragover="hoverBottomHalf = true" @mouseleave="hoverBottomHalf = false"
                            data-targetposition="after">
                        </div>
                    </TopicContentGridItem>

                    <!-- <div v-if="showBottomFake" @dragover="hoverBottomFake = true" @drageleave="hoverBottomFake = false">
                        hello hell
                    </div> -->

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
        padding: 10px;

        &.open {}

        &.isDroppableItemActive {
            background-color: lightpink;
        }
    }

    .is-moving {
        // transform: scale(0);
    }
}
</style>