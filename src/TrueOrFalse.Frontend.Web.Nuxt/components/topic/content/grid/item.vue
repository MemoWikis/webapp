<script lang="ts" setup>

interface Item {
    name: string,
    id: number,
    children?: Item[]
}
interface Props {
    item: Item
}
const props = defineProps<Props>()
const isDroppableItemActive = ref(false)
function onDragOver() {
    isDroppableItemActive.value = true
}
function onDragLeave() {
    isDroppableItemActive.value = false
}
function onDrop(event: any) {
    console.log('dropped', props.item.id)
    const e = JSON.parse(event.dataTransfer.getData('value'))
    console.log(e)
}
</script>

<template>
    <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">
        <SharedDraggable :transfer-data="props.item.id" class="draggable">

            <div class="item">
                {{ item.name }}
            </div>
            <TopicContentGridItem v-for="c in props.item.children" :item="c" />
        </SharedDraggable>

    </SharedDroppable>
</template>

<style lang="less" scoped></style>