<script lang="ts" setup>
// import { addElementAtPath, removeElementAtPath } from 'components/shared/utils'

interface Item {
    name: string,
    id: number,
    isMoving: boolean,
    children?: Item[]
}

const items = ref<Item[]>([{
    name: 'test1',
    isMoving: false,
    id: 1,
}, {
    name: 'test2',
    isMoving: false,
    id: 2,
    children: [{
        name: 'test2-1',
        isMoving: false,
        id: 21,
    },
    {
        name: 'test2-2',
        isMoving: false,
        id: 22,
    }]
}, {
    name: 'test3',
    isMoving: false,
    id: 3,
}, {
    name: 'test4',
    isMoving: false,
    id: 4,
}, {
    name: 'test5',
    isMoving: false,
    id: 5,
    children: [{
        name: 'test5-1',
        isMoving: false,
        id: 51,
    },
    {
        name: 'test5-2',
        isMoving: false,
        id: 52,
    }]
},])

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

function onDrop(event: any) {
    console.log('target')
    const e = event.dataTransfer.getData('value')
    console.log(e)

    moveElement(items.value, e, [0])
}

const isUpdatingArray = ref(false)

async function onDndDrop({ event, targetPath }: { event: any, targetPath: IndexPath }) {
    if (isUpdatingArray.value)
        return
    isUpdatingArray.value = true
    const oldItems = [...items.value];
    items.value = moveElement(items.value, event.payload.index, targetPath)
    console.log(items.value)
    // Wait for child events to possibly update the items
    await nextTick()
    isUpdatingArray.value = false
    // If the items are the same as before, a child event has updated the items, so don't do anything
    if (oldItems === items.value) {
        return;
    }
    // console.log(event.payload.item)
    // const newarray = moveElement(items.value, event.payload.index, targetPath)
    // console.log('newarray', newarray)
    // applyDrag(event)
}

function applyDrag(dragResult: any) {
    const { removedIndex, addedIndex, payload } = dragResult
    if (removedIndex === null && addedIndex === null) return

    const result = [...items.value]
    let itemToAdd = payload

    if (removedIndex !== null) {
        itemToAdd = result.splice(removedIndex, 1)[0]
    }

    if (addedIndex !== null) {
        result.splice(addedIndex, 0, itemToAdd)
    }

    items.value = result
}

function getPayload(index: number) {
    const payload = {
        item: items.value[index],
        index: [index]
    }
    return payload
}

function setNewArr(e: Item[]) {
    console.log('newarr', e)
    // items.value = e
}
</script>

<template>
    <div class="row">
        <div class="col-xs-12">
            <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">
                <div class="grid-container" v-if="items">
                    <TopicContentGridItem v-for="item, i in items" :item="item" :index="[i]" :items="items"
                        @set-new-arr="setNewArr" />

                </div>
            </SharedDroppable>
        </div>

        <!-- <div class="grid-container">
            <Container @drop="onDndDrop({ event: $event, targetPath: [0] })" :get-child-payload="getPayload"
                group-name="test">
                <TopicContentGridItem v-for="item, i in items" :item="item" :index="[i]" @on-dnd-drop="onDndDrop"
                    :base-items="items" />
            </Container>
        </div> -->
    </div>
</template>

<style lang="less" scoped>
.grid-container {
    margin-left: 10px;
    padding: 10px;
    border: solid 1px silver;
    width: 100%;
}
</style>