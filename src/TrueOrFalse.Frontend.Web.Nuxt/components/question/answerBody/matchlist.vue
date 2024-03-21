<script lang="ts" setup>

interface Props {
    solution: string
    showAnswer: boolean
}
const props = defineProps<Props>()

interface Pair {
    elementLeft: Element;
    elementRight: Element;
}
interface Element {
    text: string
}

interface Solution {
    pairs: Pair[]
    rightElements: Element[]
    isSolutionOrdered: boolean
}
onBeforeMount(() => {
    const solution: Solution = JSON.parse(props.solution)
    solution.pairs.forEach(p => {
        const newPair: Pair = {
            elementLeft: {
                text: p.elementLeft.text
            },
            elementRight: {
                text: ''
            }
        }
        pairs.value.push(newPair)
    })
    rightElements.value = solution.rightElements
})

const pairs = ref<Pair[]>([])
const rightElements = ref<Element[]>([])
async function getAnswerDataString(): Promise<string> {
    await nextTick()
    let data = {
        Pairs: pairs.value
    }
    return JSON.stringify(data)
}
function getAnswerText(): string {
    return 'falsche Antwort'

}
defineExpose({ getAnswerDataString, getAnswerText })

const isDroppableItemActive = ref(false)
function onDragOver() {
    isDroppableItemActive.value = true
}
function onDragLeave() {
    isDroppableItemActive.value = false
}
function onDrop(event: any) {
    const index: number = event.target.getAttribute('data-index')
    const e = event.dataTransfer.getData('value')
    if (index != null) {
        pairs.value[index].elementRight.text = e
        temp.value = null
    }
    if (movingAnswerIndex.value != null) {
        pairs.value[movingAnswerIndex.value].elementRight.text = ''
        movingAnswerIndex.value = null
    }

    isDroppableItemActive.value = false
    dragStarted.value = false
}

const hover = ref<number | null>(null)
function dropClass(i: number) {
    let classes = ''
    if (pairs.value[i].elementRight.text != '')
        classes = 'has-input'
    if (isDroppableItemActive.value && hover.value == i && dragStarted)
        return classes + ' active'
    else if (dragStarted.value)
        return classes + 'draggable'
    else return classes
}

const temp = ref<any>()
const preDrop = ref<any>()

function dragEnter(i: number, event: any) {
    hover.value = i
    preDrop.value = pairs.value[i].elementRight.text

    var index: number = event.target.getAttribute('data-index')
    if (index != null)
        pairs.value[index].elementRight.text = temp.value

}
function dragLeave(i: number) {
    if (isDroppableItemActive.value)
        pairs.value[i].elementRight.text = preDrop.value
    if (movingAnswerIndex.value)
        pairs.value[movingAnswerIndex.value].elementRight.text = ''
    hover.value = null
}
const dragStarted = ref(false)
function dragStart(e: any) {
    dragStarted.value = true
    temp.value = e
}
const movingAnswerIndex = ref<number | null>()
function dragPlacedAnswer(i: number) {
    if (pairs.value[i].elementRight.text == '')
        return
    movingAnswerIndex.value = i
    dragStarted.value = true
    temp.value = pairs.value[i].elementRight.text
}
function handleDragEnd(i: number) {
    if (movingAnswerIndex.value == i)
        pairs.value[i].elementRight.text = ''
    movingAnswerIndex.value = null
    dragStarted.value = false
}
</script>   

<template>
    <div class="row" id="MatchlistAnswerbody">
        <div class="col-sm-12">
            <div class="row">
                <div class="col-sm-12">
                    <div class="matchlist-pairs" v-for="p, i in pairs">
                        <div class="left">{{ p.elementLeft.text }}</div>
                        <font-awesome-icon icon="fa-solid fa-arrow-right" class="pair-divider" />
                        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }" class="drop-section">
                            <SharedDraggable @dragstart="dragPlacedAnswer(i)" :transferData="pairs[i].elementRight.text"
                                :disabled="pairs[i].elementRight.text == ''" @drag-ended="handleDragEnd(i)">
                                <div class="drop-container" :class="dropClass(i)" :data-index="i"
                                    @dragenter="dragEnter(i, $event)" @dragleave="dragLeave(i)">

                                    <font-awesome-icon v-if="pairs[i].elementRight.text == ''"
                                        icon="fa-solid fa-arrow-right-to-bracket" class="drop-icon" rotation="90" />
                                    <template v-else>{{ pairs[i].elementRight.text }}</template>
                                </div>
                            </SharedDraggable>
                        </SharedDroppable>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="row">
                        <div id="matchlist-rightelements row">
                            <SharedDraggable v-for="e in rightElements" :transferData="e.text"
                                class="draggable-element col-xs-6" @dragstart="dragStart(e.text)">
                                <div class="drag">
                                    {{ e.text }}
                                </div>
                            </SharedDraggable>


                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pair-divider {
    margin: 0 12px;
}

.drop-icon {
    color: @memo-grey-light;
}

.drop-section {
    padding: 2px;
    border: 1px dashed @memo-grey-dark;
    margin: 2px;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
}

.drop-container {
    display: inline-block;
    width: 180px;
    min-height: 40px;
    display: flex;
    justify-content: center;
    align-items: center;
    transition: all 0.1s ease-in;
    background: white;
    overflow-wrap: break-word;
    padding: 12px;
    text-align: center;

    &.active {
        background: @memo-grey-lighter;
        filter: brightness(0.85)
    }

    &.draggable {
        filter: brightness(0.925)
    }
}

.hover {
    background-color: rgb(172, 255, 158);
}

.has-input {
    cursor: grab;

    &:active {
        cursor: grabbing;
    }
}

.draggable-element {
    margin: 4px;
}

.drag {
    min-height: 40px;
    height: 100%;
    width: 160px;
    padding: 8px 12px;
    background: white;
    border: 1px solid @memo-grey-darker;
    display: flex;
    justify-content: center;
    align-items: center;
    cursor: grab;
    overflow-wrap: break-word;
    padding: 12px;
    text-align: center;

    &:active {
        cursor: grabbing;
    }

    &:hover {
        filter: brightness(0.925)
    }
}

.matchlist-pairs {
    min-height: 40px;
    display: flex;
    flex-wrap: nowrap;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 12px;

    .left,
    .right {
        padding: 5px;
        width: 160px;
        min-height: 50px;
        overflow-wrap: break-word;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .right {
        background: lightgrey;
        border: solid 1px grey;
    }

    .box {
        background: lightblue;
        border: solid 1px grey;
    }
}
</style>