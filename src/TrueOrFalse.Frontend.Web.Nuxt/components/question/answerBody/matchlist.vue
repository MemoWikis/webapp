<script lang="ts" setup>
import { exportDefaultSpecifier } from '@babel/types';

interface Props {
    solution: string
    showAnswer: boolean
}
const props = defineProps<Props>()

interface Pair {
    ElementLeft: Element;
    ElementRight: Element;
}
interface Element {
    Text: string
}

interface Solution {
    Pairs: Pair[]
    RightElements: Element[]
    IsSolutionOrdered: boolean
}
onBeforeMount(() => {
    const solution: Solution = JSON.parse(props.solution)
    solution.Pairs.forEach(p => {
        const newPair: Pair = {
            ElementLeft: {
                Text: p.ElementLeft.Text
            },
            ElementRight: {
                Text: ''
            }
        }
        pairs.value.push(newPair)
    })
    rightElements.value = solution.RightElements
})

const pairs = ref<Pair[]>([])
const rightElements = ref<Element[]>([])
function getAnswerDataString(): string {
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
const onDragOver = () => {
    isDroppableItemActive.value = true
}
const onDragLeave = () => isDroppableItemActive.value = false
function onDrop(event: any) {
    const index: number = event.target.getAttribute('data-index')
    const e = JSON.parse(event.dataTransfer.getData('value'))
    if (index != null) {
        pairs.value[index].ElementRight.Text = e
        temp.value = null
    }
    if (movingAnswerIndex.value != null) {
        pairs.value[movingAnswerIndex.value].ElementRight.Text = ''
        movingAnswerIndex.value = null
    }

    isDroppableItemActive.value = false
    dragStarted.value = false
}

const hover = ref<number | null>(null)
function dropClass(i: number) {
    let classes = ''
    if (pairs.value[i].ElementRight.Text != '')
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
    preDrop.value = pairs.value[i].ElementRight.Text

    var index: number = event.target.getAttribute('data-index')
    if (index != null)
        pairs.value[index].ElementRight.Text = temp.value

}
function dragLeave(i: number) {
    if (isDroppableItemActive.value)
        pairs.value[i].ElementRight.Text = preDrop.value
    if (movingAnswerIndex.value)
        pairs.value[movingAnswerIndex.value].ElementRight.Text = ''
    hover.value = null
}
const dragStarted = ref(false)
function dragStart(e: any) {
    dragStarted.value = true
    temp.value = e
}
const movingAnswerIndex = ref<number | null>()
function dragPlacedAnswer(i: number) {
    if (pairs.value[i].ElementRight.Text == '')
        return
    movingAnswerIndex.value = i
    dragStarted.value = true
    temp.value = pairs.value[i].ElementRight.Text
}
function handleDragEnd(i: number) {
    if (movingAnswerIndex.value == i)
        pairs.value[i].ElementRight.Text = ''
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
                        <div class="left">{{ p.ElementLeft.Text }}</div>
                        <font-awesome-icon icon="fa-solid fa-arrow-right" class="pair-divider" />
                        <SharedDroppable v-bind="{ onDragOver, onDragLeave, onDrop }" class="drop-section">
                            <SharedDraggable @dragstart="dragPlacedAnswer(i)" :transferData="pairs[i].ElementRight.Text"
                                :disabled="pairs[i].ElementRight.Text == ''" @drag-ended="handleDragEnd(i)">
                                <div class="drop-container" :class="dropClass(i)" :data-index="i"
                                    @dragenter="dragEnter(i, $event)" @dragleave="dragLeave(i)">

                                    <font-awesome-icon v-if="pairs[i].ElementRight.Text == ''"
                                        icon="fa-solid fa-arrow-right-to-bracket" class="drop-icon" rotation="90" />
                                    <template v-else>{{ pairs[i].ElementRight.Text }}</template>
                                </div>
                            </SharedDraggable>
                        </SharedDroppable>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="row">
                        <div id="matchlist-rightelements">
                            <SharedDraggable v-for="e in rightElements" :transferData="e.Text" class="draggable-element"
                                @dragstart="dragStart(e.Text)">
                                <div class="drag">
                                    {{ e.Text }}
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
    word-break: break-word;
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
    word-break: break-word;
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
        word-break: break-all;
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