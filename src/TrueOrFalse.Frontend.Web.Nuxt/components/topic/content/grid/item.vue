<script lang="ts" setup>

interface Item {
    name: string,
    id: number,
    children?: Item[]
}
interface Props {
    item: Item,
    index: IndexPath,
    items: Item[]
}
const props = defineProps<Props>()
const isDroppableItemActive = ref(false)
function onDragOver() {
    isDroppableItemActive.value = true
}
function onDragLeave() {
    isDroppableItemActive.value = false
}


function removeElementAtPath(arr: Item[], indexPath: IndexPath): { element: Item, array: Item[] } | undefined {
    let pathCopy = [...indexPath];
    let targetIndex = pathCopy.pop();

    let targetArray: Item[] = arr;
    for (let index of pathCopy) {
        if (index < targetArray.length && targetArray[index].children) {
            targetArray = targetArray[index].children || [];
        } else {
            return undefined;
        }
    }

    if (targetIndex !== undefined && targetIndex < targetArray.length) {
        let removedElement = targetArray.splice(targetIndex, 1);
        return { element: removedElement[0], array: arr };
    } else {
        return undefined;
    }
}

function addElementAtPath(arr: Item[], indexPath: IndexPath, element: Item): void {
    let pathCopy = [...indexPath];
    let targetIndex = pathCopy.pop();

    let targetArray: Item[] = arr;
    for (let index of pathCopy) {
        if (index < targetArray.length && targetArray[index].children) {
            targetArray = targetArray[index].children || [];
        } else {
            throw new Error("Invalid index path: encountered non-array element before reaching target location");
        }
    }

    if (targetIndex !== undefined) {
        targetArray.splice(targetIndex, 0, element);
    } else {
        throw new Error("Invalid index path: did not resolve to array element");
    }
}

function moveElement(arr: Item[], fromIndexPath: IndexPath, toIndexPath: IndexPath): Item[] {
    let removed = removeElementAtPath(arr, fromIndexPath);
    if (removed) {
        addElementAtPath(removed.array, toIndexPath, removed.element);
        return removed.array;
    } else {
        return arr;
    }
}
const emit = defineEmits(['setNewArr'])

const newArr = ref<Item[]>([])

function onDrop(event: any) {
    console.log('target', props.index)
    const e = event.dataTransfer.getData('value')
    console.log('drop', e)

    newArr.value = moveElement(props.items, e, props.index)
    emit('setNewArr', newArr)
}

// function getPayload(index: number) {
//     const payload = {
//         item: props.item.children![index],
//         index: [...props.index, index]
//     }
//     return payload
// }
</script>

<template>
    <SharedDraggable :transfer-data="index" class="draggable">
        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">

            <div class="item">
                {{ item.name }}
                <div>
                    <TopicContentGridItem v-for="c, i in props.item.children" :item="c" :index="[...index, i]"
                        :items="props.items" @set-new-arr="emit('setNewArr', newArr)" />
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
    }

    .is-moving {
        // transform: scale(0);
    }
}
</style>