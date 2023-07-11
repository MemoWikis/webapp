<script lang="ts" setup>

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
function onDrop(event: any) {
    console.log('dropped')
    const e = JSON.parse(event.dataTransfer.getData('value'))
    console.log(e)
}

</script>

<template>
    <div class="grid-container">
        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }">
            <TopicContentGridItem v-for="item in items" :item="item" />
        </SharedDroppable>
    </div>
</template>

<style lang="less" scoped>
.grid-container {
    margin-left: 10px;

}

.draggable {
    transition: all 2s;
    transform: scale(1);


    .item {
        width: 100%;
        border: solid 1px silver;
        padding: 10px;

        transition: all 0.1s;
        transform: scale(1);
    }

    .is-moving {
        transform: scale(0);
    }
}
</style>